using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace INA_ACS_Server.UI
{
    public partial class UseSelectForm : Form
    {
        private string inputValue = string.Empty;
        private string drawNo = string.Empty;

        public UseSelectForm()
        {
            InitializeComponent();
        }

        public string InputValue
        {
            get { return inputValue; }
            set { inputValue = value; }
        }

        private void Use_Select(object sender, EventArgs e)
        {
            string Use_Select = ((Button)sender).Text;
            this.inputValue = Use_Select;
            DialogResult = DialogResult.OK;
        }
    }
}
