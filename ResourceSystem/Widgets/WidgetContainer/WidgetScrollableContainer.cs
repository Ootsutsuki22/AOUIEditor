using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetScrollableContainer.html

    public class WidgetScrollableContainer : WidgetContainer
    {
        [Category("WidgetContainer")]
        [Description("Элемент для прокрутки содержимого")]
        public WidgetGlideScrollBar scrollBar { get; set; }

        [DefaultValue(0)]
        [Category("WidgetContainer")]
        [Description("Интервал между соседними элементами. По умолчанию 0")]
        public int? elementsInterval { get; set; }
    }
}
