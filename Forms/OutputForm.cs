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
    public partial class OutputForm : DockForm
    {
        public OutputForm()
        {
            InitializeComponent();
        }

        public void Log(string message, Color? color = null)
        {
            Color c = color ?? richTextBox1.ForeColor;
            richTextBox1.AppendText(DateTime.Now + " " + message + Environment.NewLine + Environment.NewLine, c);
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        public void Clear()
        {
            richTextBox1.Clear();
        }
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
