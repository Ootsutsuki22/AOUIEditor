using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class UIRelatedTextsGroup
    {
        public string groupName { get; set; }
        public UIRelatedTexts texts { get; set; }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
