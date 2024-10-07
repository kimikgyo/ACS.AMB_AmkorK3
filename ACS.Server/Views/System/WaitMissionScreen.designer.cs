
namespace INA_ACS_Server
{
    partial class WaitMissionScreen
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtg_WaitMission = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_WaitMissionMaxNum = new System.Windows.Forms.TextBox();
            this.btn_WaitMssionBackup = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_WaitMission)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dtg_WaitMission);
            this.groupBox1.Controls.Add(this.txt_WaitMissionMaxNum);
            this.groupBox1.Controls.Add(this.btn_WaitMssionBackup);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(3, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1145, 521);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Waitting Mission";
            // 
            // dtg_WaitMission
            // 
            this.dtg_WaitMission.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dtg_WaitMission.AllowUserToAddRows = false;
            this.dtg_WaitMission.AllowUserToDeleteRows = false;
            this.dtg_WaitMission.AllowUserToResizeColumns = false;
            this.dtg_WaitMission.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtg_WaitMission.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dtg_WaitMission.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtg_WaitMission.BackgroundColor = System.Drawing.Color.White;
            this.dtg_WaitMission.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(89)))), ((int)(((byte)(96)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_WaitMission.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dtg_WaitMission.ColumnHeadersHeight = 50;
            this.dtg_WaitMission.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtg_WaitMission.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(52)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_WaitMission.DefaultCellStyle = dataGridViewCellStyle4;
            this.dtg_WaitMission.EnableHeadersVisualStyles = false;
            this.dtg_WaitMission.GridColor = System.Drawing.Color.LightGray;
            this.dtg_WaitMission.Location = new System.Drawing.Point(6, 63);
            this.dtg_WaitMission.MultiSelect = false;
            this.dtg_WaitMission.Name = "dtg_WaitMission";
            this.dtg_WaitMission.ReadOnly = true;
            this.dtg_WaitMission.RowHeadersVisible = false;
            this.dtg_WaitMission.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(52)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtg_WaitMission.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dtg_WaitMission.RowTemplate.Height = 23;
            this.dtg_WaitMission.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtg_WaitMission.Size = new System.Drawing.Size(1133, 443);
            this.dtg_WaitMission.TabIndex = 10;
            this.dtg_WaitMission.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dtg_WaitMission_CellMouseClick);
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn2.HeaderText = "EtnLampSettnig_Id";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // txt_WaitMissionMaxNum
            // 
            this.txt_WaitMissionMaxNum.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_WaitMissionMaxNum.Location = new System.Drawing.Point(352, 24);
            this.txt_WaitMissionMaxNum.Name = "txt_WaitMissionMaxNum";
            this.txt_WaitMissionMaxNum.Size = new System.Drawing.Size(118, 26);
            this.txt_WaitMissionMaxNum.TabIndex = 5;
            this.txt_WaitMissionMaxNum.Text = "0";
            this.txt_WaitMissionMaxNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_WaitMissionMaxNum.Click += new System.EventHandler(this.txt_WaitMissionMaxNum_Click);
            // 
            // btn_WaitMssionBackup
            // 
            this.btn_WaitMssionBackup.Location = new System.Drawing.Point(520, 16);
            this.btn_WaitMssionBackup.Name = "btn_WaitMssionBackup";
            this.btn_WaitMssionBackup.Size = new System.Drawing.Size(156, 41);
            this.btn_WaitMssionBackup.TabIndex = 6;
            this.btn_WaitMssionBackup.Text = "Back-Up Save File";
            this.btn_WaitMssionBackup.UseVisualStyleBackColor = true;
            this.btn_WaitMssionBackup.Click += new System.EventHandler(this.btn_BackUpSaveFile_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(194, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Waitting MaxNum";
            // 
            // WaitMissionScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 534);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WaitMissionScreen";
            this.Text = "AutoMission_TowerLampScreen";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_WaitMission)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dtg_WaitMission;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.TextBox txt_WaitMissionMaxNum;
        private System.Windows.Forms.Button btn_WaitMssionBackup;
        private System.Windows.Forms.Label label1;
    }
}