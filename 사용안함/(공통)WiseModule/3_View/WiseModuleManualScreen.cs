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
    public partial class WiseModuleManualScreen : Form
    {
        private readonly MainForm mainForm;
        private readonly IUnitOfWork uow;

        public WiseModuleManualScreen(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.uow = uow;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dtg_WiseModuleManual.AlternatingRowsDefaultCellStyle = null;
            dtg_WiseModuleManual.DoubleBuffered(true);
            WiseModuleManualInit();
            WiseModuleManualDisplay();

        }

        private void WiseModuleManualInit()
        {
            dtg_WiseModuleManual.ScrollBars = ScrollBars.Both;
            dtg_WiseModuleManual.AllowUserToResizeColumns = true;
            dtg_WiseModuleManual.ColumnHeadersHeight = 40;
            dtg_WiseModuleManual.RowTemplate.Height = 40;
            dtg_WiseModuleManual.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_WiseModuleManual.Columns[0].CellTemplate;
            dtg_WiseModuleManual.Columns.Clear();
            dtg_WiseModuleManual.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseModuleManual_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_WiseModuleManual.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseModuleManual_DisplayName", HeaderText = "DisplayName", Width = 200, CellTemplate = currentCellTemplate });
            dtg_WiseModuleManual.Columns.Add(new DataGridViewButtonColumn() { Name = "DGV_WiseModuleManual_OutPutOn", HeaderText = "OutPutOn", Width = 100 });
            dtg_WiseModuleManual.Columns.Add(new DataGridViewButtonColumn() { Name = "DGV_WiseModuleManual_OutPutOff", HeaderText = "OutPutOff", Width = 100 });
            dtg_WiseModuleManual.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseModuleManual_ModuleOutValue", HeaderText = "ModuleOutValue", Width = 130, CellTemplate = currentCellTemplate });
            dtg_WiseModuleManual.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_WiseModuleManual.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_WiseModuleManual.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_WiseModuleManual.Rows.Clear();
        }

        private void WiseModuleManualDisplay()
        {
            dtg_WiseModuleManual.Columns["DGV_WiseModuleManual_Id"].ReadOnly = true;
            dtg_WiseModuleManual.Columns["DGV_WiseModuleManual_DisplayName"].ReadOnly = true;

            int firstRowIndex = dtg_WiseModuleManual.FirstDisplayedScrollingColumnIndex;
            dtg_WiseModuleManual.Rows.Clear();
            dtg_WiseModuleManual.RowTemplate.Height = 40;

            foreach (var wiseModuleConfigs in uow.WiseModuleConfig.GetAll())
            {
                int newRowIndex = dtg_WiseModuleManual.Rows.Add();
                var newRow = dtg_WiseModuleManual.Rows[newRowIndex];
                newRow.Cells["DGV_WiseModuleManual_Id"].Value = wiseModuleConfigs.Id.ToString();
                newRow.Cells["DGV_WiseModuleManual_DisplayName"].Value = wiseModuleConfigs.DisplayName;
                newRow.Cells["DGV_WiseModuleManual_OutPutOn"].Value = "On";
                newRow.Cells["DGV_WiseModuleManual_OutPutOff"].Value = "Off";
                newRow.Cells["DGV_WiseModuleManual_ModuleOutValue"].Value = wiseModuleConfigs.ModuleOut_Value;
                newRow.Tag = wiseModuleConfigs;

                if(wiseModuleConfigs.serviceData.WriteOutputSignalFlag == 1) dtg_WiseModuleManual.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                else dtg_WiseModuleManual.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.White;


                if (firstRowIndex >= 0 && firstRowIndex < dtg_WiseModuleManual.Rows.Count)
                {
                    dtg_WiseModuleManual.FirstDisplayedScrollingRowIndex = firstRowIndex;
                }
                dtg_WiseModuleManual.ClearSelection();
            }
        }

        private void DisplayTimer_Tick(object sender, EventArgs e)
        {
            DisplayTimer.Enabled = false;
            WiseModuleManualDisplay();
            DisplayTimer.Interval = 1000; //타이머 인터벌 1초
            DisplayTimer.Enabled = true;
        }

        private void dtg_WiseModuleManual_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

            if (e.ColumnIndex == grid.Columns["DGV_WiseModuleManual_OutPutOn"].Index)
            {

                int Index = Convert.ToInt32(selectedRow.Cells["DGV_WiseModuleManual_Id"].Value);
                var ChangeData = uow.WiseModuleConfig.Find(m => m.Id == Index).FirstOrDefault();

                if (ChangeData != null)
                {
                    ChangeData.serviceData.WriteOutputSignalFlag = 1;
                    ChangeData.serviceData.WriteOutputSignalValue = 1;
                    dtg_WiseModuleManual.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                    mainForm.UserLog("WiseModuleManual Screen", $"WiseModule OutPutOn ModuleIndex = {ChangeData.Id} , ModuleName = {ChangeData.ModuleName}");
                }
            }

            else if (e.ColumnIndex == grid.Columns["DGV_WiseModuleManual_OutPutOff"].Index)
            {
                int Index = Convert.ToInt32(selectedRow.Cells["DGV_WiseModuleManual_Id"].Value);
                var ChangeData = uow.WiseModuleConfig.Find(m => m.Id == Index).FirstOrDefault();

                if (ChangeData != null)
                {
                    ChangeData.serviceData.WriteOutputSignalFlag = 1;
                    ChangeData.serviceData.WriteOutputSignalValue = 0;
                    dtg_WiseModuleManual.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                    mainForm.UserLog("WiseModule Screen", $"WiseModule OutPutOff ModuleIndex = {ChangeData.Id} , ModuleName = {ChangeData.ModuleName}");


                }
            }
        }
    }
}



