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
    public partial class PLCMonitoringScreen : Form
    {
       private readonly IUnitOfWork uow;

        public PLCMonitoringScreen(IUnitOfWork uow)
        {
            InitializeComponent();
            this.uow = uow;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dtg_PLCReadDataMonitoring.AlternatingRowsDefaultCellStyle = null;
            dtg_PLCWriteDataMonitoring.AlternatingRowsDefaultCellStyle = null;
            dtg_PLCReadDataMonitoring.DoubleBuffered(true);
            dtg_PLCWriteDataMonitoring.DoubleBuffered(true);
            PLCReadDataMonitoringInit();
            PLCWriteDataMonitoringInit();
            PLCReadDataMonitoringDisplay();
            PLCWriteDataMonitoringDisplay();
        }

        private void PLCReadDataMonitoringInit()
        {
            dtg_PLCReadDataMonitoring.ScrollBars = ScrollBars.Both;
            dtg_PLCReadDataMonitoring.AllowUserToResizeColumns = true;
            dtg_PLCReadDataMonitoring.ColumnHeadersHeight = 50;
            dtg_PLCReadDataMonitoring.RowTemplate.Height = 50;
            dtg_PLCReadDataMonitoring.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_PLCReadDataMonitoring.Columns[0].CellTemplate;
            dtg_PLCReadDataMonitoring.Columns.Clear();
            dtg_PLCReadDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCReadDataMonitoring_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_PLCReadDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCReadDataMonitoring_DisplayName", HeaderText = "PLCModuleName", Width = 150, CellTemplate = currentCellTemplate });
            dtg_PLCReadDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCReadDataMonitoring_Connected", HeaderText = "Connected", Width = 150, CellTemplate = currentCellTemplate });
            dtg_PLCReadDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCReadDataMonitoring_DT11000", HeaderText = "DT11000", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCReadDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCReadDataMonitoring_DT11001", HeaderText = "DT11001", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCReadDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCReadDataMonitoring_DT11002", HeaderText = "DT11002", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCReadDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCReadDataMonitoring_DT11003", HeaderText = "DT11003", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCReadDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCReadDataMonitoring_DT11004", HeaderText = "DT11004", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCReadDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCReadDataMonitoring_DT11005", HeaderText = "DT11005", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCReadDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCReadDataMonitoring_DT11006", HeaderText = "DT11006", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCReadDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCReadDataMonitoring_DT11007", HeaderText = "DT11007", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCReadDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCReadDataMonitoring_DT11008", HeaderText = "DT11008", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCReadDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCReadDataMonitoring_DT11009", HeaderText = "DT11009", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCReadDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCReadDataMonitoring_DT11010", HeaderText = "DT11010", Width = 100, CellTemplate = currentCellTemplate });
        
            dtg_PLCReadDataMonitoring.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_PLCReadDataMonitoring.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_PLCReadDataMonitoring.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11000"].HeaderText = "Read(DT)" + "\r\n" + "11000";
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11001"].HeaderText = "Read(DT)" + "\r\n" + "11001";
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11002"].HeaderText = "Read(DT)" + "\r\n" + "11002";
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11003"].HeaderText = "Read(DT)" + "\r\n" + "11003";
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11004"].HeaderText = "Read(DT)" + "\r\n" + "11004";
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11005"].HeaderText = "Read(DT)" + "\r\n" + "11005";
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11006"].HeaderText = "Read(DT)" + "\r\n" + "11006";
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11007"].HeaderText = "Read(DT)" + "\r\n" + "11007";
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11008"].HeaderText = "Read(DT)" + "\r\n" + "11008";
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11009"].HeaderText = "Read(DT)" + "\r\n" + "11009";
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11010"].HeaderText = "Read(DT)" + "\r\n" + "11010";

            dtg_PLCReadDataMonitoring.Rows.Clear();
        }

        private void PLCReadDataMonitoringDisplay()
        {
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_Id"].ReadOnly = true;
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DisplayName"].ReadOnly = true;
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_Connected"].ReadOnly = true;
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11000"].ReadOnly = true;
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11001"].ReadOnly = true;
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11002"].ReadOnly = true;
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11003"].ReadOnly = true;
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11004"].ReadOnly = true;
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11005"].ReadOnly = true;
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11006"].ReadOnly = true;
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11007"].ReadOnly = true;
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11008"].ReadOnly = true;
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11009"].ReadOnly = true;
            dtg_PLCReadDataMonitoring.Columns["DGV_PLCReadDataMonitoring_DT11010"].ReadOnly = true;

            int firstRowIndex = dtg_PLCReadDataMonitoring.FirstDisplayedScrollingColumnIndex;
            dtg_PLCReadDataMonitoring.Rows.Clear();
            dtg_PLCReadDataMonitoring.RowTemplate.Height = 50;

            foreach (var plcConfig in uow.PlcConfigs.GetAll())
            {
                int newRowIndex = dtg_PLCReadDataMonitoring.Rows.Add();
                var newRow = dtg_PLCReadDataMonitoring.Rows[newRowIndex];
                newRow.Cells["DGV_PLCReadDataMonitoring_Id"].Value = plcConfig.Id.ToString();
                newRow.Cells["DGV_PLCReadDataMonitoring_DisplayName"].Value = plcConfig.PlcModuleName;
                newRow.Cells["DGV_PLCReadDataMonitoring_Connected"].Value = plcConfig.Connect;
                newRow.Cells["DGV_PLCReadDataMonitoring_DT11000"].Value = plcConfig.serviceReadData.DT11000;
                newRow.Cells["DGV_PLCReadDataMonitoring_DT11001"].Value = plcConfig.serviceReadData.DT11001;
                newRow.Cells["DGV_PLCReadDataMonitoring_DT11002"].Value = plcConfig.serviceReadData.DT11002;
                newRow.Cells["DGV_PLCReadDataMonitoring_DT11003"].Value = plcConfig.serviceReadData.DT11003;
                newRow.Cells["DGV_PLCReadDataMonitoring_DT11004"].Value = plcConfig.serviceReadData.DT11004;
                newRow.Cells["DGV_PLCReadDataMonitoring_DT11005"].Value = plcConfig.serviceReadData.DT11005;
                newRow.Cells["DGV_PLCReadDataMonitoring_DT11006"].Value = plcConfig.serviceReadData.DT11006;
                newRow.Cells["DGV_PLCReadDataMonitoring_DT11007"].Value = plcConfig.serviceReadData.DT11007;
                newRow.Cells["DGV_PLCReadDataMonitoring_DT11008"].Value = plcConfig.serviceReadData.DT11008;
                newRow.Cells["DGV_PLCReadDataMonitoring_DT11009"].Value = plcConfig.serviceReadData.DT11009;
                newRow.Cells["DGV_PLCReadDataMonitoring_DT11010"].Value = plcConfig.serviceReadData.DT11010;
                newRow.Tag = plcConfig;

                if (plcConfig.Connect == true) dtg_PLCReadDataMonitoring.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.White;
                else dtg_PLCReadDataMonitoring.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.Yellow;


                if (firstRowIndex >= 0 && firstRowIndex < dtg_PLCReadDataMonitoring.Rows.Count)
                {
                    dtg_PLCReadDataMonitoring.FirstDisplayedScrollingRowIndex = firstRowIndex;
                }
                dtg_PLCReadDataMonitoring.ClearSelection();
            }
        }

        private void PLCWriteDataMonitoringInit()
        {
            dtg_PLCWriteDataMonitoring.ScrollBars = ScrollBars.Both;
            dtg_PLCWriteDataMonitoring.AllowUserToResizeColumns = true;
            dtg_PLCWriteDataMonitoring.ColumnHeadersHeight = 50;
            dtg_PLCWriteDataMonitoring.RowTemplate.Height = 50;
            dtg_PLCWriteDataMonitoring.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_PLCWriteDataMonitoring.Columns[0].CellTemplate;
            dtg_PLCWriteDataMonitoring.Columns.Clear();
            dtg_PLCWriteDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCWriteDataMonitoring_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_PLCWriteDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCWriteDataMonitoring_DisplayName", HeaderText = "PLCModuleName", Width = 150, CellTemplate = currentCellTemplate });
            dtg_PLCWriteDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCWriteDataMonitoring_Connected", HeaderText = "Connected", Width = 150, CellTemplate = currentCellTemplate });
            dtg_PLCWriteDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCWriteDataMonitoring_DT12000", HeaderText = "DT12000", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCWriteDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCWriteDataMonitoring_DT12001", HeaderText = "DT12001", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCWriteDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCWriteDataMonitoring_DT12002", HeaderText = "DT12002", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCWriteDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCWriteDataMonitoring_DT12003", HeaderText = "DT12003", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCWriteDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCWriteDataMonitoring_DT12004", HeaderText = "DT12004", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCWriteDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCWriteDataMonitoring_DT12005", HeaderText = "DT12005", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCWriteDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCWriteDataMonitoring_DT12006", HeaderText = "DT12006", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCWriteDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCWriteDataMonitoring_DT12007", HeaderText = "DT12007", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCWriteDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCWriteDataMonitoring_DT12008", HeaderText = "DT12008", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCWriteDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCWriteDataMonitoring_DT12009", HeaderText = "DT12009", Width = 100, CellTemplate = currentCellTemplate });
            dtg_PLCWriteDataMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCWriteDataMonitoring_DT12010", HeaderText = "DT12010", Width = 100, CellTemplate = currentCellTemplate });

            dtg_PLCWriteDataMonitoring.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_PLCWriteDataMonitoring.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_PLCWriteDataMonitoring.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12000"].HeaderText = "Write(DT)" + "\r\n" + "12000";
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12001"].HeaderText = "Write(DT)" + "\r\n" + "12001";
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12002"].HeaderText = "Write(DT)" + "\r\n" + "12002";
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12003"].HeaderText = "Write(DT)" + "\r\n" + "12003";
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12004"].HeaderText = "Write(DT)" + "\r\n" + "12004";
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12005"].HeaderText = "Write(DT)" + "\r\n" + "12005";
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12006"].HeaderText = "Write(DT)" + "\r\n" + "12006";
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12007"].HeaderText = "Write(DT)" + "\r\n" + "12007";
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12008"].HeaderText = "Write(DT)" + "\r\n" + "12008";
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12009"].HeaderText = "Write(DT)" + "\r\n" + "12009";
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12010"].HeaderText = "Write(DT)" + "\r\n" + "12010";

            dtg_PLCWriteDataMonitoring.Rows.Clear();
        }

        private void PLCWriteDataMonitoringDisplay()
        {
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_Id"].ReadOnly = true;
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DisplayName"].ReadOnly = true;
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_Connected"].ReadOnly = true;
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12000"].ReadOnly = true;
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12001"].ReadOnly = true;
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12002"].ReadOnly = true;
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12003"].ReadOnly = true;
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12004"].ReadOnly = true;
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12005"].ReadOnly = true;
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12006"].ReadOnly = true;
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12007"].ReadOnly = true;
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12008"].ReadOnly = true;
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12009"].ReadOnly = true;
            dtg_PLCWriteDataMonitoring.Columns["DGV_PLCWriteDataMonitoring_DT12010"].ReadOnly = true;

            int firstRowIndex = dtg_PLCWriteDataMonitoring.FirstDisplayedScrollingColumnIndex;
            dtg_PLCWriteDataMonitoring.Rows.Clear();
            dtg_PLCWriteDataMonitoring.RowTemplate.Height = 50;

            foreach (var plcConfig in uow.PlcConfigs.GetAll())
            {
                int newRowIndex = dtg_PLCWriteDataMonitoring.Rows.Add();
                var newRow = dtg_PLCWriteDataMonitoring.Rows[newRowIndex];
                newRow.Cells["DGV_PLCWriteDataMonitoring_Id"].Value = plcConfig.Id.ToString();
                newRow.Cells["DGV_PLCWriteDataMonitoring_DisplayName"].Value = plcConfig.PlcModuleName;
                newRow.Cells["DGV_PLCWriteDataMonitoring_Connected"].Value = plcConfig.Connect;
                newRow.Cells["DGV_PLCWriteDataMonitoring_DT12000"].Value = plcConfig.serviceWriteData.DT12000;
                newRow.Cells["DGV_PLCWriteDataMonitoring_DT12001"].Value = plcConfig.serviceWriteData.DT12001;
                newRow.Cells["DGV_PLCWriteDataMonitoring_DT12002"].Value = plcConfig.serviceWriteData.DT12002;
                newRow.Cells["DGV_PLCWriteDataMonitoring_DT12003"].Value = plcConfig.serviceWriteData.DT12003;
                newRow.Cells["DGV_PLCWriteDataMonitoring_DT12004"].Value = plcConfig.serviceWriteData.DT12004;
                newRow.Cells["DGV_PLCWriteDataMonitoring_DT12005"].Value = plcConfig.serviceWriteData.DT12005;
                newRow.Cells["DGV_PLCWriteDataMonitoring_DT12006"].Value = plcConfig.serviceWriteData.DT12006;
                newRow.Cells["DGV_PLCWriteDataMonitoring_DT12007"].Value = plcConfig.serviceWriteData.DT12007;
                newRow.Cells["DGV_PLCWriteDataMonitoring_DT12008"].Value = plcConfig.serviceWriteData.DT12008;
                newRow.Cells["DGV_PLCWriteDataMonitoring_DT12009"].Value = plcConfig.serviceWriteData.DT12009;
                newRow.Cells["DGV_PLCWriteDataMonitoring_DT12010"].Value = plcConfig.serviceWriteData.DT12010;
                newRow.Tag = plcConfig;

                if (plcConfig.Connect == true) dtg_PLCWriteDataMonitoring.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.White;
                else dtg_PLCWriteDataMonitoring.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.Yellow;


                if (firstRowIndex >= 0 && firstRowIndex < dtg_PLCReadDataMonitoring.Rows.Count)
                {
                    dtg_PLCWriteDataMonitoring.FirstDisplayedScrollingRowIndex = firstRowIndex;
                }
                dtg_PLCWriteDataMonitoring.ClearSelection();
            }
        }

        private void DisPlayTimer_Tick(object sender, EventArgs e)
        {
            DisPlayTimer.Enabled = false;
            PLCReadDataMonitoringDisplay();
            PLCWriteDataMonitoringDisplay();
            DisPlayTimer.Interval = 1000; //타이머 인터벌 1초
            DisPlayTimer.Enabled = true;
        }
    }
}
