using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetButton.html

    public class WidgetButton : Widget
    {
        [Category("WidgetButton")]
        [Description("Название текстового тэга в строке форматирования текущего варианта, в который будет подставляться текст из выбранного варианта при вызове ButtonSafe:SetVariant( self, variant )")]
        public string TextTag { get; set; }

        [Category("WidgetButton")]
        [Description("Реакции, которые кнопка будет переводить в нажатия для текущего варианта (независимо от bindedReactions). См. Widget")]
        public WidgetBindSection[] pushingBindSections { get; set; }

        [Category("WidgetButton")]
        [Description("Список вариантов кнопки, между которыми она может переключаться")]
        public WidgetButtonVariant[] Variants { get; set; }

        [Category("WidgetButton")]
        [Description("Текстовый стиль кнопки")]
        public WidgetTextStyle TextStyle { get; set; }

        [DefaultValue(true)]
        [Category("WidgetButton")]
        [Description("Использовать звук клика по умолчанию. По умолчанию true")]
        public bool? useDefaultSounds { get; set; }

        public WidgetButton()
        {
            TextStyle = new WidgetTextStyle();
        }
    }
}
