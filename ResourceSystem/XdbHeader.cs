using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AOUIEditor.ResourceSystem
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class XdbHeader
    {
        private bool? _isPrototype;
        [DefaultValue(false)]
        public bool? isPrototype
        {
            get { return _isPrototype; }
            set
            {
                if (value == true)
                {
                    _isPrototype = true;
                }
                else
                {
                    _isPrototype = null;
                }
            }
        }

        public XdbObject Prototype { get; set; }

        public override string ToString()
        {
            if (_isPrototype == true || Prototype != null)
                return "Prototype";
            return string.Empty;
        }
    }
}
