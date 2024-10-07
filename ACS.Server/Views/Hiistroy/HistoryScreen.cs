using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using INA_ACS_Server.UI;
using INA_ACS_Server.Views;

namespace INA_ACS_Server
{
    public partial class HistoryScreen : Form
    {
        private readonly MainForm mainForm;
        private readonly IUnitOfWork uow;

        public HistoryScreen(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.uow = uow;

            this.FormClosing += (s, e) =>
            {
                if (e.CloseReason == CloseReason.UserClosing) // 사용자가 ALT-F4 누르거나 x 버튼 눌러서 창을 닫으려 할때
                    e.Cancel = true;
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            tabSetting_Init();
            System_ScreenInit();
            ScreenChangeButtonInit();
        }

        public void Init()
        {

        }

        public void System_ScreenInit()
        {
            //subFunc_System_Config_TextBox_Display();
        }

        void ScreenChangeButtonInit()
        {
            btn_ElevatorModeHistory.Visible = false;
            btn_JobChart.Visible = true;
            btn_ErrorChart.Visible = true;
        }

        void tabSetting_Init()
        {
            // 탭 페이지 설정
            var jobHistoryScreen = new JobHistoryScreen(mainForm, uow) { TopLevel = false, /*Dock = DockStyle.Fill,*/ FormBorderStyle = FormBorderStyle.None, Visible = true };
            var errHistorySrceen = new ErrorHistoryScreen(mainForm, uow) { TopLevel = false, /*Dock = DockStyle.Fill,*/ FormBorderStyle = FormBorderStyle.None, Visible = true };
            //var elvHistoryScreen = new ElevatorModeHistoryScreen(mainForm, uow) { TopLevel = false, /*Dock = DockStyle.Fill,*/ FormBorderStyle = FormBorderStyle.None, Visible = true };
            tabSetting.TabPages[0].Controls.Add(jobHistoryScreen);
            tabSetting.TabPages[1].Controls.Add(errHistorySrceen);
            //tabSetting.TabPages[2].Controls.Add(elvHistoryScreen);

            // 차트 탭 페이지 설정
            ChartHelper.connectionString = ConnectionStrings.DB1;
            var jobChartScreen = new JobHistoryChartScreen(ConnectionStrings.DB1/*, mainForm, uow*/) { TopLevel = false, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, Visible = true };
            var errChartScreen = new ErrorHistoryChartScreen(ConnectionStrings.DB1/*, mainForm, uow*/) { TopLevel = false, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, Visible = true };
            tabSetting.TabPages[3].Controls.Add(jobChartScreen);
            tabSetting.TabPages[4].Controls.Add(errChartScreen);


            // 차트 폼 콜백함수 설정
            jobChartScreen.SetCallback_GetConfigFilter(GetConfigFilterItems_Job);
            errChartScreen.SetCallback_GetConfigFilter(GetConfigFilterItems_Err);


            // 처음 보여줄 탭 지정
            tabSetting.SelectedIndex = 0;
            SettingButton_Config(btn_JobHistory, null);

            // 버튼 억세스 설정
            //sAccessLevelButtonVisible();
        }

        private JobHistoryChartConfigFilter GetConfigFilterItems_Job()
        {
            var validConfigs = uow.JobConfigs.GetAll()
                .Where(x => !string.IsNullOrWhiteSpace(x.ACSMissionGroup) && x.ACSMissionGroup != "None")
                .Where(x => !string.IsNullOrWhiteSpace(x.CallName) && x.CallName.Contains("_"))
                .Select(x => x.CallName)
                .Distinct()
                .ToList();

            var fromPosNames = validConfigs.Select(x => x.Split('_')[0]).Distinct().ToList();
            var toPosNames = validConfigs.Select(x => x.Split('_')[1]).Distinct().ToList();

            var robotInfos = uow.Robots.GetAll().Where(x => !string.IsNullOrWhiteSpace(x.RobotName)).Select(x => new { x.RobotName, x.RobotAlias }).OrderBy(x => x.RobotName).ToList();
            var robotNames = robotInfos.Select(x => x.RobotName).ToList();
            var robotAlias = robotInfos.Select(x => x.RobotAlias).ToList();

            return new JobHistoryChartConfigFilter { RobotNames = robotNames, RobotAlias = robotAlias, StartPos = fromPosNames, EndPos = toPosNames };
        }

        private ErrorHistoryChartConfigFilter GetConfigFilterItems_Err()
        {
            var validConfigs = uow.JobConfigs.GetAll()
                .Where(x => !string.IsNullOrWhiteSpace(x.ACSMissionGroup) && x.ACSMissionGroup != "None")
                .Where(x => !string.IsNullOrWhiteSpace(x.CallName) && x.CallName.Contains("_"))
                .Select(x => x.CallName)
                .Distinct()
                .ToList();

            var fromPosNames = validConfigs.Select(x => x.Split('_')[0]).Distinct().ToList();
            var toPosNames = validConfigs.Select(x => x.Split('_')[1]).Distinct().ToList();

            var robotInfos = uow.Robots.GetAll().Where(x => !string.IsNullOrWhiteSpace(x.RobotName)).Select(x => new { x.RobotName, x.RobotAlias }).OrderBy(x => x.RobotName).ToList();
            var robotNames = robotInfos.Select(x => x.RobotName).ToList();
            var robotAlias = robotInfos.Select(x => x.RobotAlias).ToList();

            return new ErrorHistoryChartConfigFilter { RobotNames = robotNames, RobotAlias = robotAlias };
        }


        private void SettingButton_Config(object sender, EventArgs e)
        {
            string ButtonName = ((Button)sender).Name;
            string name = ButtonName.Replace("btn_", "");

            btn_JobHistory.BackColor = Color.WhiteSmoke;
            btn_ErrorHistory.BackColor = Color.WhiteSmoke;
            btn_ElevatorModeHistory.BackColor = Color.WhiteSmoke;
            btn_JobChart.BackColor = Color.WhiteSmoke;
            btn_ErrorChart.BackColor = Color.WhiteSmoke;

            switch (ButtonName)
            {
                case "btn_JobHistory":
                    tabSetting.SelectedTab = tab_JobHistory;
                    break;

                case "btn_ErrorHistory":
                    tabSetting.SelectedTab = tab_ErrorHistory;
                    break;

                case "btn_ElevatorModeHistory":
                    tabSetting.SelectedTab = tab_ElevatorModeHistory;
                    break;

                case "btn_JobChart":
                    tabSetting.SelectedTab = tab_JobChart;
                    break;

                case "btn_ErrorChart":
                    tabSetting.SelectedTab = tab_ErrorChart;
                    break;
            }

            ((Button)sender).BackColor = Color.YellowGreen;

            mainForm.UserLog("Setting Screen", ((Button)sender).Name + " Click");
        }
    }
}
