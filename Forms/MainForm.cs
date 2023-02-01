using AOUIEditor.ResourceSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using WeifenLuo.WinFormsUI.Docking;

namespace AOUIEditor
{
    public partial class MainForm : Form
    {
        DockPanel dockPanel;

        EditorForm editorForm;
        PropertiesForm propertiesForm;
        TreeForm treeForm;
        OutputForm outputForm;

        public MainForm()
        {
            InitializeComponent();

            Prototype.Init();

            Version version = Assembly.GetEntryAssembly().GetName().Version;
            versionLabel.Text = "v" + version.ToString();

            ResolutionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ResolutionComboBox.SelectedIndex = 0;
            ResolutionComboBox.SelectedIndexChanged += ResolutionComboBox_SelectedIndexChanged;

            toolStrip2.Enabled = false;
            toolStrip3.Enabled = false;

            dockPanel = new DockPanel();
            dockPanel.Theme = new VS2015BlueTheme();
            dockPanel.Dock = DockStyle.Fill;
            panel1.Controls.Add(dockPanel);

            editorForm = new EditorForm();
            editorForm.DockAreas = DockAreas.Document | DockAreas.Float;
            editorForm.OnHide += EditorForm_OnHide;
            editorForm.Show(dockPanel, DockState.Document);

            treeForm = new TreeForm();
            treeForm.OnHide += TreeForm_OnHide;
            treeForm.Show(dockPanel, DockState.DockRight);

            propertiesForm = new PropertiesForm();
            propertiesForm.OnHide += PropertiesForm_OnHide;
            propertiesForm.Show(treeForm.Pane, DockAlignment.Bottom, 0.6);

            outputForm = new OutputForm();
            outputForm.OnHide += OutputForm_OnHide;
            outputForm.Show(dockPanel, DockState.DockBottom);

            dockPanel.UpdateDockWindowZOrder(DockStyle.Right, true);

            Logger.output = outputForm;
            treeForm.OnSelectChanged += TreeForm_OnSelectChanged;
            //WindowState = FormWindowState.Maximized;

            editorForm.Activate();
        }

        #region Menu Strip

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewProject();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenProject();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProject();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void addNewWidgetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateWidget();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutForm form = new AboutForm())
            {
                form.ShowDialog();
            }
        }

        #endregion

        #region Tool Strip

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            NewProject();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenProject();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveProject();
        }

        private void addNewToolStripButton_Click(object sender, EventArgs e)
        {
            CreateWidget();
        }

        private void ResolutionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MonoControl.Instance.Resolution = ResolutionComboBox.SelectedIndex;
        }

        private void xRayToolStripButton_Click(object sender, EventArgs e)
        {
            MonoControl.Instance.XRay = !MonoControl.Instance.XRay;
            xRayToolStripButton.Checked = MonoControl.Instance.XRay;
        }

        #endregion

        #region Placement toolstrips

        private void toolStripPosX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (Item.selectedItem != null)
                {
                    Item.selectedItem.widget.Placement.X.Pos = GetFloat(toolStripPosX.Text);
                    Project.UpdateInfo();
                }
            }
        }

        private void toolStripPosY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (Item.selectedItem != null)
                {
                    Item.selectedItem.widget.Placement.Y.Pos = GetFloat(toolStripPosY.Text);
                    Project.UpdateInfo();
                }
            }
        }

        private void toolStripSizeX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (Item.selectedItem != null)
                {
                    Item.selectedItem.widget.Placement.X.Size = GetFloat(toolStripSizeX.Text);
                    Project.UpdateInfo();
                }
            }
        }

        private void toolStripSizeY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (Item.selectedItem != null)
                {
                    Item.selectedItem.widget.Placement.Y.Size = GetFloat(toolStripSizeY.Text);
                    Project.UpdateInfo();
                }
            }
        }

        private void toolStripHighPosX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (Item.selectedItem != null)
                {
                    Item.selectedItem.widget.Placement.X.HighPos = GetFloat(toolStripHighPosX.Text);
                    Project.UpdateInfo();
                }
            }
        }

        private void toolStripHighPosY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (Item.selectedItem != null)
                {
                    Item.selectedItem.widget.Placement.Y.HighPos = GetFloat(toolStripHighPosY.Text);
                    Project.UpdateInfo();
                }
            }
        }

        private void alignLowX_Click(object sender, EventArgs e)
        {
            if (Item.selectedItem != null)
            {
                Item.selectedItem.widget.Placement.X.Align = WidgetAlign.WIDGET_ALIGN_LOW;
                Project.UpdateInfo();
            }
        }

        private void alignCenterX_Click(object sender, EventArgs e)
        {
            if (Item.selectedItem != null)
            {
                Item.selectedItem.widget.Placement.X.Align = WidgetAlign.WIDGET_ALIGN_CENTER;
                Project.UpdateInfo();
            }
        }

        private void alignHighX_Click(object sender, EventArgs e)
        {
            if (Item.selectedItem != null)
            {
                Item.selectedItem.widget.Placement.X.Align = WidgetAlign.WIDGET_ALIGN_HIGH;
                Project.UpdateInfo();
            }
        }

        private void alignBothX_Click(object sender, EventArgs e)
        {
            if (Item.selectedItem != null)
            {
                Item.selectedItem.widget.Placement.X.Align = WidgetAlign.WIDGET_ALIGN_BOTH;
                Project.UpdateInfo();
            }
        }

        private void alignAbsX_Click(object sender, EventArgs e)
        {
            if (Item.selectedItem != null)
            {
                Item.selectedItem.widget.Placement.X.Align = WidgetAlign.WIDGET_ALIGN_LOW_ABS;
                Project.UpdateInfo();
            }
        }

        private void alignLowY_Click(object sender, EventArgs e)
        {
            if (Item.selectedItem != null)
            {
                Item.selectedItem.widget.Placement.Y.Align = WidgetAlign.WIDGET_ALIGN_LOW;
                Project.UpdateInfo();
            }
        }

        private void alignCenterY_Click(object sender, EventArgs e)
        {
            if (Item.selectedItem != null)
            {
                Item.selectedItem.widget.Placement.Y.Align = WidgetAlign.WIDGET_ALIGN_CENTER;
                Project.UpdateInfo();
            }
        }

        private void alignHighY_Click(object sender, EventArgs e)
        {
            if (Item.selectedItem != null)
            {
                Item.selectedItem.widget.Placement.Y.Align = WidgetAlign.WIDGET_ALIGN_HIGH;
                Project.UpdateInfo();
            }
        }

        private void alignBothY_Click(object sender, EventArgs e)
        {
            if (Item.selectedItem != null)
            {
                Item.selectedItem.widget.Placement.Y.Align = WidgetAlign.WIDGET_ALIGN_BOTH;
                Project.UpdateInfo();
            }
        }

        private void alignAbsY_Click(object sender, EventArgs e)
        {
            if (Item.selectedItem != null)
            {
                Item.selectedItem.widget.Placement.Y.Align = WidgetAlign.WIDGET_ALIGN_LOW_ABS;
                Project.UpdateInfo();
            }
        }

        private void sizingDefaultX_Click(object sender, EventArgs e)
        {
            if (Item.selectedItem != null)
            {
                Item.selectedItem.widget.Placement.X.Sizing = WidgetSizing.WIDGET_SIZING_DEFAULT;
                Project.UpdateInfo();
            }
        }

        private void sizingInternalX_Click(object sender, EventArgs e)
        {
            if (Item.selectedItem != null)
            {
                Item.selectedItem.widget.Placement.X.Sizing = WidgetSizing.WIDGET_SIZING_INTERNAL;
                Project.UpdateInfo();
            }
        }

        private void sizingChildrenX_Click(object sender, EventArgs e)
        {
            if (Item.selectedItem != null)
            {
                Item.selectedItem.widget.Placement.X.Sizing = WidgetSizing.WIDGET_SIZING_CHILDREN;
                Project.UpdateInfo();
            }
        }

        private void sizingDefaultY_Click(object sender, EventArgs e)
        {
            if (Item.selectedItem != null)
            {
                Item.selectedItem.widget.Placement.Y.Sizing = WidgetSizing.WIDGET_SIZING_DEFAULT;
                Project.UpdateInfo();
            }
        }

        private void sizingInternalY_Click(object sender, EventArgs e)
        {
            if (Item.selectedItem != null)
            {
                Item.selectedItem.widget.Placement.Y.Sizing = WidgetSizing.WIDGET_SIZING_INTERNAL;
                Project.UpdateInfo();
            }
        }

        private void sizingChildrenY_Click(object sender, EventArgs e)
        {
            if (Item.selectedItem != null)
            {
                Item.selectedItem.widget.Placement.Y.Sizing = WidgetSizing.WIDGET_SIZING_CHILDREN;
                Project.UpdateInfo();
            }
        }

        private void toolStripPosX_Leave(object sender, EventArgs e)
        {
            UpdatePlacementToolstrips();
        }

        #endregion

        private void NewProject()
        {
            using (NewAddonDialog newAddonDualog = new NewAddonDialog())
            {
                if (newAddonDualog.ShowDialog() == DialogResult.OK)
                {
                    Project.New(newAddonDualog.AddonName, newAddonDualog.AddonsLocation, newAddonDualog.AddForm);
                    MonoControl.Instance.ResetCamera();
                    Item.FillData();
                    TreeForm.RefreshTree();
                    Item.Sort();
                    addNewToolStripButton.Enabled = true;
                    this.Text = "AOUIEditor - " + Project.Addon.Name;
                }
            }
        }

        private void OpenProject()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "AddonDesc.(UIAddon).xdb (*.xdb)|*.xdb";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Project.Open(openFileDialog.FileName);
                    MonoControl.Instance.ResetCamera();
                    Item.FillData();
                    TreeForm.RefreshTree();
                    Item.Sort();
                    addNewToolStripButton.Enabled = true;
                    this.Text = "AOUIEditor - " + Project.Addon.Name;
                }
            }
        }

        private void SaveProject()
        {
            Project.Save();
        }

        private void CreateWidget()
        {
            using (NewWidgetDialog newWidgetDialog = new NewWidgetDialog())
            {
                if (newWidgetDialog.ShowDialog() == DialogResult.OK)
                {
                    Logger.Log($"Виджет успешно создан!{Environment.NewLine}'{newWidgetDialog.WidgetFilename}");
                }
            }
        }

        private void TreeForm_OnSelectChanged(object sender, EventArgs e)
        {
            if (sender == Project.Addon)
            {
                Item.selectedItem = null;
                toolStrip2.Enabled = false;
                toolStrip3.Enabled = false;
                PropertiesForm.SelectedObject = sender;
            }
            else
            {
                Item.selectedItem = sender as Item;
                toolStrip2.Enabled = true;
                toolStrip3.Enabled = true;
                UpdatePlacementToolstrips();
            }
        }

        private void UpdatePlacementToolstrips()
        {
            if (Item.selectedItem == null)
                return;

            PropertiesForm.SelectedObject = Item.selectedItem.widget;

            WidgetPlacement itemPlacementX = Item.selectedItem.realPlacement.X;
            WidgetPlacement itemPlacementY = Item.selectedItem.realPlacement.Y;

            toolStripPosX.Text = itemPlacementX.Pos.ToString();
            toolStripSizeX.Text = itemPlacementX.Size.ToString();
            toolStripHighPosX.Text = itemPlacementX.HighPos.ToString();

            toolStripPosY.Text = itemPlacementY.Pos.ToString();
            toolStripSizeY.Text = itemPlacementY.Size.ToString();
            toolStripHighPosY.Text = itemPlacementY.HighPos.ToString();

            alignLowX.Checked = itemPlacementX.Align == WidgetAlign.WIDGET_ALIGN_LOW;
            alignCenterX.Checked = itemPlacementX.Align == WidgetAlign.WIDGET_ALIGN_CENTER;
            alignHighX.Checked = itemPlacementX.Align == WidgetAlign.WIDGET_ALIGN_HIGH;
            alignBothX.Checked = itemPlacementX.Align == WidgetAlign.WIDGET_ALIGN_BOTH;
            alignAbsX.Checked = itemPlacementX.Align == WidgetAlign.WIDGET_ALIGN_LOW_ABS;

            alignLowY.Checked = itemPlacementY.Align == WidgetAlign.WIDGET_ALIGN_LOW;
            alignCenterY.Checked = itemPlacementY.Align == WidgetAlign.WIDGET_ALIGN_CENTER;
            alignHighY.Checked = itemPlacementY.Align == WidgetAlign.WIDGET_ALIGN_HIGH;
            alignBothY.Checked = itemPlacementY.Align == WidgetAlign.WIDGET_ALIGN_BOTH;
            alignAbsY.Checked = itemPlacementY.Align == WidgetAlign.WIDGET_ALIGN_LOW_ABS;

            sizingDefaultX.Checked = itemPlacementX.Sizing == WidgetSizing.WIDGET_SIZING_DEFAULT;
            sizingInternalX.Checked = itemPlacementX.Sizing == WidgetSizing.WIDGET_SIZING_INTERNAL;
            sizingChildrenX.Checked = itemPlacementX.Sizing == WidgetSizing.WIDGET_SIZING_CHILDREN;

            sizingDefaultY.Checked = itemPlacementY.Sizing == WidgetSizing.WIDGET_SIZING_DEFAULT;
            sizingInternalY.Checked = itemPlacementY.Sizing == WidgetSizing.WIDGET_SIZING_INTERNAL;
            sizingChildrenY.Checked = itemPlacementY.Sizing == WidgetSizing.WIDGET_SIZING_CHILDREN;

            WidgetPlacement widgetPlacementX = Item.selectedItem.widget.Placement.X;
            WidgetPlacement widgetPlacementY = Item.selectedItem.widget.Placement.Y;

            toolStripPosX.BackColor = widgetPlacementX.Pos == null ? Color.MistyRose : SystemColors.Window;
            toolStripSizeX.BackColor = widgetPlacementX.Size == null ? Color.MistyRose : SystemColors.Window;
            toolStripHighPosX.BackColor = widgetPlacementX.HighPos == null ? Color.MistyRose : SystemColors.Window;

            toolStripPosY.BackColor = widgetPlacementY.Pos == null ? Color.MistyRose : SystemColors.Window;
            toolStripSizeY.BackColor = widgetPlacementY.Size == null ? Color.MistyRose : SystemColors.Window;
            toolStripHighPosY.BackColor = widgetPlacementY.HighPos == null ? Color.MistyRose : SystemColors.Window;
        }

        private void editorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorForm.Show();
            editorToolStripMenuItem.Checked = true;
        }

        private void widgetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeForm.Show();
            widgetsToolStripMenuItem.Checked = true;
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            propertiesForm.Show();
            propertiesToolStripMenuItem.Checked = true;
        }

        private void outputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            outputForm.Show();
            outputToolStripMenuItem.Checked = true;
        }

        private void EditorForm_OnHide(object sender, EventArgs e)
        {
            editorToolStripMenuItem.Checked = false;
        }

        private void TreeForm_OnHide(object sender, EventArgs e)
        {
            widgetsToolStripMenuItem.Checked = false;
        }

        private void PropertiesForm_OnHide(object sender, EventArgs e)
        {
            propertiesToolStripMenuItem.Checked = false;
        }

        private void OutputForm_OnHide(object sender, EventArgs e)
        {
            outputToolStripMenuItem.Checked = false;
        }

        private float? GetFloat(string text)
        {
            float value = 0f;
            if (float.TryParse(text, out value))
            {
                return value;
            }
            else if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                return value;
            }
            return null;
        }
    }
}
