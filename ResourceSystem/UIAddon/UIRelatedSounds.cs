using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    public class UIRelatedSounds : XdbObject
    {
        public UISoundResource[] Items { get; set; }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class UISoundResource
    {
        public string name { get; set; }
        public Sound2D resource { get; set; }

        public UISoundResource()
        {
            resource = new Sound2D();
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Sound2D
    {
        public string name { get; set; }
        public FMODProject project { get; set; }

        public override string ToString()
        {
            return GetType().Name;
        }
    }

    public class FMODProject : XdbObject
    {
        protected override bool ShouldSave()
        {
            return false;
        }
    }
}
