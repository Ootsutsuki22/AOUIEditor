using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AOUIEditor
{
    public class DockForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public event EventHandler OnHide;

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
            OnHide?.Invoke(this, EventArgs.Empty);
            base.OnFormClosing(e);
        }
    }
}
