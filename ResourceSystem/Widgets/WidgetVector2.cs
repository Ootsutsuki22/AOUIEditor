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
    public class WidgetVector2
    {
        [DefaultValue(0f)]
        [XmlAttribute]
        public float? x { get; set; }

        [DefaultValue(0f)]
        [XmlAttribute]
        public float? y { get; set; }

        public override string ToString()
        {
            return $"x = \"{x}\" y = \"{y}\"";
        }
    }
}
