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
    public partial class WiseTowerLampMonitoringScreen : Form
    {
       private readonly IUnitOfWork uow;

        public WiseTowerLampMonitoringScreen(IUnitOfWork uow)
        {
            InitializeComponent();
            this.uow = uow;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dtg_WiseTowerLampMonitoring.AlternatingRowsDefaultCellStyle = null;
            dtg_WiseTowerLampMonitoring.DoubleBuffered(true);
            WiseTowerLampValueInit();
            WiseTowerLampValueDisplay();
        }

        private void WiseTowerLampValueInit()
        {
            dtg_WiseTowerLampMonitoring.ScrollBars = ScrollBars.Both;
            dtg_WiseTowerLampMonitoring.AllowUserToResizeColumns = true;
            dtg_WiseTowerLampMonitoring.ColumnHeadersHeight = 40;
            dtg_WiseTowerLampMonitoring.RowTemplate.Height = 40;
            dtg_WiseTowerLampMonitoring.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_WiseTowerLampMonitoring.Columns[0].CellTemplate;
            dtg_WiseTowerLampMonitoring.Columns.Clear();
            dtg_WiseTowerLampMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampMonitoring_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_WiseTowerLampMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampMonitoring_DisplayName", HeaderText = "TowerLampDisplayName", Width = 200, CellTemplate = currentCellTemplate });
            dtg_WiseTowerLampMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampMonitoring_Status", HeaderText = "Status", Width = 100, CellTemplate = currentCellTemplate });
            dtg_WiseTowerLampMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampMonitoring_ModuleInValue", HeaderText = "ModuleInValue", Width = 100, CellTemplate = currentCellTemplate });
            dtg_WiseTowerLampMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampMonitoring_ModuleOutValue", HeaderText = "ModuleOutValue", Width = 100, CellTemplate = currentCellTemplate });
            dtg_WiseTowerLampMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampMonitoring_ModuleOnTime", HeaderText = "ModuleOnTime", Width = 200, CellTemplate = currentCellTemplate });
            dtg_WiseTowerLampMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampMonitoring_ModuleOffTime", HeaderText = "ModuleOffTime", Width = 200, CellTemplate = currentCellTemplate });
            dtg_WiseTowerLampMonitoring.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_WiseTowerLampMonitoring.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_WiseTowerLampMonitoring.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_WiseTowerLampMonitoring.Columns["DGV_WiseTowerLampMonitoring_ModuleInValue"].HeaderText = "Module" + "\r\n" + "InValue";
            dtg_WiseTowerLampMonitoring.Columns["DGV_WiseTowerLampMonitoring_ModuleOutValue"].HeaderText = "Module" + "\r\n" + "OutValue";
            dtg_WiseTowerLampMonitoring.Columns["DGV_WiseTowerLampMonitoring_ModuleOnTime"].HeaderText = "Module" + "\r\n" + "OutValue OnTime";
            dtg_WiseTowerLampMonitoring.Columns["DGV_WiseTowerLampMonitoring_ModuleOffTime"].HeaderText = "Module" + "\r\n" + "OutValue OffTime";
            dtg_WiseTowerLampMonitoring.Rows.Clear();
        }
        private void WiseTowerLampValueDisplay()
        {
            dtg_WiseTowerLampMonitoring.Columns["DGV_WiseTowerLampMonitoring_Id"].ReadOnly = true;
            dtg_WiseTowerLampMonitoring.Columns["DGV_WiseTowerLampMonitoring_DisplayName"].ReadOnly = true;
            dtg_WiseTowerLampMonitoring.Columns["DGV_WiseTowerLampMonitoring_Status"].ReadOnly = true;
            dtg_WiseTowerLampMonitoring.Columns["DGV_WiseTowerLampMonitoring_ModuleInValue"].ReadOnly = true;
            dtg_WiseTowerLampMonitoring.Columns["DGV_WiseTowerLampMonitoring_ModuleOutValue"].ReadOnly = true;
            dtg_WiseTowerLampMonitoring.Columns["DGV_WiseTowerLampMonitoring_ModuleOnTime"].ReadOnly = true;
            dtg_WiseTowerLampMonitoring.Columns["DGV_WiseTowerLampMonitoring_ModuleOffTime"].ReadOnly = true;

            int firstRowIndex = dtg_WiseTowerLampMonitoring.FirstDisplayedScrollingColumnIndex;
            dtg_WiseTowerLampMonitoring.Rows.Clear();
            dtg_WiseTowerLampMonitoring.RowTemplate.Height = 40;

            foreach (var wiseTowerLampConfigs in uow.WiseTowerLampConfigs.GetAll())
            {
                int newRowIndex = dtg_WiseTowerLampMonitoring.Rows.Add();
                var newRow = dtg_WiseTowerLampMonitoring.Rows[newRowIndex];
                newRow.Cells["DGV_WiseTowerLampMonitoring_Id"].Value = wiseTowerLampConfigs.Id.ToString();
                newRow.Cells["DGV_WiseTowerLampMonitoring_DisplayName"].Value = wiseTowerLampConfigs.DisplayNameSetting;
                newRow.Cells["DGV_WiseTowerLampMonitoring_Status"].Value = wiseTowerLampConfigs.serviceData.Status;
                newRow.Cells["DGV_WiseTowerLampMonitoring_ModuleInValue"].Value = wiseTowerLampConfigs.serviceData.Module_InValue;
                newRow.Cells["DGV_WiseTowerLampMonitoring_ModuleOutValue"].Value = wiseTowerLampConfigs.serviceData.Module_OutValue;
                newRow.Cells["DGV_WiseTowerLampMonitoring_ModuleOnTime"].Value = wiseTowerLampConfigs.serviceData.WriteOutputOnSignalTime;
                newRow.Cells["DGV_WiseTowerLampMonitoring_ModuleOffTime"].Value = wiseTowerLampConfigs.serviceData.WriteOutputOffSignalTime;
                newRow.Tag = wiseTowerLampConfigs;

                if (wiseTowerLampConfigs.serviceData.Status == "Connect") dtg_WiseTowerLampMonitoring.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.White;
                else dtg_WiseTowerLampMonitoring.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.Yellow;


                if (firstRowIndex >= 0 && firstRowIndex < dtg_WiseTowerLampMonitoring.Rows.Count)
                {
                    dtg_WiseTowerLampMonitoring.FirstDisplayedScrollingRowIndex = firstRowIndex;
                }
                dtg_WiseTowerLampMonitoring.ClearSelection();
            }
        }

        private void DisPlayTimer_Tick(object sender, EventArgs e)
        {
            DisPlayTimer.Enabled = false;
            WiseTowerLampValueDisplay();
            DisPlayTimer.Interval = 1000; //타이머 인터벌 1초
            DisPlayTimer.Enabled = true;
        }
    }
}
