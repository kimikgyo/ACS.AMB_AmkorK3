
namespace INA_ACS_Server.Views
{
    partial class WiseModuleConfigScreen
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt_WiseModuleResponseTime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_ManualControl = new System.Windows.Forms.Button();
            this.btn_AutoControl = new System.Windows.Forms.Button();
            this.dtg_WiseModuleConfig = new System.Windows.Forms.DataGridView();
            this.DGV_EtnLampSettnig_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_WiseModule_MaxNum = new System.Windows.Forms.TextBox();
            this.btn_WiseModuleConfigBackUp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dtg_WiseModuleMonitoring = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.DisPlayTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_WiseModuleConfig)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_WiseModuleMonitoring)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txt_WiseModuleResponseTime);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btn_ManualControl);
            this.groupBox1.Controls.Add(this.btn_AutoControl);
            this.groupBox1.Controls.Add(this.dtg_WiseModuleConfig);
            this.groupBox1.Controls.Add(this.txt_WiseModule_MaxNum);
            this.groupBox1.Controls.Add(this.btn_WiseModuleConfigBackUp);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(706, 564);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Wise Module Config";
            // 
            // txt_WiseModuleResponseTime
            // 
            this.txt_WiseModuleResponseTime.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_WiseModuleResponseTime.Location = new System.Drawing.Point(205, 41);
            this.txt_WiseModuleResponseTime.Name = "txt_WiseModuleResponseTime";
            this.txt_WiseModuleResponseTime.Size = new System.Drawing.Size(118, 26);
            this.txt_WiseModuleResponseTime.TabIndex = 15;
            this.txt_WiseModuleResponseTime.Text = "0";
            this.txt_WiseModuleResponseTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_WiseModuleResponseTime.Click += new System.EventHandler(this.txt_WiseModuleConfig_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(210, 25);
            this.label2.TabIndex = 14;
            this.label2.Text = "Wise Module ResponseTime";
            // 
            // btn_ManualControl
            // 
            this.btn_ManualControl.Location = new System.Drawing.Point(583, 14);
            this.btn_ManualControl.Name = "btn_ManualControl";
            this.btn_ManualControl.Size = new System.Drawing.Size(115, 49);
            this.btn_ManualControl.TabIndex = 12;
            this.btn_ManualControl.Text = "Manual\r\nControl";
            this.btn_ManualControl.UseVisualStyleBackColor = true;
            this.btn_ManualControl.Click += new System.EventHandler(this.btn_ScreenButton_Click);
            // 
            // btn_AutoControl
            // 
            this.btn_AutoControl.Location = new System.Drawing.Point(462, 13);
            this.btn_AutoControl.Name = "btn_AutoControl";
            this.btn_AutoControl.Size = new System.Drawing.Size(115, 49);
            this.btn_AutoControl.TabIndex = 11;
            this.btn_AutoControl.Text = "Auto\r\nControl";
            this.btn_AutoControl.UseVisualStyleBackColor = true;
            this.btn_AutoControl.Click += new System.EventHandler(this.btn_ScreenButton_Click);
            // 
            // dtg_WiseModuleConfig
            // 
            this.dtg_WiseModuleConfig.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dtg_WiseModuleConfig.AllowUserToAddRows = false;
            this.dtg_WiseModuleConfig.AllowUserToDeleteRows = false;
            this.dtg_WiseModuleConfig.AllowUserToResizeColumns = false;
            this.dtg_WiseModuleConfig.AllowUserToResizeRows = false;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtg_WiseModuleConfig.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle21;
            this.dtg_WiseModuleConfig.BackgroundColor = System.Drawing.Color.White;
            this.dtg_WiseModuleConfig.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle22.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle22.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle22.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_WiseModuleConfig.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle22;
            this.dtg_WiseModuleConfig.ColumnHeadersHeight = 50;
            this.dtg_WiseModuleConfig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtg_WiseModuleConfig.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DGV_EtnLampSettnig_Id});
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle24.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle24.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle24.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle24.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle24.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_WiseModuleConfig.DefaultCellStyle = dataGridViewCellStyle24;
            this.dtg_WiseModuleConfig.EnableHeadersVisualStyles = false;
            this.dtg_WiseModuleConfig.GridColor = System.Drawing.Color.LightGray;
            this.dtg_WiseModuleConfig.Location = new System.Drawing.Point(6, 70);
            this.dtg_WiseModuleConfig.MultiSelect = false;
            this.dtg_WiseModuleConfig.Name = "dtg_WiseModuleConfig";
            this.dtg_WiseModuleConfig.ReadOnly = true;
            this.dtg_WiseModuleConfig.RowHeadersVisible = false;
            this.dtg_WiseModuleConfig.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle25.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtg_WiseModuleConfig.RowsDefaultCellStyle = dataGridViewCellStyle25;
            this.dtg_WiseModuleConfig.RowTemplate.Height = 23;
            this.dtg_WiseModuleConfig.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtg_WiseModuleConfig.Size = new System.Drawing.Size(692, 484);
            this.dtg_WiseModuleConfig.TabIndex = 10;
            this.dtg_WiseModuleConfig.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtg_WiseModule_CellEndEdit);
            this.dtg_WiseModuleConfig.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dtg_WiseModule_CellMouseClick);
            // 
            // DGV_EtnLampSettnig_Id
            // 
            dataGridViewCellStyle23.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle23.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle23.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle23.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle23.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_EtnLampSettnig_Id.DefaultCellStyle = dataGridViewCellStyle23;
            this.DGV_EtnLampSettnig_Id.HeaderText = "EtnLampSettnig_Id";
            this.DGV_EtnLampSettnig_Id.Name = "DGV_EtnLampSettnig_Id";
            this.DGV_EtnLampSettnig_Id.ReadOnly = true;
            this.DGV_EtnLampSettnig_Id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // txt_WiseModule_MaxNum
            // 
            this.txt_WiseModule_MaxNum.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_WiseModule_MaxNum.Location = new System.Drawing.Point(205, 13);
            this.txt_WiseModule_MaxNum.Name = "txt_WiseModule_MaxNum";
            this.txt_WiseModule_MaxNum.Size = new System.Drawing.Size(118, 26);
            this.txt_WiseModule_MaxNum.TabIndex = 5;
            this.txt_WiseModule_MaxNum.Text = "0";
            this.txt_WiseModule_MaxNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_WiseModule_MaxNum.Click += new System.EventHandler(this.txt_WiseModuleConfig_Click);
            // 
            // btn_WiseModuleConfigBackUp
            // 
            this.btn_WiseModuleConfigBackUp.Location = new System.Drawing.Point(331, 14);
            this.btn_WiseModuleConfigBackUp.Name = "btn_WiseModuleConfigBackUp";
            this.btn_WiseModuleConfigBackUp.Size = new System.Drawing.Size(125, 48);
            this.btn_WiseModuleConfigBackUp.TabIndex = 6;
            this.btn_WiseModuleConfigBackUp.Text = "Back-Up\r\nSave File";
            this.btn_WiseModuleConfigBackUp.UseVisualStyleBackColor = true;
            this.btn_WiseModuleConfigBackUp.Click += new System.EventHandler(this.btn_ScreenButton_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Wise Module MaxNum";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dtg_WiseModuleMonitoring);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(724, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(459, 564);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Wise Module Monitoring";
            // 
            // dtg_WiseModuleMonitoring
            // 
            this.dtg_WiseModuleMonitoring.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dtg_WiseModuleMonitoring.AllowUserToAddRows = false;
            this.dtg_WiseModuleMonitoring.AllowUserToDeleteRows = false;
            this.dtg_WiseModuleMonitoring.AllowUserToResizeColumns = false;
            this.dtg_WiseModuleMonitoring.AllowUserToResizeRows = false;
            dataGridViewCellStyle26.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtg_WiseModuleMonitoring.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle26;
            this.dtg_WiseModuleMonitoring.BackgroundColor = System.Drawing.Color.White;
            this.dtg_WiseModuleMonitoring.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle27.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle27.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle27.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle27.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle27.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle27.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_WiseModuleMonitoring.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle27;
            this.dtg_WiseModuleMonitoring.ColumnHeadersHeight = 50;
            this.dtg_WiseModuleMonitoring.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtg_WiseModuleMonitoring.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle29.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle29.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle29.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle29.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle29.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle29.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_WiseModuleMonitoring.DefaultCellStyle = dataGridViewCellStyle29;
            this.dtg_WiseModuleMonitoring.EnableHeadersVisualStyles = false;
            this.dtg_WiseModuleMonitoring.GridColor = System.Drawing.Color.LightGray;
            this.dtg_WiseModuleMonitoring.Location = new System.Drawing.Point(6, 25);
            this.dtg_WiseModuleMonitoring.MultiSelect = false;
            this.dtg_WiseModuleMonitoring.Name = "dtg_WiseModuleMonitoring";
            this.dtg_WiseModuleMonitoring.ReadOnly = true;
            this.dtg_WiseModuleMonitoring.RowHeadersVisible = false;
            this.dtg_WiseModuleMonitoring.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle30.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle30.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtg_WiseModuleMonitoring.RowsDefaultCellStyle = dataGridViewCellStyle30;
            this.dtg_WiseModuleMonitoring.RowTemplate.Height = 23;
            this.dtg_WiseModuleMonitoring.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtg_WiseModuleMonitoring.Size = new System.Drawing.Size(447, 529);
            this.dtg_WiseModuleMonitoring.TabIndex = 13;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle28.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle28.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle28.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle28.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle28.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle28;
            this.dataGridViewTextBoxColumn1.HeaderText = "Index";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(805, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 41);
            this.button1.TabIndex = 13;
            this.button1.Text = "Display";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // DisPlayTimer
            // 
            this.DisPlayTimer.Enabled = true;
            this.DisPlayTimer.Tick += new System.EventHandler(this.DisPlayTimer_Tick);
            // 
            // WiseModuleConfigScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 578);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "WiseModuleConfigScreen";
            this.Text = "WiseModuleConfigScreen";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_WiseModuleConfig)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtg_WiseModuleMonitoring)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txt_WiseModuleResponseTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_ManualControl;
        private System.Windows.Forms.Button btn_AutoControl;
        private System.Windows.Forms.DataGridView dtg_WiseModuleConfig;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_EtnLampSettnig_Id;
        private System.Windows.Forms.TextBox txt_WiseModule_MaxNum;
        private System.Windows.Forms.Button btn_WiseModuleConfigBackUp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dtg_WiseModuleMonitoring;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer DisPlayTimer;
    }
}