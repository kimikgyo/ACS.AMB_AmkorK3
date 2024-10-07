using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace INA_ACS_Server.UI
{
    public partial class ElevatorModeSelectForm : Form
    {
        private string inputValue = string.Empty;
        private string drawNo = string.Empty;

        public ElevatorModeSelectForm()
        {
            InitializeComponent();
            subFunc_cbo_ElevatorMode_Select_ListAdd();
        }

        public string InputValue
        {
            get { return inputValue; }
            set { inputValue = value; }
        }

        void subFunc_cbo_ElevatorMode_Select_ListAdd()
        {
            try
            {
                cbo_ElevatorMode_Select.Items.Clear();
                cbo_ElevatorMode_Select.Items.Add("AGVMode");
                cbo_ElevatorMode_Select.Items.Add("NotAGVMode");
            }
            catch
            {
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (cbo_ElevatorMode_Select.Text.Length > 0)
            {
                this.inputValue = cbo_ElevatorMode_Select.Text;
                DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.inputValue = string.Empty;
            Close();
        }
    }
}
