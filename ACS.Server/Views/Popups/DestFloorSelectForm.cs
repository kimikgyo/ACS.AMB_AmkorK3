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
    public partial class DestFloorSelectForm : Form
    {
        private IUnitOfWork uow;
        private string inputValue = string.Empty;
        private string PositionGroup = null;
        public DestFloorSelectForm(string group,IUnitOfWork uow)
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
                cbo_DestFloor_Select.Items.Clear();
                cbo_DestFloor_Select.Items.Add("None");

                if (PositionGroup != null)
                {
                    var Floors = uow.FloorMapIDConfigs.GetAll();

                    foreach (var floor in Floors)
                    {
                        cbo_DestFloor_Select.Items.Add($"{floor.FloorName}");
                    }
                }
            }
            catch
            {

            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (cbo_DestFloor_Select.Text.Length > 0)
            {
                this.inputValue = cbo_DestFloor_Select.Text;
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
