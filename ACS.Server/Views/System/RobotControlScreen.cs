using INA_ACS_Server.Models;
using INA_ACS_Server.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INA_ACS_Server
{
    public partial class RobotControlScreen : Form
    {
        MainForm main;
        IUnitOfWork uow;

        private readonly Font textFont1 = new Font("맑은 고딕", 12, FontStyle.Bold);

        public RobotControlScreen(MainForm main, IUnitOfWork uow)
        {
            InitializeComponent();
            this.main = main;
            this.uow = uow;

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.None; //윈도우(상단) 테두리 제거 source code

            dtg_RobotControl.AlternatingRowsDefaultCellStyle = null;
            dtg_RobotFloorMapID.AlternatingRowsDefaultCellStyle = null;
            dtg_ACSRobotGroup.AlternatingRowsDefaultCellStyle = null;
            dtg_ACSChargerCountConfig.AlternatingRowsDefaultCellStyle = null;

            dtg_RobotControl.DoubleBuffered(true);
            dtg_RobotFloorMapID.DoubleBuffered(true);
            dtg_ACSRobotGroup.DoubleBuffered(true);
            dtg_ACSChargerCountConfig.DoubleBuffered(true);

            Init();

            GroupBoxInit();
            TextBoxInit();
            RobotControlInit();
            RobotControlDisplay();
            RobotFloorMapIdInit();
            RobotFloorMapIdDisplay();
            ACSRobotGroupInit();
            ACSRobotGroup_Display();
            ACSChargerCountConfigInit();
            ACSChargerCountConfigDisplay();
        }

        private void Init()
        {
            this.BackColor = main.skinColor;
            tableLayoutPanel1.BackColor = main.skinColor;

            groupBox3.ForeColor = Color.White;
            label3.ForeColor = Color.White;
            btn_ACSRobotGroupBackup.BackColor = main.skinColor;
            btn_ACSRobotGroupBackup.ForeColor = Color.White;
            dtg_ACSRobotGroup.ScrollBars = ScrollBars.Both;
            dtg_ACSRobotGroup.AllowUserToResizeColumns = true;
            dtg_ACSRobotGroup.ColumnHeadersHeight = 70;
            dtg_ACSRobotGroup.RowTemplate.Height = 82;
            dtg_ACSRobotGroup.ReadOnly = false;
            dtg_ACSRobotGroup.BackgroundColor = main.skinColor;
            dtg_ACSRobotGroup.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dtg_ACSRobotGroup.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtg_ACSRobotGroup.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtg_ACSRobotGroup.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dtg_ACSRobotGroup.ColumnHeadersDefaultCellStyle.Font = textFont1;
            dtg_ACSRobotGroup.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(80, 89, 96);
            dtg_ACSRobotGroup.EnableHeadersVisualStyles = false;
            dtg_ACSRobotGroup.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            groupBox2.ForeColor = Color.White;
            label2.ForeColor = Color.White;
            lbl_RobotResetMission.ForeColor = Color.White;
            btn_ACSRobotControlSettingBackUp.BackColor = main.skinColor;
            btn_ACSRobotControlSettingBackUp.ForeColor = Color.White;
            dtg_RobotControl.ScrollBars = ScrollBars.Both;
            dtg_RobotControl.AllowUserToResizeColumns = true;
            dtg_RobotControl.ColumnHeadersHeight = 70;
            dtg_RobotControl.RowTemplate.Height = 82;
            dtg_RobotControl.ReadOnly = false;
            dtg_RobotControl.BackgroundColor = main.skinColor;
            dtg_RobotControl.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dtg_RobotControl.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtg_RobotControl.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtg_RobotControl.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dtg_RobotControl.ColumnHeadersDefaultCellStyle.Font = textFont1;
            dtg_RobotControl.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(80, 89, 96);
            dtg_RobotControl.EnableHeadersVisualStyles = false;
            dtg_RobotControl.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            groupBox6.ForeColor = Color.White;
            label6.ForeColor = Color.White;
            btn_ACSChargerCountBackup.BackColor = main.skinColor;
            btn_ACSChargerCountBackup.ForeColor = Color.White;
            dtg_ACSChargerCountConfig.ScrollBars = ScrollBars.Both;
            dtg_ACSChargerCountConfig.AllowUserToResizeColumns = true;
            dtg_ACSChargerCountConfig.ColumnHeadersHeight = 70;
            dtg_ACSChargerCountConfig.RowTemplate.Height = 82;
            dtg_ACSChargerCountConfig.ReadOnly = false;
            dtg_ACSChargerCountConfig.BackgroundColor = main.skinColor;
            dtg_ACSChargerCountConfig.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dtg_ACSChargerCountConfig.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtg_ACSChargerCountConfig.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtg_ACSChargerCountConfig.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dtg_ACSChargerCountConfig.ColumnHeadersDefaultCellStyle.Font = textFont1;
            dtg_ACSChargerCountConfig.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(80, 89, 96);
            dtg_ACSChargerCountConfig.EnableHeadersVisualStyles = false;
            dtg_ACSChargerCountConfig.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            groupBox1.ForeColor = Color.White;
            label1.ForeColor = Color.White;
            btn_RobotFloorMapIDBackUp.BackColor = main.skinColor;
            btn_RobotFloorMapIDBackUp.ForeColor = Color.White;
            dtg_RobotFloorMapID.ScrollBars = ScrollBars.Both;
            dtg_RobotFloorMapID.AllowUserToResizeColumns = true;
            dtg_RobotFloorMapID.ColumnHeadersHeight = 70;
            dtg_RobotFloorMapID.RowTemplate.Height = 82;
            dtg_RobotFloorMapID.ReadOnly = false;
            dtg_RobotFloorMapID.BackgroundColor = main.skinColor;
            dtg_RobotFloorMapID.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dtg_RobotFloorMapID.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtg_RobotFloorMapID.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtg_RobotFloorMapID.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dtg_RobotFloorMapID.ColumnHeadersDefaultCellStyle.Font = textFont1;
            dtg_RobotFloorMapID.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(80, 89, 96);
            dtg_RobotFloorMapID.EnableHeadersVisualStyles = false;
            dtg_RobotFloorMapID.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void GroupBoxInit()
        {
            groupBox12.Visible = false;
            groupBox4.Visible = false;
        }

        private void TextBoxInit()
        {
            lal_IpAdressChangeMsg.Font = new Font("맑은 고딕", 15, FontStyle.Bold);
            lal_IpAdressChangeMsg.ForeColor = Color.Red;
            lal_IpAdressChangeMsg.Text = $"*****Robot IP 변경되었습니다*****" + "\r\n" + "*****!ACS Program 재시작 후 변경사항이 반영됩니다!*****";
            lal_IpAdressChangeMsg.Visible = false;

            lbl_RobotResetMission.Visible = true;
            txt_RobotResetMissionName.Visible = true;

            txt_Robot_MaxNum.ReadOnly = true;
            txt_Robot_MaxNum.BackColor = Color.White;

            txt_ACSRobotGroupMaxNum.ReadOnly = true;
            txt_ACSRobotGroupMaxNum.BackColor = Color.White;

            txt_ACSChargerCountMaxNum.ReadOnly = true;
            txt_ACSChargerCountMaxNum.BackColor = Color.White;

            txt_FloorMapID_MaxNum.ReadOnly = true;
            txt_FloorMapID_MaxNum.BackColor = Color.White;

            txt_Robot_MaxNum.Text = ConfigData.MiR_MaxNum.ToString();
            txt_Fleet_IP_Address.Text = ConfigData.sFleet_IP_Address_SV.ToString();
            txt_Fleet_ResponseTime.Text = ConfigData.sFleet_ResponseTime.ToString();
            txt_FloorMapID_MaxNum.Text = ConfigData.FloorMapID_MaxNum.ToString();
            txt_ACSRobotGroupMaxNum.Text = ConfigData.ACSRobotGroup_MaxNum.ToString();
            txt_ACSChargerCountMaxNum.Text = ConfigData.ACSChargerCount_MaxNum.ToString();
         

        }

        private void ACSRobotGroupInit()
        {

            dtg_ACSRobotGroup.ScrollBars = ScrollBars.Both;
            dtg_ACSRobotGroup.AllowUserToResizeColumns = true;
            dtg_ACSRobotGroup.ColumnHeadersHeight = 70;
            dtg_ACSRobotGroup.RowTemplate.Height = 70;
            dtg_ACSRobotGroup.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_ACSRobotGroup.Columns[0].CellTemplate;
            dtg_ACSRobotGroup.Columns.Clear();
            dtg_ACSRobotGroup.Columns.Add(new DataGridViewColumn() { Name = "DGV_ACSRobotGroup_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_ACSRobotGroup.Columns.Add(new DataGridViewColumn() { Name = "DGV_ACSRobotGroup_Use", HeaderText = "Use/UnUse", Width = 80, CellTemplate = currentCellTemplate });
            dtg_ACSRobotGroup.Columns.Add(new DataGridViewColumn() { Name = "DGV_ACSRobotGroup_Name", HeaderText = "GroupName", Width = 150, CellTemplate = currentCellTemplate });
            dtg_ACSRobotGroup.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_ACSRobotGroup.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_ACSRobotGroup.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_ACSRobotGroup.Columns["DGV_ACSRobotGroup_Use"].HeaderText = "Use" + "\r\n" + "Unuse";
            dtg_ACSRobotGroup.Rows.Clear();
        }

        private void ACSRobotGroup_Display()
        {
            dtg_ACSRobotGroup.Columns["DGV_ACSRobotGroup_Id"].ReadOnly = true;
            dtg_ACSRobotGroup.Columns["DGV_ACSRobotGroup_Use"].ReadOnly = true;

            int firstRowIndex = dtg_ACSRobotGroup.FirstDisplayedScrollingColumnIndex;
            dtg_ACSRobotGroup.CurrentCell = null;
            dtg_ACSRobotGroup.Rows.Clear();
            dtg_ACSRobotGroup.RowTemplate.Height = 40;

            foreach (var aCSRobotGroup in uow.ACSRobotGroups.GetAll())
            {
                int newRowIndex = dtg_ACSRobotGroup.Rows.Add();
                var newRow = dtg_ACSRobotGroup.Rows[newRowIndex];
                newRow.Cells["DGV_ACSRobotGroup_Id"].Value = aCSRobotGroup.Id.ToString();
                newRow.Cells["DGV_ACSRobotGroup_Use"].Value = aCSRobotGroup.GroupUse;
                newRow.Cells["DGV_ACSRobotGroup_Name"].Value = aCSRobotGroup.GroupName;
                newRow.Tag = aCSRobotGroup;
                if (aCSRobotGroup.GroupUse == "Unuse")
                {
                    dtg_ACSRobotGroup.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                    dtg_ACSRobotGroup.Rows[newRowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
                else
                {
                    dtg_ACSRobotGroup.Rows[newRowIndex].DefaultCellStyle.BackColor = main.skinColor;
                    dtg_ACSRobotGroup.Rows[newRowIndex].DefaultCellStyle.ForeColor = Color.White;
                }
            }

            if (firstRowIndex >= 0 && firstRowIndex < dtg_ACSRobotGroup.Rows.Count)
            {
                dtg_ACSRobotGroup.FirstDisplayedScrollingRowIndex = firstRowIndex;
            }
            dtg_ACSRobotGroup.ClearSelection();
        }

        private void ACSChargerCountConfigInit()
        {
            dtg_ACSChargerCountConfig.ScrollBars = ScrollBars.Both;
            dtg_ACSChargerCountConfig.AllowUserToResizeColumns = true;
            dtg_ACSChargerCountConfig.ColumnHeadersHeight = 70;
            dtg_ACSChargerCountConfig.RowTemplate.Height = 70;
            dtg_ACSChargerCountConfig.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_ACSChargerCountConfig.Columns[0].CellTemplate;
            dtg_ACSChargerCountConfig.Columns.Clear();
            dtg_ACSChargerCountConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_ACSChergerCount_Id", HeaderText = "Index", Width = 50, CellTemplate = currentCellTemplate });
            dtg_ACSChargerCountConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_ACSChergerCount_Use", HeaderText = "Use/UnUse", Width = 70, CellTemplate = currentCellTemplate });
            //dtg_ACSChargerCountConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_ACSChergerCount_RobotGroupName", HeaderText = "RobotGroupName", Width = 100, CellTemplate = currentCellTemplate });
            //dtg_ACSChargerCountConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_ACSChergerCount_FloorName", HeaderText = "FloorName", Width = 70, CellTemplate = currentCellTemplate });
            dtg_ACSChargerCountConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_ACSChergerCount_ChargerGroupName", HeaderText = "FloorName", Width = 100, CellTemplate = currentCellTemplate });
            dtg_ACSChargerCountConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_ACSChergerCount_ChargerCount", HeaderText = "ChargerCount", Width = 80, CellTemplate = currentCellTemplate });
            dtg_ACSChargerCountConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_ACSChergerCount_ChargerCountStatus", HeaderText = "ChargerCountStatus", Width = 80, CellTemplate = currentCellTemplate });
            dtg_ACSChargerCountConfig.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_ACSChargerCountConfig.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_ACSChargerCountConfig.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_ACSChargerCountConfig.Columns["DGV_ACSChergerCount_Use"].HeaderText = "Use" + "\r\n" + "Unuse";
            //dtg_ACSChargerCountConfig.Columns["DGV_ACSChergerCount_RobotGroupName"].HeaderText = "Robot" + "\r\n" + "Group" + "\r\n" + "Name";
            dtg_ACSChargerCountConfig.Columns["DGV_ACSChergerCount_ChargerGroupName"].HeaderText = "Charger" + "\r\n" + "Group" + "\r\n" + "Name";
            //dtg_ACSChargerCountConfig.Columns["DGV_ACSChergerCount_FloorName"].HeaderText = "Floor" + "\r\n" + "Name";
            dtg_ACSChargerCountConfig.Columns["DGV_ACSChergerCount_ChargerCount"].HeaderText = "Charger" + "\r\n" + "Count";
            dtg_ACSChargerCountConfig.Columns["DGV_ACSChergerCount_ChargerCountStatus"].HeaderText = "Charger" + "\r\n" + "Count" + "\r\n" + "Status";

            dtg_ACSChargerCountConfig.Rows.Clear();
        }

        private void ACSChargerCountConfigDisplay()
        {

            dtg_ACSChargerCountConfig.Columns["DGV_ACSChergerCount_Id"].ReadOnly = true;
            dtg_ACSChargerCountConfig.Columns["DGV_ACSChergerCount_Use"].ReadOnly = true;
            //dtg_ACSChargerCountConfig.Columns["DGV_ACSChergerCount_RobotGroupName"].ReadOnly = true;
            //dtg_ACSChargerCountConfig.Columns["DGV_ACSChergerCount_FloorName"].ReadOnly = true;
            dtg_ACSChargerCountConfig.Columns["DGV_ACSChergerCount_ChargerCount"].ReadOnly = true;
            dtg_ACSChargerCountConfig.Columns["DGV_ACSChergerCount_ChargerCountStatus"].ReadOnly = true;

            int firstRowIndex = dtg_ACSChargerCountConfig.FirstDisplayedScrollingColumnIndex;
            dtg_ACSChargerCountConfig.CurrentCell = null;
            dtg_ACSChargerCountConfig.Rows.Clear();
            dtg_ACSChargerCountConfig.RowTemplate.Height = 40;

            foreach (var aCSChargerCountConfig in uow.ACSChargerCountConfigs.GetAll())
            {
                int newRowIndex = dtg_ACSChargerCountConfig.Rows.Add();
                var newRow = dtg_ACSChargerCountConfig.Rows[newRowIndex];
                newRow.Cells["DGV_ACSChergerCount_Id"].Value = aCSChargerCountConfig.Id.ToString();
                newRow.Cells["DGV_ACSChergerCount_Use"].Value = aCSChargerCountConfig.ChargerCountUse;
                //newRow.Cells["DGV_ACSChergerCount_RobotGroupName"].Value = aCSChargerCountConfig.RobotGroupName;
                //newRow.Cells["DGV_ACSChergerCount_FloorName"].Value = aCSChargerCountConfig.FloorName;
                newRow.Cells["DGV_ACSChergerCount_ChargerGroupName"].Value = aCSChargerCountConfig.ChargerGroupName;
                newRow.Cells["DGV_ACSChergerCount_ChargerCount"].Value = aCSChargerCountConfig.ChargerCount.ToString();
                newRow.Cells["DGV_ACSChergerCount_ChargerCountStatus"].Value = aCSChargerCountConfig.ChargerCountStatus.ToString();
                newRow.Tag = aCSChargerCountConfig;
                if (aCSChargerCountConfig.ChargerCountUse == "Unuse")
                {
                    dtg_ACSChargerCountConfig.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                    dtg_ACSChargerCountConfig.Rows[newRowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
                else
                {
                    dtg_ACSChargerCountConfig.Rows[newRowIndex].DefaultCellStyle.BackColor = main.skinColor;
                    dtg_ACSChargerCountConfig.Rows[newRowIndex].DefaultCellStyle.ForeColor = Color.White;
                }
            }

            if (firstRowIndex >= 0 && firstRowIndex < dtg_ACSChargerCountConfig.Rows.Count)
            {
                dtg_ACSChargerCountConfig.FirstDisplayedScrollingRowIndex = firstRowIndex;
            }
            dtg_ACSChargerCountConfig.ClearSelection();
        }

        private void RobotControlInit()
        {
            dtg_RobotControl.ScrollBars = ScrollBars.Both;
            dtg_RobotControl.AllowUserToResizeColumns = true;
            dtg_RobotControl.ColumnHeadersHeight = 40;
            dtg_RobotControl.RowTemplate.Height = 80;
            dtg_RobotControl.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_RobotControl.Columns[0].CellTemplate;
            dtg_RobotControl.Columns.Clear();
            dtg_RobotControl.Columns.Add(new DataGridViewColumn() { Name = "DGV_RobotControl_Group", HeaderText = "RobotGroup", Width = 120, CellTemplate = currentCellTemplate });
            dtg_RobotControl.Columns.Add(new DataGridViewColumn() { Name = "DGV_RobotControl_ID", HeaderText = "RobotID", Width = 100, CellTemplate = currentCellTemplate });
            dtg_RobotControl.Columns.Add(new DataGridViewColumn() { Name = "DGV_RobotControl_Name", HeaderText = "RobotName", Width = 120, CellTemplate = currentCellTemplate });
            dtg_RobotControl.Columns.Add(new DataGridViewColumn() { Name = "DGV_RobotControl_IpAddress", HeaderText = "IpAddress", Width = 150, CellTemplate = currentCellTemplate });
            dtg_RobotControl.Columns.Add(new DataGridViewColumn() { Name = "DGV_RobotControl_Alias", HeaderText = "RobotAlias", Width = 100, CellTemplate = currentCellTemplate });
            dtg_RobotControl.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "DGV_RobotControl_Active", HeaderText = "RobotActive", Width = 120 });
            dtg_RobotControl.Columns.Add(new DataGridViewButtonColumn() { Name = "DGV_RobotControl_JobReset", HeaderText = "JobReset", Text = "JobReset", Width = 120 });
            dtg_RobotControl.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_RobotControl.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_RobotControl.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_RobotControl.Columns["DGV_RobotControl_Alias"].Visible = false;                                      //사용안함
            //dtg_RobotControl.Columns["DGV_RobotControl_JobReset"].Visible = false;                                   //사용안함
            dtg_RobotControl.Rows.Clear();
        }

        private void RobotControlDisplay()
        {
            try
            {
                dtg_RobotControl.Columns["DGV_RobotControl_Group"].ReadOnly = true; //편집사용 불가 . 클릭시 팝업창에서 편집
                dtg_RobotControl.Columns["DGV_RobotControl_ID"].ReadOnly = true; //편집사용 불가.  클릭시 팝업창에서 편집.
                dtg_RobotControl.Columns["DGV_RobotControl_Name"].ReadOnly = true; //편집사용 불가.  클릭시 팝업창에서 편집.
                dtg_RobotControl.Columns["DGV_RobotControl_IpAddress"].ReadOnly = false; //편집사용 불가.  클릭시 팝업창에서 편집.
                //dtg_RobotControl.Columns["DGV_RobotControl_Alias"].ReadOnly = false; //편집사용.  클릭시 직접 편집.
                dtg_RobotControl.Columns["DGV_RobotControl_Active"].ReadOnly = true; //편집사용 불가.  클릭시 팝업창에서 편집.

                int firstRowIndex = dtg_RobotControl.FirstDisplayedScrollingColumnIndex;
                dtg_RobotControl.CurrentCell = null;
                dtg_RobotControl.Rows.Clear();
                dtg_RobotControl.RowTemplate.Height = 60;

                foreach (var robot in uow.Robots.GetAll().OrderBy(i => i.RobotID))
                {
                    int newRowIndex = dtg_RobotControl.Rows.Add();
                    var newRow = dtg_RobotControl.Rows[newRowIndex];

                    newRow.Cells["DGV_RobotControl_Group"].Value = robot.ACSRobotGroup;
                    newRow.Cells["DGV_RobotControl_ID"].Value = robot.RobotID;
                    newRow.Cells["DGV_RobotControl_Name"].Value = robot.RobotName;
                    newRow.Cells["DGV_RobotControl_IpAddress"].Value = robot.RobotIp;
                    //newRow.Cells["DGV_RobotControl_Alias"].Value = robot.RobotAlias;
                    newRow.Cells["DGV_RobotControl_Active"].Value = robot.ACSRobotActive;
                    newRow.Cells["DGV_RobotControl_JobReset"].Value = "JobReset";
                    newRow.Tag = robot;
                }
                if (firstRowIndex >= 0 && firstRowIndex < dtg_RobotControl.Rows.Count)
                {
                    dtg_RobotControl.FirstDisplayedScrollingRowIndex = firstRowIndex;
                }
                dtg_RobotControl.ClearSelection();
            }
            catch (Exception ex)
            {
                main.LogExceptionMessage(ex);
            }
        }

        private void RobotFloorMapIdInit()
        {
            dtg_RobotFloorMapID.ScrollBars = ScrollBars.Both;
            dtg_RobotFloorMapID.AllowUserToResizeColumns = true;
            dtg_RobotFloorMapID.ColumnHeadersHeight = 40;
            dtg_RobotFloorMapID.RowTemplate.Height = 40;
            dtg_RobotFloorMapID.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_RobotFloorMapID.Columns[0].CellTemplate;
            dtg_RobotFloorMapID.Columns.Clear();
            dtg_RobotFloorMapID.Columns.Add(new DataGridViewColumn() { Name = "DGV_RobotFloorMapID_Index", HeaderText = "Index", Width = 100, CellTemplate = currentCellTemplate });
            dtg_RobotFloorMapID.Columns.Add(new DataGridViewColumn() { Name = "DGV_RobotFloorMapID_FloorName", HeaderText = "FloorName", Width = 200, CellTemplate = currentCellTemplate });
            dtg_RobotFloorMapID.Columns.Add(new DataGridViewColumn() { Name = "DGV_RobotFloorMapID_FloorMapID", HeaderText = "FloorMapID", Width = 430, CellTemplate = currentCellTemplate });
            dtg_RobotFloorMapID.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_RobotFloorMapID.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_RobotFloorMapID.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_RobotFloorMapID.Rows.Clear();
        }

        private void RobotFloorMapIdDisplay()
        {

            dtg_RobotFloorMapID.CurrentCell = null;
            dtg_RobotFloorMapID.ClearSelection();
            dtg_RobotFloorMapID.Rows.Clear();
            dtg_RobotFloorMapID.RowTemplate.Height = 60;
            int firstRowIndex = dtg_RobotFloorMapID.FirstDisplayedScrollingColumnIndex;

            foreach (var floorMapIDConfig in uow.FloorMapIDConfigs.GetAll())
            {

                int newRowIndex = dtg_RobotFloorMapID.Rows.Add();
                var newRow = dtg_RobotFloorMapID.Rows[newRowIndex];
                newRow.Cells["DGV_RobotFloorMapID_Index"].Value = floorMapIDConfig.Id;
                newRow.Cells["DGV_RobotFloorMapID_FloorName"].Value = floorMapIDConfig.FloorName;
                newRow.Cells["DGV_RobotFloorMapID_FloorMapID"].Value = floorMapIDConfig.MapID;
                newRow.Tag = floorMapIDConfig;
            }

            if (firstRowIndex >= 0 && firstRowIndex < dtg_RobotFloorMapID.Rows.Count)
            {
                dtg_RobotFloorMapID.FirstDisplayedScrollingRowIndex = firstRowIndex;
            }
            dtg_RobotFloorMapID.ClearSelection();
        }

        private void dtg_ACSRobotGroup_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

            ACSRobotGroupConfigModel selectedRowTag = (ACSRobotGroupConfigModel)selectedRow.Tag;
            ACSRobotGroupConfigModel targetConfig = uow.ACSRobotGroups.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

            string ChangeDatamsg = null;

            if (targetConfig != null)
            {

                if (e.ColumnIndex == grid.Columns["DGV_ACSRobotGroup_Use"].Index)
                {
                    var insertUse = new UseSelectForm();
                    if (insertUse.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertUse.InputValue.Trim();

                        if (targetConfig.GroupUse != newValue)
                        {
                            string oldValue = targetConfig.GroupUse;

                            targetConfig.GroupUse = newValue;

                            insertUse.Close();
                            ChangeDatamsg = $"RobotControl , ACSRobotGroup Configs{targetConfig.Id} GroupUse Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                            if (targetConfig.GroupUse == "Unuse")
                            {
                                dtg_ACSRobotGroup.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                                dtg_ACSRobotGroup.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                            }
                            else
                            {
                                dtg_ACSRobotGroup.Rows[e.RowIndex].DefaultCellStyle.BackColor = main.skinColor;
                                dtg_ACSRobotGroup.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                            }
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                ACSRobotGroup_Display();
            }
            void ChageData(string newValue, string changeDatamsg)
            {
                if (changeDatamsg != null)
                {
                    selectedCell.Value = newValue;
                    selectedRowTag = targetConfig;
                    uow.ACSRobotGroups.Update(targetConfig);
                    string[] ChangeDatamsgSplit = ChangeDatamsg.Split(',');
                    main.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                }
            }

        }

        private void dtg_ACSRobotGroup_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

                DataGridView grid = (DataGridView)sender;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

                ACSRobotGroupConfigModel selectedRowTag = (ACSRobotGroupConfigModel)selectedRow.Tag;
                ACSRobotGroupConfigModel targetConfig = uow.ACSRobotGroups.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

                string ChangeDatamsg = null;

                //Convert.ToString 사용시 변수가 null인경우 빈문자열을 반환함 
                //.Tostring() 사용시 변수가 null인경우 에러발생
                if (!string.IsNullOrEmpty(Convert.ToString(selectedCell.Value)) && targetConfig != null)
                {
                    string newValue = String.Concat(selectedCell.Value.ToString().Where(c => !char.IsWhiteSpace(c)));    //문자열 빈칸없애기

                    if (e.ColumnIndex == grid.Columns["DGV_ACSRobotGroup_Name"].Index)
                    {
                        //selectedCell.Value = String.Concat(selectedCell.Value.ToString().Where(c => !char.IsWhiteSpace(c)));    //문자열 빈칸없애기
                        //string newValue = selectedCell.Value.ToString();
                        if (newValue.Contains("_"))
                        {
                            MessageBox.Show(" '_ '문자를 사용하실수 없습니다 확인후 다시 등록해주세요!!");
                            ACSRobotGroup_Display();
                        }
                        else
                        {

                            if (targetConfig.GroupName != newValue)
                            {
                                string oldValue = targetConfig.GroupName;
                                targetConfig.GroupName = newValue;
                                ChangeDatamsg = $"RobotControl,ACSRobotGroup Config{targetConfig.Id} ACSRobotGroup GroupName Change from {oldValue} to {newValue}";
                                ChageData(newValue, ChangeDatamsg);

                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                    ACSRobotGroup_Display();
                }

                void ChageData(string newValue, string changeDatamsg)
                {
                    if (changeDatamsg != null)
                    {
                        selectedCell.Value = newValue;
                        selectedRowTag = targetConfig;
                        uow.ACSRobotGroups.Update(targetConfig);
                        string[] ChangeDatamsgSplit = ChangeDatamsg.Split(',');
                        main.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                        MessageBox.Show("저장되었습니다");

                    }
                }
            }
            catch (Exception ex)
            {
                main.LogExceptionMessage(ex);
            }
        }

        private void dtg_RobotControl_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

            //Robot targetRobot = uow.Robots[e.RowIndex];

            Robot selectedRowTag = (Robot)selectedRow.Tag;
            Robot targetRobot = uow.Robots.Find(m => m.RobotID == selectedRowTag.RobotID).FirstOrDefault();

            string ChangeDatamsg = null;

            if (targetRobot != null)
            {

                if (e.ColumnIndex == grid.Columns["DGV_RobotControl_Group"].Index) //Robot ACSRobotGroup
                {
                    var insertNum = new GroupSelectForm(main, uow);
                    if (insertNum.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertNum.InputValue.Trim();

                        if (targetRobot.ACSRobotGroup != newValue)
                        {
                            string oldValue = targetRobot.ACSRobotGroup;
                            targetRobot.ACSRobotGroup = newValue;
                            insertNum.Close();
                            ChangeDatamsg = $"RobotControl,RobotControl {targetRobot.RobotName} Robot ACSRobotGroup from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }

                //else if (e.ColumnIndex == grid.Columns["DGV_Control_Setting_Button"].Index) //Reset Mission Button
                //{
                //    DialogResult result = MessageBox.Show(((DataGridView)sender)[e.ColumnIndex - 1, e.RowIndex].Value + " MiR Mission Reset 하시겠습니까 ? ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                //    if (result == DialogResult.Yes)
                //    {
                //        ConfigData.sReset_MiR_No = ((DataGridView)sender)[e.ColumnIndex - 2, e.RowIndex].Value.ToString();
                //        ConfigData.sReset_MiR_Name = ((DataGridView)sender)[e.ColumnIndex - 1, e.RowIndex].Value.ToString();
                //        mainForm.UserLog("MiRControl Screen", "MiR Reset Control CellMouseClick " + "sReset_MiR_No = " + ConfigData.sReset_MiR_No + "sReset_MiR_Name = " + ConfigData.sReset_MiR_Name);
                //    }
                //}

                else if (e.ColumnIndex == grid.Columns["DGV_RobotControl_Active"].Index) //Robot Active 미션전송 가능 (true) 미션전송 불가능 (false)
                {
                    //bool oldValue = Convert.ToBoolean(selectedCell.Value);
                    //bool newValue = !oldValue;
                    bool newValue = !Convert.ToBoolean(selectedRow.Cells["DGV_RobotControl_Active"].Value);

                    if (targetRobot.ACSRobotActive != newValue)
                    {
                        bool oldValue = targetRobot.ACSRobotActive;
                        targetRobot.ACSRobotActive = newValue;
                        ChangeDatamsg = $"RobotControl,RobotControl {targetRobot.RobotName} Robot ACSRobotActive from {oldValue} to {newValue}";
                        ChageData(newValue.ToString(), ChangeDatamsg);
                    }
                }
                else if (e.ColumnIndex == grid.Columns["DGV_RobotControl_JobReset"].Index) //Robot Active 미션전송 가능 (true) 미션전송 불가능 (false)
                {
                    DialogResult result = MessageBox.Show($"{targetRobot.RobotName}" + " Robot Job Reset 하시겠습니까 ? ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        ConfigData.jobResetRobotNo = targetRobot.RobotID.ToString();
                        ConfigData.jobResetRobotName = targetRobot.RobotName;
                        main.UserLog("RobotControl", $"job Reset Control CellMouseClick jobResetRobotNo = {ConfigData.jobResetRobotNo}, jobResetRobotName ={ConfigData.jobResetRobotName}");
                    }
                }
            }
            else
            {
                MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                RobotControlDisplay();
            }
            void ChageData(string newValue, string changeDatamsg)
            {
                if (changeDatamsg != null)
                {
                    selectedCell.Value = newValue;
                    selectedRowTag = targetRobot;
                    uow.Robots.Update(targetRobot);
                    string[] ChangeDatamsgSplit = ChangeDatamsg.Split(',');
                    main.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                }
            }

        }

        private void dtg_RobotControl_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

                DataGridView grid = (DataGridView)sender;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

                //Robot targetRobot = uow.Robots[e.RowIndex];

                Robot selectedRowTag = (Robot)selectedRow.Tag;
                Robot targetRobot = uow.Robots.Find(m => m.RobotID == selectedRowTag.RobotID).FirstOrDefault();

                string ChangeDatamsg = null;

                //Convert.ToString 사용시 변수가 null인경우 빈문자열을 반환함 
                //.Tostring() 사용시 변수가 null인경우 에러발생
                if (!string.IsNullOrEmpty(Convert.ToString(selectedCell.Value)) && targetRobot != null)
                {
                    //selectedCell.Value = String.Concat(selectedCell.Value.ToString().Where(c => !char.IsWhiteSpace(c)));    //문자열 빈칸없애기
                    //string newValue = selectedCell.Value.ToString();

                    // update cell value
                    string newValue = String.Concat(selectedCell.Value.ToString().Where(c => !char.IsWhiteSpace(c)));    //문자열 빈칸없애기

                    // update robot alias
                    if (e.ColumnIndex == grid.Columns["DGV_RobotControl_Alias"].Index)
                    {
                        if (targetRobot.RobotAlias != newValue)
                        {
                            string oldValue = targetRobot.RobotAlias;
                            string dataKey = "RobotAlias";
                            ChangeDatamsg = $"RobotControl,Robot Alias changed from {oldValue} to {newValue}";

                            ChageData(dataKey, newValue, ChangeDatamsg);
                        }
                    }

                    // update robot ip address
                    else if (e.ColumnIndex == grid.Columns["DGV_RobotControl_IpAddress"].Index)
                    {
                        if (IPAddress.TryParse(newValue, out IPAddress parsedIpAddress)) // check ip address
                        {
                            string newIpAddress = parsedIpAddress.ToString();
                            string oldIpAddress = uow.Robots.FindIpAddress(targetRobot.Id);

                            if (oldIpAddress != newIpAddress)
                            {
                                string datakey = "IpAddress";
                                ChangeDatamsg = $"RobotControl, Robot IpAddress Change from {oldIpAddress} to {newIpAddress}";
                                ChageData(datakey, newIpAddress, ChangeDatamsg);
                            }
                        }
                        else
                        {
                            selectedCell.Value = newValue;
                            MessageBox.Show("잘못된 IP 주소입니다 " + "\r\n" + "확인후 입력해주시기 바랍니다.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                    RobotControlDisplay();
                }

                void ChageData(string dataKey, string newValue, string changeDatamsg)
                {
                    if (changeDatamsg != null)
                    {
                        switch (dataKey)
                        {
                            case "RobotAlias":
                                targetRobot.RobotAlias = newValue;
                                selectedCell.Value = newValue;
                                selectedRowTag = targetRobot;
                                uow.Robots.Update(targetRobot);
                                MessageBox.Show("저장되었습니다");
                                break;

                            case "IpAddress":
                                //targetRobot.RobotIp = newValue;
                                selectedCell.Value = newValue;
                                selectedRowTag = targetRobot;
                                //===== ip 변경사항 업데이트b
                                targetRobot.RobotIp = newValue;
                                uow.Robots.UpdateIpAddress(targetRobot.Id, newValue);
                                AppConfiguration.ConfigDataSetting($"RobotIPAddress{selectedRow.Index + 1}", newValue);
                                //===== ip 변경사항 업데이트e
                                MessageBox.Show("저장되었습니다");
                                lal_IpAdressChangeMsg.Visible = true;
                                break;
                        }

                        string[] ChangeDatamsgSplit = ChangeDatamsg.Split(',');
                        main.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                main.LogExceptionMessage(ex);
            }
        }

        private void dtg_RobotFloorMapId_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

                DataGridView grid = (DataGridView)sender;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

                //Robot targetRobot = uow.Robots[e.RowIndex];

                FloorMapIdConfigModel selectedRowTag = (FloorMapIdConfigModel)selectedRow.Tag;
                FloorMapIdConfigModel targetConfig = uow.FloorMapIDConfigs.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

                string ChangeDatamsg = null;

                //Convert.ToString 사용시 변수가 null인경우 빈문자열을 반환함 
                //.Tostring() 사용시 변수가 null인경우 에러발생
                if (!string.IsNullOrEmpty(Convert.ToString(selectedCell.Value)) && targetConfig != null)
                {
                    // update cell value
                    //selectedCell.Value = String.Concat(selectedCell.Value.ToString().Where(c => !char.IsWhiteSpace(c)));    //문자열 빈칸없애기
                    //string newValue = selectedCell.Value.ToString();

                    string newValue = String.Concat(selectedCell.Value.ToString().Where(c => !char.IsWhiteSpace(c)));    //문자열 빈칸없애기

                    // update robot alias
                    if (e.ColumnIndex == grid.Columns["DGV_RobotFloorMapID_FloorName"].Index)
                    {
                        if (newValue.Contains("_"))
                        {
                            MessageBox.Show(" '_ '문자를 사용하실수 없습니다 확인후 다시 등록해주세요!!");
                            RobotFloorMapIdDisplay();
                        }
                        else
                        {
                            if (targetConfig.FloorName != newValue)
                            {
                                string oldValue = targetConfig.FloorName;
                                targetConfig.FloorName = newValue;
                                ChangeDatamsg = $"RobotControl,Floor Name changed from {oldValue} to {newValue}.";
                                ChageData(newValue, ChangeDatamsg);
                            }
                        }
                    }

                    else if (e.ColumnIndex == grid.Columns["DGV_RobotFloorMapID_FloorMapID"].Index)
                    {
                        if (targetConfig.MapID != newValue)
                        {
                            string oldValue = targetConfig.MapID;
                            targetConfig.MapID = newValue;
                            ChangeDatamsg = $"RobotControl,Floor MapID changed from {oldValue} to {newValue}.";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                    RobotFloorMapIdDisplay();
                }

                void ChageData(string newValue, string changeDatamsg)
                {
                    if (changeDatamsg != null)
                    {
                        selectedCell.Value = newValue;
                        selectedRowTag = targetConfig;
                        uow.FloorMapIDConfigs.Update(targetConfig);

                        var posMapID = uow.PositionAreaConfigs.Find(m => !string.IsNullOrWhiteSpace(m.PositionAreaFloorName) && m.PositionAreaFloorName == targetConfig.FloorName);
                        foreach (var model in posMapID)
                        {
                            model.PositionAreaFloorMapId = targetConfig.MapID;
                            uow.PositionAreaConfigs.Update(model);
                        }

                        //var chargerMapIDs = uow.ACSChargerCountConfig.Find(a => !string.IsNullOrWhiteSpace(a.FloorName) && a.FloorName == targetConfig.FloorName);
                        //foreach (var chargerMapID in chargerMapIDs)
                        //{
                        //    chargerMapID.FloorMapId = targetConfig.MapID;
                        //    uow.ACSChargerCountConfig.Update(chargerMapID);
                        //}

                        string[] ChangeDatamsgSplit = ChangeDatamsg.Split(',');
                        main.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                        MessageBox.Show("저장되었습니다");

                    }
                }

            }
            catch (Exception ex)
            {
                main.LogExceptionMessage(ex);
            }
        }

        private void dtg_ACSChargerCountConfig_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

            ACSChargerCountConfigModel selectedRowTag = (ACSChargerCountConfigModel)selectedRow.Tag;
            ACSChargerCountConfigModel targetConfig = uow.ACSChargerCountConfigs.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

            string ChangeDatamsg = null;

            if (targetConfig != null)
            {
                if (e.ColumnIndex == grid.Columns["DGV_ACSChergerCount_Use"].Index)
                {
                    var insertUse = new UseSelectForm();
                    if (insertUse.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertUse.InputValue.Trim();

                        if (targetConfig.ChargerCountUse != newValue)
                        {
                            string oldValue = targetConfig.ChargerCountUse;

                            targetConfig.ChargerCountUse = newValue;

                            insertUse.Close();
                            ChangeDatamsg = $"RobotControl , ACSChargerCountConfig Configs{targetConfig.Id} ChargerUse Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);

                            if (targetConfig.ChargerCountUse == "Unuse")
                            {
                                dtg_ACSChargerCountConfig.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                                dtg_ACSChargerCountConfig.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                            }
                            else
                            {
                                dtg_ACSChargerCountConfig.Rows[e.RowIndex].DefaultCellStyle.BackColor = main.skinColor;
                                dtg_ACSChargerCountConfig.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                            }
                        }
                    }
                }
                //else if (e.ColumnIndex == grid.Columns["DGV_ACSChergerCount_RobotGroupName"].Index)
                //{
                //    var insertNum = new GroupSelectForm(mainForm, uow);
                //    if (insertNum.ShowDialog() == DialogResult.OK)
                //    {
                //        string newValue = insertNum.InputValue.Trim();

                //        if (targetConfig.RobotGroupName != newValue)
                //        {
                //            string oldValue = targetConfig.RobotGroupName;
                //            targetConfig.RobotGroupName = newValue;
                //            insertNum.Close();
                //            ChangeDatamsg = $"RobotControl , ACSChargerCountConfig Configs{targetConfig.Id} RobotGroupName Change from {oldValue} to {newValue}";
                //            ChageData(newValue, ChangeDatamsg);
                //        }
                //    }
                //}
                //else if (e.ColumnIndex == grid.Columns["DGV_ACSChergerCount_FloorName"].Index)
                //{
                //    var insertFloor = new FloorMapIdSelectForm(uow);
                //    if (insertFloor.ShowDialog() == DialogResult.OK)
                //    {
                //        string newValue = insertFloor.InputValue.Trim();

                //        var positionAreaFloorMapId = uow.FloorMapIDConfig.Find(m => m.FloorName == newValue).FirstOrDefault();
                //        if (positionAreaFloorMapId != null && targetConfig.FloorName != newValue)
                //        {
                //            string oldValue = targetConfig.FloorName;
                //            targetConfig.FloorName = newValue;
                //            targetConfig.FloorMapId = positionAreaFloorMapId.MapID;

                //            insertFloor.Close();
                //            ChangeDatamsg = $"RobotControl , ACSChargerCountConfig Configs{targetConfig.Id} FloorName Change from {oldValue} to {newValue}";
                //            ChageData(newValue, ChangeDatamsg);

                //        }
                //    }
                //}


                else if (e.ColumnIndex == grid.Columns["DGV_ACSChergerCount_ChargerGroupName"].Index)
                {
                    var insertNum = new GroupSelectForm(main, uow);
                    if (insertNum.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertNum.InputValue.Trim();

                        if (targetConfig.ChargerGroupName != newValue)
                        {
                            string oldValue = targetConfig.ChargerGroupName;
                            targetConfig.ChargerGroupName = newValue;
                            insertNum.Close();
                            ChangeDatamsg = $"RobotControl , ACSChargerCountConfig Configs{targetConfig.Id} ChargerGroupName Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }
                else if (e.ColumnIndex == grid.Columns["DGV_ACSChergerCount_ChargerCount"].Index)
                {
                    NumPadForm insert = new NumPadForm(dtg_ACSChargerCountConfig.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dtg_ACSChargerCountConfig", "RT15");
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (Convert.ToString(targetConfig.ChargerCount) != newValue)
                        {
                            string oldValue = Convert.ToString(targetConfig.ChargerCount);
                            targetConfig.ChargerCount = int.Parse(newValue);
                            insert.Close();
                            ChangeDatamsg = $"RobotControl , ACSChargerCountConfig Configs{targetConfig.Id} ChargerCount Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);

                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                ACSChargerCountConfigDisplay();
            }
            void ChageData(string newValue, string changeDatamsg)
            {
                if (changeDatamsg != null)
                {
                    selectedCell.Value = newValue;
                    selectedRowTag = targetConfig;
                    uow.ACSChargerCountConfigs.Update(targetConfig);
                    string[] ChangeDatamsgSplit = ChangeDatamsg.Split(',');
                    main.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                }
            }
        }

        private void txt_Fleet_ResponseTime_Click(object sender, EventArgs e)
        {
            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, ((TextBox)sender).AccessibleDescription);
            if (insertNum.ShowDialog() == DialogResult.OK)
            {
                string newValue = int.Parse(insertNum.InputValue).ToString();
                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;

                    ((TextBox)sender).Text = newValue;
                    AppConfiguration.ConfigDataSetting("sFleet_ResponseTime", newValue);
                    ConfigData.sFleet_ResponseTime = AppConfiguration.GetAppConfig("sFleet_ResponseTime");
                    insertNum.Close();

                    main.UserLog("RobotControl", $"Fleet_ResponseTime changed from {oldValue} to {newValue}.");
                }
            }
        }

        private void txt_Robot_MaxNum_Click(object sender, EventArgs e)
        {
            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, "RT31");
            if (insertNum.ShowDialog() == DialogResult.OK)
            {
                string newValue = int.Parse(insertNum.InputValue).ToString();
                if (((TextBox)sender).Text != newValue)
                {
                    DialogResult results = MessageBox.Show("프로그램이 종료됩니다 다시 프로그램을 실행해주세요", "MissionConfigs Count", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (results == DialogResult.Yes)
                    {
                        string oldValue = ((TextBox)sender).Text;

                        ((TextBox)sender).Text = newValue;
                        AppConfiguration.ConfigDataSetting("MiR_MaxNum", newValue);
                        ConfigData.MiR_MaxNum = Convert.ToInt32(AppConfiguration.GetAppConfig("MiR_MaxNum"));
                        insertNum.Close();

                        //=======Robot 관련된 모든 항목을 삭제한다
                        uow.Robots.Delete();
                        uow.Jobs.Delete();
                        uow.Missions.Delete();
                        //========================================

                        main.UserLog("RobotControl", $"Robot MaxNum changed from {oldValue} to {newValue} 프로그램 종료.");
                        Application.Exit();
                    }
                }
            }
        }

        private void txt_FloorMapID_MaxNum_Click(object sender, EventArgs e)
        {
            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, "RT31");
            if (insertNum.ShowDialog() == DialogResult.OK)
            {
                string newValue = int.Parse(insertNum.InputValue).ToString();
                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;

                    ((TextBox)sender).Text = newValue;
                    AppConfiguration.ConfigDataSetting("FloorMapID_MaxNum", newValue);
                    ConfigData.FloorMapID_MaxNum = Convert.ToInt32(AppConfiguration.GetAppConfig("FloorMapID_MaxNum"));
                    insertNum.Close();
                    uow.FloorMapIDConfigs.Validate_DB_Items();
                    RobotFloorMapIdDisplay();
                    main.UserLog("RobotControl", $"FloorMapID_MaxNum changed from {oldValue} to {newValue}.");

                }
            }
        }

        private void txt_ACSRobotGroupMaxNum_Click(object sender, EventArgs e)
        {
            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, "RT31");
            if (insertNum.ShowDialog() == DialogResult.OK)
            {
                string newValue = int.Parse(insertNum.InputValue).ToString();
                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;

                    ((TextBox)sender).Text = newValue;
                    AppConfiguration.ConfigDataSetting("ACSRobotGroup_MaxNum", newValue);
                    ConfigData.ACSRobotGroup_MaxNum = Convert.ToInt32(AppConfiguration.GetAppConfig("ACSRobotGroup_MaxNum"));
                    insertNum.Close();
                    uow.ACSRobotGroups.Validate_DB_Items();
                    ACSRobotGroup_Display();
                    main.UserLog("RobotControl", $"ACSRobotGroup_MaxNum changed from {oldValue} to {newValue}.");

                }
            }
        }

        private void txt_Fleet_IP_Address_Click(object sender, EventArgs e)
        {
            //Fleet통신방식일 경우 Fleet PC의 IP Address 설정
            IP_AddressBoard insertNum = new IP_AddressBoard();
            if (insertNum.ShowDialog() == DialogResult.OK)
            {
                string newValue = insertNum.Return_Value;
                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;

                    ((TextBox)sender).Text = newValue;
                    AppConfiguration.ConfigDataSetting("sFleet_IP_Address_SV", newValue);
                    ConfigData.sFleet_IP_Address_SV = AppConfiguration.GetAppConfig("sFleet_IP_Address_SV");

                    insertNum.Close();
                    main.UserLog("RobotControl", $"Fleet_IP_Address changed from {oldValue} to {newValue}.");
                }

            }
        }

        private void txt_ACSChargerCountMaxNum_Click(object sender, EventArgs e)
        {

            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, "RT31");
            if (insertNum.ShowDialog() == DialogResult.OK)
            {
                string newValue = int.Parse(insertNum.InputValue).ToString();
                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;

                    ((TextBox)sender).Text = newValue;
                    AppConfiguration.ConfigDataSetting("ACSChargerCount_MaxNum", newValue);
                    ConfigData.ACSChargerCount_MaxNum = Convert.ToInt32(AppConfiguration.GetAppConfig("ACSChargerCount_MaxNum"));
                    insertNum.Close();
                    uow.ACSChargerCountConfigs.Validate_DB_Items();
                    ACSChargerCountConfigDisplay();
                    main.UserLog("RobotControl", $"ACSChargerCount_MaxNum changed from {oldValue} to {newValue}.");

                }
            }
        }
     

        private void dtg_RobotControlMissingData()
        {
            //1.데이터 베이스를 지운후 다시 접속하였을때 데이터가 정상적으로 들어오지 않음 발견 
            //2.정상적인 데이터가 들어올수있도록 다시 Display함
            //string RobotId = Convert.ToString(dtg_RobotControl["DGV_RobotControl_ID", 0].Value);
            //string RobotName = Convert.ToString(dtg_RobotControl["DGV_RobotControl_Name", 0].Value);
            //if (string.IsNullOrEmpty(RobotId) || (RobotId == "0") && string.IsNullOrEmpty(RobotName)) RobotControlDisplay(); //Data정보가 표기않됨


            //Robot Id 경우 초기 ID값을 강제로 ACS 에서 입력함 조건을 잡을 필요없다고 판단
            //Robot Name 경우 초기에 못불러오는 경우가 발생! Robot 리스트와 일치하지 않은 데이터가 있을경우 다시 Display
            foreach (DataGridViewRow item in dtg_RobotControl.Rows)
            {
                string RobotName = Convert.ToString(dtg_RobotControl["DGV_RobotControl_Name", item.Index].Value);
                if (string.IsNullOrEmpty(RobotName))
                {
                    var RobotDataChange = uow.Robots.Find(x => x.RobotName == RobotName).FirstOrDefault();
                    if (RobotDataChange == null) RobotControlDisplay();
                }
            }
        }

        private void DisplayTimer_Tick(object sender, EventArgs e)
        {
            DisplayTimer.Enabled = false;
            dtg_RobotControlMissingData();
            ACSChargerCountConfigDisplay();
            DisplayTimer.Interval = 1000;
            DisplayTimer.Enabled = true;
        }

        private void btn_BackUpSaveFile_Click(object sender, EventArgs e)
        {
            DataGridView BackUpData = null;
            string ButtonName = ((Button)sender).Name;

            switch (ButtonName)
            {
                case "btn_ACSRobotGroupBackup":
                    if (dtg_ACSRobotGroup.Rows.Count == 0) return;
                    BackUpData = dtg_ACSRobotGroup;
                    break;

                case "btn_ACSChargerCountBackup":
                    if (dtg_ACSChargerCountConfig.Rows.Count == 0) return;
                    BackUpData = dtg_ACSChargerCountConfig;
                    break;
                case "btn_ACSRobotControlSettingBackUp":
                    if (dtg_RobotControl.Rows.Count == 0) return;
                    BackUpData = dtg_RobotControl;
                    break;
                case "btn_RobotFloorMapIDBackUp":
                    if (dtg_RobotFloorMapID.Rows.Count == 0) return;
                    BackUpData = dtg_RobotFloorMapID;
                    break;
            }
            if (BackUpData != null)
            {
                main.SaveAsDataGridviewToCSV(BackUpData);
                main.UserLog("RobotControl Screen", $"{ButtonName} BackUp Click ");
            }
        }
        private void txt_RobotResetMissionName_Click(object sender, EventArgs e)
        {
            var insertNum = new MissionSelectForm(main);
            if (insertNum.ShowDialog() == DialogResult.OK)
            {
                string newValue = insertNum.InputValue;
                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;

                    ((TextBox)sender).Text = newValue;
                    AppConfiguration.ConfigDataSetting("RobotResetMissionName", newValue);
                    ConfigData.RobotResetMissionName = AppConfiguration.GetAppConfig("RobotResetMissionName");
                    insertNum.Close();
                    main.UserLog("RobotControl", $"RobotResetMissionName changed from {oldValue} to {newValue}.");
                }
            }
        }

    }
}
