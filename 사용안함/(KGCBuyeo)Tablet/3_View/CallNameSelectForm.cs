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
    public partial class CallNameSelectForm : Form
    {
        private MainForm mainForm;
        private IUnitOfWork uow;
        private string inputValue = string.Empty;
        private string drawNo = string.Empty;

        public CallNameSelectForm(MainForm mainForm,IUnitOfWork uow)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.uow = uow;
            subFunc_cbo_Mission1_Select_ListAdd();
        }

        public string InputValue
        {
            get { return inputValue; }
            set { inputValue = value; }
        }

        void subFunc_cbo_Mission1_Select_ListAdd()
        {
            try
            {
                cbo_CallName_Select.Items.Clear();
                cbo_CallName_Select.Items.Add("None");

                foreach (var callName in uow.JobConfigs.GetAll())
                {
                    {
                        cbo_CallName_Select.Items.Add(callName.CallName);
                    }
                }
            }
            catch
            {
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (cbo_CallName_Select.Text.Length > 0)
            {
                this.inputValue = cbo_CallName_Select.Text;
                DialogResult = DialogResult.OK;
            }
            else
            {
                mainForm.subFuncMessagePopUp("Mission이 선택되지 않았습니다.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.inputValue = string.Empty;
            Close();
        }
    }
}
