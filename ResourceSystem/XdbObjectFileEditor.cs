using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AOUIEditor.ResourceSystem
{
    public class XdbObjectFileEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (value is XdbObject)
                {
                    openFileDialog.InitialDirectory = (value as XdbObject).directory;
                    openFileDialog.FileName = (value as XdbObject).file;
                }

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Type type = context.PropertyDescriptor.PropertyType;
                    MethodInfo method = typeof(XdbObject).GetMethod("Load").MakeGenericMethod(new Type[] { type });
                    return method.Invoke(this, new object[] { openFileDialog.FileName, null, false });
                }
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.Modal;
    }
}
