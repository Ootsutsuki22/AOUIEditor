using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    public class UIRelatedVisObjects : XdbObject
    {
        public UIVisObjectResource[] Items { get; set; }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class UIVisObjectResource
    {
        public string name { get; set; }
        public VisObjectTemplate resource { get; set; }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
