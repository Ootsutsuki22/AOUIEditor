using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AOUIEditor
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            Version version = Assembly.GetEntryAssembly().GetName().Version;
            label2.Text = "Версия: " + version.ToString();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var destinationurl = "https://github.com/Ootsutsuki22/AOUIEditor";
                var sInfo = new ProcessStartInfo(destinationurl) { UseShellExecute = true };
                Process.Start(sInfo);
            }
            catch
            { }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var destinationurl = "https://alloder.pro/";
                var sInfo = new ProcessStartInfo(destinationurl) { UseShellExecute = true };
                Process.Start(sInfo);
            }
            catch
            { }
        }
    }
}
