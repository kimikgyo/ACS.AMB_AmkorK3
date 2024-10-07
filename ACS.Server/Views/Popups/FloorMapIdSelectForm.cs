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
    public partial class FloorMapIdSelectForm : Form
    {
        private IUnitOfWork uow;
        private string inputValue = string.Empty;

        public FloorMapIdSelectForm(IUnitOfWork uow)
        {
            InitializeComponent();
            this.uow = uow;
            subFunc_cbo_FloorMapId_Select_ListAdd();
        }

        public string InputValue
        {
            get { return inputValue; }
            set { inputValue = value; }
        }

        void subFunc_cbo_FloorMapId_Select_ListAdd()
        {
            try
            {
                cbo_FloorMapId_Select.Items.Clear();

                var floorMapIDConfig = uow.FloorMapIDConfigs.Find(m => m.FloorName != "None" && m.FloorName != "none");
                foreach (var floorMapID in floorMapIDConfig)
                {
                    cbo_FloorMapId_Select.Items.Add(floorMapID.FloorName);

                }
            }
            catch
            {
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (cbo_FloorMapId_Select.Text.Length > 0)
            {
                this.inputValue = cbo_FloorMapId_Select.Text;
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
