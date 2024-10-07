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
    public partial class ErrorMissionSelectForm : Form
    {
        private MainForm mainForm;

        private string inputValue = string.Empty;
        private string drawNo = string.Empty;

        public ErrorMissionSelectForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            subFunc_cbo_ErrorMission_Select_ListAdd();
        }

        public string InputValue
        {
            get { return inputValue; }
            set { inputValue = value; }
        }

        void subFunc_cbo_ErrorMission_Select_ListAdd()
        {
            try
            {
                cbo_ErrorMission_Select.Items.Clear();
                cbo_ErrorMission_Select.Items.Add("None");

                if (mainForm.bFleetConnected == true)
                {
                    var missionNames = mainForm.GetMissions
                                        .Select(x => x.name)
                                        .Where(name => name.Contains("Error"))
                                        .ToList();

                    foreach (var missionName in missionNames)
                    {
                        cbo_ErrorMission_Select.Items.Add(missionName);
                    }
                }
            }
            catch
            {
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (cbo_ErrorMission_Select.Text.Length > 0)
            {
                this.inputValue = cbo_ErrorMission_Select.Text;
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
