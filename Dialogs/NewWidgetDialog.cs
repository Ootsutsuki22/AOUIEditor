using AOUIEditor.ResourceSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AOUIEditor
{
    public partial class NewWidgetDialog : Form
    {
        public string WidgetFilename { get; private set; }

        private string selectedType;
        private int selectedProtoIndex;

        private static string lastLocation;

        public NewWidgetDialog()
        {
            InitializeComponent();

            widgetTypeTree.ExpandAll();
            widgetTypeTree.AfterSelect += WidgetTypeTree_AfterSelect;
            protoListView.SelectedIndexChanged += ListView1_SelectedIndexChanged;

            widgetTypeTree.SelectedNode = widgetTypeTree.Nodes[0];

            if (!string.IsNullOrEmpty(lastLocation))
            {
                locationTextBox.Text = lastLocation;
            }
            else
            {
                locationTextBox.Text = Project.Location;
            }
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (protoListView.SelectedIndices.Count > 0)
            {
                selectedProtoIndex = protoListView.SelectedIndices[0];
                protoTextBox.Text = Prototype.Dictionary[selectedType].Prototypes[selectedProtoIndex].MainFile;
                richTextBox1.Text = Prototype.Dictionary[selectedType].Prototypes[selectedProtoIndex].Description;
            }
        }

        private void WidgetTypeTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            selectedType = e.Node.Text;
            protoListView.BeginUpdate();
            protoListView.Items.Clear();
            List<Prototype> list = Prototype.Dictionary[e.Node.Text].Prototypes;
            for (int i = 0; i < list.Count; i++)
            {
                ListViewItem item = new ListViewItem(list[i].Name);
                if (!protoImageList.Images.ContainsKey(list[i].Icon))
                {
                    string iconFile = Path.Combine(@"Widgets", list[i].Icon);
                    if (File.Exists(iconFile))
                    {
                        protoImageList.Images.Add(list[i].Icon, Image.FromFile(iconFile));
                    }
                }
                item.ImageKey = list[i].Icon;
                protoListView.Items.Add(item);
            }
            protoListView.Items[0].Selected = true;
            selectedProtoIndex = 0;
            protoListView.EndUpdate();
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    locationTextBox.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(locationTextBox.Text) || !Directory.Exists(locationTextBox.Text))
            {
                MessageBox.Show("Укажите существующую директорию!");
                return;
            }
            if (string.IsNullOrEmpty(nameTextBox.Text))
            {
                MessageBox.Show("Введите имя виджета!");
                return;
            }

            Type type = Type.GetType("AOUIEditor.ResourceSystem." + selectedType);
            string filename = Path.Combine(locationTextBox.Text, nameTextBox.Text) + ".(" + type.Name + ").xdb";
            if (File.Exists(filename))
            {
                MessageBox.Show($"Файл уже существует! '{filename}'");
                return;
            }

            XdbObject instance = Activator.CreateInstance(type) as XdbObject;
            Widget widget = instance as Widget;
            if (widget != null)
            {
                widget.Name = nameTextBox.Text;
            }
            instance.file = Path.GetFileName(filename);
            instance.directory = Path.GetDirectoryName(filename);

            if (selectedProtoIndex == 0)
            {
                if (instance.GetType() == typeof(WidgetForm))
                {
                    WidgetForm widgetForm = (WidgetForm)instance;
                    widgetForm.Priority = 4000;
                    widgetForm.Placement.X.Align = WidgetAlign.WIDGET_ALIGN_BOTH;
                    widgetForm.Placement.Y.Align = WidgetAlign.WIDGET_ALIGN_BOTH;
                    widgetForm.PickChildrenOnly = true;
                }
                if (instance.GetType() == typeof(WidgetTextView))
                {
                    string formatFile = Path.Combine(locationTextBox.Text, nameTextBox.Text) + ".txt";
                    if (File.Exists(formatFile))
                    {
                        MessageBox.Show($"Файл форматирования текста уже существует! '{formatFile}'");
                        return;
                    }
                    try
                    {
                        File.WriteAllText(formatFile, Prototype.Dictionary[selectedType].Prototypes[0].Format, Encoding.Unicode);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Не удалось сохранить файл форматирования: " + ex.ToString());
                        return;
                    }
                    WidgetTextView textView = (WidgetTextView)instance;
                    textView.FormatFileRef = new TextObject()
                    {
                        file = Path.GetFileName(formatFile),
                        directory= Path.GetDirectoryName(formatFile)
                    };
                    textView.Placement.X.Sizing = WidgetSizing.WIDGET_SIZING_INTERNAL;
                    textView.Placement.Y.Sizing = WidgetSizing.WIDGET_SIZING_INTERNAL;
                    textView.TransparentInput = true;
                }
                if (makePrototypeCheckbox.Checked)
                {
                    instance.Header.isPrototype = true;
                }
                instance.Save();
            }
            else
            {
                Prototype proto = Prototype.Dictionary[selectedType].Prototypes[selectedProtoIndex];
                if (string.IsNullOrEmpty(proto.MainFile))
                {
                    MessageBox.Show($"Прототип {selectedType}[{selectedProtoIndex}] содержит пустое значение MainFile. Проверьте Prototypes.json");
                    return;
                }
                string protoMainFile = Path.Combine("Widgets", proto.MainFile);
                if (!File.Exists(protoMainFile))
                {
                    MessageBox.Show($"Прототип {selectedType}[{selectedProtoIndex}] указывает на несуществующий файл MainFile. Проверьте Prototypes.json");
                    return;
                }
                else
                {
                    try
                    {
                        string mainFileLocation = Path.Combine(Project.Location, protoMainFile);

                        instance.Header.Prototype = new XdbObject();
                        instance.Header.Prototype.file = Path.GetFileName(mainFileLocation);
                        instance.Header.Prototype.directory = Path.GetDirectoryName(mainFileLocation);
                        if (makePrototypeCheckbox.Checked)
                        {
                            instance.Header.isPrototype = true;
                        }

                        // Костыль, сначала сохраняется пустой прототип, затем в этот же файл копируется реальный
                        // По другому не получится создать файл не загрузив сам прототип в редактор
                        instance.Save();
                        File.Copy(protoMainFile, mainFileLocation, true);

                        if (proto.Files != null)
                        {
                            for (int i = 0; i < proto.Files.Count; i++)
                            {
                                if (string.IsNullOrEmpty(proto.Files[i]))
                                {
                                    MessageBox.Show($"Прототип {selectedType}[{selectedProtoIndex}] содержит пустое значение Files[{i}]. Этот файл не был скопирован Проверьте Prototypes.json");
                                    continue;
                                }
                                string protoFile = Path.Combine("Widgets", proto.Files[i]);
                                if (!File.Exists(protoFile))
                                {
                                    MessageBox.Show($"Прототип {selectedType}[{selectedProtoIndex}] указывает на несуществующий файл Files[{i}]. Этот файл не был скопирован Проверьте Prototypes.json");
                                    continue;
                                }
                                string fileLocation = Path.Combine(Project.Location, protoFile);
                                if (!Directory.Exists(Path.GetDirectoryName(fileLocation)))
                                    Directory.CreateDirectory(Path.GetDirectoryName(fileLocation));
                                File.Copy(protoFile, fileLocation, true);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Не удалось скопировать прототип и создать виджет: " + ex.ToString());
                        return;
                    }
                }
            }

            lastLocation = locationTextBox.Text;
            WidgetFilename = filename;
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        public static void Clear()
        {
            lastLocation = null;
        }
    }
}
