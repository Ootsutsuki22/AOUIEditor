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
    public partial class PropertiesForm : DockForm
    {
        private static PropertiesForm Instance;

        public PropertiesForm()
        {
            InitializeComponent();

            propertyGrid1.Dock = DockStyle.Fill;
            propertyGrid1.Name = "propertyGrid";
            //propertyGrid1.PropertySort = PropertySort.NoSort;
            propertyGrid1.PropertyValueChanged += PropertyGrid_PropertyValueChanged;

            label1.Text = "";

            Instance = this;
        }

        private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Project.UpdateInfo();
        }

        public static void RefreshGrid()
        {
            Instance.propertyGrid1.Refresh();
        }

        public static object SelectedObject
        {
            get { return Instance.propertyGrid1.SelectedObject; }
            set
            {
                Instance.propertyGrid1.SelectedObject = value;
                if (value != null)
                {
                    Instance.label1.Text = value.ToString();
                }
                else
                {
                    Instance.label1.Text = "";
                }
            }
        }
    }
}
