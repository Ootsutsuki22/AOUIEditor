using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace AOUIEditor.ResourceSystem
{
    /// <summary>
    /// Базовый класс для XDB-объекта. Содержит всю основную магию по чтению XDB-объектов из файла и записи в файл
    /// </summary>
    [TypeConverter(typeof(XdbObjectConverter))]
    [Editor(typeof(XdbObjectFileEditor), typeof(UITypeEditor))]
    public class XdbObject
    {
        [SkipProperty]
        [ReadOnly(true)]
        [DisplayName("(File Name)")]
        public string file { get; set; }

        [SkipProperty]
        [ReadOnly(true)]
        [DisplayName("(Full Path)")]
        public string directory { get; set; }

        [SkipProperty]
        [Browsable(false)]
        public bool isIngame { get; protected set; }

        [DisplayName("(Header)")]
        public XdbHeader Header { get; protected set; }

        private static Dictionary<string, XdbObject> loadedObjects = new Dictionary<string, XdbObject>();

        public XdbObject()
        {
            Header = new XdbHeader();
        }

        public static T Load<T>(string filename, string parentfile = null, bool ingame = false) where T : XdbObject
        {
            if (!loadedObjects.ContainsKey(filename))
            {
                if (ingame)
                {
                    object instance = Activator.CreateInstance(typeof(T));
                    XdbObject xdbObject = instance as XdbObject;
                    xdbObject.file = filename;
                    xdbObject.directory = "";
                    xdbObject.isIngame = true;
                    loadedObjects.Add(filename, xdbObject);
                    return xdbObject as T;
                }
                else
                {
                    if (!File.Exists(filename))
                    {
                        Logger.LogError($"XdbObject.Load: Файл не найден '{filename}'" + Environment.NewLine + $"Ссылка указана в файле: '{parentfile}'");
                        return null;
                    }

                    XmlReaderSettings settings = new XmlReaderSettings() { IgnoreComments = true };
                    XmlDocument document = new XmlDocument();
                    try
                    {
                        using (FileStream stream = File.OpenRead(filename))
                        using (XmlReader reader = XmlReader.Create(stream, settings))
                        {
                            document.Load(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"XdbObject.Load: Не удалось прочитать файл '{filename}'" + Environment.NewLine + $"Ошибка: '{ex.Message}'");
                        return null;
                    }

                    XPointer xPointer = XPointer.None;
                    Enum.TryParse(document.DocumentElement.Name, out xPointer);

                    if (xPointer == XPointer.None)
                    {
                        Logger.LogError($"XdbObject.Load: Неподдерживаемый тип '{document.DocumentElement.Name}'" + Environment.NewLine + $"Файл: '{filename}'");
                        return null;
                    }

                    // Это необходимо, чтобы даже не создавать объект, если тип не подходит
                    Type type = Type.GetType("AOUIEditor.ResourceSystem." + xPointer.ToString());
                    if (type != typeof(T) && !type.IsSubclassOf(typeof(T)))
                    {
                        Logger.LogError($"XdbObject.Load: Несовместимые типы '{typeof(T).Name}' и '{type.Name}'" + Environment.NewLine + $"Файл: '{filename}'");
                        return null;
                    }

                    object instance = Activator.CreateInstance(type);
                    XdbObject xdbObject = instance as XdbObject;
                    xdbObject.file = Path.GetFileName(filename);
                    xdbObject.directory = Path.GetDirectoryName(filename);
                    try
                    {
                        xdbObject.Read(document.DocumentElement);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"XdbObject.Load: Не удалось десериализовать файл '{filename}' и/или его потомков" + Environment.NewLine + $"Ошибка: '{ex.Message}'");
                    }
                    xdbObject.Initialize();
                    loadedObjects.Add(filename, xdbObject);
                    return xdbObject as T;
                }
            }
            else
            {
                if (ingame)
                {
                    return loadedObjects[filename] as T;
                }
                else
                {
                    Type type = loadedObjects[filename].GetType();
                    if (type != typeof(T) && !type.IsSubclassOf(typeof(T)))
                    {
                        Logger.LogError($"XdbObject.Load: Несовместимые типы '{typeof(T).Name}' и '{type.Name}'" + Environment.NewLine + $"Файл: '{filename}'");
                        return null;
                    }
                    return loadedObjects[filename] as T;
                }
            }
        }

        public void Save()
        {
            if (!ShouldSave() || isIngame)
                return;

            UTF8Encoding utf8WithoutBom = new UTF8Encoding(false);
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true, IndentChars = "\t", Encoding = utf8WithoutBom };
            string relative = Project.GetRelativePath(GetFullPath());
            //relative = Path.Combine(Project.ExportPath, relative);
            relative = Path.Combine(Project.Location, relative);
            string dir = Path.GetDirectoryName(relative);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            try
            {
                using (FileStream stream = File.Create(relative))
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    writer.WriteStartElement(this.GetType().Name);
                    Write(writer);
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"XdbObject.Save: Не удалось сохранить файл '{relative}' и/или его потомков" + Environment.NewLine + $"Ошибка: '{ex.Message}'", Color.Red);
            }
        }

        protected virtual bool ShouldSave()
        {
            return true;
        }

        public string GetFullPath()
        {
            if (isIngame)
            {
                return file;
            }
            else
            {
                return Path.Combine(directory, file);
            }
        }

        public static void Clear()
        {
            foreach (var item in loadedObjects)
            {
                if (item.Value != null)
                    item.Value.Free();
            }
            loadedObjects = new Dictionary<string, XdbObject>();
        }

        // Вызывается после загрузки объекта, используется в WidgetLayer для загрузки Texture2D
        protected virtual void Initialize()
        {

        }

        // Вызывается при очитске проекта, используется в WidgetLayer для удаления из памяти Texture2D
        protected virtual void Free()
        {

        }

        /*
         * Мы сами контролируем чтение из файла и запись в файл, да - это выглядит как костыль
         * Но нам нужно создавать xdb-объекты во время десериализации и сохранять только ссылки
         * (href) на них при сериализации. Для автоматизации используем рефлексию
        */

        private void Read(XmlNode root)
        {
            foreach (XmlNode node in root.ChildNodes)
            {
                PropertyInfo propertyInfo = this.GetType().GetProperty(node.Name);
                if (propertyInfo == null)
                    continue;
                ReadProperty(this, node, propertyInfo);
            }
        }

        private void ReadProperty(object obj, XmlNode node, PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsValueType)
            {
                Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                if (!string.IsNullOrEmpty(node.InnerText))
                {
                    if (t.IsEnum)
                    {
                        object en;
                        if (Enum.TryParse(t, node.InnerText, out en))
                        {
                            propertyInfo.SetValue(obj, en);
                        }
                    }
                    else
                    {
                        propertyInfo.SetValue(obj, Convert.ChangeType(node.InnerText, t, CultureInfo.InvariantCulture));
                    }
                }
            }
            else if (propertyInfo.PropertyType == typeof(string))
            {
                // Этот аттрибут используется только в свойсте binaryFile у UITexture и только при чтении, т.к. UITexture мы не сохраняем
                var fileRef = Attribute.GetCustomAttribute(propertyInfo, typeof(FileRefAttribute)) as FileRefAttribute;
                if (fileRef != null)
                {
                    string href = node.Attributes["href"]?.Value;
                    if (!string.IsNullOrEmpty(href))
                    {
                        propertyInfo.SetValue(obj, href);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(node.InnerText))
                        propertyInfo.SetValue(obj, node.InnerText);
                }
            }
            else if (propertyInfo.PropertyType.IsArray)
            {
                Type elementType = propertyInfo.PropertyType.GetElementType();
                if (node.ChildNodes.Count > 0)
                {
                    Array arr = Array.CreateInstance(elementType, node.ChildNodes.Count);
                    for (int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        if (elementType.IsValueType)
                        {
                            Type t = Nullable.GetUnderlyingType(elementType) ?? elementType;
                            if (!string.IsNullOrEmpty(node.ChildNodes[i].InnerText))
                            {
                                if (t.IsEnum)
                                {
                                    object en;
                                    if (Enum.TryParse(t, node.ChildNodes[i].InnerText, out en))
                                    {
                                        arr.SetValue(en, i);
                                    }
                                }
                                else
                                {
                                    arr.SetValue(Convert.ChangeType(node.ChildNodes[i].InnerText, t, CultureInfo.InvariantCulture), i);
                                }
                            }
                        }
                        else if (elementType == typeof(string))
                        {
                            if (!string.IsNullOrEmpty(node.ChildNodes[i].InnerText))
                                arr.SetValue(node.ChildNodes[i].InnerText, i);
                        }
                        else if (elementType == typeof(XdbObject) || elementType.IsSubclassOf(typeof(XdbObject)))
                        {
                            string href = node.ChildNodes[i].Attributes["href"]?.Value;
                            href = Project.ClearHref(href);
                            if (!string.IsNullOrEmpty(href))
                            {
                                if (Project.IsIngameHref(href))
                                {
                                    MethodInfo method = typeof(XdbObject).GetMethod("Load").MakeGenericMethod(new Type[] { elementType });
                                    XdbObject xdbObject = method.Invoke(this, new object[] { href, this.GetFullPath(), true }) as XdbObject;
                                    arr.SetValue(xdbObject, i);
                                }
                                else
                                {
                                    XdbObject xdbObject = XdbObject.Load<XdbObject>(Project.GetFullPath(this.directory, href), this.GetFullPath());
                                    arr.SetValue(xdbObject, i);
                                }
                            }
                        }
                        else if (elementType == typeof(TextObject) || elementType.IsSubclassOf(typeof(TextObject)))
                        {
                            string href = node.ChildNodes[i].Attributes["href"]?.Value;
                            href = Project.ClearHref(href);
                            if (!string.IsNullOrEmpty(href))
                            {
                                if (Project.IsIngameHref(href))
                                {
                                    TextObject textObject = TextObject.Load(href, true);
                                    arr.SetValue(textObject, i);
                                }
                                else
                                {
                                    TextObject textObject = TextObject.Load(Project.GetFullPath(this.directory, href));
                                    arr.SetValue(textObject, i);
                                }
                            }
                        }
                        else if (elementType.IsClass)
                        {
                            var instance = Activator.CreateInstance(elementType);
                            foreach (XmlNode node2 in node.ChildNodes[i])
                            {
                                PropertyInfo propertyInfo2 = instance.GetType().GetProperty(node2.Name);
                                if (propertyInfo2 == null)
                                    continue;
                                ReadProperty(instance, node2, propertyInfo2);
                            }
                            arr.SetValue(instance, i);
                        }
                    }
                    propertyInfo.SetValue(obj, arr);
                }
            }
            else if (propertyInfo.PropertyType == typeof(XdbObject) || propertyInfo.PropertyType.IsSubclassOf(typeof(XdbObject)))
            {
                string href = node.Attributes["href"]?.Value;
                href = Project.ClearHref(href);
                if (!string.IsNullOrEmpty(href))
                {
                    if (Project.IsIngameHref(href))
                    {
                        MethodInfo method = typeof(XdbObject).GetMethod("Load").MakeGenericMethod(new Type[] { propertyInfo.PropertyType });
                        XdbObject xdbObject = method.Invoke(this, new object[] { href, this.GetFullPath(), true }) as XdbObject;
                        propertyInfo.SetValue(obj, xdbObject);
                    }
                    else
                    {
                        XdbObject xdbObject = XdbObject.Load<XdbObject>(Project.GetFullPath(this.directory, href), this.GetFullPath());
                        propertyInfo.SetValue(obj, xdbObject);
                    }
                }
            }
            else if (propertyInfo.PropertyType == typeof(TextObject) || propertyInfo.PropertyType.IsSubclassOf(typeof(TextObject)))
            {
                string href = node.Attributes["href"]?.Value;
                href = Project.ClearHref(href);
                if (!string.IsNullOrEmpty(href))
                {
                    if (Project.IsIngameHref(href))
                    {
                        TextObject textObject = TextObject.Load(href, true);
                        propertyInfo.SetValue(obj, textObject);
                    }
                    else
                    {
                        TextObject textObject = TextObject.Load(Project.GetFullPath(this.directory, href));
                        propertyInfo.SetValue(obj, textObject);
                    }
                }
            }
            else if (propertyInfo.PropertyType.IsClass)
            {
                if (propertyInfo.PropertyType == typeof(WidgetVector2))
                {
                    var instance = Activator.CreateInstance(propertyInfo.PropertyType);
                    string x = node.Attributes["x"]?.Value;
                    string y = node.Attributes["y"]?.Value;
                    if (!string.IsNullOrEmpty(x))
                    {
                        float xx;
                        if (float.TryParse(x, NumberStyles.Float, CultureInfo.InvariantCulture, out xx))
                        {
                            (instance as WidgetVector2).x = xx;
                        }
                    }
                    if (!string.IsNullOrEmpty(y))
                    {
                        float yy;
                        if (float.TryParse(y, NumberStyles.Float, CultureInfo.InvariantCulture, out yy))
                        {
                            (instance as WidgetVector2).y = yy;
                        }
                    }
                    propertyInfo.SetValue(obj, instance);
                }
                else
                {
                    var instance = Activator.CreateInstance(propertyInfo.PropertyType);
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        PropertyInfo propertyInfo2 = instance.GetType().GetProperty(node2.Name);
                        if (propertyInfo2 == null)
                            continue;
                        ReadProperty(instance, node2, propertyInfo2);
                    }
                    propertyInfo.SetValue(obj, instance);
                }
            }
        }

        private void Write(XmlWriter writer)
        {
            var properties = this.GetType().GetFilteredProperties();
            foreach (var propertyInfo in properties)
            {
                WriteProperty(this, writer, propertyInfo);
            }
        }

        private void WriteProperty(object obj, XmlWriter writer, PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsValueType)
            {
                var v = propertyInfo.GetValue(obj);
                if (v != null)
                {
                    Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                    if (t == typeof(bool))
                    {
                        bool b = (bool)v;
                        writer.WriteElementString(propertyInfo.Name, b.ToString().ToLower());
                    }
                    else if (t == typeof(float))
                    {
                        float f = (float)v;
                        writer.WriteElementString(propertyInfo.Name, f.ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        writer.WriteElementString(propertyInfo.Name, v.ToString());
                    }
                }
            }
            else if (propertyInfo.PropertyType == typeof(string))
            {
                string value = propertyInfo.GetValue(obj) as string;
                if (!string.IsNullOrEmpty(value))
                {
                    writer.WriteElementString(propertyInfo.Name, value);
                }
            }
            else if (propertyInfo.PropertyType.IsArray)
            {
                Array a = (Array)propertyInfo.GetValue(obj);
                Type elementType = propertyInfo.PropertyType.GetElementType();
                if (a != null && a.Length > 0)
                {
                    writer.WriteStartElement(propertyInfo.Name);
                    for (int i = 0; i < a.Length; i++)
                    {
                        writer.WriteStartElement("Item");
                        var item = a.GetValue(i);
                        if (item != null)
                        {
                            if (elementType.IsValueType)
                            {
                                Type t = Nullable.GetUnderlyingType(elementType) ?? elementType;
                                if (t == typeof(bool))
                                {
                                    bool b = (bool)item;
                                    writer.WriteString(b.ToString().ToLower());
                                }
                                else if (t == typeof(float))
                                {
                                    float f = (float)item;
                                    writer.WriteString(f.ToString(CultureInfo.InvariantCulture));
                                }
                                else
                                {
                                    writer.WriteString(item.ToString());
                                }
                            }
                            else if (elementType == typeof(string))
                            {
                                writer.WriteString(item as string);
                            }
                            else if (elementType == typeof(XdbObject) || elementType.IsSubclassOf(typeof(XdbObject)))
                            {
                                XdbObject xdbObject = item as XdbObject;
                                if (xdbObject != null)
                                {
                                    if (xdbObject.isIngame)
                                    {
                                        writer.WriteAttributeString("href", xdbObject.file);
                                    }
                                    else
                                    {
                                        writer.WriteAttributeString("href", Project.GetGameReadyRelativePath(this.directory, xdbObject.GetFullPath()));
                                    }
                                    xdbObject.Save();
                                }
                            }
                            else if (elementType == typeof(TextObject) || elementType.IsSubclassOf(typeof(TextObject)))
                            {
                                TextObject textObject = item as TextObject;
                                if (textObject != null)
                                {
                                    if (textObject.isIngame)
                                    {
                                        writer.WriteAttributeString("href", textObject.file);
                                    }
                                    else
                                    {
                                        writer.WriteAttributeString("href", Project.GetGameReadyRelativePath(this.directory, textObject.GetFullPath()));
                                    }
                                }
                            }
                            else if (elementType.IsClass)
                            {
                                var properties = item.GetType().GetFilteredProperties();
                                foreach (var propertyInfo2 in properties)
                                {
                                    WriteProperty(item, writer, propertyInfo2);
                                }
                            }
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
            }
            else if (propertyInfo.PropertyType == typeof(XdbObject) || propertyInfo.PropertyType.IsSubclassOf(typeof(XdbObject)))
            {
                XdbObject item = propertyInfo.GetValue(obj) as XdbObject;
                if (item != null)
                {
                    writer.WriteStartElement(propertyInfo.Name);
                    if (item.isIngame)
                    {
                        writer.WriteAttributeString("href", item.file);
                    }
                    else
                    {
                        writer.WriteAttributeString("href", Project.GetGameReadyRelativePath(this.directory, item.GetFullPath()));
                    }
                    item.Save();
                    writer.WriteEndElement();
                }
            }
            else if (propertyInfo.PropertyType == typeof(TextObject) || propertyInfo.PropertyType.IsSubclassOf(typeof(TextObject)))
            {
                TextObject item = propertyInfo.GetValue(obj) as TextObject;
                if (item != null)
                {
                    writer.WriteStartElement(propertyInfo.Name);
                    if (item.isIngame)
                    {
                        writer.WriteAttributeString("href", item.file);
                    }
                    else
                    {
                        writer.WriteAttributeString("href", Project.GetGameReadyRelativePath(this.directory, item.GetFullPath()));
                    }
                    writer.WriteEndElement();
                }
            }
            else if (propertyInfo.PropertyType.IsClass)
            {
                var item = propertyInfo.GetValue(obj);
                if (item != null)
                {
                    if (propertyInfo.PropertyType == typeof(WidgetVector2))
                    {
                        WidgetVector2 vec2 = item as WidgetVector2;
                        if (vec2.x.HasValue || vec2.y.HasValue)
                        {
                            writer.WriteStartElement(propertyInfo.Name);
                            writer.WriteAttributeString("x", (item as WidgetVector2).x.Value.ToString(CultureInfo.InvariantCulture));
                            writer.WriteAttributeString("y", (item as WidgetVector2).y.Value.ToString(CultureInfo.InvariantCulture));
                            writer.WriteEndElement();
                        }
                    }
                    else
                    {
                        if (NotNullObject(item))
                        {
                            writer.WriteStartElement(propertyInfo.Name);
                            var properties = item.GetType().GetFilteredProperties();
                            foreach (var propertyInfo2 in properties)
                            {
                                WriteProperty(item, writer, propertyInfo2);
                            }
                            writer.WriteEndElement();
                        }
                    }
                }
            }
        }

        private bool NotNullObject(object obj)
        {
            var properties = obj.GetType().GetFilteredProperties();
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.PropertyType.IsValueType)
                {
                    var v = propertyInfo.GetValue(obj);
                    if (v != null)
                        return true;
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    if (!string.IsNullOrEmpty(propertyInfo.GetValue(obj) as string))
                        return true;
                }
                else if (propertyInfo.PropertyType.IsArray)
                {
                    Array a = (Array)propertyInfo.GetValue(obj);
                    if (a != null && a.Length > 0)
                        return true;
                }
                else if (propertyInfo.PropertyType == typeof(XdbObject) || propertyInfo.PropertyType.IsSubclassOf(typeof(XdbObject)))
                {
                    var v = propertyInfo.GetValue(obj);
                    if (v != null)
                        return true;
                }
                else if (propertyInfo.PropertyType == typeof(TextObject) || propertyInfo.PropertyType.IsSubclassOf(typeof(TextObject)))
                {
                    var v = propertyInfo.GetValue(obj);
                    if (v != null)
                        return true;
                }
                else if (propertyInfo.PropertyType.IsClass)
                {
                    var v = propertyInfo.GetValue(obj);
                    if (v != null)
                    {
                        if (NotNullObject(v) == true)
                            return true;
                    }
                }
            }
            return false;
        }
    }
}