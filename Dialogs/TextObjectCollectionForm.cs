using AOUIEditor.ResourceSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AOUIEditor
{
    public partial class TextObjectCollectionForm : Form
    {
        private TextObject[] _value;
        public TextObject[] Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnValueChanged();
            }
        }

        private List<TextObject> list;

        public TextObjectCollectionForm()
        {
            InitializeComponent();
        }

        private void OnValueChanged()
        {

            if (_value == null)
            {
                list = new List<TextObject>();
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
                    if (list[i].isIngame)
                    {
                        ListViewItem item = new ListViewItem(i.ToString());
                        item.SubItems.Add(list[i].GetFullPath());
                        listView.Items.Add(item);
                    }
                    else
                    {
                        ListViewItem item = new ListViewItem(i.ToString());
                        item.SubItems.Add(Project.GetRelativePath(list[i].GetFullPath()));
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
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    TextObject xdbObject = TextObject.Load(openFileDialog.FileName);
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
            if (listView.SelectedIndices[0] >= 0)
            {
                list.RemoveAt(listView.SelectedIndices[0]);
                UpdateList();
            }
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            int idx = listView.SelectedIndices[0];
            if (idx > 0)
            {
                TextObject a = list[idx];
                list[idx] = list[idx - 1];
                list[idx - 1] = a;
                UpdateList();
                listView.Items[idx - 1].Selected = true;
            }
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            int idx = listView.SelectedIndices[0];
            if (idx >= 0 && idx < list.Count - 1)
            {
                TextObject a = list[idx];
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
