using System;
using System.Windows.Forms;

namespace INA_ACS_Server
{
    public partial class RobotNameSelectForm : Form
    {
        private MainForm mainForm;
        private IUnitOfWork uow;
        private string inputValue = string.Empty;
        private string drawNo = string.Empty;

        public RobotNameSelectForm(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.uow = uow;
            subFunc_cbo_RobotName_Select_ListAdd();
        }

        public string InputValue
        {
            get { return inputValue; }
            set { inputValue = value; }
        }

        void subFunc_cbo_RobotName_Select_ListAdd()
        {
            try
            {
                cbo_RobotName_Select.Items.Clear();
                cbo_RobotName_Select.Items.Add("None");

                if (mainForm.bFleetConnected == true)
                {
                    foreach (var Robots in uow.Robots.GetAll())
                    {
                        {
                            cbo_RobotName_Select.Items.Add(Robots.RobotName);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (cbo_RobotName_Select.Text.Length > 0)
            {
                this.inputValue = cbo_RobotName_Select.Text;
                DialogResult = DialogResult.OK;
            }
            else
            {
                mainForm.subFuncMessagePopUp("RobotName 이 선택되지 않았습니다.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.inputValue = string.Empty;
            Close();
        }
    }
}
