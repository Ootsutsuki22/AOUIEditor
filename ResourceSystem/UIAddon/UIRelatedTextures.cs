using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    public class UIRelatedTextures : XdbObject
    {
        public UITextureResource[] Items { get; set; }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class UITextureResource
    {
        public string name { get; set; }
        public UITextureItem textureItem { get; set; }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
