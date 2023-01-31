using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    public class WidgetControl3D : Widget
    {
        [Category("WidgetControl3D")]
        [Description("Use black (true) or transparent (false) background")]
        public bool? sceneClearColor { get; set; }

        [Category("WidgetControl3D")]
        [Description("Реакции")]
        public WidgetControlReactions3D Reactions3D { get; set; }

        public WidgetControl3D()
        {
            Reactions3D = new WidgetControlReactions3D();
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WidgetControlReactions3D
    {
        public string leftMouseDblClick { get; set; }
        public string leftMouseDown { get; set; }
        public string leftMouseUp { get; set; }
        public string pointingChanged { get; set; }
        public string rightMouseDblClick { get; set; }
        public string rightMouseDown { get; set; }
        public string rightMouseUp { get; set; }
        public string selfMousePosChanged { get; set; }
    }
}
