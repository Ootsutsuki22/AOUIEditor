using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetSlider.html

    public class WidgetSlider : Widget
    {
        [Category("WidgetSlider")]
        [Description("Ползунок")]
        public WidgetButton sliderButton { get; set; }

        [DefaultValue(WidgetsArrangement.WIDGETS_ARRANGEMENT_T2B)]
        [Category("WidgetSlider")]
        [Description("Направление перемещения")]
        public WidgetsArrangement? moveArrangement { get; set; }

        [Category("WidgetSlider")]
        [Description("Реакция на изменение положение ползунка (исключая прямое задание его позиции из скрипта)")]
        public string reactionChanged { get; set; }
    }
}
