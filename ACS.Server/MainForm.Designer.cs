
namespace INA_ACS_Server
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.fluentDesignFormContainer1 = new DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.Ibl_LoginAlarm = new DevExpress.XtraEditors.LabelControl();
            this.lbl_LoginOut = new DevExpress.XtraEditors.LabelControl();
            this.lbl_Login = new DevExpress.XtraEditors.LabelControl();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.fluentFormDefaultManager1 = new DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager(this.components);
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.accordionControl1 = new DevExpress.XtraBars.Navigation.AccordionControl();
            this.accordionControlMainView = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlReport = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlErrorLog = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlJobLog = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlMapView = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlSetting = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.fluentDesignFormControl1 = new DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentFormDefaultManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentDesignFormControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // fluentDesignFormContainer1
            // 
            this.fluentDesignFormContainer1.Appearance.BackColor = System.Drawing.Color.White;
            this.fluentDesignFormContainer1.Appearance.Options.UseBackColor = true;
            this.fluentDesignFormContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fluentDesignFormContainer1.Location = new System.Drawing.Point(260, 69);
            this.fluentDesignFormContainer1.Name = "fluentDesignFormContainer1";
            this.fluentDesignFormContainer1.Size = new System.Drawing.Size(1079, 387);
            this.fluentDesignFormContainer1.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.Ibl_LoginAlarm);
            this.panelControl1.Controls.Add(this.lbl_LoginOut);
            this.panelControl1.Controls.Add(this.lbl_Login);
            this.panelControl1.Controls.Add(this.pictureEdit1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 27);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1339, 42);
            this.panelControl1.TabIndex = 3;
            // 
            // Ibl_LoginAlarm
            // 
            this.Ibl_LoginAlarm.Location = new System.Drawing.Point(117, 14);
            this.Ibl_LoginAlarm.Name = "Ibl_LoginAlarm";
            this.Ibl_LoginAlarm.Size = new System.Drawing.Size(0, 14);
            this.Ibl_LoginAlarm.TabIndex = 3;
            // 
            // lbl_LoginOut
            // 
            this.lbl_LoginOut.Appearance.ForeColor = System.Drawing.Color.Transparent;
            this.lbl_LoginOut.Appearance.Options.UseForeColor = true;
            this.lbl_LoginOut.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbl_LoginOut.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.lbl_LoginOut.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("lbl_LoginOut.ImageOptions.SvgImage")));
            this.lbl_LoginOut.Location = new System.Drawing.Point(1296, 3);
            this.lbl_LoginOut.Name = "lbl_LoginOut";
            this.lbl_LoginOut.Size = new System.Drawing.Size(38, 34);
            this.lbl_LoginOut.TabIndex = 2;
            // 
            // lbl_Login
            // 
            this.lbl_Login.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbl_Login.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.lbl_Login.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("lbl_Login.ImageOptions.SvgImage")));
            this.lbl_Login.Location = new System.Drawing.Point(1296, 5);
            this.lbl_Login.Name = "lbl_Login";
            this.lbl_Login.Size = new System.Drawing.Size(38, 34);
            this.lbl_Login.TabIndex = 1;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Location = new System.Drawing.Point(7, 3);
            this.pictureEdit1.MenuManager = this.fluentFormDefaultManager1;
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Size = new System.Drawing.Size(100, 36);
            this.pictureEdit1.TabIndex = 0;
            // 
            // fluentFormDefaultManager1
            // 
            this.fluentFormDefaultManager1.Form = this;
            this.fluentFormDefaultManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem1});
            this.fluentFormDefaultManager1.MaxItemId = 1;
            // 
            // barSubItem1
            // 
            this.barSubItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barSubItem1.Caption = "barSubItem1";
            this.barSubItem1.Id = 0;
            this.barSubItem1.Name = "barSubItem1";
            // 
            // accordionControl1
            // 
            this.accordionControl1.Appearance.AccordionControl.BackColor = System.Drawing.Color.White;
            this.accordionControl1.Appearance.AccordionControl.Options.UseBackColor = true;
            this.accordionControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.accordionControl1.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.accordionControlMainView,
            this.accordionControlReport,
            this.accordionControlMapView,
            this.accordionControlSetting});
            this.accordionControl1.Location = new System.Drawing.Point(0, 69);
            this.accordionControl1.Name = "accordionControl1";
            this.accordionControl1.ScrollBarMode = DevExpress.XtraBars.Navigation.ScrollBarMode.Touch;
            this.accordionControl1.Size = new System.Drawing.Size(260, 387);
            this.accordionControl1.TabIndex = 1;
            this.accordionControl1.ViewType = DevExpress.XtraBars.Navigation.AccordionControlViewType.HamburgerMenu;
            // 
            // accordionControlMainView
            // 
            this.accordionControlMainView.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("accordionControlMainView.ImageOptions.SvgImage")));
            this.accordionControlMainView.Name = "accordionControlMainView";
            this.accordionControlMainView.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlMainView.Text = "MainView";
            // 
            // accordionControlReport
            // 
            this.accordionControlReport.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.accordionControlErrorLog,
            this.accordionControlJobLog});
            this.accordionControlReport.Expanded = true;
            this.accordionControlReport.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("accordionControlReport.ImageOptions.SvgImage")));
            this.accordionControlReport.Name = "accordionControlReport";
            this.accordionControlReport.Text = "Report";
            // 
            // accordionControlErrorLog
            // 
            this.accordionControlErrorLog.Name = "accordionControlErrorLog";
            this.accordionControlErrorLog.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlErrorLog.Text = "ErrorLog";
            // 
            // accordionControlJobLog
            // 
            this.accordionControlJobLog.Name = "accordionControlJobLog";
            this.accordionControlJobLog.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlJobLog.Text = "JobLog";
            // 
            // accordionControlMapView
            // 
            this.accordionControlMapView.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("accordionControlMapView.ImageOptions.SvgImage")));
            this.accordionControlMapView.Name = "accordionControlMapView";
            this.accordionControlMapView.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlMapView.Text = "MapView";
            // 
            // accordionControlSetting
            // 
            this.accordionControlSetting.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("accordionControlSetting.ImageOptions.SvgImage")));
            this.accordionControlSetting.Name = "accordionControlSetting";
            this.accordionControlSetting.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlSetting.Text = "Setting";
            this.accordionControlSetting.Visible = false;
            // 
            // fluentDesignFormControl1
            // 
            this.fluentDesignFormControl1.FluentDesignForm = this;
            this.fluentDesignFormControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem1});
            this.fluentDesignFormControl1.Location = new System.Drawing.Point(0, 0);
            this.fluentDesignFormControl1.Manager = this.fluentFormDefaultManager1;
            this.fluentDesignFormControl1.Name = "fluentDesignFormControl1";
            this.fluentDesignFormControl1.Size = new System.Drawing.Size(1339, 27);
            this.fluentDesignFormControl1.TabIndex = 2;
            this.fluentDesignFormControl1.TabStop = false;
            // 
            // MainForm
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(35)))), ((int)(((byte)(45)))));
            this.Appearance.ForeColor = System.Drawing.Color.DarkRed;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1339, 456);
            this.ControlContainer = this.fluentDesignFormContainer1;
            this.Controls.Add(this.fluentDesignFormContainer1);
            this.Controls.Add(this.accordionControl1);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.fluentDesignFormControl1);
            this.FluentDesignFormControl = this.fluentDesignFormControl1;
            this.LookAndFeel.SkinMaskColor = System.Drawing.Color.White;
            this.LookAndFeel.SkinMaskColor2 = System.Drawing.Color.White;
            this.LookAndFeel.SkinName = "DevExpress Style";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "MainForm";
            this.NavigationControl = this.accordionControl1;
            this.TransparencyKey = System.Drawing.Color.WhiteSmoke;
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentFormDefaultManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentDesignFormControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer fluentDesignFormContainer1;
        private DevExpress.XtraBars.Navigation.AccordionControl accordionControl1;
        private DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl fluentDesignFormControl1;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlSetting;
        private DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager fluentFormDefaultManager1;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.LabelControl lbl_Login;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlMainView;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlReport;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlMapView;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlErrorLog;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlJobLog;
        private DevExpress.XtraEditors.LabelControl lbl_LoginOut;
        private DevExpress.XtraEditors.LabelControl Ibl_LoginAlarm;
    }
}