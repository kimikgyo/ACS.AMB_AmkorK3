using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.Utils.Svg;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
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

namespace WindowsFormsApp9
{
    /// <summary>
    /// TODO
    /// 1. 엘리베이터 상태값 연결해야됨
    /// 2. 엘리베이터 통신 연결해야됨
    /// 3. DB 작업 - (IP, Port, Alias)
    /// 4. 
    /// </summary>

    public partial class ElevatorScreen : Form
    {
        public ElevatorScreen()
        {
            InitializeComponent();

            InitData();
            InitDesign();
            InitEvent();
        }

        private void InitData()
        {
            List<ElevatorInfo> infos = new List<ElevatorInfo>();

            ElevatorInfo info = new ElevatorInfo();
            info._Floor = "1층";
            info._Alias = "Amkor K3";
            info._Photo = Properties.Resources.Floor_1;
            infos.Add(info);

            ElevatorInfo info2 = new ElevatorInfo();
            info2._Floor = "2층";
            info2._Alias = "Amkor K3";
            info2._Photo = Properties.Resources.Floor_2;
            infos.Add(info2);

            ElevatorInfo info3 = new ElevatorInfo();
            info3._Floor = "3층";
            info3._Alias = "Amkor K3";
            info3._Photo = Properties.Resources.Floor_3;
            infos.Add(info3);

            var list = infos;
            var binddingList = new BindingList<ElevatorInfo>(list);
            CB_Floor.Properties.Items.AddRange(binddingList);
        }

        private void InitDesign()
        {
            GC_connect.GroupStyle = DevExpress.Utils.GroupStyle.Title;
            GC_Status.GroupStyle = DevExpress.Utils.GroupStyle.Title;
            GC_Controls.GroupStyle = DevExpress.Utils.GroupStyle.Title;

            Btn_Add.Appearance.BackColor = DXSkinColors.FillColors.Success;
            Btn_Add.Appearance.ForeColor = DXSkinColors.ForeColors.WindowText;
            Btn_Add.LookAndFeel.SetSkinStyle(SkinStyle.WXI);

            Pic_Open.Image = Properties.Resources.Elevator_Open;
            Pic_Open.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            Pic_Open.BackColor = Color.Transparent;
            Pic_Open.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            Pic_Close.Image = Properties.Resources.Elevator_Close;
            Pic_Close.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            Pic_Close.BackColor = Color.Transparent;
            Pic_Close.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            L_Floor.Text = "층수를 선택해주세요.";
            L_Floor.Font = new Font("맑은 고딕", 30);

            CB_Floor.Font = new Font("맑은 고딕", 30);
            CB_Floor.Properties.AppearanceDropDown.Font = new Font("맑은 고딕", 30);
        }

        private void InitEvent()
        {
            Btn_Add.Click += Btn_Add_Click;
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
            }
        }
    }

    public class ElevatorInfo
    {
        public string _Alias { get; set; }
        public string _Floor { get; set; }
        public Image _Photo { get; set; }

        public override string ToString()
        {
            return _Floor;
        }
    }
}
