using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetForm.html

    public class WidgetForm : Widget
    {
        [Category("WidgetForm")]
        [Description("Уведомление о завершении загрузки содержимого")]
        public string reactionContentLoaded { get; set; }
    }
}
