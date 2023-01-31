using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AOUIEditor
{
    // Имеются два ужасных костыля, придёт озарение - исправлю как-нибудь :)
    public partial class TreeForm : DockForm
    {
        private static TreeForm Instance;
        private TreeNode selectedNode;

        public event EventHandler OnSelectChanged;

        Font boldFont;
        Font italicFont;
        Font boldItalicFont;

        public TreeForm()
        {
            InitializeComponent();

            widgetTree.AfterSelect += TreeView1_AfterSelect;

            boldFont = new Font(widgetTree.Font, FontStyle.Bold);
            italicFont = new Font(widgetTree.Font, FontStyle.Italic);
            boldItalicFont = new Font(widgetTree.Font, FontStyle.Bold | FontStyle.Italic);

            Instance = this;
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            OnSelectChanged?.Invoke(e.Node.Tag, EventArgs.Empty);
        }

        public static void RefreshTree()
        {
            Instance.RecreateTree();
        }

        private void RecreateTree()
        {
            selectedNode = null;
            widgetTree.BeginUpdate();
            widgetTree.Nodes.Clear();
            if (Project.Addon != null && Item.rootItem != null)
            {
                widgetTree.Nodes.Add(new TreeNode() { Text = Project.Addon.Name + " (UIAddon)", Tag = Project.Addon });
                widgetTree.Nodes[0].ImageKey = GetImageKeyByType("UIAddon");
                widgetTree.Nodes[0].SelectedImageKey = widgetTree.Nodes[0].ImageKey;
                if (Item.rootItem.children != null)
                {
                    for (int i = 0; i < Item.rootItem.children.Count; i++)
                    {
                        AddTreeNode(widgetTree.Nodes[0], Item.rootItem.children[i]);
                    }
                }
                widgetTree.ExpandAll();
                if (selectedNode != null)
                {
                    widgetTree.SelectedNode = selectedNode;
                    selectedNode.EnsureVisible();
                }
                else
                {
                    widgetTree.Nodes[0].EnsureVisible();
                }
            }
            widgetTree.EndUpdate();
        }

        private void AddTreeNode(TreeNode parentNode, Item item)
        {
            int idx = parentNode.Nodes.Add(new TreeNode());
            string name;
            if (string.IsNullOrEmpty(item.widget.Name))
            {
                name = item.widget.GetType().Name;
            }
            else
            {
                name = item.widget.Name;
            }
            parentNode.Nodes[idx].Tag = item;
            parentNode.Nodes[idx].ImageKey = GetImageKeyByType(item.widget.GetType().Name);
            parentNode.Nodes[idx].SelectedImageKey = parentNode.Nodes[idx].ImageKey;
            if (item.hasPrototype && !item.directChild)
            {
                parentNode.Nodes[idx].NodeFont = boldItalicFont;
            }
            else if (item.hasPrototype)
            {
                parentNode.Nodes[idx].NodeFont = boldFont;
            }
            else if (!item.directChild)
            {
                parentNode.Nodes[idx].NodeFont = italicFont;
            }
            if (item.isPrototypeChild)
            {
                parentNode.Nodes[idx].ForeColor = Color.Green;
            }
            if (!item.directChild)
                name = "[" + name + "]";
            parentNode.Nodes[idx].Text = name;// + " ";

            // Костыль, так мы можем выбрать вообще не тот элемент, ведь это указатель на конкретный файл виджета, которых в дереве может быть много
            if (item.widget == PropertiesForm.SelectedObject)
            {
                selectedNode = parentNode.Nodes[idx];
            }
            if (item.children != null)
            {
                for (int i = 0; i < item.children.Count; i++)
                {
                    AddTreeNode(parentNode.Nodes[idx], item.children[i]);
                }
            }
        }

        // Очень плохой костыль. Вся беда в том, что виджет, который мы выбираем мышкой ищется по сортированному списку
        // А в дереве объектов список несортированный и приходится искать заново...
        public static void SelectItem(Item item)
        {
            if (item == null)
                return;
            if (Instance.widgetTree.Nodes == null || Instance.widgetTree.Nodes[0] == null || Instance.widgetTree.Nodes[0].Nodes == null)
                return;
            for (int i = 0; i < Instance.widgetTree.Nodes[0].Nodes.Count; i++)
            {
                Instance.FindItem(Instance.widgetTree.Nodes[0].Nodes[i], item);
            }
        }

        private void FindItem(TreeNode node, Item item)
        {
            if (node.Tag == item)
            {
                widgetTree.SelectedNode = node;
            }
            else
            {
                if (node.Nodes != null)
                {
                    for (int i = 0; i < node.Nodes.Count; i++)
                    {
                        FindItem(node.Nodes[i], item);
                    }
                }
            }
        }

        private static string GetImageKeyByType(string type)
        {
            switch (type)
            {
                case "UIAddon":
                    return "CSApplication.png";
                case "WidgetForm":
                    return "WindowsForm.png";
                case "WidgetPanel":
                    return "Panel.png";
                case "WidgetTextView":
                    return "Label.png";
                case "WidgetButton":
                    return "ButtonClick.png";
                case "WidgetEditLine":
                    return "TextBox.png";
                case "WidgetEditBox":
                    return "TextArea.png";
                case "WidgetScrollableContainer":
                    return "ScrollViewer.png";
                case "WidgetTextContainer":
                    return "ListBox.png";
                case "WidgetControl3D":
                    return "TransformPositionCursor.png";
                case "WidgetDiscreteScrollBar":
                    return "ScrollBox.png";
                case "WidgetGlideScrollBar":
                    return "ScrollBox.png";
                case "WidgetDiscreteSlider":
                    return "Slider.png";
                case "WidgetGlideSlider":
                    return "Slider.png";
            }
            return "TransformPositionCursor.png";
        }

        private void widgetTree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node == null)
                return;

            var selected = (e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected;
            var font = e.Node.NodeFont ?? e.Node.TreeView.Font;
            var textSize = e.Graphics.MeasureString(e.Node.Text, font);
            var bounds = new Rectangle(e.Bounds.X, e.Bounds.Y, Math.Max(Convert.ToInt32(textSize.Width), e.Bounds.Width), Math.Max(Convert.ToInt32(textSize.Height), e.Bounds.Height));

            e.DrawDefault = false;

            if (selected)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, bounds);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, bounds, SystemColors.HighlightText, TextFormatFlags.GlyphOverhangPadding);
            }
            else
            {
                e.Graphics.FillRectangle(SystemBrushes.Window, bounds);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, bounds, e.Node.ForeColor, TextFormatFlags.GlyphOverhangPadding);
            }
        }
    }
}
