using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace AOUIEditor.ResourceSystem
{
    /// <summary>
    /// Аналог XDB-объекта, только без типа. Нужен для нормальной работы ссылок на текстовые файлы и файлы скриптов
    /// </summary>
    [TypeConverter(typeof(TextObjectConverter))]
    [Editor(typeof(TextObjectFileEditor), typeof(UITypeEditor))]
    public class TextObject
    {
        [SkipProperty]
        [ReadOnly(true)]
        [DisplayName("File Name")]
        public string file { get; set; }

        [SkipProperty]
        [ReadOnly(true)]
        [DisplayName("Full Path")]
        public string directory { get; set; }

        [SkipProperty]
        [Browsable(false)]
        [DisplayName("Is ingame path")]
        public bool isIngame { get; protected set; }

        private static Dictionary<string, TextObject> loadedObjects = new Dictionary<string, TextObject>();

        public static TextObject Load(string filename, bool ingame = false)
        {
            if (!loadedObjects.ContainsKey(filename))
            {
                if (ingame)
                {
                    TextObject obj = new TextObject();
                    obj.file = filename;
                    obj.directory = "";
                    obj.isIngame = true;
                    loadedObjects.Add(filename, obj);
                    return obj;
                }
                else
                {
                    TextObject obj = new TextObject();
                    obj.file = Path.GetFileName(filename);
                    obj.directory = Path.GetDirectoryName(filename);
                    loadedObjects.Add(filename, obj);
                    return obj;
                }
            }
            else
            {
                return loadedObjects[filename];
            }
        }

        public static void Clear()
        {
            loadedObjects = new Dictionary<string, TextObject>();
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
    }

    public class TextObjectConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value != null)
            {
                if (((TextObject)value).isIngame)
                {
                    return ((TextObject)value).GetFullPath();
                }
                else
                {
                    return Project.GetRelativePath(((TextObject)value).GetFullPath());
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string str = value as string;
                if (!string.IsNullOrEmpty(str))
                {
                    if (Project.IsIngameHref(str))
                    {
                        return TextObject.Load(str, true);
                    }
                    else if (File.Exists(str))
                    {
                        return TextObject.Load(str);
                    }
                    else
                    {
                        str = Project.GetFullPath(str);
                        if (File.Exists(str))
                        {
                            return TextObject.Load(str);
                        }
                    }
                }
                return null;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    public class TextObjectFileEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (value is TextObject)
                {
                    openFileDialog.InitialDirectory = (value as TextObject).directory;
                    openFileDialog.FileName = (value as TextObject).file;
                }

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return TextObject.Load(openFileDialog.FileName);
                }
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.Modal;
    }
}
