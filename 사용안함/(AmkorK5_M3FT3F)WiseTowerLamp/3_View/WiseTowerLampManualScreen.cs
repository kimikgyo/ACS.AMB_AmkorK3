using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INA_ACS_Server
{
    public partial class WiseTowerLampManualScreen : Form
    {
        private readonly MainForm mainForm;
        private readonly IUnitOfWork uow;

        public WiseTowerLampManualScreen(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.uow = uow;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dtg_WiseTowerLampManual.AlternatingRowsDefaultCellStyle = null;
            dtg_WiseTowerLampManual.DoubleBuffered(true);
            WiseTowerLampManualInit();
            WiseTowerLampManualDisplay();

        }

        private void WiseTowerLampManualInit()
        {
            dtg_WiseTowerLampManual.ScrollBars = ScrollBars.Both;
            dtg_WiseTowerLampManual.AllowUserToResizeColumns = true;
            dtg_WiseTowerLampManual.ColumnHeadersHeight = 40;
            dtg_WiseTowerLampManual.RowTemplate.Height = 40;
            dtg_WiseTowerLampManual.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_WiseTowerLampManual.Columns[0].CellTemplate;
            dtg_WiseTowerLampManual.Columns.Clear();
            dtg_WiseTowerLampManual.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampManual_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_WiseTowerLampManual.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampManual_DisplayName", HeaderText = "TowerLampDisplayName", Width = 200, CellTemplate = currentCellTemplate });
            dtg_WiseTowerLampManual.Columns.Add(new DataGridViewButtonColumn() { Name = "DGV_WiseTowerLampManual_OutPutOn", HeaderText = "OutPutOn", Width = 100 });
            dtg_WiseTowerLampManual.Columns.Add(new DataGridViewButtonColumn() { Name = "DGV_WiseTowerLampManual_OutPutOff", HeaderText = "OutPutOff", Width = 100 });
            dtg_WiseTowerLampManual.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampManual_ModuleOutValue", HeaderText = "ModuleOutValue", Width = 130, CellTemplate = currentCellTemplate });
            dtg_WiseTowerLampManual.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_WiseTowerLampManual.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_WiseTowerLampManual.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_WiseTowerLampManual.Rows.Clear();
        }

        private void WiseTowerLampManualDisplay()
        {
            dtg_WiseTowerLampManual.Columns["DGV_WiseTowerLampManual_Id"].ReadOnly = true;
            dtg_WiseTowerLampManual.Columns["DGV_WiseTowerLampManual_DisplayName"].ReadOnly = true;

            int firstRowIndex = dtg_WiseTowerLampManual.FirstDisplayedScrollingColumnIndex;
            dtg_WiseTowerLampManual.Rows.Clear();
            dtg_WiseTowerLampManual.RowTemplate.Height = 40;

            foreach (var wiseTowerLampConfigs in uow.WiseTowerLampConfigs.GetAll())
            {
                int newRowIndex = dtg_WiseTowerLampManual.Rows.Add();
                var newRow = dtg_WiseTowerLampManual.Rows[newRowIndex];
                newRow.Cells["DGV_WiseTowerLampManual_Id"].Value = wiseTowerLampConfigs.Id.ToString();
                newRow.Cells["DGV_WiseTowerLampManual_DisplayName"].Value = wiseTowerLampConfigs.DisplayNameSetting;
                newRow.Cells["DGV_WiseTowerLampManual_OutPutOn"].Value = "On";
                newRow.Cells["DGV_WiseTowerLampManual_OutPutOff"].Value = "Off";
                newRow.Cells["DGV_WiseTowerLampManual_ModuleOutValue"].Value = wiseTowerLampConfigs.serviceData.Module_OutValue;
                newRow.Tag = wiseTowerLampConfigs;

                if(wiseTowerLampConfigs.serviceData.WriteOutputSignalFlag == 1) dtg_WiseTowerLampManual.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                else dtg_WiseTowerLampManual.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.White;


                if (firstRowIndex >= 0 && firstRowIndex < dtg_WiseTowerLampManual.Rows.Count)
                {
                    dtg_WiseTowerLampManual.FirstDisplayedScrollingRowIndex = firstRowIndex;
                }
                dtg_WiseTowerLampManual.ClearSelection();
            }
        }

        private void dtg_WiseTowerLampManual_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

            if (e.ColumnIndex == grid.Columns["DGV_WiseTowerLampManual_OutPutOn"].Index)
            {

                int Index =Convert.ToInt32(selectedRow.Cells["DGV_WiseTowerLampManual_Id"].Value);
                var ChangeData = uow.WiseTowerLampConfigs.Find(m => m.Id == Index).FirstOrDefault();

                if (ChangeData != null)
                {
                    ChangeData.serviceData.WriteOutputSignalFlag = 1;
                    ChangeData.serviceData.WriteOutputSignalValue = 1;
                    dtg_WiseTowerLampManual.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                    mainForm.UserLog("TowerLampManual Screen", $"WiseTowerLamp OutPutOn ModuleIndex = {ChangeData.Id} , ModuleName = {ChangeData.NameSetting}");
                }
            }

            else if (e.ColumnIndex == grid.Columns["DGV_WiseTowerLampManual_OutPutOff"].Index)
            {
                int Index = Convert.ToInt32(selectedRow.Cells["DGV_WiseTowerLampManual_Id"].Value);
                var ChangeData = uow.WiseTowerLampConfigs.Find(m => m.Id == Index).FirstOrDefault();

                if (ChangeData != null)
                {
                    ChangeData.serviceData.WriteOutputSignalFlag = 1;
                    ChangeData.serviceData.WriteOutputSignalValue = 0;
                    dtg_WiseTowerLampManual.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                    mainForm.UserLog("TowerLampManual Screen", $"WiseTowerLamp OutPutOff ModuleIndex = {ChangeData.Id} , ModuleName = {ChangeData.NameSetting}");


                }
            }
        }

        private void DisplayTimer_Tick(object sender, EventArgs e)
        {
            DisplayTimer.Enabled = false;
            WiseTowerLampManualDisplay();
            DisplayTimer.Interval = 1000; //타이머 인터벌 1초
            DisplayTimer.Enabled = true;
        }
    }
}



