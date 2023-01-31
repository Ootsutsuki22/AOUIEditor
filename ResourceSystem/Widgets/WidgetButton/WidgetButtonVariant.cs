using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetButtonVariant.html

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WidgetButtonVariant
    {
        [Description("Ссылка на текстовый файл с текстом кнопки")]
        public TextObject TextFileRef { get; set; }

        [Description("Текстура подсветки")]
        public WidgetLayer LayerHighlight { get; set; }

        [Description("Смещение при нажатии")]
        public WidgetVector2 PushedOffset { get; set; }

        [Description("Обычное состояние кнопки")]
        public WidgetButtonState StateNormal { get; set; }

        [Description("Нажатое состояние кнопки")]
        public WidgetButtonState StatePushed { get; set; }

        [Description("Подсвеченное, но не нажатое состояние кнопки")]
        public WidgetButtonState StateHighlighted { get; set; }

        [Description("Подсвеченное и нажатое состояние кнопки")]
        public WidgetButtonState StatePushedHighlighted { get; set; }

        [Description("Отключенное (запрещено нажимать) состояние кнопки")]
        public WidgetButtonState StateDisabled { get; set; }

        [Description("Реакция на клик LMB")]
        public string Reaction { get; set; }

        [Description("Реакция на клик RMB")]
        public string ReactionRightClick { get; set; }

        [Description("Реакция на двойной клик")]
        public string reactionDblClick { get; set; }

        [DefaultValue(false)]
        [Description("Если true, то реакцию о клике присылать при отпускании кнопки, иначи при нажатии. По умолчанию false")]
        public bool? ReactionOnUp { get; set; }

        [Description("Звук, воспроизводимый при наведении на кнопку")]
        public WidgetSoundBase soundOver { get; set; }

        [Description("Звук, воспроизводимый при нажатии на кнопку")]
        public WidgetSoundBase soundPress { get; set; }

        public WidgetButtonVariant()
        {
            PushedOffset = new WidgetVector2();
            StateNormal = new WidgetButtonState();
            StatePushed = new WidgetButtonState();
            StateHighlighted = new WidgetButtonState();
            StatePushedHighlighted = new WidgetButtonState();
            StateDisabled = new WidgetButtonState();
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
