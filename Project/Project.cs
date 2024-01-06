using AOUIEditor.ResourceSystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AOUIEditor
{
    public static class Project
    {
        public static UIAddon Addon { get; private set; }
        public static string Filename { get; private set; }
        public static string Location { get; private set; }

        public static XdbObject IsolatedObject { get; private set; }

        public static void Open(string filename)
        {
            Clear();
            Filename = filename;
            Location = Path.GetDirectoryName(filename);
            Addon = XdbObject.Load<UIAddon>(filename);
            if (Logger.ErrorCount > 0)
            {
                Logger.LogError("ВНИМАНИЕ!!! Устраните описанные выше ошибки вручную. Все указанные файлы не были корректно загружены. " +
                    "Возможно ссылки на них будут удалены при сохранении! Возможно загружены не все свойства объектов! " +
                    Environment.NewLine + "===> ПОСЛЕ ИСПРАВЛЕНИЯ ОШИБОК ОТКРОЙТЕ АДДОН ЗАНОВО! <===", Color.Red);
            }
        }

        public static void Save()
        {
            if (Addon == null)
                return;
            Addon.Save();
            Logger.Log("Сохранено!");
        }

        public static void Clear()
        {
            Addon = null;
            IsolatedObject = null;
            PropertiesForm.SelectedObject = null;
            Item.FillData();
            TreeForm.RefreshTree();
            Logger.Clear();
            XdbObject.Clear();
            TextObject.Clear();
            NewWidgetDialog.Clear();
        }

        public static void UpdateInfo()
        {
            Item.FillData();
            TreeForm.RefreshTree();
            Item.Sort();
            PropertiesForm.RefreshGrid();
        }

        public static void New(string name, string path, bool addForm)
        {
            Clear();
            string _location = Path.Combine(path, name);
            string _filename = Path.Combine(_location, "AddonDesc.(UIAddon).xdb");
            if (File.Exists(_filename))
            {
                Logger.LogError("Аддон уже существует!");
                return;
            }
            Location = _location;
            Filename = _filename;
            Addon = new UIAddon();
            Addon.Name = name;
            Addon.file = "AddonDesc.(UIAddon).xdb";
            Addon.directory = Location;
            if (addForm)
            {
                Addon.Form = new WidgetForm();
                Addon.Form.file = "MainForm.(WidgetForm).xdb";
                Addon.Form.directory = Path.Combine(Location, "Widgets");
                Addon.Form.Name = name;
                Addon.Form.Priority = 4000;
                Addon.Form.Placement.X.Align = WidgetAlign.WIDGET_ALIGN_BOTH;
                Addon.Form.Placement.Y.Align = WidgetAlign.WIDGET_ALIGN_BOTH;
                Addon.Form.PickChildrenOnly = true;
            }
            Addon.ScriptFileRefs = new TextObject[1];
            Addon.ScriptFileRefs[0] = new TextObject();
            Addon.ScriptFileRefs[0].file = "ScriptMain.lua";
            Addon.ScriptFileRefs[0].directory = Path.Combine(Location, "Scripts");
            if (!Directory.Exists(Addon.ScriptFileRefs[0].directory))
                Directory.CreateDirectory(Addon.ScriptFileRefs[0].directory);
            File.Copy(@"Scripts\ScriptMain.lua", Path.Combine(Addon.ScriptFileRefs[0].directory, "ScriptMain.lua"));
            Save();
            Open(Filename);
        }

        public static string GetRelativePath(string path)
        {
            return Path.GetRelativePath(Location, path).Replace("/", @"\");
        }

        public static string GetGameReadyRelativePath(string mainPath, string secondPath)
        {
            return Path.GetRelativePath(mainPath, secondPath).Replace(@"\", @"/");
        }

        public static string GetFullPath(string relativePath)
        {
            return GetFullPath(Location, relativePath);
        }

        public static string GetFullPath(string mainPath, string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return mainPath;
            return Path.GetFullPath(Path.Join(mainPath, relativePath)).Replace("/", @"\");
        }

        public static string ClearHref(string href)
        {
            if (string.IsNullOrEmpty(href))
                return href;
            int idx = href.IndexOf('#');
            if (idx > -1)
            {
                href = href.Remove(idx);
            }
            return href;
        }

        public static bool IsIngameHref(string href)
        {
            return href.StartsWith("/");
        }

        public static void SetIsolatedObject(XdbObject xdbObject)
        {
            IsolatedObject = xdbObject;
            UpdateInfo();
        }
    }
}
