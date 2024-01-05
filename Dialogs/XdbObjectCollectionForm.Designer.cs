namespace AOUIEditor
{
    partial class XdbObjectCollectionForm
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
            addButton = new System.Windows.Forms.Button();
            removeButton = new System.Windows.Forms.Button();
            upButton = new System.Windows.Forms.Button();
            downButton = new System.Windows.Forms.Button();
            cancelButton = new System.Windows.Forms.Button();
            okButton = new System.Windows.Forms.Button();
            listView = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            SuspendLayout();
            // 
            // addButton
            // 
            addButton.Location = new System.Drawing.Point(630, 22);
            addButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            addButton.Name = "addButton";
            addButton.Size = new System.Drawing.Size(113, 22);
            addButton.TabIndex = 1;
            addButton.Text = "Добавить...";
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += addButton_Click;
            // 
            // removeButton
            // 
            removeButton.Location = new System.Drawing.Point(630, 52);
            removeButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            removeButton.Name = "removeButton";
            removeButton.Size = new System.Drawing.Size(113, 22);
            removeButton.TabIndex = 2;
            removeButton.Text = "Удалить";
            removeButton.UseVisualStyleBackColor = true;
            removeButton.Click += removeButton_Click;
            // 
            // upButton
            // 
            upButton.Location = new System.Drawing.Point(630, 126);
            upButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            upButton.Name = "upButton";
            upButton.Size = new System.Drawing.Size(113, 22);
            upButton.TabIndex = 3;
            upButton.Text = "Вверх";
            upButton.UseVisualStyleBackColor = true;
            upButton.Click += upButton_Click;
            // 
            // downButton
            // 
            downButton.Location = new System.Drawing.Point(630, 156);
            downButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            downButton.Name = "downButton";
            downButton.Size = new System.Drawing.Size(113, 22);
            downButton.TabIndex = 4;
            downButton.Text = "Вниз";
            downButton.UseVisualStyleBackColor = true;
            downButton.Click += downButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new System.Drawing.Point(661, 406);
            cancelButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(82, 22);
            cancelButton.TabIndex = 5;
            cancelButton.Text = "Отмена";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // okButton
            // 
            okButton.Location = new System.Drawing.Point(574, 406);
            okButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(82, 22);
            okButton.TabIndex = 6;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += okButton_Click;
            // 
            // listView
            // 
            listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.Location = new System.Drawing.Point(10, 9);
            listView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            listView.MultiSelect = false;
            listView.Name = "listView";
            listView.Size = new System.Drawing.Size(600, 372);
            listView.TabIndex = 7;
            listView.UseCompatibleStateImageBehavior = false;
            listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Id";
            columnHeader1.Width = 50;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Name";
            columnHeader2.Width = 150;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Location";
            columnHeader3.Width = 350;
            // 
            // XdbObjectCollectionForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(755, 449);
            Controls.Add(listView);
            Controls.Add(okButton);
            Controls.Add(cancelButton);
            Controls.Add(downButton);
            Controls.Add(upButton);
            Controls.Add(removeButton);
            Controls.Add(addButton);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "XdbObjectCollectionForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Xdb Collection Editor";
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}