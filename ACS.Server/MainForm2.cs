using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using INA_ACS_Server.OPWindows;
using INA_ACS_Server.UI;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.ComponentModel;
using INA_ACS_Server.Views.Popups;
using System.Data.SqlClient;
using Dapper;
using INA_ACS_Server.Utilities;
using System.Linq;
using ACS.RobotMap;
using System.Diagnostics;
using DevExpress.XtraGrid.Views.Grid;

namespace INA_ACS_Server
{
    public partial class MainForm2 : Form
    {
        private readonly static ILog EventLogger = LogManager.GetLogger("Event"); //Function 실행관련 Log
        private readonly static ILog UserLogger = LogManager.GetLogger("User"); //버튼 및 화면조작관련 Log
        private readonly static ILog WorkLogger = LogManager.GetLogger("Work"); //물동량 관련 Log
        private readonly static ILog TimeoutLogger = LogManager.GetLogger("Timeout");

        private readonly UnitOfWork uow;
        
        private MainLoop fleetThread = null;

        public TestForm testForm; // 테스트 폼 참조 변수
        public string ElevatorState = "";
        public string ElevatorFloor = "";
        public List<string> Floors = new List<string>();
        public ElevatorAGVMode ACSMode = ElevatorAGVMode.MiRControlMode;
        public WirelessSensorRS485 rS485;

        // 맵 데이터 큐
        MapReadDtoQueue<MapReadDto> _mapDataQueue = new MapReadDtoQueue<MapReadDto>();
        List<IMapReadService> _mapDataReadSvc = new List<IMapReadService>();

        public MainForm2()
        {
            InitializeComponent();

            lbl_LoginName.Text = ConfigData.sAccessLevel;   //"Operator"

            // data storage 초기화
            uow = new UnitOfWork();
            rS485 = new WirelessSensorRS485(this, uow);

            Init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (testForm.Visible == false) testForm.Show(this);
            if (testForm.WindowState != FormWindowState.Normal) testForm.WindowState = FormWindowState.Normal;
        }
        private void Init()
        {
            // ================ api clients 객체 초기화
            this._fleetApi = InitFleetApiClient(ApiLogger);
            this._mirApiList = InitMirApiClients(ApiLogger);

            fleetThread = new MainLoop(this, uow, _fleetApi);

            for (int mapCount = 1; mapCount<=3; mapCount++)
            {
                IMapReadService mapReadService = new FleetMapReadService(_mapDataQueue, EventLogger, _fleetApi, uow);
                var MapInfo = uow.FloorMapIDConfigs.GetAll().FirstOrDefault(x => x.Id == mapCount);
                if (MapInfo != null)
                {
                    mapReadService.MapGuid = MapInfo.MapID;
                    mapReadService.MapName = MapInfo.FloorName;
                    _mapDataReadSvc.Add(mapReadService);
                }
            }
            
            // 테스트 폼 초기화
            testForm = new TestForm(this, uow);
        }

        public void Start()
        {

            // ================ map Thread Start
            foreach (var item in _mapDataReadSvc)
            {
                item.Start();
            }
            fleetThread.Start();

        }

        protected override void OnLoad(EventArgs e)
        {
            var screens = Screen.AllScreens;
            if (screens.Length > 1) // 스크린이 2개이상 있다면
            {
                this.Location = screens[0].Bounds.Location; // 1번 스크린에 뛰움
                //this.Location = screens[1].Bounds.Location; // 2번 스크린에 뛰움
            }
            base.OnLoad(e);
        }

        protected override void OnShown(EventArgs e)
        {
            InitCIM();
            btn_Auto.BackColor = Color.YellowGreen;

            //Service_Start(); // MainForm_Load() 에서 호출하도록 이동
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing) // 사용자가 ALT-F4 누르거나 x 버튼 눌러서 창을 닫으려 할때
                e.Cancel = true;

            base.OnFormClosing(e);
        }

        private void Connect_Display() //Fleet_Connect 함수
        {
            if (bFleetConnected)
            {

                grb_Fleet_Connect.Visible = true;
                pnl_Fleet_Connect.Visible = true;
                pnl_Fleet_Disconnect.Visible = false;
                lbl_Fleet_Connection.Text = "Connected";
                lbl_Fleet_Connection.ForeColor = Color.Green;
            }
            else
            {
                pnl_Fleet_Connect.Visible = false;
                pnl_Fleet_Disconnect.Visible = true;

                lbl_Fleet_Connection.Text = "Disconnected";
                lbl_Fleet_Connection.ForeColor = Color.OrangeRed;
            }

            var RobotConnected = uow.Robots.Find(r => r.ConnectState == true).Count();
            if (RobotConnected > 0)
            {
                grb_RobotConnect.Visible = true;
                pnl_RobotConnect.Visible = true;
                pnl_RobotDisConnect.Visible = false;
                lbl_RobotConnectCount.Visible = true;

                lbl_RobotConnectCount.Text = RobotConnected.ToString();
                lbl_RobotConnection.Text = "Connected";
                lbl_RobotConnection.ForeColor = Color.Green;
            }
            else
            {

                pnl_RobotConnect.Visible = false;
                pnl_RobotDisConnect.Visible = true;

                lbl_RobotConnectCount.Text = RobotConnected.ToString();
                lbl_RobotConnection.Text = "Disconnected";
                lbl_RobotConnection.ForeColor = Color.OrangeRed;
            }

        }

        private void InitCIM()
        {
            EventLogger.Info("================ Program Start ================");
            UserLogger.Info("================ Program Start ================");

            // 경광등 ready
            //var f = new TowerLamp();
            //f.AlarmLampOff();
            //f.Close();
        }

        public void UserLog(string ScreenName, string Comment)
        {
            //작업자의 조작으로 인해 발생된 Event를 Log로 남기는 루틴
            UserLogger.InfoFormat("{0}_Scrren, {1}", ScreenName, Comment);
        }

        public void EventLog(string ScreenName, string Comment)
        {
            //Program 내에서 실행되는 함수들의 Event상황을 Log로 남기는 루틴
            EventLogger.InfoFormat("{0}, {1}", ScreenName, Comment);
        }

        public void TimeoutLog(string ScreenName, string Comment)
        {
            //Program 내에서 실행되는 함수들의 Event상황을 Log로 남기는 루틴
            TimeoutLogger.InfoFormat("{0}, {1}", ScreenName, Comment);
        }

        #region Form Load

        // ChildForm 전환
        AutoScreen ChildMainForm;



        private void MainForm_Load(object sender, EventArgs e)
        {
            Start(); // => 서브폼보다 서비스객체를 먼저 생성하기 위해 여기서 호출한다.

            ChildFormLoad();
        }

        private void ChildFormLoad()
        {
            ChildMainForm = new AutoScreen(this, uow)
            {
                MdiParent = this,
                Dock = DockStyle.Fill
            };

            var ChildSystemForm = new SystemScreen(this, uow)
            {
                MdiParent = this,
                Dock = DockStyle.Fill
            };

            var ChildAcsLogViewerForm = new AcsLogViewerScreen(this, uow)
            {
                MdiParent = this,
                Dock = DockStyle.Fill
            };

            var ChildReportForm = new JobHistoryScreen(this, uow)
            {
                MdiParent = this,
                Dock = DockStyle.Fill,
                FormBorderStyle = FormBorderStyle.None,
            };

            var ChildErrorHistoryForm = new ErrorHistoryScreen(this, uow)
            {
                MdiParent = this,
                Dock = DockStyle.Fill,
                FormBorderStyle = FormBorderStyle.None,
            };

            var ChildHistoryForm = new HistoryScreen(this, uow)
            {
                MdiParent = this,
                Dock = DockStyle.Fill,
                FormBorderStyle = FormBorderStyle.None,
            };

            var ChildMapForm = new MapMonitoring(this, uow, _mapDataQueue, _mapDataReadSvc)
            {
                MdiParent = this,
                Dock = DockStyle.Fill,
                FormBorderStyle = FormBorderStyle.None
            };

            ChildMainForm.Show();
            ChildSystemForm.Show();
            ChildAcsLogViewerForm.Show();
            ChildReportForm.Show();
            ChildErrorHistoryForm.Show();
            ChildHistoryForm.Show();
            //ChildPartListForm.Show();
            ChildMapForm.Show();

            ChildMainForm.Activate();
            ActivateOrShowForm("AutoScreen");
        }

        //호출 하는 메뉴들에 대한 예외 처리 루틴
        public void ActivateOrShowForm(string name)
        {
            foreach (Form child in this.MdiChildren)
            {
                if (child.Name.Equals(name))
                {
                    child.Dock = DockStyle.Fill;
                    child.Activate();
                    child.Refresh();

                    Changescreen_TransData(child);
                    return;
                }
            }
        }

        public void Changescreen_TransData(Form child)
        {
            string name = child.Name;

            switch (name)
            {
                case "AutoScreen":
                    ((AutoScreen)child).Auto_ScreenInit();
                    break;

                case "SystemScreen":
                    ((SystemScreen)child).System_ScreenInit();
                    break;

                case "AcsLogViewer":
                    ((AcsLogViewerScreen)child).Init();
                    break;

                case "JobHistoryScreen":
                    ((JobHistoryScreen)child).Init();
                    break;

                case "ErrorHistoryScreen":
                    ((ErrorHistoryScreen)child).Init();
                    break;

                case "MapMonitoring":
                    ((MapMonitoring)child).Init();
                    break;

                case "HistoryScreen":
                    ((HistoryScreen)child).Init();
                    break;

                    //case "PartListScreen":
                    //    ((PartListScreen)child).Init();
                    //    break;
            }
        }

        #endregion

        #region 화면전환관련
        public void ScreenChange(object sender, EventArgs e)
        {
            //화면전환하는 루틴
            string Screen_Keyword = ((Button)sender).Name;
            string name = Screen_Keyword.Replace("btn_", "");

            InitMenuButton_Color();

            switch (Screen_Keyword)
            {
                case "btn_Auto":
                    UserLogger.Info("MainForm = " + Screen_Keyword + " Click");
                    ActivateOrShowForm("AutoScreen");
                    ((Button)sender).BackColor = Color.YellowGreen;
                    break;

                case "btn_Setting":
                    UserLogger.Info("MainForm = " + Screen_Keyword + " Click");
                    ActivateOrShowForm("SystemScreen");
                    ((Button)sender).BackColor = Color.YellowGreen;
                    break;

                case "btn_AcsLogViewer":
                    UserLogger.Info("MainForm = " + Screen_Keyword + " Click");
                    ActivateOrShowForm("AcsLogViewerScreen");
                    ((Button)sender).BackColor = Color.YellowGreen;
                    break;

                case "btn_Data":
                    UserLogger.Info("MainForm = " + Screen_Keyword + " Click");
                    ActivateOrShowForm("HistoryScreen");
                    ((Button)sender).BackColor = Color.YellowGreen;
                    break;

                case "btn_ErrorHistory":
                    UserLogger.Info("MainForm = " + Screen_Keyword + " Click");
                    ActivateOrShowForm("ErrorHistoryScreen");
                    ((Button)sender).BackColor = Color.YellowGreen;
                    break;

                case "btn_Parts":
                    UserLogger.Info("MainForm = " + Screen_Keyword + " Click");
                    ActivateOrShowForm("PartListScreen");
                    ((Button)sender).BackColor = Color.YellowGreen;
                    break;

                case "btn_Map":
                    UserLogger.Info("MainForm = " + Screen_Keyword + " Click");
                    ActivateOrShowForm("MapMonitoring");
                    ((Button)sender).BackColor = Color.YellowGreen;
                    break;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string return_value = string.Empty;
            string Current_AccessLevel = string.Empty;
            string Change_AccessLevel = string.Empty;
            Current_AccessLevel = ConfigData.sAccessLevel;

            //Login 이 되어 있는 경우
            if (Current_AccessLevel != "Operator")
            {
                DialogResult result = MessageBox.Show("Logout하시겠습니까? ", "User Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    UserLogger.Info("MainForm = Logout Click");
                    ConfigData.sAccessLevel = "Operator";
                    AppConfiguration.ConfigDataSetting("sAccessLevel", "Operator");
                    lbl_LoginName.Text = "Operator";
                    btn_Login.Text = "LOGIN";
                    InitMenuButton_Color();
                    ActivateOrShowForm("AutoScreen");
                    btn_Auto.BackColor = Color.YellowGreen;
                }
            }
            //Log in을 해야 하는 경우
            else
            {
                LoginForm LinForm = new LoginForm();

                DialogResult result = LinForm.ShowDialog();
                string Temp_Password = string.Empty;

                if (LinForm.Update_index == 1)
                {
                    Temp_Password = LinForm.Login_UserPassword; // 임시로 비번을 저장 
                    if (LinForm.Login_UserName.ToUpper() == "INATECH")
                    {
                        string Tstring = DateTime.Now.ToString("MMddHHmm");
                        if (Temp_Password == Tstring)
                        {
                            UserLogger.Info("MainForm = Admin Login");

                            ConfigData.sAccessLevel = "INATECH";
                            AppConfiguration.ConfigDataSetting("sAccessLevel", "INATECH");
                            lbl_LoginName.Text = ConfigData.sAccessLevel;
                            btn_Login.Text = "LOGOUT";
                        }
                        else if (LinForm.Login_UserPassword.ToUpper() == "INATECH")
                        {
                            ConfigData.sAccessLevel = "Engineer";
                            AppConfiguration.ConfigDataSetting("sAccessLevel", "Engineer");
                            lbl_LoginName.Text = ConfigData.sAccessLevel;
                            btn_Login.Text = "LOGOUT";
                            UserLogger.Info("MainForm = " + ConfigData.sAccessLevel + " Login");
                        }
                        else
                        {
                            MessageBox.Show("비밀번호가 일치하지 않습니다.");
                            return;
                        }
                    }
                    else if (LinForm.Login_UserName.ToUpper() == "MAINT")
                    {
                        if (Temp_Password == ConfigData.sMaint_Password)
                        {
                            ConfigData.sAccessLevel = "Maint";
                            AppConfiguration.ConfigDataSetting("sAccessLevel", "Maint");
                            lbl_LoginName.Text = ConfigData.sAccessLevel;
                            btn_Login.Text = "LOGOUT";
                            UserLogger.Info("MainForm = " + ConfigData.sAccessLevel + " Login");
                        }
                        else
                        {
                            MessageBox.Show("비밀번호가 일치하지 않습니다.");
                            return;
                        }
                    }
                    else MessageBox.Show("입력된 User가 없거나 비밀번호가 일치하지 않습니다.");
                }
                LinForm.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Exit the program?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                EventLogger.Info("================ Program Exit ================");
                UserLogger.Info("================ Program Exit ================");

                Application.Exit();
            }
        }

        void InitMenuButton_Color()
        {
            //Button Color 초기화
            btn_Auto.BackColor = Color.WhiteSmoke;
            btn_Setting.BackColor = Color.WhiteSmoke;
            btn_AcsLogViewer.BackColor = Color.WhiteSmoke;
            btn_Data.BackColor = Color.WhiteSmoke;
            btn_Parts.BackColor = Color.WhiteSmoke;
            btn_Map.BackColor = Color.WhiteSmoke;
            btn_ErrorHistory.BackColor = Color.WhiteSmoke;
        }

        public void subFuncMessagePopUp(string sText)
        {
            MsgPopupForm MsgPopup = new MsgPopupForm(sText);
            MsgPopup.Show();
        }

        #endregion

        #region Timer
        private void Timer_1Sec_Tick(object sender, EventArgs e)
        {
            Timer_1Sec.Enabled = false;
            lbl_System_Day.Text = DateTime.Now.ToString("yyyy-MM-dd");
            lbl_System_Time.Text = DateTime.Now.ToString("HH:mm:ss");
            //Connect_Display();

            if (ConfigData.sAccessLevel == "Operator") btn_Setting.Visible = false;
            else btn_Setting.Visible = true;
            if (lbl_System_Time.Text.Equals("10:00:00"))
            {
                DateBaselogDelete();
                deleteSystemLogFile_Time();
            }

            Timer_1Sec.Enabled = true;
        }
        #endregion

        #region ACS Display Log 및 LogFile Delete관련

        public void ACS_UI_Log(string logMessage, [CallerFilePath] string file = "",
                                                  [CallerLineNumber] int line = 0,
                                                  [CallerMemberName] string member = "")
        {
            string tmp = $"{Path.GetFileName(file)}({line})";
            string logMessageFormatted = string.Format("{0:HH:mm:ss}, {1,-35}, {2}", DateTime.Now, tmp, logMessage);

            AcsLogMessageQueue.Enqueue(logMessageFormatted);
        }

        private void deleteSystemLogFile_Time()
        {
            //Log 삭제
            try
            {
                string Log_Directory = @"\Log\ACS\System\";
                int deleteAddYears = 1;

                foreach (var subDirPath in Directory.GetDirectories(Log_Directory))
                {
                    //DirectoryInfo dirInfo = new DirectoryInfo(Log_Directory);
                    DirectoryInfo dirInfo = new DirectoryInfo(subDirPath);

                    if (dirInfo.Exists)
                    {
                        foreach (FileInfo file in dirInfo.GetFiles())
                        {
                            if (file.Extension != ".log")
                            {
                                continue;
                            }

                            if (file.CreationTime < DateTime.Now.AddYears(-(deleteAddYears)))
                            {
                                file.Delete();
                                EventLogger.Info("deleteSystemLogFile_Time()");
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }
        }



        #endregion




        private List<ErrorMessagePopupForm> errorForms = new List<ErrorMessagePopupForm>();

        private void PopCall_Msg_Timer_Tick(object sender, EventArgs e)
        {
            while (this.PopCallErrorMessageQueue.Count > 0)
            {
                // 경광등 blink
                //var f = new TowerLamp(); f.AlarmOn(); f.Close();

                if (this.PopCallErrorMessageQueue.TryDequeue(out string msg))
                {
                    var errorForm = new ErrorMessagePopupForm(msg);
                    errorForms.Add(errorForm);

                    var result = errorForm.ShowDialog(this);
                    //if (result == DialogResult.OK)
                    //{
                    //    errorForm.Close();
                    //}
                }
            }
        }

        private void DateBaselogDelete()
        {
            try
            {
                int deleteAddYears = 1;
                DateTime searchDate1 = DateTime.Now.AddYears(-(deleteAddYears));
                using (var con = new SqlConnection(ConnectionStrings.DB1))
                {
                    string SELECT_SQL = @"DELETE JobHistory WHERE CallTime < @searchDate1";
                    object queryParams = new { searchDate1 };
                    var list = con.Query(SELECT_SQL, queryParams);
                }
                using (var con = new SqlConnection(ConnectionStrings.DB1))
                {
                    string SELECT_SQL = @"DELETE ErrorHistory WHERE ErrorCreateTime < @searchDate1";
                    object queryParams = new { searchDate1 };
                    var list = con.Query(SELECT_SQL, queryParams);
                }
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }
        }

        public void SaveAsDataGridviewToCSV(DataGridView dataGrid)
        {
            DataGridView grid = dataGrid;
            //string dtgSaveName = dataGrid.Name.Substring(3, dataGrid.Name.Length);
            string dtgSaveName = dataGrid.Name.ToString();

            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Title = "Save";
                dlg.Filter = "Text files (*.csv)|*.csv|All files (*.*)|*.*";
                //dlg.InitialDirectory = Application.StartupPath + "\\SaveFolder\\";
                dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                string fileSaveName = string.Format("{0}_{1}", dtgSaveName, DateTime.Now.ToString("yyyyMMddhhmmss"));

                dlg.FileName = fileSaveName;
                DialogResult Result = dlg.ShowDialog();
                if (Result == DialogResult.OK)
                {
                    string fileName = dlg.FileName;
                    StringBuilder sb = new StringBuilder(1024 * 128);

                    int columCount = grid.ColumnCount;
                    //int columCount = 11;
                    int rowCount = grid.Rows.Count;
                    string text = string.Empty;
                    string head = string.Empty;

                    //for (int i = 0; i < columCount; i++)  //HeadText 저장
                    for (int i = 1; i < columCount; i++)  //HeadText 저장
                    {
                        string tmp = string.Format(grid.Columns[i].HeaderText.ToString() + ",");
                        head += tmp.Replace("\r\n", " ");
                    }
                    sb.AppendFormat("{0},{1}", head, Environment.NewLine);

                    for (int i = 0; i < rowCount; i++)    //rows data 저장","
                    {
                        //for (int j = 0; j < columCount; j++)
                        for (int j = 1; j < columCount; j++)
                        {
                            string tmp;
                            if (grid[j, i].Value == null) tmp = ",";
                            else
                            {
                                tmp = grid[j, i].FormattedValue.ToString();
                                tmp = tmp + ",";
                            }
                            text += tmp;
                        }
                        sb.AppendFormat("{0},{1}", text, Environment.NewLine);
                        text = string.Empty;
                    }
                    File.WriteAllText(fileName, sb.ToString(), Encoding.UTF8);

                }
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }
        }

        public void SaveAsDataGridviewToCSV(GridView dataGrid)
        {
            GridView grid = dataGrid;
            //string dtgSaveName = dataGrid.Name.Substring(3, dataGrid.Name.Length);
            string dtgSaveName = dataGrid.Name.ToString();

            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Title = "Save";
                dlg.Filter = "Text files (*.csv)|*.csv|All files (*.*)|*.*";
                //dlg.InitialDirectory = Application.StartupPath + "\\SaveFolder\\";
                dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                string fileSaveName = string.Format("{0}_{1}", dtgSaveName, DateTime.Now.ToString("yyyyMMddhhmmss"));

                dlg.FileName = fileSaveName;
                DialogResult Result = dlg.ShowDialog();
                if (Result == DialogResult.OK)
                {
                    string fileName = dlg.FileName;
                    StringBuilder sb = new StringBuilder(1024 * 128);

                    int columCount = grid.Columns.Count;
                    //int columCount = 11;
                    int rowCount = grid.RowCount;
                    string text = string.Empty;
                    string head = string.Empty;

                    for (int i = 1; i < columCount; i++)  //HeadText 저장
                    {
                        string tmp = string.Format(grid.Columns[i].GetCaption() + ",");
                        head += tmp.Replace("\r\n", " ");
                    }
                    sb.AppendFormat("{0},{1}", head, Environment.NewLine);

                    for (int i = 0; i < rowCount; i++)    //rows data 저장","
                    {
                        //for (int j = 0; j < columCount; j++)
                        for (int j = 1; j < columCount; j++)
                        {
                            string tmp;
                            if (grid.GetRowCellValue(i, grid.Columns[j]) == null) tmp = ",";
                            else
                            {
                                tmp = grid.GetRowCellValue(i, grid.Columns[j]).ToString();
                                tmp = tmp + ",";
                            }
                            text += tmp;
                        }
                        sb.AppendFormat("{0},{1}", text, Environment.NewLine);
                        text = string.Empty;
                    }
                    File.WriteAllText(fileName, sb.ToString(), Encoding.UTF8);

                }
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }
        }

        public void LogExceptionMessage(Exception ex)
        {
            //string message = ex.InnerException?.Message ?? ex.Message;
            //string message = ex.ToString();
            string message = ex.GetFullMessage() + Environment.NewLine + ex.StackTrace;
            Debug.WriteLine(message);
            EventLogger.Info(message);
            ACS_UI_Log(message);
        }

        public bool flag = false;
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            flag = true;
        }
    }
}
