namespace AOUIEditor
{
    partial class TreeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TreeForm));
            this.widgetTree = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // widgetTree
            // 
            this.widgetTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.widgetTree.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.widgetTree.HideSelection = false;
            this.widgetTree.ImageIndex = 0;
            this.widgetTree.ImageList = this.imageList1;
            this.widgetTree.Location = new System.Drawing.Point(0, 0);
            this.widgetTree.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.widgetTree.Name = "widgetTree";
            this.widgetTree.SelectedImageIndex = 0;
            this.widgetTree.Size = new System.Drawing.Size(433, 521);
            this.widgetTree.TabIndex = 0;
            this.widgetTree.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.widgetTree_DrawNode);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "CSApplication.png");
            this.imageList1.Images.SetKeyName(1, "FolderClosed.png");
            this.imageList1.Images.SetKeyName(2, "WindowsForm.png");
            this.imageList1.Images.SetKeyName(3, "Panel.png");
            this.imageList1.Images.SetKeyName(4, "Label.png");
            this.imageList1.Images.SetKeyName(5, "ButtonClick.png");
            this.imageList1.Images.SetKeyName(6, "TextBox.png");
            this.imageList1.Images.SetKeyName(7, "TextArea.png");
            this.imageList1.Images.SetKeyName(8, "ScrollBox.png");
            this.imageList1.Images.SetKeyName(9, "ListBox.png");
            this.imageList1.Images.SetKeyName(10, "ScrollViewer.png");
            this.imageList1.Images.SetKeyName(11, "Slider.png");
            this.imageList1.Images.SetKeyName(12, "TransformPositionCursor.png");
            // 
            // TreeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 521);
            this.Controls.Add(this.widgetTree);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TreeForm";
            this.Text = "Дерево виджетов";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView widgetTree;
        private System.Windows.Forms.ImageList imageList1;
    }
}