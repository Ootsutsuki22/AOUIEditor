using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetDiscreteScrollBar.html

    public class WidgetDiscreteScrollBar : WidgetScrollBar
    {
        [Category("WidgetScrollBar")]
        [Description("Слайдер")]
        public WidgetDiscreteSlider slider { get; set; }
    }
}
