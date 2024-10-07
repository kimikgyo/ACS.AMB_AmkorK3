using ACS.RobotMap;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraGrid.Views.Grid;
using INA_ACS_Server.OPWindows;
using INA_ACS_Server.UI;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace INA_ACS_Server
{
    public partial class MainForm : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        private readonly static ILog EventLogger = LogManager.GetLogger("Event");
        private readonly static ILog UserLogger = LogManager.GetLogger("User");
        private readonly static ILog TimeoutLogger = LogManager.GetLogger("Timeout");
        private static string AmkorImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image", "Amkor.png");
        Image AmkorImage = Image.FromFile(AmkorImagePath);
        private static string AmkorIconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image", "Monitoring_Icon.ico");
        Image AmkorIcon = Image.FromFile(AmkorIconPath);
        private AutoScreen ChildMainForm;
        private JobHistoryScreen ChildReportForm;
        private ErrorHistoryScreen ChildErrorHistoryForm;
        private MapMonitoring ChildMapForm;
        private SystemScreen ChildSystemForm; 
        private readonly UnitOfWork uow;
        private MainLoop fleetThread = null;
        private MapReadDtoQueue<MapReadDto> _mapDataQueue = new MapReadDtoQueue<MapReadDto>();
        private List<IMapReadService> _mapDataReadSvc = new List<IMapReadService>();

        public string ElevatorState = "";
        public string ElevatorFloor = "";
        public List<string> Floors = new List<string>();
        public TestForm testForm;
        public ElevatorAGVMode ACSMode = ElevatorAGVMode.MiRControlMode;
        public WirelessSensorRS485 rS485;
        public Color skinColor = Color.FromArgb(43, 52, 59);
        public Color backColor = Color.FromArgb(30, 39, 46);
        public Color mouseOverColor = Color.FromArgb(45, 65, 77);
        public Color mouseOverTextColor = Color.FromArgb(57, 173, 233);
        public Color nomalTextColor = Color.FromArgb(167, 168, 169);

        public MainForm()
        {
            InitializeComponent();

            uow = new UnitOfWork();
            rS485 = new WirelessSensorRS485(this, uow);

            Init();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FormInit();
            accordionControlInit();
            ChildMainFormFunc();
        }

        #region LoadData
        private void Init()
        {
            // ================ api clients 객체 초기화
            this._fleetApi = InitFleetApiClient(ApiLogger);
            this._mirApiList = InitMirApiClients(ApiLogger);

            fleetThread = new MainLoop(this, uow, _fleetApi);
            fleetThread.Start();

            for (int mapCount = 1; mapCount <= 3; mapCount++)
            {
                IMapReadService mapReadService = new FleetMapReadService(_mapDataQueue, EventLogger, _fleetApi, uow);
                var MapInfo = uow.FloorMapIDConfigs.GetAll().FirstOrDefault(x => x.Id == mapCount);
                if (MapInfo != null)
                {
                    mapReadService.MapGuid = MapInfo.MapID;
                    mapReadService.MapName = MapInfo.FloorName;
                    _mapDataReadSvc.Add(mapReadService);
                    mapReadService.Start();
                }
            }

            // 테스트 폼 초기화
            testForm = new TestForm(this, uow);

            lbl_Login.Visible = true;
            lbl_LoginOut.Visible = false;
        }
        private void FormInit()
        {
            this.MinimizeBox = false;  // 최소화 버튼 비활성
            this.MaximizeBox = true;  // 최대화 버튼 비활성화

            //this.LookAndFeel.UseDefaultLookAndFeel = false;
            //this.LookAndFeel.SkinName = "DevExpress Dark Style";
            //this.LookAndFeel.SkinMaskColor = skinColor;
            //this.LookAndFeel.SkinMaskColor2 = skinColor;
            //this.BackColor = backColor;
            //this.ForeColor = Color.White;
            //this.IconOptions.Image = AmkorIcon;

            fluentDesignFormContainer1.BackColor = backColor;
            fluentDesignFormContainer1.ForeColor = Color.Black;

            panelControl1.Visible = true;
            panelControl1.BackColor = Color.FromArgb(80, 89, 96);

            pictureEdit1.BackColor = skinColor;
            pictureEdit1.Image = AmkorImage;

            lbl_Login.Cursor = Cursors.Hand;
            lbl_Login.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lbl_Login.Click += Lbl_Login_Click;

            lbl_LoginOut.Cursor = Cursors.Hand;
            lbl_LoginOut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lbl_LoginOut.Click += Lbl_LoginOut_Click;
        }

        private void accordionControlInit()
        {
            //설정창 ViewType
            accordionControl1.ViewType = AccordionControlViewType.HamburgerMenu;

            accordionControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            accordionControl1.LookAndFeel.SkinName = "DevExpress Dark Style";
            accordionControl1.BackColor = skinColor;
            accordionControl1.LookAndFeel.SkinMaskColor = skinColor;
            accordionControl1.LookAndFeel.SkinMaskColor2 = skinColor;

            accordionControl1.Cursor = Cursors.Hand;


            #region 그룹과 아이템에 대한 색상 변경
            accordionControl1.Appearance.Group.Normal.ForeColor = nomalTextColor; // 기본 텍스트 색상
            //accordionControl1.Appearance.Group.Hovered.ForeColor = Color.Yellow; // 마우스 오버 시 텍스트 색상
            //accordionControl1.Appearance.Group.Pressed.ForeColor = Color.Red; // 클릭 시 텍스트 색상

            accordionControl1.Appearance.Item.Normal.ForeColor = nomalTextColor; // 기본 텍스트 색상
                                                                                 //accordionControl1.Appearance.Item.Hovered.ForeColor = Color.Yellow; // 마우스 오버 시 텍스트 색상
                                                                                 //accordionControl1.Appearance.Item.Pressed.ForeColor = Color.Red; // 클릭 시 텍스트 색상
            #endregion

            #region accordionControl 색상 변경

            accordionControlMainView.Appearance.Normal.BackColor = skinColor; // 기본 색상
            accordionControlMainView.Appearance.Hovered.BackColor = mouseOverColor; // 마우스 오버 색상
            accordionControlMainView.Appearance.Hovered.ForeColor = mouseOverTextColor; // 마우스 오버 글자 색상
            accordionControlMainView.Tag = "MainView";

            accordionControlReport.Appearance.Normal.BackColor = skinColor; // 기본 색상
            accordionControlReport.Appearance.Hovered.BackColor = mouseOverColor; // 마우스 오버 색상
            accordionControlReport.Appearance.Hovered.ForeColor = mouseOverTextColor; // 마우스 오버 글자 색상
            accordionControlReport.Tag = "Report";

            accordionControlErrorLog.Appearance.Normal.BackColor = skinColor; // 기본 색상
            accordionControlErrorLog.Appearance.Hovered.BackColor = mouseOverColor; // 마우스 오버 색상
            accordionControlErrorLog.Appearance.Hovered.ForeColor = mouseOverTextColor; // 마우스 오버 글자 색상
            accordionControlErrorLog.Tag = "ErrorLog";

            accordionControlJobLog.Appearance.Normal.BackColor = skinColor; // 기본 색상
            accordionControlJobLog.Appearance.Hovered.BackColor = mouseOverColor; // 마우스 오버 색상
            accordionControlJobLog.Appearance.Hovered.ForeColor = mouseOverTextColor; // 마우스 오버 글자 색상
            accordionControlJobLog.Tag = "JobLog";

            accordionControlMapView.Appearance.Normal.BackColor = skinColor; // 기본 색상
            accordionControlMapView.Appearance.Hovered.BackColor = mouseOverColor; // 마우스 오버 색상
            accordionControlMapView.Appearance.Hovered.ForeColor = mouseOverTextColor; // 마우스 오버 글자 색상
            accordionControlMapView.Tag = "MapView";

            accordionControlSetting.Appearance.Normal.BackColor = skinColor; // 기본 색상
            accordionControlSetting.Appearance.Hovered.BackColor = mouseOverColor; // 마우스 오버 색상
            accordionControlSetting.Appearance.Hovered.ForeColor = mouseOverTextColor; // 마우스 오버 글자 색상
            accordionControlSetting.Tag = "Setting";

            #endregion

            accordionControl1.ElementClick += AccordionControl1_ElementClick;
        }

        private void ChildMainFormFunc()
        {
            ChildMainForm = new AutoScreen(this, uow);
            ChildMainForm.TopLevel = false;
            ChildMainForm.Dock = DockStyle.Fill;
            fluentDesignFormContainer1.BackColor = Color.White;
            fluentDesignFormContainer1.Controls.Add(ChildMainForm);
            ChildMainForm.Activate();
            ChildMainForm.Show();

            ChildReportForm = new JobHistoryScreen(this, uow);
            ChildReportForm.TopLevel = false;
            ChildReportForm.Dock = DockStyle.Fill;

            ChildErrorHistoryForm = new ErrorHistoryScreen(this, uow);
            ChildErrorHistoryForm.TopLevel = false;
            ChildErrorHistoryForm.Dock = DockStyle.Fill;

            ChildMapForm = new MapMonitoring(this, uow, _mapDataQueue, _mapDataReadSvc);
            ChildMapForm.TopLevel = false;
            ChildMapForm.Dock = DockStyle.Fill;

            ChildSystemForm = new SystemScreen(this, uow);
            ChildSystemForm.TopLevel = false;
            ChildSystemForm.Dock = DockStyle.Fill;
        }
        #endregion

        #region Event
        private void Lbl_Login_Click(object sender, EventArgs e)
        {
            //사번 입력 Form을 불러옴
            var UserNumberForm = new UserNumberForm();
            DialogResult result = UserNumberForm.ShowDialog();
            if (result == DialogResult.Yes)
            {
                //사번 입력하여 검색
                var UserNumber = uow.UserNumber.DBGetAll().FirstOrDefault(u => u.UserNumber == UserNumberForm.UserNumber);
                //사번이 없는경우 설정창을 Clear한후 다시 초기설정함
                if (UserNumber == null)
                {
                    if (UserNumberForm.UserNumber == "MAINT" && UserNumberForm.UserPassword == ConfigData.sMaint_Password)
                    {
                        ConfigData.UserNumber = "MAINT";
                        ConfigData.UserName = ConfigData.sMaint_Password;
                        ConfigData.UserElevatorAuthority = 1;
                        ConfigData.UserCallAuthority = 1;
                        ConfigData.sAccessLevel = "Maint";

                        lbl_Login.Visible = false;
                        lbl_LoginOut.Visible = true;
                        lbl_LoginOut.BringToFront();
                        accordionControlSetting.Visible = true;
                    }
                    else if (UserNumberForm.UserNumber == "INATECH" && UserNumberForm.UserPassword == "INATECH")
                    {
                        ConfigData.UserNumber = "INATECH";
                        ConfigData.UserName = "INATECH";
                        ConfigData.UserElevatorAuthority = 1;
                        ConfigData.UserCallAuthority = 1;
                        ConfigData.sAccessLevel = "Engineer";

                        lbl_Login.Visible = false;
                        lbl_LoginOut.Visible = true;
                        lbl_LoginOut.BringToFront();
                        accordionControlSetting.Visible = true;
                    }
                    else
                    {
                        lbl_Login.Visible = true;
                        lbl_LoginOut.Visible = false;

                        MessageBox.Show("등록되지 않은 사원번호 이거나 비밀번호가 틀립니다!");
                    }
                }
                else
                {
                    ConfigData.UserNumber = UserNumber.UserNumber;
                    ConfigData.UserName = UserNumber.UserName;
                    ConfigData.UserElevatorAuthority = UserNumber.ElevatorAuthority;
                    ConfigData.UserCallAuthority = UserNumber.CallMissionAuthority;

                    lbl_Login.Visible = false;
                    lbl_LoginOut.Visible = true;
                    lbl_LoginOut.BringToFront();

                    string LableText = $"사원번호 = {ConfigData.UserNumber} / 사원이름 = {ConfigData.UserName}";

                    if (ConfigData.UserElevatorAuthority == 1)
                    {
                        LableText += " / ElevatorSystem 사용가능";
                    }
                    if (ConfigData.UserCallAuthority == 1)
                    {
                        LableText += " / CallSystem 사용가능";
                    }

                    Ibl_LoginAlarm.Text = LableText;
                    Ibl_LoginAlarm.Font = new Font("맑은 고딕", 15, FontStyle.Bold);
                    Ibl_LoginAlarm.ForeColor = Color.White;
                }
            }
        }

        private void Lbl_LoginOut_Click(object sender, EventArgs e)
        {
            ConfigData.UserNumber = "";
            ConfigData.UserName = "";
            ConfigData.UserElevatorAuthority = 0;
            ConfigData.UserCallAuthority = 0;
            ConfigData.sAccessLevel = "";

            Ibl_LoginAlarm.Text = "";

            lbl_Login.Visible = true;
            lbl_Login.BringToFront();
            lbl_LoginOut.Visible = false;
            accordionControlSetting.Visible = false;
        }
        private void AccordionControl1_ElementClick(object sender, ElementClickEventArgs e)
        {
            if (e.Element.Style == ElementStyle.Group) return;
            if (e.Element.Tag == null) return;
            string itemID = e.Element.Tag.ToString();

            switch (itemID)
            {
                case "MainView":
                    fluentDesignFormContainer1.Controls.Clear();
                    fluentDesignFormContainer1.BackColor = Color.White;
                    fluentDesignFormContainer1.Controls.Add(ChildMainForm);
                    ChildMainForm.Activate();
                    ChildMainForm.Show();

                    break;
                case "JobLog":
                    fluentDesignFormContainer1.Controls.Clear();
                    fluentDesignFormContainer1.BackColor = Color.White;
                    fluentDesignFormContainer1.Controls.Add(ChildReportForm);
                    ChildReportForm.Activate();
                    ChildReportForm.Show();

                    break;
                case "ErrorLog":
                    fluentDesignFormContainer1.Controls.Clear();
                    fluentDesignFormContainer1.BackColor = Color.White;
                    fluentDesignFormContainer1.Controls.Add(ChildErrorHistoryForm);
                    ChildErrorHistoryForm.Activate();
                    ChildErrorHistoryForm.Show();

                    break;
                case "MapView":
                    fluentDesignFormContainer1.Controls.Clear();
                    fluentDesignFormContainer1.BackColor = Color.White;
                    fluentDesignFormContainer1.Controls.Add(ChildMapForm);
                    ChildMapForm.Activate();
                    ChildMapForm.Show();

                    break;
                case "Setting":
                    if (ConfigData.UserNumber == "MAINT" || ConfigData.UserNumber == "INATECH")
                    {
                        fluentDesignFormContainer1.Controls.Clear();
                        fluentDesignFormContainer1.BackColor = Color.White;
                        fluentDesignFormContainer1.Controls.Add(ChildSystemForm);
                        ChildSystemForm.Activate();
                        ChildSystemForm.Show();
                    }

                    break;

            }
        }
        #endregion

        #region Func
        public void LogExceptionMessage(Exception ex)
        {
            //string message = ex.InnerException?.Message ?? ex.Message;
            //string message = ex.ToString();
            string message = ex.GetFullMessage() + Environment.NewLine + ex.StackTrace;
            Debug.WriteLine(message);
            EventLogger.Info(message);
            ACS_UI_Log(message);
        }

        public void ACS_UI_Log(string logMessage, [CallerFilePath] string file = "",
                                                  [CallerLineNumber] int line = 0,
                                                  [CallerMemberName] string member = "")
        {
            string tmp = $"{Path.GetFileName(file)}({line})";
            string logMessageFormatted = string.Format("{0:HH:mm:ss}, {1,-35}, {2}", DateTime.Now, tmp, logMessage);

            AcsLogMessageQueue.Enqueue(logMessageFormatted);
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

        public void subFuncMessagePopUp(string sText)
        {
            MsgPopupForm MsgPopup = new MsgPopupForm(sText);
            MsgPopup.Show();
        }
        #endregion
    }
}
