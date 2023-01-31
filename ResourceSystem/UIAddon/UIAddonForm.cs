using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class UIAddonForm
    {
        public string Id { get; set; }
        public WidgetForm Form { get; set; }

        public override string ToString()
        {
            return Id;
        }
    }
}
