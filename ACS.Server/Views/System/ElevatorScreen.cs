using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.Utils.Svg;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using INA_ACS_Server.OPWindows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INA_ACS_Server
{
    public partial class ElevatorScreen : Form
    {
        private List<ElevatorSystemInfo> ElevatorSystems;
        private List<ElevatorInfo> infos;
        private Timer timer;
        private readonly MainForm mainForm;
        private readonly IUnitOfWork uow;

        public ElevatorScreen(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();

            this.mainForm = mainForm;
            this.uow = uow;

            InitData();
            InitDesign();
            InitEvent();
            InitBroadcast();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.BackColor = mainForm.skinColor;

            GC_connect.LookAndFeel.Style = LookAndFeelStyle.Flat;
            GC_connect.LookAndFeel.UseDefaultLookAndFeel = false;
            GC_connect.Appearance.BackColor = mainForm.skinColor;
            GC_connect.Appearance.Options.UseBackColor = true;
            GC_connect.ForeColor = Color.White;
            L_IP.ForeColor = Color.White;
            T_IP.ForeColor = Color.White;
            T_IP.BackColor = mainForm.skinColor;
            T_IP.BorderStyle = BorderStyles.NoBorder;
            L_Port.ForeColor = Color.White;
            T_Port.ForeColor = Color.White;
            T_Port.BackColor = mainForm.skinColor;
            T_Port.BorderStyle = BorderStyles.NoBorder;
            L_Alias.ForeColor = Color.White;
            T_Alias.ForeColor = Color.White;
            T_Alias.BackColor = mainForm.skinColor;
            T_Alias.BorderStyle = BorderStyles.NoBorder;
            L_ConnectTitle.ForeColor = Color.White;
            L_ConnectTitle.Font = new Font("Tahoma", 9, FontStyle.Bold);
            
            panelControl1.BackColor = mainForm.skinColor;
            panelControl1.BorderStyle = BorderStyles.NoBorder;
            P_Body.BackColor = mainForm.skinColor;
            P_Body.BorderStyle = BorderStyles.NoBorder;
            panelControl2.BackColor = mainForm.skinColor;
            panelControl2.BorderStyle = BorderStyles.NoBorder;
            TLP_1.BackColor = mainForm.skinColor;
            
            P_Left.BackColor = mainForm.skinColor;
            P_Left.BorderStyle = BorderStyles.NoBorder;
            Lib_IPList.BackColor = mainForm.skinColor;
            Lib_IPList.ForeColor = Color.White;
            Lib_IPList.BorderStyle = BorderStyles.NoBorder;

            GC_Status.LookAndFeel.Style = LookAndFeelStyle.Flat;
            GC_Status.LookAndFeel.UseDefaultLookAndFeel = false;
            GC_Status.Appearance.BackColor = mainForm.skinColor;
            GC_Status.Appearance.Options.UseBackColor = true;
            L_FloorStatus.BackColor = mainForm.skinColor;
            L_FloorStatus.ForeColor = Color.White;
            L_ElevatorStatus.BackColor = mainForm.skinColor;
            L_ElevatorStatus.ForeColor = Color.White;
            L_Status.ForeColor = Color.White;
            L_Status.Font = new Font("Tahoma", 9, FontStyle.Bold);
            L_Floor.ForeColor = Color.White;

            L_Controls.ForeColor = Color.White;
            L_Controls.Font = new Font("Tahoma", 9, FontStyle.Bold);

            L_AGVMode.ForeColor = Color.White;
            L_AGVMode.Font = new Font("Tahoma", 9, FontStyle.Bold);
            L_All.ForeColor = Color.White;
            L_All.Font = new Font("Tahoma", 9, FontStyle.Bold);
            L_Floor2.ForeColor = Color.White;
            L_Floor2.Font = new Font("Tahoma", 9, FontStyle.Bold);
            L_B1F.ForeColor = Color.White;
            T_B1F.ForeColor = Color.White;
            L_1F.ForeColor = Color.White;
            T_1F.ForeColor = Color.White;
            L_2F.ForeColor = Color.White;
            T_2F.ForeColor = Color.White;
            L_3F.ForeColor = Color.White;
            T_3F.ForeColor = Color.White;
            L_4F.ForeColor = Color.White;
            T_4F.ForeColor = Color.White;
            L_5F.ForeColor = Color.White;
            T_5F.ForeColor = Color.White;
        }

        private void InitData()
        {
            //엘리베이터 층 정보
            infos = new List<ElevatorInfo>();
            infos.Clear();

            {
                ElevatorInfo info = new ElevatorInfo();
                info._Floor = "B1층";
                info._Alias = "Amkor K3";
                info._Photo = Properties.Resources.Floor_B1;
                info.DataValue = 0;
                infos.Add(info);
            }

            {
                ElevatorInfo info = new ElevatorInfo();
                info._Floor = "1층";
                info._Alias = "Amkor K3";
                info._Photo = Properties.Resources.Floor_1;
                info.DataValue = 1;
                infos.Add(info);
            }

            {
                ElevatorInfo info = new ElevatorInfo();
                info._Floor = "2층";
                info._Alias = "Amkor K3";
                info._Photo = Properties.Resources.Floor_2;
                info.DataValue = 2;
                infos.Add(info);
            }

            {
                ElevatorInfo info = new ElevatorInfo();
                info._Floor = "3층";
                info._Alias = "Amkor K3";
                info._Photo = Properties.Resources.Floor_3;
                info.DataValue = 3;
                infos.Add(info);
            }
            
            {
                var list = infos;
                var binddingList = new BindingList<ElevatorInfo>(list);
                CB_Floor.Properties.Items.AddRange(binddingList);
            }

            //엘리베이터 IP, Port 정보 불러오기
            DisplayIPList();
        }

        private void InitDesign()
        {
            GC_connect.GroupStyle = DevExpress.Utils.GroupStyle.Title;
            GC_Status.GroupStyle = DevExpress.Utils.GroupStyle.Title;
            GC_Controls.GroupStyle = DevExpress.Utils.GroupStyle.Title;

            Btn_Add.Appearance.BackColor = DXSkinColors.FillColors.Success;
            Btn_Add.Appearance.ForeColor = DXSkinColors.ForeColors.WindowText;
            Btn_Add.LookAndFeel.SetSkinStyle(SkinStyle.WXI);

            Btn_Delete.Appearance.BackColor = DXSkinColors.FillColors.Danger;
            Btn_Delete.Appearance.ForeColor = DXSkinColors.ForeColors.WindowText;
            Btn_Delete.LookAndFeel.SetSkinStyle(SkinStyle.WXI);

            Pic_Open.Image = Properties.Resources.Elevator_Open;
            Pic_Open.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            Pic_Open.BackColor = Color.Transparent;
            Pic_Open.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            Pic_Close.Image = Properties.Resources.Elevator_Close;
            Pic_Close.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            Pic_Close.BackColor = Color.Transparent;
            Pic_Close.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            L_Floor.Text = "층수를 선택해주세요.";
            L_Floor.Font = new Font("맑은 고딕", 15);

            CB_Floor.Font = new Font("맑은 고딕", 30);
            CB_Floor.Properties.AppearanceDropDown.Font = new Font("맑은 고딕", 30);

            Lib_IPList.Font = new Font("맑은 고딕", 30);
        }

        private void InitBroadcast()
        {
            mainForm.rS485.Start();
        }

        #region 일반 함수
        private void DisplayIPList()
        {
            Lib_IPList.Items.Clear();

            var Items = //uow.Elevator.Load();

            ElevatorSystems = new List<ElevatorSystemInfo>();
            ElevatorSystems.Clear();
            foreach (var item in Items)
            {
                ElevatorSystemInfo systemInfo = new ElevatorSystemInfo();
                systemInfo.IP = item.IP;
                systemInfo.Port = item.Port;
                systemInfo.Alias = item.Alias;

                ElevatorSystems.Add(systemInfo);
            }

            var list = ElevatorSystems;
            var binddingList = new BindingList<ElevatorSystemInfo>(list);
            Lib_IPList.DataSource = binddingList;
        }

        public Font AutoFontSize(LabelControl label, String text)
        {
            Font ft;
            Graphics gp;
            SizeF sz;
            Single Faktor, FaktorX, FaktorY;

            gp = label.CreateGraphics();
            sz = gp.MeasureString(text, label.Font);
            gp.Dispose();

            if (sz.Width == 0)
                sz.Width = 1;

            if (sz.Height == 0)
                sz.Height = 1;

           FaktorX = (label.Width) / sz.Width;
            FaktorY = (label.Height) / sz.Height;

            if (FaktorX > FaktorY)
                Faktor = FaktorY;
            else
                Faktor = FaktorX;
            ft = label.Font;

            return new Font(ft.Name, ft.SizeInPoints * (Faktor) - 1);
        }

        /// <summary>
        /// 0 = 초기값, 1 = AGVMode, 2 = Not AGVMode
        /// </summary>
        private int FlagAGVMode = 0;
        private void ElevatorAGVDisplay()
        {
            //엘리베이터 반송층(ToggleSwitch) 및 AGVMode(전체)
            var ElevatorInfo = uow.ElevatorInfo.DBGetAll();
            foreach (var item in ElevatorInfo)
            {
                //반송층
                if (item.FloorIndex == "B1F" && item.TransportMode.ToUpper() == "ON")
                {
                    T_B1F.IsOn = true;
                    if (mainForm.Floors.Contains(item.FloorIndex))
                        mainForm.Floors.Remove(item.FloorIndex);
                }
                else if (item.FloorIndex == "1F" && item.TransportMode.ToUpper() == "ON")
                {
                    T_1F.IsOn = true;
                    if (mainForm.Floors.Contains(item.FloorIndex))
                        mainForm.Floors.Remove(item.FloorIndex);
                }
                else if (item.FloorIndex == "2F" && item.TransportMode.ToUpper() == "ON")
                {
                    T_2F.IsOn = true;
                    if (mainForm.Floors.Contains(item.FloorIndex))
                        mainForm.Floors.Remove(item.FloorIndex);
                }
                else if (item.FloorIndex == "3F" && item.TransportMode.ToUpper() == "ON")
                {
                    T_3F.IsOn = true;
                    if (mainForm.Floors.Contains(item.FloorIndex))
                        mainForm.Floors.Remove(item.FloorIndex);
                }
                else if (item.FloorIndex == "4F" && item.TransportMode.ToUpper() == "ON")
                {
                    T_4F.IsOn = true;
                    if (mainForm.Floors.Contains(item.FloorIndex))
                        mainForm.Floors.Remove(item.FloorIndex);
                }                    
                else if (item.FloorIndex == "5F" && item.TransportMode.ToUpper() == "ON")
                {
                    T_5F.IsOn = true;
                    if (mainForm.Floors.Contains(item.FloorIndex))
                        mainForm.Floors.Remove(item.FloorIndex);
                }
                    
                if (item.FloorIndex == "B1F" && item.TransportMode.ToUpper() == "OFF")
                {
                    T_B1F.IsOn = false;
                    if (!mainForm.Floors.Contains(item.FloorIndex))
                        mainForm.Floors.Add(item.FloorIndex);
                }
                else if (item.FloorIndex == "1F" && item.TransportMode.ToUpper() == "OFF")
                {
                    T_1F.IsOn = false;
                    if (!mainForm.Floors.Contains(item.FloorIndex))
                        mainForm.Floors.Add(item.FloorIndex);
                }                    
                else if (item.FloorIndex == "2F" && item.TransportMode.ToUpper() == "OFF")
                {
                    T_2F.IsOn = false;
                    if (!mainForm.Floors.Contains(item.FloorIndex))
                        mainForm.Floors.Add(item.FloorIndex);
                }
                else if (item.FloorIndex == "3F" && item.TransportMode.ToUpper() == "OFF")
                {
                    T_3F.IsOn = false;
                    if (!mainForm.Floors.Contains(item.FloorIndex))
                        mainForm.Floors.Add(item.FloorIndex);
                }
                else if (item.FloorIndex == "4F" && item.TransportMode.ToUpper() == "OFF")
                {
                    T_4F.IsOn = false;
                    if (!mainForm.Floors.Contains(item.FloorIndex))
                        mainForm.Floors.Add(item.FloorIndex);
                }
                else if (item.FloorIndex == "5F" && item.TransportMode.ToUpper() == "OFF")
                {
                    T_5F.IsOn = false;
                    if (!mainForm.Floors.Contains(item.FloorIndex))
                        mainForm.Floors.Add(item.FloorIndex);
                }
                    
                //전체
                if (item.ACSMode == ElevatorAGVMode.MiRControlMode.ToString())
                {
                    this.Btn_AGVModeON.LookAndFeel.Style = LookAndFeelStyle.UltraFlat;
                    this.Btn_AGVModeON.LookAndFeel.UseDefaultLookAndFeel = false;
                    this.Btn_AGVModeON.Appearance.BackColor = Color.LightGreen;
                    this.Btn_AGVModeON.Appearance.Options.UseBackColor = true;

                    this.Btn_AGVModeOFF.LookAndFeel.Style = LookAndFeelStyle.UltraFlat;
                    this.Btn_AGVModeOFF.LookAndFeel.UseDefaultLookAndFeel = false;
                    this.Btn_AGVModeOFF.Appearance.BackColor = Color.White;
                    this.Btn_AGVModeOFF.Appearance.Options.UseBackColor = true;

                    mainForm.ACSMode = ElevatorAGVMode.MiRControlMode;

                    if (FlagAGVMode == 0 || FlagAGVMode == 2)
                    {
                        ElevatorInfoModel infoModel = new ElevatorInfoModel();
                        infoModel.Location = "Elevator1";
                        infoModel.ElevatorMode = "AGVMode";
                        uow.ElevatorInfo.ElevatorModeUpdate(infoModel);
                        mainForm.rS485.FlagWrite = 4;
                        FlagAGVMode = 1;
                    }
                }
                else
                {
                    this.Btn_AGVModeON.LookAndFeel.Style = LookAndFeelStyle.Flat;
                    this.Btn_AGVModeON.LookAndFeel.UseDefaultLookAndFeel = false;
                    this.Btn_AGVModeON.Appearance.BackColor = Color.White;
                    this.Btn_AGVModeON.Appearance.Options.UseBackColor = true;

                    this.Btn_AGVModeOFF.LookAndFeel.Style = LookAndFeelStyle.Flat;
                    this.Btn_AGVModeOFF.LookAndFeel.UseDefaultLookAndFeel = false;
                    this.Btn_AGVModeOFF.Appearance.BackColor = Color.LightGreen;
                    this.Btn_AGVModeOFF.Appearance.Options.UseBackColor = true;

                    mainForm.ACSMode = ElevatorAGVMode.MiRUnControlMode;

                    if (FlagAGVMode == 0 || FlagAGVMode == 1)
                    {
                        ElevatorInfoModel infoModel = new ElevatorInfoModel();
                        infoModel.Location = "Elevator1";
                        infoModel.ElevatorMode = "NotAGVMode";
                        uow.ElevatorInfo.ElevatorModeUpdate(infoModel);
                        mainForm.rS485.FlagWrite = 5;
                        FlagAGVMode = 2;
                    }
                }
            }
        }
        #endregion

        #region 이벤트 모음
        private void InitEvent()
        {
            Btn_Add.Click += Btn_Add_Click;
            Btn_Delete.Click += Btn_Delete_Click;
            L_FloorStatus.TextChanged += L_ElevatorStatus_TextChanged;
            L_ElevatorStatus.TextChanged += L_ElevatorStatus_TextChanged;
            Pic_Open.Click += Pic_Open_Click;
            Pic_Close.Click += Pic_Close_Click;
            CB_Floor.SelectedIndexChanged += CB_Floor_SelectedIndexChanged;
            Btn_AGVModeON.Click += Btn_AGVModeON_Click;
            Btn_AGVModeOFF.Click += Btn_AGVModeOFF_Click;
            T_B1F.Click += T_Floors_Click;
            T_1F.Click += T_Floors_Click;
            T_2F.Click += T_Floors_Click;
            T_3F.Click += T_Floors_Click;
            T_4F.Click += T_Floors_Click;
            T_5F.Click += T_Floors_Click;
            
            timer = new Timer();
            timer.Enabled = true;
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Enabled = false;
            L_ElevatorStatus.Text = mainForm.ElevatorState;
            L_FloorStatus.Text = mainForm.ElevatorFloor;
            this.Location = new Point(0, 0);
            ElevatorAGVDisplay();
            timer.Enabled = true;
        }

        private void Btn_Add_Click(object sender, EventArgs e)
        {
            //IP 유효성 검사
            IPAddress address;
            if (!IPAddress.TryParse(T_IP.Text, out address) || T_IP.Text.Split('.').Length != 4)
            {
                MessageBox.Show("유효한 IP가 아닙니다!");
                return;
            }

            //Port 유효성 검사
            int Port;
            if (!Int32.TryParse(T_Port.Text, out Port))
            {
                MessageBox.Show("유효한 Port가 아닙니다!");
                return;
            }

            //Alias 길이 검사
            if (T_Alias.Text.Length > 100)
            {
                MessageBox.Show("Alias 길이가 100자가 넘습니다!");
                return;
            }

            //추가되어져있는 IP, Port 중복 검사
            foreach (var item in ElevatorSystems)
            {
                if (address.ToString() == item.IP && Port == item.Port)
                {
                    MessageBox.Show("이미 같은 IP, Port가 존재합니다.");
                    return;
                }
            }

            //유효 IP일 경우
            //ElevatorModel model = new ElevatorModel();
            //model.IP = address.ToString();
            //model.Port = Port;
            //model.Alias = T_Alias.Text;

            //uow.Elevator.Add(model);
            DisplayIPList();
        }

        private void Btn_Delete_Click(object sender, EventArgs e)
        {
            //string IPList = Lib_IPList.SelectedValue.ToString();

            //ElevatorModel model = new ElevatorModel();
            //model.IP = IPList.Split(':')[0];
            //model.Port = Convert.ToInt32(IPList.Split(':')[1]);
            //uow.Elevator.Remove(model);

            DisplayIPList();
        }

        private void L_ElevatorStatus_TextChanged(object sender, EventArgs e)
        {
            L_FloorStatus.Font = AutoFontSize(L_FloorStatus, L_FloorStatus.Text);
            L_ElevatorStatus.Font = AutoFontSize(L_ElevatorStatus, L_ElevatorStatus.Text);
        }

        private void Pic_Open_Click(object sender, EventArgs e)
        {
            mainForm.rS485.FlagWrite = 1;
            mainForm.rS485.FlagData = true;
        }

        private void Pic_Close_Click(object sender, EventArgs e)
        {
            mainForm.rS485.FlagWrite = 2;
            mainForm.rS485.FlagData = true;
        }

        private void CB_Floor_SelectedIndexChanged(object sender, EventArgs e)
        {
            var button = (ComboBoxEdit) sender;
            var FloorDataValue = infos.FirstOrDefault(x => x._Floor == button.EditValue.ToString()).DataValue;

            mainForm.rS485.Floor = FloorDataValue;
            mainForm.rS485.FlagWrite = 3;
            mainForm.rS485.FlagData = true;
        }

        private void Btn_AGVModeON_Click(object sender, EventArgs e)
        {
            mainForm.ACSMode = ElevatorAGVMode.MiRControlMode;

            ElevatorInfoModel model = new ElevatorInfoModel();
            model.Location = "Elevator1";
            model.ACSMode = ElevatorAGVMode.MiRControlMode.ToString();

            uow.ElevatorInfo.ACSModeUpdate(model);

            mainForm.rS485.FlagWrite = 4;
            mainForm.rS485.FlagData = true;
        }

        private void Btn_AGVModeOFF_Click(object sender, EventArgs e)
        {
            mainForm.ACSMode = ElevatorAGVMode.MiRUnControlMode;

            ElevatorInfoModel model = new ElevatorInfoModel();
            model.Location = "Elevator1";
            model.ACSMode = ElevatorAGVMode.MiRUnControlMode.ToString();

            uow.ElevatorInfo.ACSModeUpdate(model);

            mainForm.rS485.FlagWrite = 5;
            mainForm.rS485.FlagData = true;
        }

        private void T_Floors_Click(object sender, EventArgs e)
        {
            var ToggleSwitch = (ToggleSwitch)sender;

            if (!(bool)ToggleSwitch.EditValue)
            {
                //ON
                ElevatorInfoModel model = new ElevatorInfoModel();
                model.FloorIndex = ToggleSwitch.Name.Split('_')[1];
                model.TransportMode = "ON";

                uow.ElevatorInfo.TransportModeUpdate(model);
            }
            else
            {
                //OFF
                ElevatorInfoModel model = new ElevatorInfoModel();
                model.FloorIndex = ToggleSwitch.Name.Split('_')[1];
                model.TransportMode = "OFF";

                uow.ElevatorInfo.TransportModeUpdate(model);
            }
        }
        #endregion
    }

    public class ElevatorInfo
    {
        public string _Alias { get; set; }
        public string _Floor { get; set; }
        public Image _Photo { get; set; }
        public int DataValue { get; set; }

        public override string ToString()
        {
            return _Floor;
        }
    }

    public class ElevatorSystemInfo
    {
        public string Alias { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }

        public override string ToString()
        {
            return IP + ":" + Port;
        }
    }
}
