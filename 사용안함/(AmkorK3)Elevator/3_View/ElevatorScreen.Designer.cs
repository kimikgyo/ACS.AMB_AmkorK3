
namespace WindowsFormsApp9
{
    partial class ElevatorScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ElevatorScreen));
            this.GC_connect = new DevExpress.XtraEditors.GroupControl();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.Btn_Add = new DevExpress.XtraEditors.SimpleButton();
            this.T_Port = new DevExpress.XtraEditors.TextEdit();
            this.L_Port = new DevExpress.XtraEditors.LabelControl();
            this.T_IP = new DevExpress.XtraEditors.TextEdit();
            this.L_IP = new DevExpress.XtraEditors.LabelControl();
            this.P_Body = new DevExpress.XtraEditors.PanelControl();
            this.TLP_1 = new System.Windows.Forms.TableLayoutPanel();
            this.GC_Controls = new DevExpress.XtraEditors.GroupControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.P_Floor = new DevExpress.XtraEditors.PanelControl();
            this.L_Floor = new DevExpress.XtraEditors.LabelControl();
            this.CB_Floor = new DevExpress.XtraEditors.ComboBoxEdit();
            this.htmlTemplate1 = new DevExpress.Utils.Html.HtmlTemplate();
            this.Pic_Open = new DevExpress.XtraEditors.PictureEdit();
            this.Pic_Close = new DevExpress.XtraEditors.PictureEdit();
            this.GC_Status = new DevExpress.XtraEditors.GroupControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.L_FloorStatus = new DevExpress.XtraEditors.LabelControl();
            this.L_ElevatorStatus = new DevExpress.XtraEditors.LabelControl();
            this.Lib_IPList = new DevExpress.XtraEditors.ListBoxControl();
            this.htmlTemplate2 = new DevExpress.Utils.Html.HtmlTemplate();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.GC_AGVMode = new DevExpress.XtraEditors.GroupControl();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.GC_connect)).BeginInit();
            this.GC_connect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.T_Port.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.T_IP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P_Body)).BeginInit();
            this.P_Body.SuspendLayout();
            this.TLP_1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GC_Controls)).BeginInit();
            this.GC_Controls.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.P_Floor)).BeginInit();
            this.P_Floor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CB_Floor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Open.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Close.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GC_Status)).BeginInit();
            this.GC_Status.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Lib_IPList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GC_AGVMode)).BeginInit();
            this.GC_AGVMode.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // GC_connect
            // 
            this.GC_connect.Controls.Add(this.textEdit1);
            this.GC_connect.Controls.Add(this.labelControl1);
            this.GC_connect.Controls.Add(this.Btn_Add);
            this.GC_connect.Controls.Add(this.T_Port);
            this.GC_connect.Controls.Add(this.L_Port);
            this.GC_connect.Controls.Add(this.T_IP);
            this.GC_connect.Controls.Add(this.L_IP);
            this.GC_connect.Dock = System.Windows.Forms.DockStyle.Left;
            this.GC_connect.Location = new System.Drawing.Point(2, 2);
            this.GC_connect.Name = "GC_connect";
            this.GC_connect.Size = new System.Drawing.Size(487, 61);
            this.GC_connect.TabIndex = 0;
            this.GC_connect.Text = "Elevator Connect Setting";
            // 
            // textEdit1
            // 
            this.textEdit1.EditValue = "Amkor K3";
            this.textEdit1.Location = new System.Drawing.Point(267, 26);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(72, 20);
            this.textEdit1.TabIndex = 6;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(226, 29);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(35, 14);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "Alias : ";
            // 
            // Btn_Add
            // 
            this.Btn_Add.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("Btn_Add.ImageOptions.Image")));
            this.Btn_Add.Location = new System.Drawing.Point(345, 25);
            this.Btn_Add.Name = "Btn_Add";
            this.Btn_Add.Size = new System.Drawing.Size(65, 23);
            this.Btn_Add.TabIndex = 4;
            this.Btn_Add.Text = "추가";
            // 
            // T_Port
            // 
            this.T_Port.EditValue = "2004";
            this.T_Port.Location = new System.Drawing.Point(175, 26);
            this.T_Port.Name = "T_Port";
            this.T_Port.Size = new System.Drawing.Size(45, 20);
            this.T_Port.TabIndex = 3;
            // 
            // L_Port
            // 
            this.L_Port.Location = new System.Drawing.Point(134, 29);
            this.L_Port.Name = "L_Port";
            this.L_Port.Size = new System.Drawing.Size(35, 14);
            this.L_Port.TabIndex = 2;
            this.L_Port.Text = "Port : ";
            // 
            // T_IP
            // 
            this.T_IP.EditValue = "127.0.0.1";
            this.T_IP.Location = new System.Drawing.Point(41, 26);
            this.T_IP.Name = "T_IP";
            this.T_IP.Size = new System.Drawing.Size(87, 20);
            this.T_IP.TabIndex = 1;
            // 
            // L_IP
            // 
            this.L_IP.Location = new System.Drawing.Point(12, 29);
            this.L_IP.Name = "L_IP";
            this.L_IP.Size = new System.Drawing.Size(23, 14);
            this.L_IP.TabIndex = 0;
            this.L_IP.Text = "IP : ";
            // 
            // P_Body
            // 
            this.P_Body.Controls.Add(this.TLP_1);
            this.P_Body.Controls.Add(this.Lib_IPList);
            this.P_Body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_Body.Location = new System.Drawing.Point(0, 65);
            this.P_Body.Name = "P_Body";
            this.P_Body.Size = new System.Drawing.Size(861, 449);
            this.P_Body.TabIndex = 1;
            // 
            // TLP_1
            // 
            this.TLP_1.ColumnCount = 1;
            this.TLP_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP_1.Controls.Add(this.GC_Controls, 0, 1);
            this.TLP_1.Controls.Add(this.GC_Status, 0, 0);
            this.TLP_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLP_1.Location = new System.Drawing.Point(122, 2);
            this.TLP_1.Name = "TLP_1";
            this.TLP_1.RowCount = 2;
            this.TLP_1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP_1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP_1.Size = new System.Drawing.Size(737, 445);
            this.TLP_1.TabIndex = 1;
            // 
            // GC_Controls
            // 
            this.GC_Controls.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.GC_Controls.Controls.Add(this.tableLayoutPanel1);
            this.GC_Controls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GC_Controls.Location = new System.Drawing.Point(3, 225);
            this.GC_Controls.Name = "GC_Controls";
            this.GC_Controls.Size = new System.Drawing.Size(731, 217);
            this.GC_Controls.TabIndex = 2;
            this.GC_Controls.Text = "Elevator Controls";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tableLayoutPanel1.Controls.Add(this.P_Floor, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.Pic_Open, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Pic_Close, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 23);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(727, 192);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // P_Floor
            // 
            this.P_Floor.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.P_Floor.Controls.Add(this.L_Floor);
            this.P_Floor.Controls.Add(this.CB_Floor);
            this.P_Floor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_Floor.Location = new System.Drawing.Point(487, 3);
            this.P_Floor.Name = "P_Floor";
            this.P_Floor.Size = new System.Drawing.Size(237, 186);
            this.P_Floor.TabIndex = 0;
            // 
            // L_Floor
            // 
            this.L_Floor.Location = new System.Drawing.Point(3, 3);
            this.L_Floor.Name = "L_Floor";
            this.L_Floor.Size = new System.Drawing.Size(98, 14);
            this.L_Floor.TabIndex = 3;
            this.L_Floor.Text = "층수를 선택해주세요.";
            // 
            // CB_Floor
            // 
            this.CB_Floor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_Floor.Location = new System.Drawing.Point(5, 65);
            this.CB_Floor.Name = "CB_Floor";
            this.CB_Floor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CB_Floor.Properties.HtmlTemplates.AddRange(new DevExpress.Utils.Html.HtmlTemplate[] {
            this.htmlTemplate1});
            this.CB_Floor.Size = new System.Drawing.Size(226, 20);
            this.CB_Floor.TabIndex = 2;
            // 
            // htmlTemplate1
            // 
            this.htmlTemplate1.Name = "htmlTemplate1";
            this.htmlTemplate1.Styles = resources.GetString("htmlTemplate1.Styles");
            this.htmlTemplate1.Template = resources.GetString("htmlTemplate1.Template");
            // 
            // Pic_Open
            // 
            this.Pic_Open.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pic_Open.Location = new System.Drawing.Point(3, 3);
            this.Pic_Open.Name = "Pic_Open";
            this.Pic_Open.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.Pic_Open.Size = new System.Drawing.Size(236, 186);
            this.Pic_Open.TabIndex = 0;
            // 
            // Pic_Close
            // 
            this.Pic_Close.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pic_Close.Location = new System.Drawing.Point(245, 3);
            this.Pic_Close.Name = "Pic_Close";
            this.Pic_Close.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.Pic_Close.Size = new System.Drawing.Size(236, 186);
            this.Pic_Close.TabIndex = 1;
            // 
            // GC_Status
            // 
            this.GC_Status.Controls.Add(this.tableLayoutPanel2);
            this.GC_Status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GC_Status.Location = new System.Drawing.Point(3, 3);
            this.GC_Status.Name = "GC_Status";
            this.GC_Status.Size = new System.Drawing.Size(731, 216);
            this.GC_Status.TabIndex = 1;
            this.GC_Status.Text = "Elevator Status";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.L_FloorStatus, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.L_ElevatorStatus, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 26);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 218F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(724, 185);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // L_FloorStatus
            // 
            this.L_FloorStatus.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.L_FloorStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.L_FloorStatus.Location = new System.Drawing.Point(365, 3);
            this.L_FloorStatus.Name = "L_FloorStatus";
            this.L_FloorStatus.Size = new System.Drawing.Size(356, 179);
            this.L_FloorStatus.TabIndex = 1;
            this.L_FloorStatus.Text = "B";
            // 
            // L_ElevatorStatus
            // 
            this.L_ElevatorStatus.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.L_ElevatorStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.L_ElevatorStatus.Location = new System.Drawing.Point(3, 3);
            this.L_ElevatorStatus.Name = "L_ElevatorStatus";
            this.L_ElevatorStatus.Size = new System.Drawing.Size(356, 179);
            this.L_ElevatorStatus.TabIndex = 0;
            this.L_ElevatorStatus.Text = "A";
            // 
            // Lib_IPList
            // 
            this.Lib_IPList.Cursor = System.Windows.Forms.Cursors.Default;
            this.Lib_IPList.Dock = System.Windows.Forms.DockStyle.Left;
            this.Lib_IPList.HtmlTemplates.AddRange(new DevExpress.Utils.Html.HtmlTemplate[] {
            this.htmlTemplate2});
            this.Lib_IPList.Location = new System.Drawing.Point(2, 2);
            this.Lib_IPList.Name = "Lib_IPList";
            this.Lib_IPList.Size = new System.Drawing.Size(120, 445);
            this.Lib_IPList.TabIndex = 0;
            // 
            // htmlTemplate2
            // 
            this.htmlTemplate2.Name = "htmlTemplate2";
            this.htmlTemplate2.Styles = resources.GetString("htmlTemplate2.Styles");
            this.htmlTemplate2.Template = resources.GetString("htmlTemplate2.Template");
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.GC_AGVMode);
            this.panelControl1.Controls.Add(this.GC_connect);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(861, 65);
            this.panelControl1.TabIndex = 2;
            // 
            // GC_AGVMode
            // 
            this.GC_AGVMode.Controls.Add(this.tableLayoutPanel3);
            this.GC_AGVMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GC_AGVMode.Location = new System.Drawing.Point(489, 2);
            this.GC_AGVMode.Name = "GC_AGVMode";
            this.GC_AGVMode.Size = new System.Drawing.Size(370, 61);
            this.GC_AGVMode.TabIndex = 1;
            this.GC_AGVMode.Text = "AGVMode";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.simpleButton2, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.simpleButton1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(2, 23);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(366, 36);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simpleButton1.Location = new System.Drawing.Point(3, 3);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.simpleButton1.Size = new System.Drawing.Size(177, 30);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "AGV Mode ON";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simpleButton2.Location = new System.Drawing.Point(186, 3);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.simpleButton2.Size = new System.Drawing.Size(177, 30);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "AGV Mode OFF";
            // 
            // ElevatorScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 514);
            this.Controls.Add(this.P_Body);
            this.Controls.Add(this.panelControl1);
            this.Name = "ElevatorScreen";
            this.Text = "ElevatorScreen";
            ((System.ComponentModel.ISupportInitialize)(this.GC_connect)).EndInit();
            this.GC_connect.ResumeLayout(false);
            this.GC_connect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.T_Port.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.T_IP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P_Body)).EndInit();
            this.P_Body.ResumeLayout(false);
            this.TLP_1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GC_Controls)).EndInit();
            this.GC_Controls.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.P_Floor)).EndInit();
            this.P_Floor.ResumeLayout(false);
            this.P_Floor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CB_Floor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Open.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Close.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GC_Status)).EndInit();
            this.GC_Status.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Lib_IPList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GC_AGVMode)).EndInit();
            this.GC_AGVMode.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl GC_connect;
        private DevExpress.XtraEditors.LabelControl L_IP;
        private DevExpress.XtraEditors.TextEdit T_IP;
        private DevExpress.XtraEditors.TextEdit T_Port;
        private DevExpress.XtraEditors.LabelControl L_Port;
        private DevExpress.XtraEditors.SimpleButton Btn_Add;
        private DevExpress.XtraEditors.PanelControl P_Body;
        private DevExpress.XtraEditors.ListBoxControl Lib_IPList;
        private DevExpress.XtraEditors.GroupControl GC_Status;
        private DevExpress.XtraEditors.GroupControl GC_Controls;
        private DevExpress.XtraEditors.PictureEdit Pic_Open;
        private DevExpress.XtraEditors.PictureEdit Pic_Close;
        private System.Windows.Forms.TableLayoutPanel TLP_1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.ComboBoxEdit CB_Floor;
        private DevExpress.XtraEditors.PanelControl P_Floor;
        private DevExpress.XtraEditors.LabelControl L_Floor;
        private DevExpress.Utils.Html.HtmlTemplate htmlTemplate1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.LabelControl L_ElevatorStatus;
        private DevExpress.XtraEditors.LabelControl L_FloorStatus;
        private DevExpress.Utils.Html.HtmlTemplate htmlTemplate2;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl GC_AGVMode;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}