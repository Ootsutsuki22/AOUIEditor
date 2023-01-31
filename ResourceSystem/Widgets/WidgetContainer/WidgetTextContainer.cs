using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetTextContainer.html

    public class WidgetTextContainer : WidgetContainer
    {
        [Category("WidgetContainer")]
        [Description("Элемент для прокрутки содержимого")]
        public WidgetGlideScrollBar scrollBar { get; set; }

        [Category("WidgetContainer")]
        [Description("Ссылка на файл, содержащий формат добавляемых простых строк")]
        public TextObject formatFileRef { get; set; }

        [Category("WidgetContainer")]
        [Description("Название тега в форматной строке formatFileRef для добавляемой строки")]
        public string defaultTag { get; set; }

        [DefaultValue(0)]
        [Category("WidgetContainer")]
        [Description("Интервал между соседними элементами. По умолчанию 0")]
        public int? elementsInterval { get; set; }

        [DefaultValue(false)]
        [Category("WidgetContainer")]
        [Description("Кликабельны только объекты в тексте и скроллбар, но не сам виджет и его запчасти, включая текстовые элементы")]
        public bool? pickObjectsOnly { get; set; }
    }
}
