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
    public partial class PositionZoneSelectForm : Form
    {
        private IUnitOfWork uow;
        private string inputValue = string.Empty;
        private string PositionGroup = null;
        public PositionZoneSelectForm(string group,IUnitOfWork uow)
        {
            InitializeComponent();
            this.uow = uow;
            this.PositionGroup = group;
            subFunc_cbo_PositionZone_Select_ListAdd();
        }

        public string InputValue
        {
            get { return inputValue; }
            set { inputValue = value; }
        }

        void subFunc_cbo_PositionZone_Select_ListAdd()
        {
            try
            {
                
                cbo_PositionZone_Select.Items.Clear();
                cbo_PositionZone_Select.Items.Add("None");

                if (PositionGroup != null)
                {
                    var PositionZone = uow.PositionAreaConfigs.Find(m => m.ACSRobotGroup == PositionGroup && m.PositionAreaName != "None" && m.PositionAreaName != "none" && m.PositionAreaUse == "Use");
                    foreach (var positionZone in PositionZone)
                    {
                        cbo_PositionZone_Select.Items.Add($"{positionZone.PositionAreaName}");

                    }
                }
                else
                {
                    var PositionZone = uow.PositionAreaConfigs.Find(m =>m.PositionAreaName != "None" && m.PositionAreaName != "none" && m.PositionAreaUse == "Use");
                    foreach (var positionZone in PositionZone)
                    {
                        cbo_PositionZone_Select.Items.Add($"{positionZone.PositionAreaName}");

                    }
                }

               
            }
            catch
            {
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (cbo_PositionZone_Select.Text.Length > 0)
            {
                this.inputValue = cbo_PositionZone_Select.Text;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("PositionZone이 선택되지 않았습니다.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.inputValue = string.Empty;
            Close();
        }
    }
}
