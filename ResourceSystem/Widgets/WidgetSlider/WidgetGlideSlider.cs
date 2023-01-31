using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetGlideSlider.html

    public class WidgetGlideSlider : WidgetSlider
    {
        [DefaultValue(10)]
        [Category("WidgetSlider")]
        [Description("Шаг дискретного перемещения. По умолчанию 10")]
        public int? discreteStep { get; set; }
    }
}
