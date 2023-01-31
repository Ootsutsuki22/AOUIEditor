using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    public class UIRelatedTexts : XdbObject
    {
        public UITextResource[] items { get; set; }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class UITextResource
    {
        public string name { get; set; }
        public TextObject resource { get; set; }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
