namespace AOUIEditor
{
    partial class EditorForm
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
            this.monoControl1 = new AOUIEditor.MonoControl();
            this.SuspendLayout();
            // 
            // monoControl1
            // 
            this.monoControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.monoControl1.Location = new System.Drawing.Point(0, 0);
            this.monoControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.monoControl1.Name = "monoControl1";
            this.monoControl1.Size = new System.Drawing.Size(914, 600);
            this.monoControl1.TabIndex = 0;
            this.monoControl1.Text = "monoControl1";
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 600);
            this.Controls.Add(this.monoControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "EditorForm";
            this.Text = "Редактор";
            this.ResumeLayout(false);

        }

        #endregion

        private MonoControl monoControl1;
    }
}