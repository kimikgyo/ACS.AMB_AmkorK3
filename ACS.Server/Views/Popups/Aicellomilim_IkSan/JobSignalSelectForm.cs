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
    public partial class JobSignalSelectForm : Form
    {
        private MainForm mainForm;

        private string inputValue = string.Empty;
        private string drawNo = string.Empty;

        public JobSignalSelectForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            subFunc_cbo_JobSignal_Select_ListAdd();
        }

        public string InputValue
        {
            get { return inputValue; }
            set { inputValue = value; }
        }

        void subFunc_cbo_JobSignal_Select_ListAdd()
        {
            try
            {
                cbo_JobSignal_Select.Items.Clear();
                cbo_JobSignal_Select.Items.Add("None");
               
                //cbo_JobSignal_Select.Items.Add("Executing");
                //cbo_JobSignal_Select.Items.Add("Cancel");

                for (int i = 0; i <= 5; i++)
                {
                    cbo_JobSignal_Select.Items.Add($"{i}");
                }

            }
            catch
            {
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (cbo_JobSignal_Select.Text.Length > 0)
            {
                this.inputValue = cbo_JobSignal_Select.Text;
                DialogResult = DialogResult.OK;
            }
            else
            {
                mainForm.subFuncMessagePopUp("JobSignal 선택되지 않았습니다.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.inputValue = string.Empty;
            Close();
        }
    }
}
