using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetDiscreteSlider.html

    public class WidgetDiscreteSlider : WidgetSlider
    {
        [DefaultValue(0)]
        [Category("WidgetSlider")]
        [Description("Количество дискретных позиций. По умолчанию 0")]
        public int? stepsCount { get; set; }
    }
}
