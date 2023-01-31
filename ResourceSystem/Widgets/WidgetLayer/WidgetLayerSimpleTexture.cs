using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AOUIEditor.ResourceSystem
{
    public class WidgetLayerSimpleTexture : WidgetLayer
    {
        [Description("Растягивать текстуру. по умолчанию false")]
        public bool? Scaling { get; set; }

        [Description("Текстура")]
        public UITextureItem textureItem { get; set; }

        [Description("Текстура с альфой. Используется для задания маски, по которой будет обрезана основная текстура")]
        public UISingleTexture textureMask { get; set; }
    }
}
