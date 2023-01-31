using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    public class UIRelatedDecalObjects : XdbObject
    {
        public UIDecalObjectResource[] Items { get; set; }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class UIDecalObjectResource
    {
        public string name { get; set; }
        public DecalTemplate resource { get; set; }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
