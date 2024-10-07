namespace INA_ACS_Server.OPWindows
{
    partial class SystemScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemScreen));
            this.Auto_dtg_Display_Timer = new System.Windows.Forms.Timer(this.components);
            this.stackPanel1 = new DevExpress.Utils.Layout.StackPanel();
            this.btn_RobotControl = new DevExpress.XtraEditors.SimpleButton();
            this.btn_PositionArea = new DevExpress.XtraEditors.SimpleButton();
            this.btn_JobConfig = new DevExpress.XtraEditors.SimpleButton();
            this.btn_WaitMission = new DevExpress.XtraEditors.SimpleButton();
            this.btn_ChargeMission = new DevExpress.XtraEditors.SimpleButton();
            this.btn_RegistarSync = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Elevator = new DevExpress.XtraEditors.SimpleButton();
            this.btn_UserData = new DevExpress.XtraEditors.SimpleButton();
            this.tabSetting = new DevExpress.XtraTab.XtraTabControl();
            this.tab_JobConfig = new DevExpress.XtraTab.XtraTabPage();
            this.tab_PositionArea = new DevExpress.XtraTab.XtraTabPage();
            this.tab_WaitMission = new DevExpress.XtraTab.XtraTabPage();
            this.tab_RobotControl = new DevExpress.XtraTab.XtraTabPage();
            this.tab_AutoJobConfig = new DevExpress.XtraTab.XtraTabPage();
            this.tab_RegisterSync = new DevExpress.XtraTab.XtraTabPage();
            this.tab_ChargeMission = new DevExpress.XtraTab.XtraTabPage();
            this.tab_ErrorCodeList = new DevExpress.XtraTab.XtraTabPage();
            this.tab_Elevator = new DevExpress.XtraTab.XtraTabPage();
            this.tab_UserData = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.stackPanel1)).BeginInit();
            this.stackPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabSetting)).BeginInit();
            this.tabSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // Auto_dtg_Display_Timer
            // 
            this.Auto_dtg_Display_Timer.Interval = 1000;
            this.Auto_dtg_Display_Timer.Tick += new System.EventHandler(this.Auto_dtg_Display_Timer_Tick);
            // 
            // stackPanel1
            // 
            this.stackPanel1.Controls.Add(this.btn_RobotControl);
            this.stackPanel1.Controls.Add(this.btn_PositionArea);
            this.stackPanel1.Controls.Add(this.btn_JobConfig);
            this.stackPanel1.Controls.Add(this.btn_WaitMission);
            this.stackPanel1.Controls.Add(this.btn_ChargeMission);
            this.stackPanel1.Controls.Add(this.btn_RegistarSync);
            this.stackPanel1.Controls.Add(this.btn_Elevator);
            this.stackPanel1.Controls.Add(this.btn_UserData);
            this.stackPanel1.Location = new System.Drawing.Point(12, -10);
            this.stackPanel1.Name = "stackPanel1";
            this.stackPanel1.Size = new System.Drawing.Size(1648, 102);
            this.stackPanel1.TabIndex = 260;
            this.stackPanel1.UseSkinIndents = true;
            // 
            // btn_RobotControl
            // 
            this.btn_RobotControl.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btn_RobotControl.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_RobotControl.ImageOptions.SvgImage")));
            this.btn_RobotControl.Location = new System.Drawing.Point(13, 13);
            this.btn_RobotControl.Name = "btn_RobotControl";
            this.btn_RobotControl.Size = new System.Drawing.Size(110, 75);
            this.btn_RobotControl.TabIndex = 147;
            this.btn_RobotControl.Text = "Robot Control";
            this.btn_RobotControl.Click += new System.EventHandler(this.SettingButton_Config);
            // 
            // btn_PositionArea
            // 
            this.btn_PositionArea.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btn_PositionArea.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_PositionArea.ImageOptions.SvgImage")));
            this.btn_PositionArea.Location = new System.Drawing.Point(127, 13);
            this.btn_PositionArea.Name = "btn_PositionArea";
            this.btn_PositionArea.Size = new System.Drawing.Size(110, 75);
            this.btn_PositionArea.TabIndex = 148;
            this.btn_PositionArea.Text = "Position Area";
            this.btn_PositionArea.Click += new System.EventHandler(this.SettingButton_Config);
            // 
            // btn_JobConfig
            // 
            this.btn_JobConfig.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btn_JobConfig.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_JobConfig.ImageOptions.SvgImage")));
            this.btn_JobConfig.Location = new System.Drawing.Point(241, 13);
            this.btn_JobConfig.Name = "btn_JobConfig";
            this.btn_JobConfig.Size = new System.Drawing.Size(110, 75);
            this.btn_JobConfig.TabIndex = 149;
            this.btn_JobConfig.Text = "Job Config";
            this.btn_JobConfig.Click += new System.EventHandler(this.SettingButton_Config);
            // 
            // btn_WaitMission
            // 
            this.btn_WaitMission.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btn_WaitMission.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_WaitMission.ImageOptions.SvgImage")));
            this.btn_WaitMission.Location = new System.Drawing.Point(355, 13);
            this.btn_WaitMission.Name = "btn_WaitMission";
            this.btn_WaitMission.Size = new System.Drawing.Size(110, 75);
            this.btn_WaitMission.TabIndex = 150;
            this.btn_WaitMission.Text = "Wait Mission";
            this.btn_WaitMission.Click += new System.EventHandler(this.SettingButton_Config);
            // 
            // btn_ChargeMission
            // 
            this.btn_ChargeMission.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btn_ChargeMission.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_ChargeMission.ImageOptions.SvgImage")));
            this.btn_ChargeMission.Location = new System.Drawing.Point(469, 13);
            this.btn_ChargeMission.Name = "btn_ChargeMission";
            this.btn_ChargeMission.Size = new System.Drawing.Size(110, 75);
            this.btn_ChargeMission.TabIndex = 151;
            this.btn_ChargeMission.Text = "Charge Mission";
            this.btn_ChargeMission.Click += new System.EventHandler(this.SettingButton_Config);
            // 
            // btn_RegistarSync
            // 
            this.btn_RegistarSync.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btn_RegistarSync.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_RegistarSync.ImageOptions.SvgImage")));
            this.btn_RegistarSync.Location = new System.Drawing.Point(583, 13);
            this.btn_RegistarSync.Name = "btn_RegistarSync";
            this.btn_RegistarSync.Size = new System.Drawing.Size(152, 75);
            this.btn_RegistarSync.TabIndex = 152;
            this.btn_RegistarSync.Text = "Robot Position Register";
            this.btn_RegistarSync.Click += new System.EventHandler(this.SettingButton_Config);
            // 
            // btn_Elevator
            // 
            this.btn_Elevator.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btn_Elevator.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Elevator.ImageOptions.SvgImage")));
            this.btn_Elevator.Location = new System.Drawing.Point(739, 13);
            this.btn_Elevator.Name = "btn_Elevator";
            this.btn_Elevator.Size = new System.Drawing.Size(110, 75);
            this.btn_Elevator.TabIndex = 153;
            this.btn_Elevator.Text = "Elevator";
            this.btn_Elevator.Click += new System.EventHandler(this.SettingButton_Config);
            // 
            // btn_UserData
            // 
            this.btn_UserData.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btn_UserData.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_UserData.ImageOptions.SvgImage")));
            this.btn_UserData.Location = new System.Drawing.Point(853, 13);
            this.btn_UserData.Name = "btn_UserData";
            this.btn_UserData.Size = new System.Drawing.Size(110, 75);
            this.btn_UserData.TabIndex = 154;
            this.btn_UserData.Text = "User Data";
            this.btn_UserData.Click += new System.EventHandler(this.SettingButton_Config);
            // 
            // tabSetting
            // 
            this.tabSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabSetting.Location = new System.Drawing.Point(25, 66);
            this.tabSetting.Name = "tabSetting";
            this.tabSetting.SelectedTabPage = this.tab_JobConfig;
            this.tabSetting.Size = new System.Drawing.Size(1624, 878);
            this.tabSetting.TabIndex = 261;
            this.tabSetting.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tab_JobConfig,
            this.tab_PositionArea,
            this.tab_WaitMission,
            this.tab_RobotControl,
            this.tab_AutoJobConfig,
            this.tab_RegisterSync,
            this.tab_ChargeMission,
            this.tab_ErrorCodeList,
            this.tab_Elevator,
            this.tab_UserData});
            // 
            // tab_JobConfig
            // 
            this.tab_JobConfig.Name = "tab_JobConfig";
            this.tab_JobConfig.Size = new System.Drawing.Size(1622, 852);
            this.tab_JobConfig.Text = "Wait Mission";
            // 
            // tab_PositionArea
            // 
            this.tab_PositionArea.Name = "tab_PositionArea";
            this.tab_PositionArea.Size = new System.Drawing.Size(1622, 852);
            this.tab_PositionArea.Text = "xtraTabPage3";
            // 
            // tab_WaitMission
            // 
            this.tab_WaitMission.Name = "tab_WaitMission";
            this.tab_WaitMission.Size = new System.Drawing.Size(1622, 852);
            this.tab_WaitMission.Text = "xtraTabPage3";
            // 
            // tab_RobotControl
            // 
            this.tab_RobotControl.Name = "tab_RobotControl";
            this.tab_RobotControl.Size = new System.Drawing.Size(1622, 852);
            this.tab_RobotControl.Text = "xtraTabPage3";
            // 
            // tab_AutoJobConfig
            // 
            this.tab_AutoJobConfig.Name = "tab_AutoJobConfig";
            this.tab_AutoJobConfig.Size = new System.Drawing.Size(1622, 852);
            this.tab_AutoJobConfig.Text = "xtraTabPage3";
            // 
            // tab_RegisterSync
            // 
            this.tab_RegisterSync.Name = "tab_RegisterSync";
            this.tab_RegisterSync.Size = new System.Drawing.Size(1622, 852);
            this.tab_RegisterSync.Text = "xtraTabPage3";
            // 
            // tab_ChargeMission
            // 
            this.tab_ChargeMission.Name = "tab_ChargeMission";
            this.tab_ChargeMission.Size = new System.Drawing.Size(1622, 852);
            this.tab_ChargeMission.Text = "xtraTabPage3";
            // 
            // tab_ErrorCodeList
            // 
            this.tab_ErrorCodeList.Name = "tab_ErrorCodeList";
            this.tab_ErrorCodeList.Size = new System.Drawing.Size(1622, 852);
            this.tab_ErrorCodeList.Text = "xtraTabPage3";
            // 
            // tab_Elevator
            // 
            this.tab_Elevator.Name = "tab_Elevator";
            this.tab_Elevator.Size = new System.Drawing.Size(1622, 852);
            this.tab_Elevator.Text = "xtraTabPage3";
            // 
            // tab_UserData
            // 
            this.tab_UserData.Name = "tab_UserData";
            this.tab_UserData.Size = new System.Drawing.Size(1622, 852);
            this.tab_UserData.Text = "xtraTabPage3";
            // 
            // SystemScreen
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(1680, 956);
            this.Controls.Add(this.stackPanel1);
            this.Controls.Add(this.tabSetting);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SystemScreen";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.stackPanel1)).EndInit();
            this.stackPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabSetting)).EndInit();
            this.tabSetting.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer Auto_dtg_Display_Timer;
        private DevExpress.Utils.Layout.StackPanel stackPanel1;
        private DevExpress.XtraTab.XtraTabControl tabSetting;
        private DevExpress.XtraTab.XtraTabPage tab_JobConfig;
        private DevExpress.XtraTab.XtraTabPage tab_PositionArea;
        private DevExpress.XtraTab.XtraTabPage tab_WaitMission;
        private DevExpress.XtraTab.XtraTabPage tab_RobotControl;
        private DevExpress.XtraTab.XtraTabPage tab_AutoJobConfig;
        private DevExpress.XtraTab.XtraTabPage tab_RegisterSync;
        private DevExpress.XtraTab.XtraTabPage tab_ChargeMission;
        private DevExpress.XtraTab.XtraTabPage tab_ErrorCodeList;
        private DevExpress.XtraTab.XtraTabPage tab_Elevator;
        private DevExpress.XtraTab.XtraTabPage tab_UserData;
        private DevExpress.XtraEditors.SimpleButton btn_RobotControl;
        private DevExpress.XtraEditors.SimpleButton btn_PositionArea;
        private DevExpress.XtraEditors.SimpleButton btn_JobConfig;
        private DevExpress.XtraEditors.SimpleButton btn_WaitMission;
        private DevExpress.XtraEditors.SimpleButton btn_ChargeMission;
        private DevExpress.XtraEditors.SimpleButton btn_RegistarSync;
        private DevExpress.XtraEditors.SimpleButton btn_Elevator;
        private DevExpress.XtraEditors.SimpleButton btn_UserData;
    }
}