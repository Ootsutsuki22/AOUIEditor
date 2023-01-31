using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AOUIEditor.ResourceSystem
{
    // Creating.html

    public class UIAddon : XdbObject
    {
        [Category("UIAddon")]
        [Description("Имя аддона")]
        public string Name { get; set; }

        [Category("UIAddon")]
        [Description("Ссылка на текстовый файл с именем аддона")]
        public TextObject localizedNameFileRef { get; set; }

        [Category("UIAddon")]
        [Description("Ссылка на текстовый файл с описанием аддона")]
        public TextObject localizedDescFileRef { get; set; }

        [DefaultValue(true)]
        [Category("UIAddon")]
        [Description("Автоматический запуск дополнения после старта игры")]
        public bool AutoStart { get; set; }

        [Category("UIAddon")]
        public string[] addonGroups { get; set; }

        [Category("UIAddon")]
        [Description("Список исполняемых Lua-скриптов")]
        [Editor(typeof(TextObjectCollectionEditor), typeof(UITypeEditor))]
        public TextObject[] ScriptFileRefs { get; set; }

        [Category("UIAddon")]
        [Description("Идентификатор главной формы")]
        public string MainFormId { get; set; }

        [Category("UIAddon")]
        [Description("Список форм")]
        public UIAddonForm[] Forms { get; set; }

        [Category("UIAddon")]
        public UIRelatedVisObjects visObjects { get; set; }

        [Category("UIAddon")]
        public UIRelatedVisObjects aliasVisObjects { get; set; }

        [Category("UIAddon")]
        public UIRelatedTexts texts { get; set; }

        [Category("UIAddon")]
        public UIRelatedTextsGroup[] textsGroups { get; set; }

        [Category("UIAddon")]
        public UIRelatedTextures textures { get; set; }

        [Category("UIAddon")]
        public UIRelatedTexturesGroup[] texturesGroups { get; set; }

        [Category("UIAddon")]
        public UIRelatedSounds sounds { get; set; }

        [Category("UIAddon")]
        public UIRelatedSoundsGroup[] soundsGroups { get; set; }

        [Category("UIAddon")]
        public UIRelatedDecalObjects decalObjects { get; set; }

        public UIAddon()
        {
            AutoStart = true;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return "(" + GetType().Name + ")";
            }
            else
            {
                return Name + " (" + GetType().Name + ")";
            }
        }
    }
}
