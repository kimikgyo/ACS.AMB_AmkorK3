using INA_ACS_Server.UI;
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
    public partial class ElevatorScreen : Form
    {
        private readonly MainForm mainForm;
        private readonly IUnitOfWork uow;


        public ElevatorScreen(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.uow = uow;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.None; //윈도우(상단) 테두리 제거 source code
            dtg_ElevatorStatusDisPlay.AlternatingRowsDefaultCellStyle = null;
            dtg_ElevatorStatusDisPlay.DoubleBuffered(true);
            ElevatorStatusDisPlayInit();
            ElevatorScreenTextBoxInit();
            ElevatorStatusDisPlay();
        }
        private void ElevatorScreenTextBoxInit()
        {
            txt_Elevator_IP_Address.Text = ConfigData.sElevator_IP_Address_SV;
            txt_Elevator_PortNumber.Text = ConfigData.sElevator_PortNumber;
            txt_M3F_ElevatorStartPOS.Text = ConfigData.M3F_ElevatorStartPOS;
            txt_M3F_ElevatorEnterPOS.Text = ConfigData.M3F_ElevatorEnterPOS;
            txt_M3F_ElevatorEnterPOS_1.Text = ConfigData.M3F_ElevatorEnterPOS;
            txt_M3F_ElevatorEndPOS.Text = ConfigData.M3F_ElevatorEndPOS;
            txt_T3F_ElevatorStartPOS.Text = ConfigData.T3F_ElevatorStartPOS;
            txt_T3F_ElevatorEnterPOS.Text = ConfigData.T3F_ElevatorEnterPOS;
            txt_T3F_ElevatorEnterPOS_1.Text = ConfigData.T3F_ElevatorEnterPOS;
            txt_T3F_ElevatorEndPOS.Text = ConfigData.T3F_ElevatorEndPOS;
        }
        private void LabalText()
        {
            label1.Text = "RobotName : Robot 이름을 표기합니다." + "\r\n" + "\r\n"
                         + "MiRStatusElevator : MiR Elevator 상태를 표기합니다 " + "\r\n" + "\r\n"
                         + "MiRStatusElevator : MiRStateElevatorLoaderStart     진입 시작 상태 입니다." + "\r\n" + "\r\n"
                         + "MiRStatusElevator : MiRStateElevatorLoaderComplet   진입 완료 상태 입니다." + "\r\n" + "\r\n"
                         + "MiRStatusElevator : MiRStateElevatorUnLoaderStart   진출 시작 상태 입니다." + "\r\n" + "\r\n"
                         + "MiRStatusElevator : MiRStateElevatorUnLoaderComplet 진출 완료 상태입니다." + "\r\n" + "\r\n"


                         + "ElevatorStatus : Elevator 상태를 표기합니다 " + "\r\n" + "\r\n"
                         + "ElevatorStatus : CallStartFloorStatus Elevator 호출 상태입니다." + "\r\n" + "\r\n"
                         + "ElevatorStatus : CallStartDoorOpen    Elevator 호출 층에서 DoorOpen 상태입니다." + "\r\n" + "\r\n"
                         + "ElevatorStatus : CallEndFloorSelect   Elevator 목적지층 선택 상태입니다." + "\r\n" + "\r\n"
                         + "ElevatorStatus : CallStartDoorClose   Elevator 호출 층에서 Door Close 상태입니다." + "\r\n" + "\r\n"
                         + "ElevatorStatus : CallEndDoorOpen      Elevator 목적지층 Door Open 상태 입니다." + "\r\n" + "\r\n"
                         + "ElevatorStatus : CallEndDoorClose     Elevator 목적지층 Door Close 상태 입니다." + "\r\n" + "\r\n"

                         + "ElevatorMissionName : 현재 진행하고 있는 MissionName을 표기합니다." + "\r\n" + "\r\n"

                         + "Cancel : 현재 진행하고있는 Elevator 상태를 취소할수있습니다.";
        }

        private void ElevatorStatusDisPlayInit()
        {
            try
            {
                LabalText();
                dtg_ElevatorStatusDisPlay.ScrollBars = ScrollBars.Both;
                dtg_ElevatorStatusDisPlay.AllowUserToResizeColumns = true;
                dtg_ElevatorStatusDisPlay.ColumnHeadersHeight = 50;
                dtg_ElevatorStatusDisPlay.RowTemplate.Height = 40;
                dtg_ElevatorStatusDisPlay.ReadOnly = false;

                DataGridViewCell currentCellTemplate = dtg_ElevatorStatusDisPlay.Columns[0].CellTemplate;
                dtg_ElevatorStatusDisPlay.Columns.Clear();
                dtg_ElevatorStatusDisPlay.Columns.Add(new DataGridViewColumn() { Name = "DGV_ElevatorStatusDisPlay_RobotName", HeaderText = "RobotName", Width = 200, CellTemplate = currentCellTemplate });
                dtg_ElevatorStatusDisPlay.Columns.Add(new DataGridViewColumn() { Name = "DGV_ElevatorStatusDisPlay_MirStateElevator", HeaderText = "MiRStatusElevator", Width = 200, CellTemplate = currentCellTemplate });
                dtg_ElevatorStatusDisPlay.Columns.Add(new DataGridViewColumn() { Name = "DGV_ElevatorStatusDisPlay_ElevatorState", HeaderText = "ElevatorStatus", Width = 260, CellTemplate = currentCellTemplate });
                dtg_ElevatorStatusDisPlay.Columns.Add(new DataGridViewColumn() { Name = "DGV_ElevatorStatusDisPlay_ElevatorMissionName", HeaderText = "ElevatorMissionName", Width = 260, CellTemplate = currentCellTemplate });
                dtg_ElevatorStatusDisPlay.Columns.Add(new DataGridViewButtonColumn() { Name = "DGV_ElevatorStatusDisPlay_Cancel", HeaderText = "Cancel", Width = 100 });
                dtg_ElevatorStatusDisPlay.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
                dtg_ElevatorStatusDisPlay.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
                dtg_ElevatorStatusDisPlay.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬

                dtg_ElevatorStatusDisPlay.Rows.Clear();
            }
            catch (Exception ex)
            {
                mainForm.EventLog("ElevatorScreen", "ElevatorStatusDisPlayInit() Fail = " + ex);
                mainForm.ACS_UI_Log("ElevatorScreen" + "/" + "ElevatorStatusDisPlayInit() Fail = " + ex);
            }
        }
        private void ElevatorStatusDisPlay()
        {
            try
            {
                dtg_ElevatorStatusDisPlay.Columns["DGV_ElevatorStatusDisPlay_RobotName"].ReadOnly = true; //편집사용 불가 . 클릭시 팝업창에서 편집
                dtg_ElevatorStatusDisPlay.Columns["DGV_ElevatorStatusDisPlay_MirStateElevator"].ReadOnly = true; //편집사용 불가.  클릭시 팝업창에서 편집.
                dtg_ElevatorStatusDisPlay.Columns["DGV_ElevatorStatusDisPlay_ElevatorState"].ReadOnly = true; //편집사용 불가.  클릭시 팝업창에서 편집.
                dtg_ElevatorStatusDisPlay.Columns["DGV_ElevatorStatusDisPlay_ElevatorMissionName"].ReadOnly = true; //편집사용 불가.  클릭시 팝업창에서 편집.

                dtg_ElevatorStatusDisPlay.CurrentCell = null;
                dtg_ElevatorStatusDisPlay.ClearSelection();
                dtg_ElevatorStatusDisPlay.Rows.Clear();
                dtg_ElevatorStatusDisPlay.RowTemplate.Height = 50;

                foreach (var elevatorStatusDisPlay in uow.ElevatorState.GetAll())
                {
                    int newRowIndex = dtg_ElevatorStatusDisPlay.Rows.Add();
                    var newRow = dtg_ElevatorStatusDisPlay.Rows[newRowIndex];

                    newRow.Cells["DGV_ElevatorStatusDisPlay_RobotName"].Value = elevatorStatusDisPlay.RobotName;
                    newRow.Cells["DGV_ElevatorStatusDisPlay_MirStateElevator"].Value = elevatorStatusDisPlay.MiRStateElevator;
                    newRow.Cells["DGV_ElevatorStatusDisPlay_ElevatorState"].Value = elevatorStatusDisPlay.ElevatorState;
                    newRow.Cells["DGV_ElevatorStatusDisPlay_ElevatorMissionName"].Value = elevatorStatusDisPlay.ElevatorMissionName;
                    newRow.Cells["DGV_ElevatorStatusDisPlay_Cancel"].Value = "Cancel";
                    newRow.Tag = elevatorStatusDisPlay;
                }
            }
            catch (Exception ex)
            {
                mainForm.EventLog("ElevatorScreen", "ElevatorStatusDisPlay() Fail = " + ex);
                mainForm.ACS_UI_Log("ElevatorScreen" + "/" + "ElevatorStatusDisPlay() Fail = " + ex);
            }

        }

        private void dtg_ElevatorStatusDisPlay_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                DataGridView grid = (DataGridView)sender;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];
                if (e.ColumnIndex == grid.Columns["DGV_ElevatorStatusDisPlay_Cancel"].Index)
                {
                    var ChangeData = uow.ElevatorState.Find(m => m.RobotName == selectedRow.Cells["DGV_ElevatorStatusDisPlay_RobotName"].Value).FirstOrDefault();

                    if (ChangeData != null)
                    {
                        DialogResult result = MessageBox.Show("Elevator 상태를 삭제 하시겠습니까 ? ", "Elevator_Status", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            mainForm.UserLog("ElevatorStatusDisPlay Screen", $"RobotName = {ChangeData.RobotName} , Elevator Status = {ChangeData.ElevatorState} , MiRElevatorStatus = {ChangeData.MiRStateElevator} , CancelClick / Cancel");
                            mainForm.EventLog("ElevatorStatusDisPlay Screen", $"RobotName = {ChangeData.RobotName} , Elevator Status = {ChangeData.ElevatorState} , MiRElevatorStatus = {ChangeData.MiRStateElevator} , CancelClick / Cancel");
                            mainForm.ACS_UI_Log("ElevatorStatusDisPlay Screen", $"RobotName = {ChangeData.RobotName} , Elevator Status = {ChangeData.ElevatorState} , MiRElevatorStatus = {ChangeData.MiRStateElevator} , CancelClick / Cancel");
                            uow.ElevatorState.Remove(ChangeData);
                            ElevatorStatusDisPlay();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mainForm.EventLog("ElevatorScreen", "ElevatorStatusDisPlay() Fail = " + ex);
                mainForm.ACS_UI_Log("ElevatorScreen" + "/" + "ElevatorStatusDisPlay() Fail = " + ex);
            }
        }
        private void txt_Elevator_IP_Address_Click(object sender, EventArgs e)
        {
            IP_AddressBoard insertNum = new IP_AddressBoard();
            DialogResult result = insertNum.ShowDialog();
            if (result == DialogResult.OK)
            {
                string newValue = insertNum.Return_Value;
                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;
                    ((TextBox)sender).Text = newValue;
                    AppConfiguration.ConfigDataSetting("sElevator_IP_Address_SV", newValue);
                    ConfigData.sElevator_IP_Address_SV = AppConfiguration.GetAppConfig("sElevator_IP_Address_SV");

                    insertNum.Close();
                    mainForm.UserLog("Elevator", $"Elevator IpAddress Change from {oldValue} to {newValue}");

                }
            }
        }

        private void txt_Elevator_PortNumber_Click(object sender, EventArgs e)
        {
            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, "0", "99999");
            DialogResult result = insertNum.ShowDialog();
            if (result == DialogResult.OK)
            {
                string newValue = insertNum.InputValue;
                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;
                    ((TextBox)sender).Text = newValue;
                    AppConfiguration.ConfigDataSetting("sElevator_PortNumber", newValue);
                    ConfigData.sElevator_PortNumber = AppConfiguration.GetAppConfig("sElevator_PortNumber");

                    insertNum.Close();
                    mainForm.UserLog($"Elevator", $"Elevator Port Number Change from {oldValue} to {newValue}");
                }
            }
        }

        private void ElevatorAgvMode_Click(object sender, EventArgs e)
        {

            string Screen_Keyword = ((Button)sender).Name;
            var ACSContorlMode = uow.ACSModeInfo.Find(m =>m.Location == "Elevator" && m.ACSMode != null).FirstOrDefault();
            if (ACSContorlMode != null)
            {
                switch (Screen_Keyword)
                {
                    case "btn_ElevatorAGVMode":
                        if (ACSContorlMode.ACSMode == "MiRUnContorlMode")
                        {
                            ACSContorlMode.ACSMode = "MiRContorlMode";
                            uow.ACSModeInfo.Update(ACSContorlMode);
                            btn_ElevatorAGVMode.BackColor = Color.YellowGreen;
                            btn_ElevatorAGVModeCancel.BackColor = Color.WhiteSmoke;
                            mainForm.UserLog("Elevator", "ElevatorAGVMode Value Change " + "MiRUnContorlMode" + " -> " + ACSContorlMode.ACSMode);
                        }
                        break;

                    case "btn_ElevatorAGVModeCancel":
                        if (ACSContorlMode.ACSMode == "MiRContorlMode")
                        {
                            ACSContorlMode.ACSMode = "MiRUnContorlMode";
                            uow.ACSModeInfo.Update(ACSContorlMode);
                            btn_ElevatorAGVMode.BackColor = Color.WhiteSmoke;
                            btn_ElevatorAGVModeCancel.BackColor = Color.YellowGreen;
                            mainForm.UserLog("Elevator", "ElevatorAGVMode Value Change " + "MiRContorlMode" + " -> " + ACSContorlMode.ACSMode);
                        }
                        break;
                }
            }
        }

        private void ElevatorModeChange_Display()
        {
            var ACSContorlMode = uow.ACSModeInfo.Find(m =>m.Location == "Elevator" && m.ACSMode == "MiRContorlMode").FirstOrDefault();
            if (ACSContorlMode != null)
            {
                btn_ElevatorAGVMode.BackColor = Color.YellowGreen;
                btn_ElevatorAGVModeCancel.BackColor = Color.WhiteSmoke;
            }
            else
            {
                btn_ElevatorAGVMode.BackColor = Color.WhiteSmoke;
                btn_ElevatorAGVModeCancel.BackColor = Color.YellowGreen;
            }
        }
        private void DisPlaytimer_Tick(object sender, EventArgs e)
        {
            DisPlaytimer.Enabled = false;
            ElevatorStatusDisPlay();
            ElevatorModeChange_Display();
            DisPlaytimer.Interval = 1000; //타이머 인터벌 1초
            DisPlaytimer.Enabled = true;

        }

        private void txt_ElevatorPOSSetting_Click(object sender, EventArgs e)
        {
            PositionZoneSelectForm insertNum = new PositionZoneSelectForm(null,uow);
            if (insertNum.ShowDialog() == DialogResult.OK)
            {
                string TextBoxName = ((TextBox)sender).Name;
                string TextBoxText = ((TextBox)sender).Text;
                string newValue = insertNum.InputValue;
                string oldValue = null;
                if (TextBoxText != newValue)
                {
                    if (TextBoxName == "txt_M3F_ElevatorStartPOS")
                    {
                        oldValue = TextBoxText;
                        ((TextBox)sender).Text = newValue;
                        AppConfiguration.ConfigDataSetting("M3F_ElevatorStartPOS", newValue);
                        ConfigData.M3F_ElevatorStartPOS = AppConfiguration.GetAppConfig("M3F_ElevatorStartPOS");
                        insertNum.Close();
                        mainForm.UserLog($"Elevator", $"Elevator M3F_ElevatorStartPOS Change from {oldValue} to {newValue}");
                    }
                    else if (TextBoxName == "txt_M3F_ElevatorEnterPOS" || TextBoxName == "txt_M3F_ElevatorEnterPOS_1")
                    {
                        oldValue = TextBoxText;
                        txt_M3F_ElevatorEnterPOS.Text = newValue;
                        txt_M3F_ElevatorEnterPOS_1.Text = newValue;
                        AppConfiguration.ConfigDataSetting("M3F_ElevatorEnterPOS", newValue);
                        ConfigData.M3F_ElevatorEnterPOS = AppConfiguration.GetAppConfig("M3F_ElevatorEnterPOS");
                        insertNum.Close();
                        mainForm.UserLog($"Elevator", $"Elevator M3F_ElevatorEnterPOS Change from {oldValue} to {newValue}");
                    }
                    else if (TextBoxName == "txt_M3F_ElevatorEndPOS")
                    {
                        oldValue = TextBoxText;
                        ((TextBox)sender).Text = newValue;
                        AppConfiguration.ConfigDataSetting("M3F_ElevatorEndPOS", newValue);
                        ConfigData.M3F_ElevatorEndPOS = AppConfiguration.GetAppConfig("M3F_ElevatorEndPOS");
                        insertNum.Close();
                        mainForm.UserLog($"Elevator", $"Elevator M3F_ElevatorEndPOS Change from {oldValue} to {newValue}");
                    }
                    else if (TextBoxName == "txt_T3F_ElevatorStartPOS")
                    {
                        oldValue = TextBoxText;
                        ((TextBox)sender).Text = newValue;
                        AppConfiguration.ConfigDataSetting("T3F_ElevatorStartPOS", newValue);
                        ConfigData.T3F_ElevatorStartPOS = AppConfiguration.GetAppConfig("T3F_ElevatorStartPOS");
                        insertNum.Close();
                        mainForm.UserLog($"Elevator", $"Elevator T3F_ElevatorStartPOS Change from {oldValue} to {newValue}");
                    }
                    else if (TextBoxName == "txt_T3F_ElevatorEnterPOS" || TextBoxName == "txt_T3F_ElevatorEnterPOS_1")
                    {
                        oldValue = TextBoxText;
                        txt_T3F_ElevatorEnterPOS.Text = newValue;
                        txt_T3F_ElevatorEnterPOS_1.Text = newValue;
                        AppConfiguration.ConfigDataSetting("T3F_ElevatorEnterPOS", newValue);
                        ConfigData.T3F_ElevatorEnterPOS = AppConfiguration.GetAppConfig("T3F_ElevatorEnterPOS");
                        insertNum.Close();
                        mainForm.UserLog($"Elevator", $"Elevator T3F_ElevatorEnterPOS Change from {oldValue} to {newValue}");
                    }
                    else if (TextBoxName == "txt_T3F_ElevatorEndPOS")
                    {
                        oldValue = TextBoxText;
                        ((TextBox)sender).Text = newValue;
                        AppConfiguration.ConfigDataSetting("T3F_ElevatorEndPOS", newValue);
                        ConfigData.T3F_ElevatorEndPOS = AppConfiguration.GetAppConfig("T3F_ElevatorEndPOS");
                        insertNum.Close();
                        mainForm.UserLog($"Elevator", $"Elevator T3F_ElevatorEndPOS Change from {oldValue} to {newValue}");
                    }

                }
            }
        }
    }
}
