namespace AOUIEditor
{
    partial class NewWidgetDialog
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("WidgetForm");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("WidgetPanel");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("WidgetButton");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("WidgetTextView");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("WidgetEditLine");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("WidgetEditBox");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("WidgetScrollableContainer");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("WidgetTextContainer");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("WidgetDiscreteScrollBar");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("WidgetGlideScrollBar");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("WidgetDiscreteSlider");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("WidgetGlideSlider");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("WidgetControl3D");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewWidgetDialog));
            this.widgetTypeTree = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.protoListView = new System.Windows.Forms.ListView();
            this.protoImageList = new System.Windows.Forms.ImageList(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.locationTextBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.protoTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.makePrototypeCheckbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // widgetTypeTree
            // 
            this.widgetTypeTree.HideSelection = false;
            this.widgetTypeTree.ImageIndex = 0;
            this.widgetTypeTree.ImageList = this.imageList1;
            this.widgetTypeTree.Location = new System.Drawing.Point(14, 36);
            this.widgetTypeTree.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.widgetTypeTree.Name = "widgetTypeTree";
            treeNode1.ImageKey = "WindowsForm.png";
            treeNode1.Name = "WidgetForm";
            treeNode1.SelectedImageKey = "WindowsForm.png";
            treeNode1.Text = "WidgetForm";
            treeNode2.ImageKey = "Panel.png";
            treeNode2.Name = "WidgetPanel";
            treeNode2.SelectedImageKey = "Panel.png";
            treeNode2.Text = "WidgetPanel";
            treeNode3.ImageKey = "ButtonClick.png";
            treeNode3.Name = "WidgetButton";
            treeNode3.SelectedImageKey = "ButtonClick.png";
            treeNode3.Text = "WidgetButton";
            treeNode4.ImageKey = "Label.png";
            treeNode4.Name = "WidgetTextView";
            treeNode4.SelectedImageKey = "Label.png";
            treeNode4.Text = "WidgetTextView";
            treeNode5.ImageKey = "TextBox.png";
            treeNode5.Name = "WidgetEditLine";
            treeNode5.SelectedImageKey = "TextBox.png";
            treeNode5.Text = "WidgetEditLine";
            treeNode6.ImageKey = "TextArea.png";
            treeNode6.Name = "WidgetEditBox";
            treeNode6.SelectedImageKey = "TextArea.png";
            treeNode6.Text = "WidgetEditBox";
            treeNode7.ImageKey = "ScrollViewer.png";
            treeNode7.Name = "WidgetScrollableContainer";
            treeNode7.SelectedImageKey = "ScrollViewer.png";
            treeNode7.Text = "WidgetScrollableContainer";
            treeNode8.ImageKey = "ListBox.png";
            treeNode8.Name = "WidgetTextContainer";
            treeNode8.SelectedImageKey = "ListBox.png";
            treeNode8.Text = "WidgetTextContainer";
            treeNode9.ImageKey = "ScrollBox.png";
            treeNode9.Name = "WidgetDiscreteScrollBar";
            treeNode9.SelectedImageKey = "ScrollBox.png";
            treeNode9.Text = "WidgetDiscreteScrollBar";
            treeNode10.ImageKey = "ScrollBox.png";
            treeNode10.Name = "WidgetGlideScrollBar";
            treeNode10.SelectedImageKey = "ScrollBox.png";
            treeNode10.Text = "WidgetGlideScrollBar";
            treeNode11.ImageKey = "Slider.png";
            treeNode11.Name = "WidgetDiscreteSlider";
            treeNode11.SelectedImageKey = "Slider.png";
            treeNode11.Text = "WidgetDiscreteSlider";
            treeNode12.ImageKey = "Slider.png";
            treeNode12.Name = "WidgetGlideSlider";
            treeNode12.SelectedImageKey = "Slider.png";
            treeNode12.Text = "WidgetGlideSlider";
            treeNode13.ImageKey = "TransformPositionCursor.png";
            treeNode13.Name = "WidgetControl3D";
            treeNode13.SelectedImageKey = "TransformPositionCursor.png";
            treeNode13.Text = "WidgetControl3D";
            this.widgetTypeTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11,
            treeNode12,
            treeNode13});
            this.widgetTypeTree.SelectedImageIndex = 0;
            this.widgetTypeTree.Size = new System.Drawing.Size(282, 756);
            this.widgetTypeTree.TabIndex = 0;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.TabIndex = 110;
            this.label1.Text = "Тип виджета:";
            // 
            // protoListView
            // 
            this.protoListView.LargeImageList = this.protoImageList;
            this.protoListView.Location = new System.Drawing.Point(328, 155);
            this.protoListView.MultiSelect = false;
            this.protoListView.Name = "protoListView";
            this.protoListView.Size = new System.Drawing.Size(995, 345);
            this.protoListView.TabIndex = 1;
            this.protoListView.UseCompatibleStateImageBehavior = false;
            // 
            // protoImageList
            // 
            this.protoImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.protoImageList.ImageSize = new System.Drawing.Size(100, 100);
            this.protoImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(401, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 20);
            this.label2.TabIndex = 101;
            this.label2.Text = "Имя:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(328, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 20);
            this.label3.TabIndex = 102;
            this.label3.Text = "Расположение:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(456, 37);
            this.nameTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(736, 27);
            this.nameTextBox.TabIndex = 103;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(1209, 72);
            this.browseButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(114, 31);
            this.browseButton.TabIndex = 104;
            this.browseButton.Text = "Обзор...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // locationTextBox
            // 
            this.locationTextBox.Location = new System.Drawing.Point(456, 76);
            this.locationTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.locationTextBox.Name = "locationTextBox";
            this.locationTextBox.Size = new System.Drawing.Size(736, 27);
            this.locationTextBox.TabIndex = 105;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(1206, 761);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(117, 31);
            this.cancelButton.TabIndex = 106;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(1083, 761);
            this.okButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(117, 31);
            this.okButton.TabIndex = 107;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // protoTextBox
            // 
            this.protoTextBox.Location = new System.Drawing.Point(456, 517);
            this.protoTextBox.Name = "protoTextBox";
            this.protoTextBox.ReadOnly = true;
            this.protoTextBox.Size = new System.Drawing.Size(867, 27);
            this.protoTextBox.TabIndex = 111;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(328, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(151, 20);
            this.label4.TabIndex = 112;
            this.label4.Text = "Выберите прототип:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(324, 520);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 20);
            this.label5.TabIndex = 113;
            this.label5.Text = "Файл прототипа:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(324, 566);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(999, 154);
            this.richTextBox1.TabIndex = 114;
            this.richTextBox1.Text = "";
            // 
            // makePrototypeCheckbox
            // 
            this.makePrototypeCheckbox.AutoSize = true;
            this.makePrototypeCheckbox.Location = new System.Drawing.Point(324, 737);
            this.makePrototypeCheckbox.Name = "makePrototypeCheckbox";
            this.makePrototypeCheckbox.Size = new System.Drawing.Size(608, 24);
            this.makePrototypeCheckbox.TabIndex = 115;
            this.makePrototypeCheckbox.Text = "Сделать создаваемый виджет также прототипом (<isPrototype>true</isPrototype>)";
            this.makePrototypeCheckbox.UseVisualStyleBackColor = true;
            // 
            // NewWidgetDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1342, 805);
            this.Controls.Add(this.makePrototypeCheckbox);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.protoTextBox);
            this.Controls.Add(this.protoListView);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.locationTextBox);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.widgetTypeTree);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewWidgetDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NewWidgetDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView widgetTypeTree;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.TextBox locationTextBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ListView protoListView;
        private System.Windows.Forms.ImageList protoImageList;
        private System.Windows.Forms.TextBox protoTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox makePrototypeCheckbox;
    }
}