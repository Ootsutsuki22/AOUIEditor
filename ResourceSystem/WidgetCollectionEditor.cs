using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace AOUIEditor.ResourceSystem
{
    // Кастомный редактор массивов виджетов, используется в:
    // Widget: Children
    // WidgetPlacementXY: sizingWidgets

    public class WidgetCollectionEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.Modal;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider?.GetService(typeof(IWindowsFormsEditorService)) is IWindowsFormsEditorService edSvc)
            {
                using (WidgetCollectionForm form = new WidgetCollectionForm())
                {
                    form.Value = value as Widget[];
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
