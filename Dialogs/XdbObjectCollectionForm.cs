using AOUIEditor.ResourceSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AOUIEditor
{
    public partial class XdbObjectCollectionForm : Form
    {
        private Type _type;

        private object[] _value;
        public object[] Value
        {
            get
            {
                MethodInfo method = typeof(XdbObjectCollectionForm).GetMethod("ConvertArray").MakeGenericMethod(new Type[] { _type });
                var result = method.Invoke(this, new object[] { _value });
                return result as object[];
            }
            set
            {
                _value = value;
                OnValueChanged();
            }
        }

        public static T[] ConvertArray<T>(object[] arr)
        {
            if (arr == null)
                return null;
            T[] result = new T[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                result[i] = (T)arr[i];
            }
            return result;
        }

        private List<object> list;

        public XdbObjectCollectionForm(Type type)
        {
            _type = type;
            InitializeComponent();
        }

        private void OnValueChanged()
        {

            if (_value == null)
            {
                list = new List<object>();
            }
            else
            {
                list = _value.ToList();
            }
            UpdateList();
        }

        private void UpdateList()
        {
            listView.BeginUpdate();
            listView.Items.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null)
                {
                    var xdb = list[i] as XdbObject;
                    var widget = xdb as Widget;
                    if (xdb.isIngame)
                    {
                        ListViewItem item = new ListViewItem(i.ToString());
                        if (widget != null)
                        {
                            item.SubItems.Add(widget.Name);
                        }
                        else
                        {
                            item.SubItems.Add("");
                        }
                        item.SubItems.Add(xdb.GetFullPath());
                        listView.Items.Add(item);
                    }
                    else
                    {
                        ListViewItem item = new ListViewItem(i.ToString());
                        if (widget != null)
                        {
                            item.SubItems.Add(widget.Name);
                        }
                        else
                        {
                            item.SubItems.Add("");
                        }
                        item.SubItems.Add(Project.GetRelativePath(xdb.GetFullPath()));
                        listView.Items.Add(item);
                    }
                }
                else
                {
                    ListViewItem item = new ListViewItem(i.ToString());
                    listView.Items.Add(item);
                }
            }
            listView.EndUpdate();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Widget (*.xdb)|*.xdb";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    MethodInfo method = typeof(XdbObject).GetMethod("Load").MakeGenericMethod(new Type[] { _type });
                    XdbObject xdbObject = method.Invoke(this, new object[] { openFileDialog.FileName, null, false }) as XdbObject;
                    //Widget xdbObject = XdbObject.Load<Widget>(openFileDialog.FileName);
                    if (xdbObject != null)
                    {
                        list.Add(xdbObject);
                        UpdateList();
                    }
                }
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices == null || listView.SelectedIndices.Count == 0)
                return;
            if (listView.SelectedIndices[0] >= 0)
            {
                list.RemoveAt(listView.SelectedIndices[0]);
                UpdateList();
            }
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices == null || listView.SelectedIndices.Count == 0)
                return;
            int idx = listView.SelectedIndices[0];
            if (idx > 0)
            {
                object a = list[idx];
                list[idx] = list[idx - 1];
                list[idx - 1] = a;
                UpdateList();
                listView.Items[idx - 1].Selected = true;
            }
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices == null || listView.SelectedIndices.Count == 0)
                return;
            int idx = listView.SelectedIndices[0];
            if (idx >= 0 && idx < list.Count - 1)
            {
                object a = list[idx];
                list[idx] = list[idx + 1];
                list[idx + 1] = a;
                UpdateList();
                listView.Items[idx + 1].Selected = true;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (list.Count == 0)
            {
                _value = null;
            }
            else
            {
                _value = list.ToArray();
            }
            DialogResult = DialogResult.OK;
        }
    }
}
