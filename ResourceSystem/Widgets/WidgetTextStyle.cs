using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetTextStyle.html

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WidgetTextStyle
    {
        [DefaultValue(false)]
        [Description("Многострочный текст. По умолчанию false")]
        public bool? multiline { get; set; }

        [DefaultValue(true)]
        [Description("Переносить текст на следующую строку, если он не влезает в размеры виджета. Для многострочного текста. По умолчанию true")]
        public bool? wrapText { get; set; }

        [DefaultValue(false)]
        [Description("Показывать частично видимый символ, если он вылез за границы виджета по ширине. По умолчанию false")]
        public bool? showClippedSymbol { get; set; }

        [DefaultValue(false)]
        [Description("Показывать частично видимый символ, если он вылез за границы виджета по высоте. По умолчанию false")]
        public bool? showClippedLine { get; set; }

        [DefaultValue(0f)]
        [Description("Фиксированное в пикселах расстояние между строками текста. По умолчанию 0 - не фиксированное расстояние")]
        public float? lineSpacing { get; set; }

        [DefaultValue(true)]
        [Description("Показывать многоточие в конце строки, если текст не влезает по горизонтали. По умолчанию true")]
        public bool? ellipsis { get; set; }

        [DefaultValue(WidgetTextStyleAlign.ALIGNY_DEFAULT)]
        [Description("Выравнивание текста по вертикали. По умолчанию ALIGNY_DEFAULT")]
        public WidgetTextStyleAlign? Align { get; set; }

        [DefaultValue(BlendEffectType.BLEND_EFFECT_ALPHABLND)]
        [Description("Способ наложения текстур. По умолчанию BLEND_EFFECT_ALPHABLND")]
        public BlendEffectType? blendEffect { get; set; }

        public override string ToString()
        {
            return GetType().Name;
        }
    }

    public enum WidgetTextStyleAlign
    {
        ALIGNY_DEFAULT,
        ALIGNY_TOP,
        ALIGNY_MIDDLE,
        ALIGNY_BOTTOM
    }
}
