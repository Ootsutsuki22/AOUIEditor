using Microsoft.Win32;
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
    public partial class NewAddonDialog : Form
    {
        public string AddonName { get; private set; }
        public string AddonsLocation { get; private set; }
        public bool AddForm { get; private set; }

        public NewAddonDialog()
        {
            InitializeComponent();

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\Аллоды Онлайн");
            if (key != null)
            {
                string regPath = (string)key.GetValue("InstallLocation");
                if (regPath != null)
                {
                    regPath = Path.Combine(regPath, @"data\Mods\Addons");
                    if (Directory.Exists(regPath))
                    {
                        locationTextBox.Text = regPath;
                    }
                }
            }
            else
            {
                key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\gcgame_0.359");
                if (key != null)
                {
                    string regPath = (string)key.GetValue("InstallLocation");
                    if (regPath != null)
                    {
                        regPath = Path.Combine(regPath, @"data\Mods\Addons");
                        if (Directory.Exists(regPath))
                        {
                            locationTextBox.Text = regPath;
                        }
                    }
                }
            }
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
            AddonName = nameTextBox.Text;
            AddonsLocation = locationTextBox.Text;
            AddForm = addFormCheckBox.Checked;
            if (string.IsNullOrEmpty(AddonName))
            {
                MessageBox.Show("Введите имя аддона!");
                return;
            }
            if (string.IsNullOrEmpty(AddonsLocation) || !Directory.Exists(AddonsLocation))
            {
                MessageBox.Show("Укажите существующую директорию!");
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
