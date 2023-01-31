using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetContainer.html

    public class WidgetContainer : Widget
    {
        [Category("WidgetContainer")]
        [Description("Окно, в котором отображаются добавленные элементы")]
        public WidgetPanel border { get; set; }

        [DefaultValue(WidgetsArrangement.WIDGETS_ARRANGEMENT_T2B)]
        [Category("WidgetContainer")]
        [Description("Порядок добавления элементов")]
        public WidgetsArrangement? widgetsArrangement { get; set; }
    }
}
