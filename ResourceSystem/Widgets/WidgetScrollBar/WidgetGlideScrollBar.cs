using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetGlideScrollBar.html

    public class WidgetGlideScrollBar : WidgetScrollBar
    {
        [Category("WidgetScrollBar")]
        [Description("Слайдер")]
        public WidgetGlideSlider slider { get; set; }
    }
}
