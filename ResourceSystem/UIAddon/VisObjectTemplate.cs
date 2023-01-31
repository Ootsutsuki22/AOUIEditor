using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    public class VisObjectTemplate : XdbObject
    {
        protected override bool ShouldSave()
        {
            return false;
        }
    }
}
