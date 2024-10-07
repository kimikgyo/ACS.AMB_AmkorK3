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
    public partial class PLCManualScreen : Form
    {
        private readonly MainForm mainForm;
        private readonly IUnitOfWork uow;

        public PLCManualScreen(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.uow = uow;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dtg_PLCModuleManual.AlternatingRowsDefaultCellStyle = null;
            dtg_PLCModuleManual.DoubleBuffered(true);
            PLCModuleManualInit();
            PLCModuleManualDisplay();

        }

        private void PLCModuleManualInit()
        {
            dtg_PLCModuleManual.ScrollBars = ScrollBars.Both;
            dtg_PLCModuleManual.AllowUserToResizeColumns = true;
            dtg_PLCModuleManual.ColumnHeadersHeight = 40;
            dtg_PLCModuleManual.RowTemplate.Height = 40;
            dtg_PLCModuleManual.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_PLCModuleManual.Columns[0].CellTemplate;
            dtg_PLCModuleManual.Columns.Clear();
            dtg_PLCModuleManual.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCModule_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_PLCModuleManual.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCModule_Name", HeaderText = "ModuleName", Width = 200, CellTemplate = currentCellTemplate });
            dtg_PLCModuleManual.Columns.Add(new DataGridViewButtonColumn() { Name = "DGV_PLCModule_DT12000", HeaderText = "DT12000", Width = 100 });
            dtg_PLCModuleManual.Columns.Add(new DataGridViewButtonColumn() { Name = "DGV_PLCModule_DT12001", HeaderText = "DT12001", Width = 100 });
            dtg_PLCModuleManual.Columns.Add(new DataGridViewButtonColumn() { Name = "DGV_PLCModule_DT12002", HeaderText = "DT12002", Width = 100 });
            dtg_PLCModuleManual.Columns.Add(new DataGridViewButtonColumn() { Name = "DGV_PLCModule_DT12003", HeaderText = "DT12003", Width = 100 });
            dtg_PLCModuleManual.Columns.Add(new DataGridViewButtonColumn() { Name = "DGV_PLCModule_DT12004", HeaderText = "DT12004", Width = 100 });
            dtg_PLCModuleManual.Columns.Add(new DataGridViewButtonColumn() { Name = "DGV_PLCModule_DT12005", HeaderText = "DT12005", Width = 100 });
            dtg_PLCModuleManual.Columns.Add(new DataGridViewButtonColumn() { Name = "DGV_PLCModule_DT12006", HeaderText = "DT12006", Width = 100 });
            dtg_PLCModuleManual.Columns.Add(new DataGridViewButtonColumn() { Name = "DGV_PLCModule_DT12007", HeaderText = "DT12007", Width = 100 });
            dtg_PLCModuleManual.Columns.Add(new DataGridViewButtonColumn() { Name = "DGV_PLCModule_DT12008", HeaderText = "DT12008", Width = 100 });
            dtg_PLCModuleManual.Columns.Add(new DataGridViewButtonColumn() { Name = "DGV_PLCModule_DT12009", HeaderText = "DT12009", Width = 100 });
            dtg_PLCModuleManual.Columns.Add(new DataGridViewButtonColumn() { Name = "DGV_PLCModule_DT12010", HeaderText = "DT12010", Width = 100 });
            dtg_PLCModuleManual.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_PLCModuleManual.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_PLCModuleManual.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_PLCModuleManual.Rows.Clear();
        }

        private void PLCModuleManualDisplay()
        {
            dtg_PLCModuleManual.Columns["DGV_PLCModule_Id"].ReadOnly = true;
            dtg_PLCModuleManual.Columns["DGV_PLCModule_Name"].ReadOnly = true;

            int firstRowIndex = dtg_PLCModuleManual.FirstDisplayedScrollingColumnIndex;
            dtg_PLCModuleManual.Rows.Clear();
            dtg_PLCModuleManual.RowTemplate.Height = 40;

            foreach (var plcConfig in uow.PlcConfigs.GetAll())
            {
                int newRowIndex = dtg_PLCModuleManual.Rows.Add();
                var newRow = dtg_PLCModuleManual.Rows[newRowIndex];
                newRow.Cells["DGV_PLCModule_Id"].Value = plcConfig.Id.ToString();
                newRow.Cells["DGV_PLCModule_Name"].Value = plcConfig.PlcModuleName;
                newRow.Cells["DGV_PLCModule_DT12000"].Value = plcConfig.serviceWriteData.DT12000;
                newRow.Cells["DGV_PLCModule_DT12001"].Value = plcConfig.serviceWriteData.DT12001;
                newRow.Cells["DGV_PLCModule_DT12002"].Value = plcConfig.serviceWriteData.DT12002;
                newRow.Cells["DGV_PLCModule_DT12003"].Value = plcConfig.serviceWriteData.DT12003;
                newRow.Cells["DGV_PLCModule_DT12004"].Value = plcConfig.serviceWriteData.DT12004;
                newRow.Cells["DGV_PLCModule_DT12005"].Value = plcConfig.serviceWriteData.DT12005;
                newRow.Cells["DGV_PLCModule_DT12006"].Value = plcConfig.serviceWriteData.DT12006;
                newRow.Cells["DGV_PLCModule_DT12007"].Value = plcConfig.serviceWriteData.DT12007;
                newRow.Cells["DGV_PLCModule_DT12008"].Value = plcConfig.serviceWriteData.DT12008;
                newRow.Cells["DGV_PLCModule_DT12009"].Value = plcConfig.serviceWriteData.DT12009;
                newRow.Cells["DGV_PLCModule_DT12010"].Value = plcConfig.serviceWriteData.DT12010;
                newRow.Tag = plcConfig;


                if (firstRowIndex >= 0 && firstRowIndex < dtg_PLCModuleManual.Rows.Count)
                {
                    dtg_PLCModuleManual.FirstDisplayedScrollingRowIndex = firstRowIndex;
                }
                dtg_PLCModuleManual.ClearSelection();
            }
        }

        private void dtg_WiseTowerLampManual_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

            PlcConfig selectedRowTag = (PlcConfig)selectedRow.Tag;
            PlcConfig targetConfig = uow.PlcConfigs.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();
            if (targetConfig != null)
            {
                if (e.ColumnIndex == grid.Columns["DGV_PLCModule_DT12000"].Index)
                {
                    if (targetConfig.serviceWriteData.DT12000 == 0) targetConfig.serviceWriteData.DT12000 = 1;
                    else targetConfig.serviceWriteData.DT12000 = 0;
                    mainForm.UserLog("PLCModuleManual Screen", $"PLC PLCModule_DT12000 ModuleIndex = {targetConfig.Id} , DT12000 Value = {targetConfig.serviceWriteData.DT12000}");
                }
                else if (e.ColumnIndex == grid.Columns["DGV_PLCModule_DT12001"].Index)
                {
                    if (targetConfig.serviceWriteData.DT12001 == 0) targetConfig.serviceWriteData.DT12001 = 1;
                    else targetConfig.serviceWriteData.DT12001 = 0;
                    mainForm.UserLog("PLCModuleManual Screen", $"PLC PLCModule_DT12001 ModuleIndex = {targetConfig.Id} , DT12001 Value = {targetConfig.serviceWriteData.DT12001}");
                }
                else if (e.ColumnIndex == grid.Columns["DGV_PLCModule_DT12002"].Index)
                {

                    if (targetConfig.serviceWriteData.DT12002 == 0) targetConfig.serviceWriteData.DT12002 = 1;
                    else targetConfig.serviceWriteData.DT12002 = 0;
                    mainForm.UserLog("PLCModuleManual Screen", $"PLC PLCModule_DT12002 ModuleIndex = {targetConfig.Id} , DT12002 Value = {targetConfig.serviceWriteData.DT12002}");
                }
                else if (e.ColumnIndex == grid.Columns["DGV_PLCModule_DT12003"].Index)
                {


                    if (targetConfig.serviceWriteData.DT12003 == 0) targetConfig.serviceWriteData.DT12003 = 1;
                    else targetConfig.serviceWriteData.DT12003 = 0;
                    mainForm.UserLog("PLCModuleManual Screen", $"PLC PLCModule_DT12003 ModuleIndex = {targetConfig.Id} , DT12003 Value = {targetConfig.serviceWriteData.DT12003}");
                }
                else if (e.ColumnIndex == grid.Columns["DGV_PLCModule_DT12004"].Index)
                {


                    if (targetConfig.serviceWriteData.DT12004 == 0) targetConfig.serviceWriteData.DT12004 = 1;
                    else targetConfig.serviceWriteData.DT12004 = 0;
                    mainForm.UserLog("PLCModuleManual Screen", $"PLC PLCModule_DT12004 ModuleIndex = {targetConfig.Id} , DT12004 Value = {targetConfig.serviceWriteData.DT12004}");
                }
                else if (e.ColumnIndex == grid.Columns["DGV_PLCModule_DT12005"].Index)
                {


                    if (targetConfig.serviceWriteData.DT12005 == 0) targetConfig.serviceWriteData.DT12005 = 1;
                    else targetConfig.serviceWriteData.DT12005 = 0;
                    mainForm.UserLog("PLCModuleManual Screen", $"PLC PLCModule_DT12005 ModuleIndex = {targetConfig.Id} , DT12005 Value = {targetConfig.serviceWriteData.DT12005}");
                }
                else if (e.ColumnIndex == grid.Columns["DGV_PLCModule_DT12006"].Index)
                {


                    if (targetConfig.serviceWriteData.DT12006 == 0) targetConfig.serviceWriteData.DT12006 = 1;
                    else targetConfig.serviceWriteData.DT12006 = 0;
                    mainForm.UserLog("PLCModuleManual Screen", $"PLC PLCModule_DT12006 ModuleIndex = {targetConfig.Id} , DT12006 Value = {targetConfig.serviceWriteData.DT12006}");
                }
                else if (e.ColumnIndex == grid.Columns["DGV_PLCModule_DT12007"].Index)
                {


                    if (targetConfig.serviceWriteData.DT12007 == 0) targetConfig.serviceWriteData.DT12007 = 1;
                    else targetConfig.serviceWriteData.DT12007 = 0;
                    mainForm.UserLog("PLCModuleManual Screen", $"PLC PLCModule_DT12007 ModuleIndex = {targetConfig.Id} , DT12007 Value = {targetConfig.serviceWriteData.DT12007}");
                }
                else if (e.ColumnIndex == grid.Columns["DGV_PLCModule_DT12008"].Index)
                {


                    if (targetConfig.serviceWriteData.DT12008 == 0) targetConfig.serviceWriteData.DT12008 = 1;
                    else targetConfig.serviceWriteData.DT12008 = 0;
                    mainForm.UserLog("PLCModuleManual Screen", $"PLC PLCModule_DT12008 ModuleIndex = {targetConfig.Id} , DT12008 Value = {targetConfig.serviceWriteData.DT12008}");
                }
                else if (e.ColumnIndex == grid.Columns["DGV_PLCModule_DT12009"].Index)
                {


                    if (targetConfig.serviceWriteData.DT12009 == 0) targetConfig.serviceWriteData.DT12009 = 1;
                    else targetConfig.serviceWriteData.DT12009 = 0;
                    mainForm.UserLog("PLCModuleManual Screen", $"PLC PLCModule_DT12009 ModuleIndex = {targetConfig.Id} , DT12009 Value = {targetConfig.serviceWriteData.DT12009}");
                }
                else if (e.ColumnIndex == grid.Columns["DGV_PLCModule_DT12010"].Index)
                {


                    if (targetConfig.serviceWriteData.DT12010 == 0) targetConfig.serviceWriteData.DT12010 = 1;
                    else targetConfig.serviceWriteData.DT12010 = 0;
                    mainForm.UserLog("PLCModuleManual Screen", $"PLC PLCModule_DT12010 ModuleIndex = {targetConfig.Id} , DT12010 Value = {targetConfig.serviceWriteData.DT12010}");
                }
            }
        }

        private void DisplayTimer_Tick(object sender, EventArgs e)
        {
            DisplayTimer.Enabled = false;
            PLCModuleManualDisplay();
            DisplayTimer.Interval = 1000; //타이머 인터벌 1초
            DisplayTimer.Enabled = true;
        }
    }
}



