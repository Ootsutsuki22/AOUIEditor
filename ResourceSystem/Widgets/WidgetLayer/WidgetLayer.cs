using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetLayer.html

    public class WidgetLayer : XdbObject
    {
        [DefaultValue(false)]
        [Description("Использовать точное позиционирование текстуры (мелкие детали могут размазываться) или дискретное (с шагом в пиксел, максимальная четкость), по умолчанию false")]
        public bool? flatPlacement { get; set; }

        [DefaultValue(false)]
        [Description("Позволяет использовать отложенную загрузку текстур для данного слоя, по умолчанию false")]
        public bool? lazyLoad { get; set; }

        [Browsable(false)]
        [DefaultValue("0xffffffff")]
        public string Color
        {
            get
            {
                return ("0x" + A.ToString("X2") + R.ToString("X2") + G.ToString("X2") + B.ToString("X2")).ToLower();
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length != 10)
                    ResetColor();
                try
                {
                    A = Convert.ToInt32(value.Substring(2, 2), 16);
                    R = Convert.ToInt32(value.Substring(4, 2), 16);
                    G = Convert.ToInt32(value.Substring(6, 2), 16);
                    B = Convert.ToInt32(value.Substring(8, 2), 16);
                }
                catch
                {
                    ResetColor();
                }
            }
        }

        [SkipProperty]
        [Browsable(false)]
        public Microsoft.Xna.Framework.Color XnaColor
        {
            get
            {
                return Microsoft.Xna.Framework.Color.FromNonPremultiplied(R, G, B, A);
            }
        }

        private int _a;
        [SkipProperty]
        [DefaultValue(255)]
        [DisplayName("Color A")]
        public int A { get { return _a; } set { _a = Math.Clamp(value, 0, 255); } }

        private int _r;
        [SkipProperty]
        [DefaultValue(255)]
        [DisplayName("Color R")]
        public int R { get { return _r; } set { _r = Math.Clamp(value, 0, 255); } }

        private int _g;
        [SkipProperty]
        [DefaultValue(255)]
        [DisplayName("Color G")]
        public int G { get { return _g; } set { _g = Math.Clamp(value, 0, 255); } }

        private int _b;
        [SkipProperty]
        [DefaultValue(255)]
        [DisplayName("Color B")]
        public int B { get { return _b; } set { _b = Math.Clamp(value, 0, 255); } }

        [DefaultValue(false)]
        [Description("Обесцвечивание, по умолчанию false")]
        public bool? Grayed { get; set; }

        [DefaultValue(BlendEffectType.BLEND_EFFECT_ALPHABLND)]
        [Description("смешивание, по умолчанию BLEND_EFFECT_ALPHABLND")]
        public BlendEffectType? BlendEffect { get; set; }

        [SkipProperty]
        [DisplayName("(Layer Type)")]
        public string layerType { get { return this.GetType().Name; } }

        public WidgetLayer()
        {
            ResetColor();
        }

        private void ResetColor()
        {
            R = 255; G = 255; B = 255; A = 255;
        }
    }
}
