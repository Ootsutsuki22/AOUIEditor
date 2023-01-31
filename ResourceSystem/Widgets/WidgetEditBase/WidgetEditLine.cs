using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetEditLine.html

    public class WidgetEditLine : WidgetEditBase
    {
        [DefaultValue(false)]
        [Category("WidgetEdit")]
        [Description("Если true, то в строке ввода находится секретная информация и текст выводится в виде звездочек. По умолчанию false")]
        public bool? isPassword { get; set; }

        [Category("WidgetEdit")]
        [Description("Нажатие Enter")]
        public string ReactionEnter { get; set; }

        [Category("WidgetEdit")]
        [Description("Отпущена клавиша на клавиатуре")]
        public string reactionUp { get; set; }

        [Category("WidgetEdit")]
        [Description("Нажата клавиша на клавиатуре")]
        public string reactionDown { get; set; }
    }
}
