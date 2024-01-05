using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    public class WidgetCss : XdbObject
    {
        public string Id { get; set; }
        public WidgetCss Parent { get; set; }
        public WidgetCssSelector[] Selectors { get; set; }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WidgetCssSelector
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public WidgetCssSelectorItem[] Items { get; set; }

        public override string ToString()
        {
            return GetType().Name;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WidgetCssSelectorItem
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
