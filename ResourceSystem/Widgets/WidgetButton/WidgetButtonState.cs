using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetButtonState.html

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WidgetButtonState
    {
        [Description("Текстурный слой этого состояния")]
        public WidgetLayer LayerMain { get; set; }

        [Description("Ссылка на текстовый файл с текстом кнопки во время этого состояния. Содержит стиль оформления, тег подстановки")]
        public TextObject FormatFileRef { get; set; }

        public override string ToString()
        {
            if (LayerMain != null || FormatFileRef != null)
            {
                return GetType().Name;
            }
            return string.Empty;
        }
    }
}
