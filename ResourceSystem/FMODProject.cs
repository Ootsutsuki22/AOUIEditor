using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    public class FMODProject : XdbObject
    {
        [Category("FMODProject")]
        [Description("Ссылка на файл проекта FMOD")]
        public TextObject fmodProject { get; set; }

        [Category("FMODProject")]
        [Description("Список банков FMOD")]
        [Editor(typeof(TextObjectCollectionEditor), typeof(UITypeEditor))]
        public TextObject[] fmodBanks { get; set; }

        [Category("FMODProject")]
        [Description("Список ивентов")]
        public EventInfo[] eventsInfo { get; set; }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class EventInfo
    {
        [Description("Имя ивента")]
        public string eventName { get; set; }
        [Description("Имена банков (только имя файла без пути!)")]
        public string[] bankNames { get; set; }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
