using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    public class UITextureItem : XdbObject
    {
        [ReadOnly(true)]
        [Description("Текстура")]
        public UITexture singleTexture { get; set; }

        protected override bool ShouldSave()
        {
            return false;
        }
    }
}
