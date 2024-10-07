using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using INA_ACS_Server.UI;
using INA_ACS_Server.Views;

namespace INA_ACS_Server.OPWindows
{
    public partial class SystemScreen : Form
    {
        private readonly MainForm mainForm;
        private readonly IUnitOfWork uow;
        private ElevatorScreen elevatorScreen;

        public SystemScreen(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();
            
            this.mainForm = mainForm;
            this.uow = uow;

            this.FormClosing += (s, e) =>
            {
                if (e.CloseReason == CloseReason.UserClosing) // 사용자가 ALT-F4 누르거나 x 버튼 눌러서 창을 닫으려 할때
                    e.Cancel = true;
            };

            tabSetting.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            tabSetting.BorderStylePage = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            tabSetting.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            tabSetting.LookAndFeel.UseDefaultLookAndFeel = false;

            for (int i = 0; i < tabSetting.TabPages.Count; i++)
            {
                tabSetting.TabPages[i].BackColor = mainForm.skinColor;
            }
             
            elevatorScreen = new ElevatorScreen(mainForm, uow);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Init();
            System_ScreenInit();
            tabSetting_Init();
        }

        private void Init()
        {
            this.BackColor = mainForm.skinColor;

            stackPanel1.BackColor = mainForm.skinColor;

            btn_RobotControl.LookAndFeel.Style = LookAndFeelStyle.UltraFlat;
            btn_RobotControl.LookAndFeel.UseDefaultLookAndFeel = false;
            btn_RobotControl.Appearance.BackColor = Color.FromArgb(80, 89, 96);
            btn_RobotControl.Appearance.BorderColor = Color.FromArgb(80, 89, 96);
            btn_RobotControl.AppearancePressed.BackColor = Color.FromArgb(80, 89, 96);
            btn_RobotControl.AppearancePressed.BorderColor = Color.FromArgb(80, 89, 96);
            btn_RobotControl.AppearanceHovered.BackColor = Color.FromArgb(80, 89, 96);
            btn_RobotControl.AppearanceHovered.BorderColor = Color.FromArgb(80, 89, 96);
            btn_RobotControl.AppearanceDisabled.BackColor = Color.FromArgb(80, 89, 96);
            btn_RobotControl.AppearanceDisabled.BorderColor = Color.FromArgb(80, 89, 96);
            btn_RobotControl.AllowFocus = false;
            btn_RobotControl.Appearance.Options.UseBackColor = true;
            btn_RobotControl.ForeColor = Color.FromArgb(57, 173, 233);

            btn_PositionArea.LookAndFeel.Style = LookAndFeelStyle.UltraFlat;
            btn_PositionArea.LookAndFeel.UseDefaultLookAndFeel = false;
            btn_PositionArea.Appearance.BackColor = Color.FromArgb(80, 89, 96);
            btn_PositionArea.Appearance.BorderColor = Color.FromArgb(80, 89, 96);
            btn_PositionArea.AppearancePressed.BackColor = Color.FromArgb(80, 89, 96);
            btn_PositionArea.AppearancePressed.BorderColor = Color.FromArgb(80, 89, 96);
            btn_PositionArea.AppearanceHovered.BackColor = Color.FromArgb(80, 89, 96);
            btn_PositionArea.AppearanceHovered.BorderColor = Color.FromArgb(80, 89, 96);
            btn_PositionArea.AppearanceDisabled.BackColor = Color.FromArgb(80, 89, 96);
            btn_PositionArea.AppearanceDisabled.BorderColor = Color.FromArgb(80, 89, 96);
            btn_PositionArea.AllowFocus = false;
            btn_PositionArea.Appearance.Options.UseBackColor = true;
            btn_PositionArea.ForeColor = Color.FromArgb(57, 173, 233);

            btn_JobConfig.LookAndFeel.Style = LookAndFeelStyle.UltraFlat;
            btn_JobConfig.LookAndFeel.UseDefaultLookAndFeel = false;
            btn_JobConfig.Appearance.BackColor = Color.FromArgb(80, 89, 96);
            btn_JobConfig.Appearance.BorderColor = Color.FromArgb(80, 89, 96);
            btn_JobConfig.AppearancePressed.BackColor = Color.FromArgb(80, 89, 96);
            btn_JobConfig.AppearancePressed.BorderColor = Color.FromArgb(80, 89, 96);
            btn_JobConfig.AppearanceHovered.BackColor = Color.FromArgb(80, 89, 96);
            btn_JobConfig.AppearanceHovered.BorderColor = Color.FromArgb(80, 89, 96);
            btn_JobConfig.AppearanceDisabled.BackColor = Color.FromArgb(80, 89, 96);
            btn_JobConfig.AppearanceDisabled.BorderColor = Color.FromArgb(80, 89, 96);
            btn_JobConfig.AllowFocus = false;
            btn_JobConfig.Appearance.Options.UseBackColor = true;
            btn_JobConfig.ForeColor = Color.FromArgb(57, 173, 233);

            btn_WaitMission.LookAndFeel.Style = LookAndFeelStyle.UltraFlat;
            btn_WaitMission.LookAndFeel.UseDefaultLookAndFeel = false;
            btn_WaitMission.Appearance.BackColor = Color.FromArgb(80, 89, 96);
            btn_WaitMission.Appearance.BorderColor = Color.FromArgb(80, 89, 96);
            btn_WaitMission.AppearancePressed.BackColor = Color.FromArgb(80, 89, 96);
            btn_WaitMission.AppearancePressed.BorderColor = Color.FromArgb(80, 89, 96);
            btn_WaitMission.AppearanceHovered.BackColor = Color.FromArgb(80, 89, 96);
            btn_WaitMission.AppearanceHovered.BorderColor = Color.FromArgb(80, 89, 96);
            btn_WaitMission.AppearanceDisabled.BackColor = Color.FromArgb(80, 89, 96);
            btn_WaitMission.AppearanceDisabled.BorderColor = Color.FromArgb(80, 89, 96);
            btn_WaitMission.AllowFocus = false;
            btn_WaitMission.Appearance.Options.UseBackColor = true;
            btn_WaitMission.ForeColor = Color.FromArgb(57, 173, 233);

            btn_ChargeMission.LookAndFeel.Style = LookAndFeelStyle.UltraFlat;
            btn_ChargeMission.LookAndFeel.UseDefaultLookAndFeel = false;
            btn_ChargeMission.Appearance.BackColor = Color.FromArgb(80, 89, 96);
            btn_ChargeMission.Appearance.BorderColor = Color.FromArgb(80, 89, 96);
            btn_ChargeMission.AppearancePressed.BackColor = Color.FromArgb(80, 89, 96);
            btn_ChargeMission.AppearancePressed.BorderColor = Color.FromArgb(80, 89, 96);
            btn_ChargeMission.AppearanceHovered.BackColor = Color.FromArgb(80, 89, 96);
            btn_ChargeMission.AppearanceHovered.BorderColor = Color.FromArgb(80, 89, 96);
            btn_ChargeMission.AppearanceDisabled.BackColor = Color.FromArgb(80, 89, 96);
            btn_ChargeMission.AppearanceDisabled.BorderColor = Color.FromArgb(80, 89, 96);
            btn_ChargeMission.AllowFocus = false;
            btn_ChargeMission.Appearance.Options.UseBackColor = true;
            btn_ChargeMission.ForeColor = Color.FromArgb(57, 173, 233);

            btn_RegistarSync.LookAndFeel.Style = LookAndFeelStyle.UltraFlat;
            btn_RegistarSync.LookAndFeel.UseDefaultLookAndFeel = false;
            btn_RegistarSync.Appearance.BackColor = Color.FromArgb(80, 89, 96);
            btn_RegistarSync.Appearance.BorderColor = Color.FromArgb(80, 89, 96);
            btn_RegistarSync.AppearancePressed.BackColor = Color.FromArgb(80, 89, 96);
            btn_RegistarSync.AppearancePressed.BorderColor = Color.FromArgb(80, 89, 96);
            btn_RegistarSync.AppearanceHovered.BackColor = Color.FromArgb(80, 89, 96);
            btn_RegistarSync.AppearanceHovered.BorderColor = Color.FromArgb(80, 89, 96);
            btn_RegistarSync.AppearanceDisabled.BackColor = Color.FromArgb(80, 89, 96);
            btn_RegistarSync.AppearanceDisabled.BorderColor = Color.FromArgb(80, 89, 96);
            btn_RegistarSync.AllowFocus = false;
            btn_RegistarSync.Appearance.Options.UseBackColor = true;
            btn_RegistarSync.ForeColor = Color.FromArgb(57, 173, 233);

            btn_Elevator.LookAndFeel.Style = LookAndFeelStyle.UltraFlat;
            btn_Elevator.LookAndFeel.UseDefaultLookAndFeel = false;
            btn_Elevator.Appearance.BackColor = Color.FromArgb(80, 89, 96);
            btn_Elevator.Appearance.BorderColor = Color.FromArgb(80, 89, 96);
            btn_Elevator.AppearancePressed.BackColor = Color.FromArgb(80, 89, 96);
            btn_Elevator.AppearancePressed.BorderColor = Color.FromArgb(80, 89, 96);
            btn_Elevator.AppearanceHovered.BackColor = Color.FromArgb(80, 89, 96);
            btn_Elevator.AppearanceHovered.BorderColor = Color.FromArgb(80, 89, 96);
            btn_Elevator.AppearanceDisabled.BackColor = Color.FromArgb(80, 89, 96);
            btn_Elevator.AppearanceDisabled.BorderColor = Color.FromArgb(80, 89, 96);
            btn_Elevator.AllowFocus = false;
            btn_Elevator.Appearance.Options.UseBackColor = true;
            btn_Elevator.ForeColor = Color.FromArgb(57, 173, 233);

            btn_UserData.LookAndFeel.Style = LookAndFeelStyle.UltraFlat;
            btn_UserData.LookAndFeel.UseDefaultLookAndFeel = false;
            btn_UserData.Appearance.BackColor = Color.FromArgb(80, 89, 96);
            btn_UserData.Appearance.BorderColor = Color.FromArgb(80, 89, 96);
            btn_UserData.AppearancePressed.BackColor = Color.FromArgb(80, 89, 96);
            btn_UserData.AppearancePressed.BorderColor = Color.FromArgb(80, 89, 96);
            btn_UserData.AppearanceHovered.BackColor = Color.FromArgb(80, 89, 96);
            btn_UserData.AppearanceHovered.BorderColor = Color.FromArgb(80, 89, 96);
            btn_UserData.AppearanceDisabled.BackColor = Color.FromArgb(80, 89, 96);
            btn_UserData.AppearanceDisabled.BorderColor = Color.FromArgb(80, 89, 96);
            btn_UserData.AllowFocus = false;
            btn_UserData.Appearance.Options.UseBackColor = true;
            btn_UserData.ForeColor = Color.FromArgb(57, 173, 233);
        }

        public void System_ScreenInit()
        {
            sAccessLevelButtonVisible();
        }

        void tabSetting_Init()
        {
            SettingButton_Config(btn_RobotControl, null);
            sAccessLevelButtonVisible();
        }

        #region 화면 전환

        //System Scrren 사용자 권한 버튼 생성
        private void sAccessLevelButtonVisible()
        {
            if (ConfigData.sAccessLevel == "INATECH" || ConfigData.sAccessLevel == "Engineer")
            {
                btn_RegistarSync.Visible = true;

            }
            else if (ConfigData.sAccessLevel.Equals("Maint"))
            {
                btn_RegistarSync.Visible = false;
            }
        }

        private void SettingButton_Config(object sender, EventArgs e)
        {
            string ButtonName = ((DevExpress.XtraEditors.SimpleButton)sender).Name;
            string name = ButtonName.Replace("btn_", "");

            //btn_RobotControl.BackColor = Color.WhiteSmoke;
            //btn_PositionArea.BackColor = Color.WhiteSmoke;
            //btn_JobConfig.BackColor = Color.WhiteSmoke;
            //btn_WaitMission.BackColor = Color.WhiteSmoke;
            //btn_ChargeMission.BackColor = Color.WhiteSmoke;
            //btn_RegistarSync.BackColor = Color.WhiteSmoke;
            //btn_Elevator.BackColor = Color.WhiteSmoke;
            //btn_UserData.BackColor = Color.WhiteSmoke;

            switch (ButtonName)
            {
                case "btn_RobotControl":
                    tabSetting.SelectedTabPage = tab_RobotControl;
                    if (tabSetting.TabPages[tabSetting.SelectedTabPageIndex].Controls.Count == 0)
                    {
                        var miRControlScreen = new RobotControlScreen(mainForm, uow);
                        miRControlScreen.TopLevel = false;
                        miRControlScreen.Dock = DockStyle.Fill;
                        tabSetting.TabPages[tabSetting.SelectedTabPageIndex].Controls.Add(miRControlScreen);
                        miRControlScreen.Show();
                    }
                    break;

                case "btn_PositionArea":
                    tabSetting.SelectedTabPage = tab_PositionArea;
                    if (tabSetting.TabPages[tabSetting.SelectedTabPageIndex].Controls.Count == 0)
                    {
                        var positionScreen = new PositionScreen(mainForm, uow);
                        positionScreen.TopLevel = false;
                        positionScreen.Dock = DockStyle.Fill;
                        tabSetting.TabPages[tabSetting.SelectedTabPageIndex].Controls.Add(positionScreen);
                        positionScreen.Show();
                    }
                    break;

                case "btn_JobConfig":
                    tabSetting.SelectedTabPage = tab_JobConfig;
                    if (tabSetting.TabPages[tabSetting.SelectedTabPageIndex].Controls.Count == 0)
                    {
                        var jobConfigsScreen = new JobConfigScreen(mainForm, uow);
                        jobConfigsScreen.TopLevel = false;
                        jobConfigsScreen.Dock = DockStyle.Fill;
                        tabSetting.TabPages[tabSetting.SelectedTabPageIndex].Controls.Add(jobConfigsScreen);
                        jobConfigsScreen.Show();
                    }
                    break;

                case "btn_WaitMission":
                    tabSetting.SelectedTabPage = tab_WaitMission;
                    if (tabSetting.TabPages[tabSetting.SelectedTabPageIndex].Controls.Count == 0)
                    {
                        var waitMissionScreen = new WaitMissionScreen(mainForm, uow);
                        waitMissionScreen.TopLevel = false;
                        waitMissionScreen.Dock = DockStyle.Fill;
                        tabSetting.TabPages[tabSetting.SelectedTabPageIndex].Controls.Add(waitMissionScreen);
                        waitMissionScreen.Show();
                    }
                    break;

                case "btn_ChargeMission":
                    tabSetting.SelectedTabPage = tab_ChargeMission;
                    if (tabSetting.TabPages[tabSetting.SelectedTabPageIndex].Controls.Count == 0)
                    {
                        var chargeMissionScreen = new ChargeMissionScreen(mainForm, uow);
                        chargeMissionScreen.TopLevel = false;
                        chargeMissionScreen.Dock = DockStyle.Fill;
                        tabSetting.TabPages[tabSetting.SelectedTabPageIndex].Controls.Add(chargeMissionScreen);
                        chargeMissionScreen.Show();
                    }
                    break;
            

                case "btn_RegistarSync":
                    tabSetting.SelectedTabPage = tab_RegisterSync;
                    if (tabSetting.TabPages[tabSetting.SelectedTabPageIndex].Controls.Count == 0)
                    {
                        var robotRegisterSyncScreen = new RobotRegisterSyncScreen(mainForm, uow);
                        robotRegisterSyncScreen.TopLevel = false;
                        robotRegisterSyncScreen.Dock = DockStyle.Fill;
                        tabSetting.TabPages[tabSetting.SelectedTabPageIndex].Controls.Add(robotRegisterSyncScreen);
                        robotRegisterSyncScreen.Show();
                    }
                    break;

                case "btn_Elevator":
                    tabSetting.SelectedTabPage = tab_Elevator;
                    if (tabSetting.TabPages[tabSetting.SelectedTabPageIndex].Controls.Count == 0)
                    {
                        elevatorScreen.TopLevel = false;
                        elevatorScreen.Dock = DockStyle.Fill;
                        tabSetting.TabPages[tabSetting.SelectedTabPageIndex].Controls.Add(elevatorScreen);
                        elevatorScreen.Show();
                    }
                    break;

                case "btn_UserData":
                    tabSetting.SelectedTabPage = tab_UserData;
                    if (tabSetting.TabPages[tabSetting.SelectedTabPageIndex].Controls.Count == 0)
                    {
                        var UserDataScreen = new UserDataScreen(mainForm, uow);
                        UserDataScreen.TopLevel = false;
                        UserDataScreen.Dock = DockStyle.Fill;
                        tabSetting.TabPages[tabSetting.SelectedTabPageIndex].Controls.Add(UserDataScreen);
                        UserDataScreen.Show();
                    }
                    break;

                case "btn_Setting_MiR_Control":
                    tabSetting.SelectedTabPage = tab_RobotControl;
                    break;
            }

            //((Button)sender).BackColor = Color.YellowGreen;

            mainForm.UserLog("Setting Screen", ((DevExpress.XtraEditors.SimpleButton)sender).Name + " Click");
        }


        #endregion

        #region System Click Event

        private void txt_AutoChargeDelay_Click(object sender, EventArgs e)
        {
            ////Auto Charge 기능이 시작가능할 경우 딜레이 타임 설정
            //NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, ((TextBox)sender).AccessibleDescription);
            //DialogResult result = insertNum.ShowDialog();
            //if (result == DialogResult.OK)
            //{
            //    mainForm.UserLog("SettingScreen", "MiR Auto Charge Delay Value Change " + ((TextBox)sender).Text + " -> " + int.Parse(insertNum.InputValue).ToString());

            //    ((TextBox)sender).Text = int.Parse(insertNum.InputValue).ToString();

            //    AppConfiguration.ConfigDataSetting("sAutoChargeDelayTime", ((TextBox)sender).Text);
            //    ConfigData.sAutoChargeDelayTime = AppConfiguration.GetAppConfig("sAutoChargeDelayTime");

            //    insertNum.Close();
            // }
        }

        private void txt_RobotSwichaingBattry_Click(object sender, EventArgs e)
        {
            ////Elevator AGV 모드가 아닐때 Robot 2대를 스위칭하면서 충전시킬수있는 배터리 용량
            //NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, ((TextBox)sender).AccessibleDescription);
            //DialogResult result = insertNum.ShowDialog();
            //if (result == DialogResult.OK)
            //{
            //    mainForm.UserLog("SettingScreen", "MiR Auto Charge Robot Swichaing Battry Value Change " + ((TextBox)sender).Text + " -> " + double.Parse(insertNum.InputValue).ToString());

            //    ((TextBox)sender).Text = Math.Round(double.Parse(insertNum.InputValue), 1).ToString();

            //    AppConfiguration.ConfigDataSetting("RobotSwichaingBattry", ((TextBox)sender).Text);
            //    ConfigData.RobotSwichaingBattry = AppConfiguration.GetAppConfig("RobotSwichaingBattry");

            //    insertNum.Close();
            //}
        }

        private void txt_MiR_Reset_Mission_Click(object sender, EventArgs e)
        {

        }

        private void btn_Mission_Count_Report_Click(object sender, EventArgs e)
        {
            //foreach (var button in uow.CallButtons.GetAll())
            //{
            //    button.MissionCount = 0;
            //    uow.CallButtons.Update(button);
            //}
        }



        #endregion

        #region Timer
        private void Auto_dtg_Display_Timer_Tick(object sender, EventArgs e)
        {
            //Auto_dtg_Display_Timer.Enabled = false;
            //Auto_dtg_Display_Timer.Enabled = true;
        }
        #endregion
    }
}



