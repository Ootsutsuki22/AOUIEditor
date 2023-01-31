using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetEditBox.html

    public class WidgetEditBox : WidgetEditBase
    {
        [Category("WidgetEdit")]
        [Description("Плавный скроллбар для скролирования теста по вертикали (не обязательный), отнимает часть внутреннего пространства виджета")]
        public WidgetGlideScrollBar scrollBar { get; set; }
    }
}
