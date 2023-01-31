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
    public class UITexture : XdbObject
    {
        [FileRef]
        [ReadOnly(true)]
        [Description("Бинарный файл текстуры")]
        public string binaryFile { get; set; }

        [ReadOnly(true)]
        [Description("Ширина экспортированной текстуры, приведённая к степени двух")]
        public int width { get; set; }

        [ReadOnly(true)]
        [Description("Высота экспортированной текстуры, приведённая к степени двух")]
        public int height { get; set; }

        [ReadOnly(true)]
        [Description("Реальная ширина экспортированной текстуры")]
        public int realWidth { get; set; }

        [ReadOnly(true)]
        [Description("Реальная высота экспортированной текстуры")]
        public int realHeight { get; set; }

        [ReadOnly(true)]
        [Description("Формат экспортированной текстуры")]
        public string type { get; set; }

        [SkipProperty]
        [Browsable(false)]
        public Texture2D texture { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            LoadTexture();
        }

        protected override void Free()
        {
            base.Free();
            FreeTexture();
        }

        protected override bool ShouldSave()
        {
            return false;
        }

        private void FreeTexture()
        {
            if (texture != null && !texture.IsDisposed)
                texture.Dispose();
        }

        private void LoadTexture()
        {
            FreeTexture();

            if (isIngame)
                return;

            if (string.IsNullOrEmpty(binaryFile))
                return;

            string binaryFullPath = Project.GetFullPath(directory, binaryFile);
            if (string.IsNullOrEmpty(binaryFullPath))
                return;

            if (!File.Exists(binaryFullPath))
            {
                Logger.LogError($"WidgetLayer.Initialize: Файл не найден '{binaryFullPath}'" + Environment.NewLine + $"Ссылка указана в файле: '{this.GetFullPath()}'");
                return;
            }

            byte[] data = null;

            try
            {
                using (FileStream stream = File.OpenRead(binaryFullPath))
                {
                    // Аллодовские bin-файлы - это zlib с добавленными в начале двумя байтами 78 9C
                    stream.Seek(2, SeekOrigin.Begin);
                    using (DeflateStream decompressor = new DeflateStream(stream, CompressionMode.Decompress))
                    using (MemoryStream uncompressedBin = new MemoryStream())
                    {
                        decompressor.CopyTo(uncompressedBin);
                        data = uncompressedBin.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"WidgetLayer.Initialized: Не удалось прочитать файл '{binaryFullPath}'" + Environment.NewLine + $"Ошибка: '{ex.Message}'");
                return;
            }

            // Первые 8 байт нам не нужны, информация о Mip уровнях
            byte[] data2 = new byte[data.Length - 8];
            Array.Copy(data, 8, data2, 0, data2.Length);

            // Массив, в который декодируем DDS по 4 байта на пиксель (RGBA)
            byte[] result = new byte[4 * width * height];

            try
            {
                switch (type)
                {
                    case "DXT1":
                        DxtDecoder.DecompressDXT1(data2, width, height, result);
                        break;
                    case "DXT3":
                        DxtDecoder.DecompressDXT3(data2, width, height, result);
                        break;
                    case "DXT5":
                        DxtDecoder.DecompressDXT5(data2, width, height, result);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"WidgetLayer.Initialized: Не удалось декодировать DDS-файл '{binaryFullPath}'" + Environment.NewLine + $"Ошибка: '{ex.Message}'");
                return;
            }

            try
            {
                using (Image<Bgra32> image = Image.LoadPixelData<Bgra32>(result, width, height))
                {
                    try
                    {
                        image.Mutate(x => x.Crop(realWidth, realHeight));
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"WidgetLayer.Initialized: Не удалось обрезать DDS-текестуру по размерам realWidth и realHeight" + Environment.NewLine + $"Проверьте файл '{this.GetFullPath()}'" + Environment.NewLine + $"Ошибка: '{ex.Message}'");
                        return;
                    }
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, new PngEncoder());
                        ms.Seek(0, SeekOrigin.Begin);
                        texture = Texture2D.FromStream(MonoControl.Instance.GraphicsDevice, ms);
                        Microsoft.Xna.Framework.Color[] buffer = new Microsoft.Xna.Framework.Color[texture.Width * texture.Height];
                        texture.GetData(buffer);
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            buffer[i] = Microsoft.Xna.Framework.Color.FromNonPremultiplied(buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
                        }
                        texture.SetData(buffer);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"WidgetLayer.Initialized: Не удалось создать текстуру из '{binaryFullPath}'" + Environment.NewLine + $"Ошибка: '{ex.Message}'");
                return;
            }
        }
    }

    public class FileRefAttribute : Attribute
    {
    }
}
