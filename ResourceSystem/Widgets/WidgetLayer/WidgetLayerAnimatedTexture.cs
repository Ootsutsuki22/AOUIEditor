using Microsoft.Xna.Framework.Graphics;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using SixLabors.ImageSharp.Processing;

namespace AOUIEditor.ResourceSystem
{
    public class WidgetLayerAnimatedTexture : WidgetLayer
    {
        [Description("Задержка между кадрами")]
        public int delayMs { get; set; }

        [Description("Стартовать анимацию сразу же после загрузки")]
        public bool? playImmidiatly { get; set; }

        [Description("Циклически повторять анимацию")]
        public bool? repeatForever { get; set; }

        [Description("Растягивать текстуру. по умолчанию false")]
        public bool? scaling { get; set; }

        [Description("После остановки анимации показывать последний кадр")]
        public bool? stopLastFrame { get; set; }

        [Description("Список кадров анимации")]
        public TextureFrame[] frames { get; set; }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TextureFrame
    {
        [Description("Текстура")]
        public UITextureItem textureItem { get; set; }

        [Description("Кадры")]
        public FrameRect[] rects { get; set; }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FrameRect
    {
        [Description("Коррекция центра по горизонтали")]
        public float centerOffsetX { get; set; }

        [Description("Коррекция центра по вертикали")]
        public float centerOffsetY { get; set; }

        [Description("Отступ по горизонтали")]
        public int offsetX { get; set; }

        [Description("Отступ по вертикали")]
        public int offsetY { get; set; }

        [Description("Размер по горизонтали")]
        public int sizeX { get; set; }

        [Description("Размер по вертикали")]
        public int sizeY { get; set; }
    }
}
