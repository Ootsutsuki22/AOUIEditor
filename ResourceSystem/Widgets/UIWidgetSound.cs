using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    public class UIWidgetSound : WidgetSoundBase
    {
        [Description("Звук для элементов системы Widgets")]
        public Sound2D sound { get; set; }

        public UIWidgetSound()
        {
            sound = new Sound2D();
        }
    }
}
