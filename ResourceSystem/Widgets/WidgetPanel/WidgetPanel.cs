using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetPanel.html

    public class WidgetPanel : Widget
    {
        [Category("WidgetPanel")]
        [Description("Уведомление о завершении загрузки содержимого")]
        public string reactionContentLoaded { get; set; }

        [Category("WidgetPanel")]
        [Description("Насильно отображать курсор когда панель активна")]
        public bool? forceShowCursorWhileActive { get; set; }
    }
}
