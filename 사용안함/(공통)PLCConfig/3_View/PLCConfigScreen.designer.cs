
namespace INA_ACS_Server
{
    partial class PLCConfigScreen
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_Display = new System.Windows.Forms.Button();
            this.dtg_PLCModule = new System.Windows.Forms.DataGridView();
            this.DGV_EtnLampSettnig_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_PLCModuleMaxNum = new System.Windows.Forms.TextBox();
            this.btn_PLCModuleBackUp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_ManualControl = new System.Windows.Forms.Button();
            this.btn_AutoControl = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_PLCModule)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_ManualControl);
            this.groupBox1.Controls.Add(this.btn_Display);
            this.groupBox1.Controls.Add(this.btn_AutoControl);
            this.groupBox1.Controls.Add(this.dtg_PLCModule);
            this.groupBox1.Controls.Add(this.txt_PLCModuleMaxNum);
            this.groupBox1.Controls.Add(this.btn_PLCModuleBackUp);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1171, 564);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PLC Module";
            // 
            // btn_Display
            // 
            this.btn_Display.Location = new System.Drawing.Point(805, 19);
            this.btn_Display.Name = "btn_Display";
            this.btn_Display.Size = new System.Drawing.Size(139, 41);
            this.btn_Display.TabIndex = 13;
            this.btn_Display.Text = "Display";
            this.btn_Display.UseVisualStyleBackColor = true;
            this.btn_Display.Click += new System.EventHandler(this.btn_ScreenButton_Click);
            // 
            // dtg_PLCModule
            // 
            this.dtg_PLCModule.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dtg_PLCModule.AllowUserToAddRows = false;
            this.dtg_PLCModule.AllowUserToDeleteRows = false;
            this.dtg_PLCModule.AllowUserToResizeColumns = false;
            this.dtg_PLCModule.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtg_PLCModule.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dtg_PLCModule.BackgroundColor = System.Drawing.Color.White;
            this.dtg_PLCModule.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_PLCModule.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dtg_PLCModule.ColumnHeadersHeight = 50;
            this.dtg_PLCModule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtg_PLCModule.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DGV_EtnLampSettnig_Id});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_PLCModule.DefaultCellStyle = dataGridViewCellStyle9;
            this.dtg_PLCModule.EnableHeadersVisualStyles = false;
            this.dtg_PLCModule.GridColor = System.Drawing.Color.LightGray;
            this.dtg_PLCModule.Location = new System.Drawing.Point(6, 70);
            this.dtg_PLCModule.MultiSelect = false;
            this.dtg_PLCModule.Name = "dtg_PLCModule";
            this.dtg_PLCModule.ReadOnly = true;
            this.dtg_PLCModule.RowHeadersVisible = false;
            this.dtg_PLCModule.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtg_PLCModule.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dtg_PLCModule.RowTemplate.Height = 23;
            this.dtg_PLCModule.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtg_PLCModule.Size = new System.Drawing.Size(1122, 484);
            this.dtg_PLCModule.TabIndex = 10;
            this.dtg_PLCModule.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtg_PLCModule_CellEndEdit);
            this.dtg_PLCModule.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dtg_PLCModule_CellMouseClick);
            // 
            // DGV_EtnLampSettnig_Id
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_EtnLampSettnig_Id.DefaultCellStyle = dataGridViewCellStyle8;
            this.DGV_EtnLampSettnig_Id.HeaderText = "EtnLampSettnig_Id";
            this.DGV_EtnLampSettnig_Id.Name = "DGV_EtnLampSettnig_Id";
            this.DGV_EtnLampSettnig_Id.ReadOnly = true;
            this.DGV_EtnLampSettnig_Id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // txt_PLCModuleMaxNum
            // 
            this.txt_PLCModuleMaxNum.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_PLCModuleMaxNum.Location = new System.Drawing.Point(205, 29);
            this.txt_PLCModuleMaxNum.Name = "txt_PLCModuleMaxNum";
            this.txt_PLCModuleMaxNum.Size = new System.Drawing.Size(118, 26);
            this.txt_PLCModuleMaxNum.TabIndex = 5;
            this.txt_PLCModuleMaxNum.Text = "0";
            this.txt_PLCModuleMaxNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_PLCModuleMaxNum.Click += new System.EventHandler(this.txt_txt_PLCModuleMaxNum_Click);
            // 
            // btn_PLCModuleBackUp
            // 
            this.btn_PLCModuleBackUp.Location = new System.Drawing.Point(347, 21);
            this.btn_PLCModuleBackUp.Name = "btn_PLCModuleBackUp";
            this.btn_PLCModuleBackUp.Size = new System.Drawing.Size(156, 41);
            this.btn_PLCModuleBackUp.TabIndex = 6;
            this.btn_PLCModuleBackUp.Text = "Back-Up Save File";
            this.btn_PLCModuleBackUp.UseVisualStyleBackColor = true;
            this.btn_PLCModuleBackUp.Click += new System.EventHandler(this.btn_ScreenButton_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "PLC Module MaxNum";
            // 
            // btn_ManualControl
            // 
            this.btn_ManualControl.Location = new System.Drawing.Point(655, 19);
            this.btn_ManualControl.Name = "btn_ManualControl";
            this.btn_ManualControl.Size = new System.Drawing.Size(144, 41);
            this.btn_ManualControl.TabIndex = 14;
            this.btn_ManualControl.Text = "ManualControl";
            this.btn_ManualControl.UseVisualStyleBackColor = true;
            this.btn_ManualControl.Click += new System.EventHandler(this.btn_ScreenButton_Click);
            // 
            // btn_AutoControl
            // 
            this.btn_AutoControl.Location = new System.Drawing.Point(509, 21);
            this.btn_AutoControl.Name = "btn_AutoControl";
            this.btn_AutoControl.Size = new System.Drawing.Size(140, 41);
            this.btn_AutoControl.TabIndex = 13;
            this.btn_AutoControl.Text = "AutoControl";
            this.btn_AutoControl.UseVisualStyleBackColor = true;
            this.btn_AutoControl.Click += new System.EventHandler(this.btn_ScreenButton_Click);
            // 
            // PLCConfigScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 578);
            this.Controls.Add(this.groupBox1);
            this.Name = "PLCConfigScreen";
            this.Text = "PLC Config Screen";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_PLCModule)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dtg_PLCModule;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_EtnLampSettnig_Id;
        private System.Windows.Forms.TextBox txt_PLCModuleMaxNum;
        private System.Windows.Forms.Button btn_PLCModuleBackUp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Display;
        private System.Windows.Forms.Button btn_ManualControl;
        private System.Windows.Forms.Button btn_AutoControl;
    }
}