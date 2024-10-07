
namespace INA_ACS_Server
{
    partial class WiseTowerLampConfigsScreen
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_RegistarMode = new System.Windows.Forms.Button();
            this.btn_BarCodeMode = new System.Windows.Forms.Button();
            this.btn_ProductNameInfo = new System.Windows.Forms.Button();
            this.txt_WiseTowerLampResponseTime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Display = new System.Windows.Forms.Button();
            this.btn_ManualControl = new System.Windows.Forms.Button();
            this.btn_AutoControl = new System.Windows.Forms.Button();
            this.dtg_WiseTowerLampModule = new System.Windows.Forms.DataGridView();
            this.DGV_EtnLampSettnig_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_TowerLampWiseModuleMaxNum = new System.Windows.Forms.TextBox();
            this.btn_WiseTowerLampModuleBackUp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_TowerLampWiseModuleGroup = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_WiseTowerLampModule)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_WiseTowerLampModuleBackUp);
            this.groupBox1.Controls.Add(this.txt_TowerLampWiseModuleGroup);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btn_RegistarMode);
            this.groupBox1.Controls.Add(this.btn_BarCodeMode);
            this.groupBox1.Controls.Add(this.btn_ProductNameInfo);
            this.groupBox1.Controls.Add(this.txt_WiseTowerLampResponseTime);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btn_Display);
            this.groupBox1.Controls.Add(this.btn_ManualControl);
            this.groupBox1.Controls.Add(this.btn_AutoControl);
            this.groupBox1.Controls.Add(this.dtg_WiseTowerLampModule);
            this.groupBox1.Controls.Add(this.txt_TowerLampWiseModuleMaxNum);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1171, 564);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tower Lamp Wise Module";
            // 
            // btn_RegistarMode
            // 
            this.btn_RegistarMode.Location = new System.Drawing.Point(9, 69);
            this.btn_RegistarMode.Name = "btn_RegistarMode";
            this.btn_RegistarMode.Size = new System.Drawing.Size(156, 41);
            this.btn_RegistarMode.TabIndex = 18;
            this.btn_RegistarMode.Text = "RegistarMode";
            this.btn_RegistarMode.UseVisualStyleBackColor = true;
            this.btn_RegistarMode.Click += new System.EventHandler(this.btn_ScreenButton_Click);
            // 
            // btn_BarCodeMode
            // 
            this.btn_BarCodeMode.Location = new System.Drawing.Point(171, 69);
            this.btn_BarCodeMode.Name = "btn_BarCodeMode";
            this.btn_BarCodeMode.Size = new System.Drawing.Size(156, 41);
            this.btn_BarCodeMode.TabIndex = 17;
            this.btn_BarCodeMode.Text = "BarCodeMode";
            this.btn_BarCodeMode.UseVisualStyleBackColor = true;
            this.btn_BarCodeMode.Click += new System.EventHandler(this.btn_ScreenButton_Click);
            // 
            // btn_ProductNameInfo
            // 
            this.btn_ProductNameInfo.Location = new System.Drawing.Point(774, 69);
            this.btn_ProductNameInfo.Name = "btn_ProductNameInfo";
            this.btn_ProductNameInfo.Size = new System.Drawing.Size(156, 41);
            this.btn_ProductNameInfo.TabIndex = 16;
            this.btn_ProductNameInfo.Text = "ProductNameInfo";
            this.btn_ProductNameInfo.UseVisualStyleBackColor = true;
            this.btn_ProductNameInfo.Click += new System.EventHandler(this.btn_ScreenButton_Click);
            // 
            // txt_WiseTowerLampResponseTime
            // 
            this.txt_WiseTowerLampResponseTime.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_WiseTowerLampResponseTime.Location = new System.Drawing.Point(205, 41);
            this.txt_WiseTowerLampResponseTime.Name = "txt_WiseTowerLampResponseTime";
            this.txt_WiseTowerLampResponseTime.Size = new System.Drawing.Size(118, 26);
            this.txt_WiseTowerLampResponseTime.TabIndex = 15;
            this.txt_WiseTowerLampResponseTime.Text = "0";
            this.txt_WiseTowerLampResponseTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_WiseTowerLampResponseTime.Click += new System.EventHandler(this.txt_WiseTowerLampConfig_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(205, 25);
            this.label2.TabIndex = 14;
            this.label2.Text = "TowerLamp_ResponseTime";
            // 
            // btn_Display
            // 
            this.btn_Display.Location = new System.Drawing.Point(629, 69);
            this.btn_Display.Name = "btn_Display";
            this.btn_Display.Size = new System.Drawing.Size(139, 41);
            this.btn_Display.TabIndex = 13;
            this.btn_Display.Text = "Display";
            this.btn_Display.UseVisualStyleBackColor = true;
            this.btn_Display.Click += new System.EventHandler(this.btn_ScreenButton_Click);
            // 
            // btn_ManualControl
            // 
            this.btn_ManualControl.Location = new System.Drawing.Point(479, 69);
            this.btn_ManualControl.Name = "btn_ManualControl";
            this.btn_ManualControl.Size = new System.Drawing.Size(144, 41);
            this.btn_ManualControl.TabIndex = 12;
            this.btn_ManualControl.Text = "ManualControl";
            this.btn_ManualControl.UseVisualStyleBackColor = true;
            this.btn_ManualControl.Click += new System.EventHandler(this.btn_ScreenButton_Click);
            // 
            // btn_AutoControl
            // 
            this.btn_AutoControl.Location = new System.Drawing.Point(333, 69);
            this.btn_AutoControl.Name = "btn_AutoControl";
            this.btn_AutoControl.Size = new System.Drawing.Size(140, 41);
            this.btn_AutoControl.TabIndex = 11;
            this.btn_AutoControl.Text = "AutoControl";
            this.btn_AutoControl.UseVisualStyleBackColor = true;
            this.btn_AutoControl.Click += new System.EventHandler(this.btn_ScreenButton_Click);
            // 
            // dtg_WiseTowerLampModule
            // 
            this.dtg_WiseTowerLampModule.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dtg_WiseTowerLampModule.AllowUserToAddRows = false;
            this.dtg_WiseTowerLampModule.AllowUserToDeleteRows = false;
            this.dtg_WiseTowerLampModule.AllowUserToResizeColumns = false;
            this.dtg_WiseTowerLampModule.AllowUserToResizeRows = false;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtg_WiseTowerLampModule.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle16;
            this.dtg_WiseTowerLampModule.BackgroundColor = System.Drawing.Color.White;
            this.dtg_WiseTowerLampModule.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_WiseTowerLampModule.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.dtg_WiseTowerLampModule.ColumnHeadersHeight = 50;
            this.dtg_WiseTowerLampModule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtg_WiseTowerLampModule.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DGV_EtnLampSettnig_Id});
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_WiseTowerLampModule.DefaultCellStyle = dataGridViewCellStyle19;
            this.dtg_WiseTowerLampModule.EnableHeadersVisualStyles = false;
            this.dtg_WiseTowerLampModule.GridColor = System.Drawing.Color.LightGray;
            this.dtg_WiseTowerLampModule.Location = new System.Drawing.Point(6, 116);
            this.dtg_WiseTowerLampModule.MultiSelect = false;
            this.dtg_WiseTowerLampModule.Name = "dtg_WiseTowerLampModule";
            this.dtg_WiseTowerLampModule.ReadOnly = true;
            this.dtg_WiseTowerLampModule.RowHeadersVisible = false;
            this.dtg_WiseTowerLampModule.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtg_WiseTowerLampModule.RowsDefaultCellStyle = dataGridViewCellStyle20;
            this.dtg_WiseTowerLampModule.RowTemplate.Height = 23;
            this.dtg_WiseTowerLampModule.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtg_WiseTowerLampModule.Size = new System.Drawing.Size(1122, 438);
            this.dtg_WiseTowerLampModule.TabIndex = 10;
            this.dtg_WiseTowerLampModule.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtg_TowerLampWiseModule_CellEndEdit);
            this.dtg_WiseTowerLampModule.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dtg_TowerLampWiseModule_CellMouseClick);
            // 
            // DGV_EtnLampSettnig_Id
            // 
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_EtnLampSettnig_Id.DefaultCellStyle = dataGridViewCellStyle18;
            this.DGV_EtnLampSettnig_Id.HeaderText = "EtnLampSettnig_Id";
            this.DGV_EtnLampSettnig_Id.Name = "DGV_EtnLampSettnig_Id";
            this.DGV_EtnLampSettnig_Id.ReadOnly = true;
            this.DGV_EtnLampSettnig_Id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // txt_TowerLampWiseModuleMaxNum
            // 
            this.txt_TowerLampWiseModuleMaxNum.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_TowerLampWiseModuleMaxNum.Location = new System.Drawing.Point(205, 13);
            this.txt_TowerLampWiseModuleMaxNum.Name = "txt_TowerLampWiseModuleMaxNum";
            this.txt_TowerLampWiseModuleMaxNum.Size = new System.Drawing.Size(118, 26);
            this.txt_TowerLampWiseModuleMaxNum.TabIndex = 5;
            this.txt_TowerLampWiseModuleMaxNum.Text = "0";
            this.txt_TowerLampWiseModuleMaxNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_TowerLampWiseModuleMaxNum.Click += new System.EventHandler(this.txt_WiseTowerLampConfig_Click);
            // 
            // btn_WiseTowerLampModuleBackUp
            // 
            this.btn_WiseTowerLampModuleBackUp.Location = new System.Drawing.Point(333, 33);
            this.btn_WiseTowerLampModuleBackUp.Name = "btn_WiseTowerLampModuleBackUp";
            this.btn_WiseTowerLampModuleBackUp.Size = new System.Drawing.Size(415, 34);
            this.btn_WiseTowerLampModuleBackUp.TabIndex = 6;
            this.btn_WiseTowerLampModuleBackUp.Text = "Back-Up Save File";
            this.btn_WiseTowerLampModuleBackUp.UseVisualStyleBackColor = true;
            this.btn_WiseTowerLampModuleBackUp.Click += new System.EventHandler(this.btn_ScreenButton_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Tower Lamp Wise Module MaxNum";
            // 
            // txt_TowerLampWiseModuleGroup
            // 
            this.txt_TowerLampWiseModuleGroup.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_TowerLampWiseModuleGroup.Location = new System.Drawing.Point(576, 10);
            this.txt_TowerLampWiseModuleGroup.Name = "txt_TowerLampWiseModuleGroup";
            this.txt_TowerLampWiseModuleGroup.Size = new System.Drawing.Size(172, 26);
            this.txt_TowerLampWiseModuleGroup.TabIndex = 20;
            this.txt_TowerLampWiseModuleGroup.Text = "0";
            this.txt_TowerLampWiseModuleGroup.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_TowerLampWiseModuleGroup.Click += new System.EventHandler(this.txt_WiseTowerLampConfig_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(330, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(240, 25);
            this.label3.TabIndex = 19;
            this.label3.Text = "Tower Lamp Wise Module Group";
            // 
            // WiseTowerLampConfigsScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 578);
            this.Controls.Add(this.groupBox1);
            this.Name = "WiseTowerLampConfigsScreen";
            this.Text = "TowerLamp_WiseMoule";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_WiseTowerLampModule)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dtg_WiseTowerLampModule;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_EtnLampSettnig_Id;
        private System.Windows.Forms.TextBox txt_TowerLampWiseModuleMaxNum;
        private System.Windows.Forms.Button btn_WiseTowerLampModuleBackUp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Display;
        private System.Windows.Forms.Button btn_ManualControl;
        private System.Windows.Forms.Button btn_AutoControl;
        private System.Windows.Forms.TextBox txt_WiseTowerLampResponseTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_ProductNameInfo;
        private System.Windows.Forms.Button btn_RegistarMode;
        private System.Windows.Forms.Button btn_BarCodeMode;
        private System.Windows.Forms.TextBox txt_TowerLampWiseModuleGroup;
        private System.Windows.Forms.Label label3;
    }
}