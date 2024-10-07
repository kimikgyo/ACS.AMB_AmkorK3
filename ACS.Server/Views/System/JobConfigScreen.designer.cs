
namespace INA_ACS_Server
{
    partial class JobConfigScreen
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtg_JobConfig = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txt_JobConfigsMaxNum = new System.Windows.Forms.TextBox();
            this.btn_JobConfigBackup = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_JobConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dtg_JobConfig);
            this.groupBox1.Controls.Add(this.txt_JobConfigsMaxNum);
            this.groupBox1.Controls.Add(this.btn_JobConfigBackup);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(3, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1162, 543);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Job Config";
            // 
            // dtg_JobConfig
            // 
            this.dtg_JobConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtg_JobConfig.Location = new System.Drawing.Point(6, 63);
            this.dtg_JobConfig.MainView = this.gridView1;
            this.dtg_JobConfig.Name = "dtg_JobConfig";
            this.dtg_JobConfig.Size = new System.Drawing.Size(1147, 474);
            this.dtg_JobConfig.TabIndex = 12;
            this.dtg_JobConfig.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.dtg_JobConfig;
            this.gridView1.Name = "gridView1";
            this.gridView1.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gridView1_RowCellClick);
            this.gridView1.CustomDrawColumnHeader += new DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventHandler(this.gridView1_CustomDrawColumnHeader);
            this.gridView1.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridView1_RowCellStyle);
            // 
            // txt_JobConfigsMaxNum
            // 
            this.txt_JobConfigsMaxNum.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_JobConfigsMaxNum.Location = new System.Drawing.Point(350, 24);
            this.txt_JobConfigsMaxNum.Name = "txt_JobConfigsMaxNum";
            this.txt_JobConfigsMaxNum.Size = new System.Drawing.Size(118, 26);
            this.txt_JobConfigsMaxNum.TabIndex = 5;
            this.txt_JobConfigsMaxNum.Text = "0";
            this.txt_JobConfigsMaxNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_JobConfigsMaxNum.Click += new System.EventHandler(this.txt_JobConfigsMaxNum_Click);
            // 
            // btn_JobConfigBackup
            // 
            this.btn_JobConfigBackup.Location = new System.Drawing.Point(546, 16);
            this.btn_JobConfigBackup.Name = "btn_JobConfigBackup";
            this.btn_JobConfigBackup.Size = new System.Drawing.Size(156, 41);
            this.btn_JobConfigBackup.TabIndex = 6;
            this.btn_JobConfigBackup.Text = "Back-Up Save File";
            this.btn_JobConfigBackup.UseVisualStyleBackColor = true;
            this.btn_JobConfigBackup.Click += new System.EventHandler(this.btn_BackUpSaveFile_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(194, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Job MaxNum";
            // 
            // JobConfigScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1187, 556);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "JobConfigScreen";
            this.Text = "MissionConfigsScreen";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_JobConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txt_JobConfigsMaxNum;
        private System.Windows.Forms.Button btn_JobConfigBackup;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraGrid.GridControl dtg_JobConfig;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    }
}