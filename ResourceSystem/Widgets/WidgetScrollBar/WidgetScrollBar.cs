using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetScrollBar.html

    public class WidgetScrollBar : Widget
    {
        [Category("WidgetScrollBar")]
        [Description("Кнопка перемещения ползунка вверх")]
        public WidgetButton decButton { get; set; }

        [Category("WidgetScrollBar")]
        [Description("Кнопка перемещения ползунка вниз")]
        public WidgetButton incButton { get; set; }
    }
}
