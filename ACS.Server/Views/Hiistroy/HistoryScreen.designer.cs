
namespace INA_ACS_Server
{
    partial class HistoryScreen
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_JobChart = new System.Windows.Forms.Button();
            this.btn_JobHistory = new System.Windows.Forms.Button();
            this.btn_ErrorChart = new System.Windows.Forms.Button();
            this.btn_ElevatorModeHistory = new System.Windows.Forms.Button();
            this.btn_ErrorHistory = new System.Windows.Forms.Button();
            this.tabSetting = new System.Windows.Forms.TabControl();
            this.tab_JobHistory = new System.Windows.Forms.TabPage();
            this.tab_ErrorHistory = new System.Windows.Forms.TabPage();
            this.tab_ElevatorModeHistory = new System.Windows.Forms.TabPage();
            this.tab_JobChart = new System.Windows.Forms.TabPage();
            this.tab_ErrorChart = new System.Windows.Forms.TabPage();
            this.panel1.SuspendLayout();
            this.tabSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btn_JobChart);
            this.panel1.Controls.Add(this.btn_JobHistory);
            this.panel1.Controls.Add(this.btn_ErrorChart);
            this.panel1.Controls.Add(this.btn_ElevatorModeHistory);
            this.panel1.Controls.Add(this.btn_ErrorHistory);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1676, 72);
            this.panel1.TabIndex = 260;
            // 
            // btn_JobChart
            // 
            this.btn_JobChart.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_JobChart.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_JobChart.Location = new System.Drawing.Point(264, 3);
            this.btn_JobChart.Name = "btn_JobChart";
            this.btn_JobChart.Size = new System.Drawing.Size(124, 64);
            this.btn_JobChart.TabIndex = 3;
            this.btn_JobChart.Text = "반송량 집계";
            this.btn_JobChart.UseVisualStyleBackColor = false;
            this.btn_JobChart.Click += new System.EventHandler(this.SettingButton_Config);
            // 
            // btn_JobHistory
            // 
            this.btn_JobHistory.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_JobHistory.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_JobHistory.Location = new System.Drawing.Point(4, 3);
            this.btn_JobHistory.Name = "btn_JobHistory";
            this.btn_JobHistory.Size = new System.Drawing.Size(124, 64);
            this.btn_JobHistory.TabIndex = 0;
            this.btn_JobHistory.Text = "Job\r\nHistory";
            this.btn_JobHistory.UseVisualStyleBackColor = false;
            this.btn_JobHistory.Click += new System.EventHandler(this.SettingButton_Config);
            // 
            // btn_ErrorChart
            // 
            this.btn_ErrorChart.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_ErrorChart.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ErrorChart.Location = new System.Drawing.Point(394, 3);
            this.btn_ErrorChart.Name = "btn_ErrorChart";
            this.btn_ErrorChart.Size = new System.Drawing.Size(124, 64);
            this.btn_ErrorChart.TabIndex = 4;
            this.btn_ErrorChart.Text = "에러 집계";
            this.btn_ErrorChart.UseVisualStyleBackColor = false;
            this.btn_ErrorChart.Click += new System.EventHandler(this.SettingButton_Config);
            // 
            // btn_ElevatorModeHistory
            // 
            this.btn_ElevatorModeHistory.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_ElevatorModeHistory.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ElevatorModeHistory.Location = new System.Drawing.Point(524, 3);
            this.btn_ElevatorModeHistory.Name = "btn_ElevatorModeHistory";
            this.btn_ElevatorModeHistory.Size = new System.Drawing.Size(124, 64);
            this.btn_ElevatorModeHistory.TabIndex = 2;
            this.btn_ElevatorModeHistory.Text = "ElevatorMode\r\nHistory";
            this.btn_ElevatorModeHistory.UseVisualStyleBackColor = false;
            this.btn_ElevatorModeHistory.Click += new System.EventHandler(this.SettingButton_Config);
            // 
            // btn_ErrorHistory
            // 
            this.btn_ErrorHistory.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_ErrorHistory.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ErrorHistory.Location = new System.Drawing.Point(134, 3);
            this.btn_ErrorHistory.Name = "btn_ErrorHistory";
            this.btn_ErrorHistory.Size = new System.Drawing.Size(124, 64);
            this.btn_ErrorHistory.TabIndex = 1;
            this.btn_ErrorHistory.Text = "Error\r\nHistory";
            this.btn_ErrorHistory.UseVisualStyleBackColor = false;
            this.btn_ErrorHistory.Click += new System.EventHandler(this.SettingButton_Config);
            // 
            // tabSetting
            // 
            this.tabSetting.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabSetting.Controls.Add(this.tab_JobHistory);
            this.tabSetting.Controls.Add(this.tab_ErrorHistory);
            this.tabSetting.Controls.Add(this.tab_ElevatorModeHistory);
            this.tabSetting.Controls.Add(this.tab_JobChart);
            this.tabSetting.Controls.Add(this.tab_ErrorChart);
            this.tabSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSetting.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabSetting.ItemSize = new System.Drawing.Size(150, 70);
            this.tabSetting.Location = new System.Drawing.Point(0, 0);
            this.tabSetting.Name = "tabSetting";
            this.tabSetting.SelectedIndex = 0;
            this.tabSetting.Size = new System.Drawing.Size(1680, 956);
            this.tabSetting.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabSetting.TabIndex = 261;
            // 
            // tab_JobHistory
            // 
            this.tab_JobHistory.Location = new System.Drawing.Point(4, 74);
            this.tab_JobHistory.Name = "tab_JobHistory";
            this.tab_JobHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tab_JobHistory.Size = new System.Drawing.Size(1672, 878);
            this.tab_JobHistory.TabIndex = 19;
            this.tab_JobHistory.Text = "JobHistory";
            this.tab_JobHistory.UseVisualStyleBackColor = true;
            // 
            // tab_ErrorHistory
            // 
            this.tab_ErrorHistory.Location = new System.Drawing.Point(4, 74);
            this.tab_ErrorHistory.Name = "tab_ErrorHistory";
            this.tab_ErrorHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tab_ErrorHistory.Size = new System.Drawing.Size(1672, 878);
            this.tab_ErrorHistory.TabIndex = 18;
            this.tab_ErrorHistory.Text = "ErrorHistory";
            this.tab_ErrorHistory.UseVisualStyleBackColor = true;
            // 
            // tab_ElevatorModeHistory
            // 
            this.tab_ElevatorModeHistory.Location = new System.Drawing.Point(4, 74);
            this.tab_ElevatorModeHistory.Name = "tab_ElevatorModeHistory";
            this.tab_ElevatorModeHistory.Size = new System.Drawing.Size(1672, 878);
            this.tab_ElevatorModeHistory.TabIndex = 21;
            this.tab_ElevatorModeHistory.Text = "ElevatorModeHistory";
            this.tab_ElevatorModeHistory.UseVisualStyleBackColor = true;
            // 
            // tab_JobChart
            // 
            this.tab_JobChart.Location = new System.Drawing.Point(4, 74);
            this.tab_JobChart.Name = "tab_JobChart";
            this.tab_JobChart.Size = new System.Drawing.Size(1672, 878);
            this.tab_JobChart.TabIndex = 22;
            this.tab_JobChart.Text = "반송량집계";
            this.tab_JobChart.UseVisualStyleBackColor = true;
            // 
            // tab_ErrorChart
            // 
            this.tab_ErrorChart.Location = new System.Drawing.Point(4, 74);
            this.tab_ErrorChart.Name = "tab_ErrorChart";
            this.tab_ErrorChart.Size = new System.Drawing.Size(1672, 878);
            this.tab_ErrorChart.TabIndex = 23;
            this.tab_ErrorChart.Text = "에러집계";
            this.tab_ErrorChart.UseVisualStyleBackColor = true;
            // 
            // HistoryScreen
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(1680, 956);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabSetting);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HistoryScreen";
            this.Text = "HistoryScreen";
            this.panel1.ResumeLayout(false);
            this.tabSetting.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_JobHistory;
        private System.Windows.Forms.Button btn_ElevatorModeHistory;
        private System.Windows.Forms.Button btn_ErrorHistory;
        private System.Windows.Forms.TabControl tabSetting;
        private System.Windows.Forms.TabPage tab_JobHistory;
        private System.Windows.Forms.TabPage tab_ErrorHistory;
        private System.Windows.Forms.TabPage tab_ElevatorModeHistory;
        private System.Windows.Forms.Button btn_JobChart;
        private System.Windows.Forms.Button btn_ErrorChart;
        private System.Windows.Forms.TabPage tab_JobChart;
        private System.Windows.Forms.TabPage tab_ErrorChart;
    }
}