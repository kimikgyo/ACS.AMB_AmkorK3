namespace INA_ACS_Server
{
    partial class MainForm2
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.grb_Fleet_Connect = new System.Windows.Forms.GroupBox();
            this.lbl_Fleet_Connection = new System.Windows.Forms.Label();
            this.pnl_Fleet_Connect = new System.Windows.Forms.Panel();
            this.pnl_Fleet_Disconnect = new System.Windows.Forms.Panel();
            this.grb_RobotConnect = new System.Windows.Forms.GroupBox();
            this.pnl_RobotConnect = new System.Windows.Forms.Panel();
            this.lbl_RobotConnectCount = new System.Windows.Forms.Label();
            this.pnl_RobotDisConnect = new System.Windows.Forms.Panel();
            this.lbl_RobotConnection = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbl_System_Day = new System.Windows.Forms.Label();
            this.lbl_System_Time = new System.Windows.Forms.Label();
            this.btn_Login = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_LoginName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_ErrorHistory = new System.Windows.Forms.Button();
            this.btn_AcsLogViewer = new System.Windows.Forms.Button();
            this.btn_Map = new System.Windows.Forms.Button();
            this.btn_Parts = new System.Windows.Forms.Button();
            this.btn_Data = new System.Windows.Forms.Button();
            this.btn_Setting = new System.Windows.Forms.Button();
            this.btn_Auto = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.Timer_1Sec = new System.Windows.Forms.Timer(this.components);
            this.Alarm_Timer = new System.Windows.Forms.Timer(this.components);
            this.PopCall_Msg_Timer = new System.Windows.Forms.Timer(this.components);
            this.textEdit2 = new DevExpress.XtraEditors.TextEdit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            this.grb_Fleet_Connect.SuspendLayout();
            this.grb_RobotConnect.SuspendLayout();
            this.pnl_RobotConnect.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textEdit2);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Controls.Add(this.simpleButton1);
            this.panel1.Controls.Add(this.textEdit1);
            this.panel1.Controls.Add(this.grb_Fleet_Connect);
            this.panel1.Controls.Add(this.grb_RobotConnect);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.btn_Login);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1920, 110);
            this.panel1.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(592, 53);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(70, 14);
            this.labelControl1.TabIndex = 299;
            this.labelControl1.Text = "labelControl1";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(747, 22);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(104, 20);
            this.simpleButton1.TabIndex = 298;
            this.simpleButton1.Text = "simpleButton1";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // textEdit1
            // 
            this.textEdit1.EditValue = "";
            this.textEdit1.Location = new System.Drawing.Point(592, 22);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(70, 20);
            this.textEdit1.TabIndex = 297;
            // 
            // grb_Fleet_Connect
            // 
            this.grb_Fleet_Connect.Controls.Add(this.lbl_Fleet_Connection);
            this.grb_Fleet_Connect.Controls.Add(this.pnl_Fleet_Connect);
            this.grb_Fleet_Connect.Controls.Add(this.pnl_Fleet_Disconnect);
            this.grb_Fleet_Connect.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.grb_Fleet_Connect.Location = new System.Drawing.Point(3, 8);
            this.grb_Fleet_Connect.Name = "grb_Fleet_Connect";
            this.grb_Fleet_Connect.Size = new System.Drawing.Size(239, 99);
            this.grb_Fleet_Connect.TabIndex = 295;
            this.grb_Fleet_Connect.TabStop = false;
            this.grb_Fleet_Connect.Text = "Fleet Connection";
            // 
            // lbl_Fleet_Connection
            // 
            this.lbl_Fleet_Connection.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Fleet_Connection.Location = new System.Drawing.Point(82, 44);
            this.lbl_Fleet_Connection.Name = "lbl_Fleet_Connection";
            this.lbl_Fleet_Connection.Size = new System.Drawing.Size(151, 21);
            this.lbl_Fleet_Connection.TabIndex = 117;
            this.lbl_Fleet_Connection.Text = "Disconnect";
            this.lbl_Fleet_Connection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnl_Fleet_Connect
            // 
            this.pnl_Fleet_Connect.BackgroundImage = global::INA_ACS_Server.Properties.Resources._60_connect;
            this.pnl_Fleet_Connect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnl_Fleet_Connect.Location = new System.Drawing.Point(16, 25);
            this.pnl_Fleet_Connect.Name = "pnl_Fleet_Connect";
            this.pnl_Fleet_Connect.Size = new System.Drawing.Size(60, 60);
            this.pnl_Fleet_Connect.TabIndex = 5;
            this.pnl_Fleet_Connect.Visible = false;
            // 
            // pnl_Fleet_Disconnect
            // 
            this.pnl_Fleet_Disconnect.BackgroundImage = global::INA_ACS_Server.Properties.Resources._60_disconnect;
            this.pnl_Fleet_Disconnect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnl_Fleet_Disconnect.Location = new System.Drawing.Point(16, 25);
            this.pnl_Fleet_Disconnect.Name = "pnl_Fleet_Disconnect";
            this.pnl_Fleet_Disconnect.Size = new System.Drawing.Size(60, 60);
            this.pnl_Fleet_Disconnect.TabIndex = 4;
            // 
            // grb_RobotConnect
            // 
            this.grb_RobotConnect.Controls.Add(this.pnl_RobotConnect);
            this.grb_RobotConnect.Controls.Add(this.pnl_RobotDisConnect);
            this.grb_RobotConnect.Controls.Add(this.lbl_RobotConnection);
            this.grb_RobotConnect.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.grb_RobotConnect.Location = new System.Drawing.Point(254, 8);
            this.grb_RobotConnect.Name = "grb_RobotConnect";
            this.grb_RobotConnect.Size = new System.Drawing.Size(239, 99);
            this.grb_RobotConnect.TabIndex = 296;
            this.grb_RobotConnect.TabStop = false;
            this.grb_RobotConnect.Text = "Robot Connection";
            // 
            // pnl_RobotConnect
            // 
            this.pnl_RobotConnect.BackgroundImage = global::INA_ACS_Server.Properties.Resources._60_connect;
            this.pnl_RobotConnect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnl_RobotConnect.Controls.Add(this.lbl_RobotConnectCount);
            this.pnl_RobotConnect.Location = new System.Drawing.Point(16, 25);
            this.pnl_RobotConnect.Name = "pnl_RobotConnect";
            this.pnl_RobotConnect.Size = new System.Drawing.Size(60, 60);
            this.pnl_RobotConnect.TabIndex = 5;
            this.pnl_RobotConnect.Visible = false;
            // 
            // lbl_RobotConnectCount
            // 
            this.lbl_RobotConnectCount.Location = new System.Drawing.Point(15, 23);
            this.lbl_RobotConnectCount.Name = "lbl_RobotConnectCount";
            this.lbl_RobotConnectCount.Size = new System.Drawing.Size(23, 19);
            this.lbl_RobotConnectCount.TabIndex = 300;
            this.lbl_RobotConnectCount.Text = "0";
            this.lbl_RobotConnectCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnl_RobotDisConnect
            // 
            this.pnl_RobotDisConnect.BackgroundImage = global::INA_ACS_Server.Properties.Resources._60_disconnect;
            this.pnl_RobotDisConnect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnl_RobotDisConnect.Location = new System.Drawing.Point(16, 25);
            this.pnl_RobotDisConnect.Name = "pnl_RobotDisConnect";
            this.pnl_RobotDisConnect.Size = new System.Drawing.Size(60, 60);
            this.pnl_RobotDisConnect.TabIndex = 4;
            // 
            // lbl_RobotConnection
            // 
            this.lbl_RobotConnection.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RobotConnection.Location = new System.Drawing.Point(82, 44);
            this.lbl_RobotConnection.Name = "lbl_RobotConnection";
            this.lbl_RobotConnection.Size = new System.Drawing.Size(151, 21);
            this.lbl_RobotConnection.TabIndex = 117;
            this.lbl_RobotConnection.Text = "Disconnect";
            this.lbl_RobotConnection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1204, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 37);
            this.button1.TabIndex = 296;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel3
            // 
            this.panel3.BackgroundImage = global::INA_ACS_Server.Properties.Resources.INA_Logo;
            this.panel3.Location = new System.Drawing.Point(1034, 49);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(266, 44);
            this.panel3.TabIndex = 114;
            this.panel3.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbl_System_Day);
            this.groupBox2.Controls.Add(this.lbl_System_Time);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(1687, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(225, 99);
            this.groupBox2.TabIndex = 294;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Time";
            // 
            // lbl_System_Day
            // 
            this.lbl_System_Day.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lbl_System_Day.AutoSize = true;
            this.lbl_System_Day.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_System_Day.Location = new System.Drawing.Point(38, 21);
            this.lbl_System_Day.Name = "lbl_System_Day";
            this.lbl_System_Day.Size = new System.Drawing.Size(153, 32);
            this.lbl_System_Day.TabIndex = 5;
            this.lbl_System_Day.Text = "2019-03-01";
            this.lbl_System_Day.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_System_Time
            // 
            this.lbl_System_Time.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lbl_System_Time.AutoSize = true;
            this.lbl_System_Time.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_System_Time.Location = new System.Drawing.Point(55, 58);
            this.lbl_System_Time.Name = "lbl_System_Time";
            this.lbl_System_Time.Size = new System.Drawing.Size(125, 32);
            this.lbl_System_Time.TabIndex = 6;
            this.lbl_System_Time.Text = "13:00:00";
            this.lbl_System_Time.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Login
            // 
            this.btn_Login.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_Login.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Login.Image = global::INA_ACS_Server.Properties.Resources._70_User;
            this.btn_Login.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Login.Location = new System.Drawing.Point(1319, 11);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(180, 90);
            this.btn_Login.TabIndex = 109;
            this.btn_Login.Text = "LOGIN";
            this.btn_Login.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Login.UseVisualStyleBackColor = false;
            this.btn_Login.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbl_LoginName);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(1516, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(153, 99);
            this.groupBox1.TabIndex = 290;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Level";
            // 
            // lbl_LoginName
            // 
            this.lbl_LoginName.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_LoginName.Location = new System.Drawing.Point(6, 42);
            this.lbl_LoginName.Name = "lbl_LoginName";
            this.lbl_LoginName.Size = new System.Drawing.Size(141, 21);
            this.lbl_LoginName.TabIndex = 116;
            this.lbl_LoginName.Text = "Operator";
            this.lbl_LoginName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(140, 935);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 115;
            this.label1.Text = "Version.220222";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_ErrorHistory);
            this.panel2.Controls.Add(this.btn_AcsLogViewer);
            this.panel2.Controls.Add(this.btn_Map);
            this.panel2.Controls.Add(this.btn_Parts);
            this.panel2.Controls.Add(this.btn_Data);
            this.panel2.Controls.Add(this.btn_Setting);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btn_Auto);
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(1680, 110);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(240, 951);
            this.panel2.TabIndex = 1;
            // 
            // btn_ErrorHistory
            // 
            this.btn_ErrorHistory.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_ErrorHistory.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ErrorHistory.Image = global::INA_ACS_Server.Properties.Resources._70_Data;
            this.btn_ErrorHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_ErrorHistory.Location = new System.Drawing.Point(3, 709);
            this.btn_ErrorHistory.Name = "btn_ErrorHistory";
            this.btn_ErrorHistory.Size = new System.Drawing.Size(225, 100);
            this.btn_ErrorHistory.TabIndex = 118;
            this.btn_ErrorHistory.Text = "ERROR\r\nHISTORY";
            this.btn_ErrorHistory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_ErrorHistory.UseVisualStyleBackColor = false;
            this.btn_ErrorHistory.Visible = false;
            this.btn_ErrorHistory.Click += new System.EventHandler(this.ScreenChange);
            // 
            // btn_AcsLogViewer
            // 
            this.btn_AcsLogViewer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_AcsLogViewer.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_AcsLogViewer.Image = global::INA_ACS_Server.Properties.Resources._70_Diagnosis;
            this.btn_AcsLogViewer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_AcsLogViewer.Location = new System.Drawing.Point(7, 151);
            this.btn_AcsLogViewer.Name = "btn_AcsLogViewer";
            this.btn_AcsLogViewer.Size = new System.Drawing.Size(225, 100);
            this.btn_AcsLogViewer.TabIndex = 117;
            this.btn_AcsLogViewer.Text = "LOG ";
            this.btn_AcsLogViewer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_AcsLogViewer.UseVisualStyleBackColor = false;
            this.btn_AcsLogViewer.Click += new System.EventHandler(this.ScreenChange);
            // 
            // btn_Map
            // 
            this.btn_Map.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_Map.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Map.Image = global::INA_ACS_Server.Properties.Resources.map;
            this.btn_Map.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Map.Location = new System.Drawing.Point(7, 398);
            this.btn_Map.Name = "btn_Map";
            this.btn_Map.Size = new System.Drawing.Size(225, 100);
            this.btn_Map.TabIndex = 116;
            this.btn_Map.Text = "MAP VIEW ";
            this.btn_Map.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Map.UseVisualStyleBackColor = false;
            this.btn_Map.Click += new System.EventHandler(this.ScreenChange);
            // 
            // btn_Parts
            // 
            this.btn_Parts.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_Parts.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Parts.Image = global::INA_ACS_Server.Properties.Resources._70_Manual;
            this.btn_Parts.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Parts.Location = new System.Drawing.Point(7, 623);
            this.btn_Parts.Name = "btn_Parts";
            this.btn_Parts.Size = new System.Drawing.Size(225, 100);
            this.btn_Parts.TabIndex = 116;
            this.btn_Parts.Text = "PART LIST ";
            this.btn_Parts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Parts.UseVisualStyleBackColor = false;
            this.btn_Parts.Visible = false;
            this.btn_Parts.Click += new System.EventHandler(this.ScreenChange);
            // 
            // btn_Data
            // 
            this.btn_Data.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_Data.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Data.Image = global::INA_ACS_Server.Properties.Resources._70_Data;
            this.btn_Data.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Data.Location = new System.Drawing.Point(7, 273);
            this.btn_Data.Name = "btn_Data";
            this.btn_Data.Size = new System.Drawing.Size(225, 100);
            this.btn_Data.TabIndex = 116;
            this.btn_Data.Text = "REPORT";
            this.btn_Data.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Data.UseVisualStyleBackColor = false;
            this.btn_Data.Click += new System.EventHandler(this.ScreenChange);
            // 
            // btn_Setting
            // 
            this.btn_Setting.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_Setting.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Setting.Image = global::INA_ACS_Server.Properties.Resources._70_System;
            this.btn_Setting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Setting.Location = new System.Drawing.Point(7, 517);
            this.btn_Setting.Name = "btn_Setting";
            this.btn_Setting.Size = new System.Drawing.Size(225, 100);
            this.btn_Setting.TabIndex = 108;
            this.btn_Setting.Text = "SETTINGS ";
            this.btn_Setting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Setting.UseVisualStyleBackColor = false;
            this.btn_Setting.Visible = false;
            this.btn_Setting.Click += new System.EventHandler(this.ScreenChange);
            // 
            // btn_Auto
            // 
            this.btn_Auto.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_Auto.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Auto.Image = global::INA_ACS_Server.Properties.Resources._70_Auto;
            this.btn_Auto.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Auto.Location = new System.Drawing.Point(7, 29);
            this.btn_Auto.Name = "btn_Auto";
            this.btn_Auto.Size = new System.Drawing.Size(225, 100);
            this.btn_Auto.TabIndex = 106;
            this.btn_Auto.Text = "OVERVIEW ";
            this.btn_Auto.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Auto.UseVisualStyleBackColor = false;
            this.btn_Auto.Click += new System.EventHandler(this.ScreenChange);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnExit.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Image = global::INA_ACS_Server.Properties.Resources._70_Exit;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(7, 815);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(225, 100);
            this.btnExit.TabIndex = 103;
            this.btnExit.Text = "PROGRAM EXIT ";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // Timer_1Sec
            // 
            this.Timer_1Sec.Enabled = true;
            this.Timer_1Sec.Interval = 1000;
            this.Timer_1Sec.Tick += new System.EventHandler(this.Timer_1Sec_Tick);
            // 
            // PopCall_Msg_Timer
            // 
            this.PopCall_Msg_Timer.Enabled = true;
            this.PopCall_Msg_Timer.Interval = 1000;
            this.PopCall_Msg_Timer.Tick += new System.EventHandler(this.PopCall_Msg_Timer_Tick);
            // 
            // textEdit2
            // 
            this.textEdit2.EditValue = "";
            this.textEdit2.Location = new System.Drawing.Point(671, 22);
            this.textEdit2.Name = "textEdit2";
            this.textEdit2.Size = new System.Drawing.Size(70, 20);
            this.textEdit2.TabIndex = 300;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(1920, 1061);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IsMdiContainer = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "INA ACS";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            this.grb_Fleet_Connect.ResumeLayout(false);
            this.grb_RobotConnect.ResumeLayout(false);
            this.pnl_RobotConnect.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btn_Setting;
        private System.Windows.Forms.Button btn_Auto;
        private System.Windows.Forms.Label lbl_System_Time;
        private System.Windows.Forms.Label lbl_System_Day;
        private System.Windows.Forms.Timer Timer_1Sec;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Login;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_LoginName;
        private System.Windows.Forms.Timer Alarm_Timer;
        private System.Windows.Forms.Button btn_Data;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_AcsLogViewer;
        private System.Windows.Forms.GroupBox grb_Fleet_Connect;
        private System.Windows.Forms.Panel pnl_Fleet_Connect;
        private System.Windows.Forms.Panel pnl_Fleet_Disconnect;
        private System.Windows.Forms.Label lbl_Fleet_Connection;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_Parts;
        private System.Windows.Forms.Button btn_Map;
        private System.Windows.Forms.Timer PopCall_Msg_Timer;
        private System.Windows.Forms.Button btn_ErrorHistory;
        private System.Windows.Forms.GroupBox grb_RobotConnect;
        private System.Windows.Forms.Label lbl_RobotConnection;
        private System.Windows.Forms.Panel pnl_RobotConnect;
        private System.Windows.Forms.Panel pnl_RobotDisConnect;
        private System.Windows.Forms.Label lbl_RobotConnectCount;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        public DevExpress.XtraEditors.LabelControl labelControl1;
        public DevExpress.XtraEditors.TextEdit textEdit1;
        public DevExpress.XtraEditors.TextEdit textEdit2;
    }
}

