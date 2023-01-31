using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace AOUIEditor.ResourceSystem
{
    // Кастомный редактор массивов TextObject, используется в:
    // UIAddon: ScriptFileRefs

    public class TextObjectCollectionEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.Modal;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider?.GetService(typeof(IWindowsFormsEditorService)) is IWindowsFormsEditorService edSvc)
            {
                using (TextObjectCollectionForm form = new TextObjectCollectionForm())
                {
                    form.Value = value as TextObject[];
                    if (edSvc.ShowDialog(form) == DialogResult.OK)
                    {
                        value = form.Value;
                    }
                }
            }
            return value;
        }
    }
}
