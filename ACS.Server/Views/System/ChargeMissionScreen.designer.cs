
namespace INA_ACS_Server
{
    partial class ChargeMissionScreen
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dtg_ChargeMission = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_ChargeMissionMaxNum = new System.Windows.Forms.TextBox();
            this.btn_ChargeMssionBackup = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_ChargeMission)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dtg_ChargeMission);
            this.groupBox2.Controls.Add(this.txt_ChargeMissionMaxNum);
            this.groupBox2.Controls.Add(this.btn_ChargeMssionBackup);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(3, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1145, 521);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Charging Mission";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // dtg_ChargeMission
            // 
            this.dtg_ChargeMission.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dtg_ChargeMission.AllowUserToAddRows = false;
            this.dtg_ChargeMission.AllowUserToDeleteRows = false;
            this.dtg_ChargeMission.AllowUserToResizeColumns = false;
            this.dtg_ChargeMission.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtg_ChargeMission.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dtg_ChargeMission.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtg_ChargeMission.BackgroundColor = System.Drawing.Color.White;
            this.dtg_ChargeMission.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(89)))), ((int)(((byte)(96)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_ChargeMission.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dtg_ChargeMission.ColumnHeadersHeight = 50;
            this.dtg_ChargeMission.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtg_ChargeMission.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(52)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_ChargeMission.DefaultCellStyle = dataGridViewCellStyle4;
            this.dtg_ChargeMission.EnableHeadersVisualStyles = false;
            this.dtg_ChargeMission.GridColor = System.Drawing.Color.LightGray;
            this.dtg_ChargeMission.Location = new System.Drawing.Point(6, 63);
            this.dtg_ChargeMission.MultiSelect = false;
            this.dtg_ChargeMission.Name = "dtg_ChargeMission";
            this.dtg_ChargeMission.ReadOnly = true;
            this.dtg_ChargeMission.RowHeadersVisible = false;
            this.dtg_ChargeMission.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(52)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtg_ChargeMission.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dtg_ChargeMission.RowTemplate.Height = 23;
            this.dtg_ChargeMission.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtg_ChargeMission.Size = new System.Drawing.Size(1133, 443);
            this.dtg_ChargeMission.TabIndex = 10;
            this.dtg_ChargeMission.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dtg_ChargeMission_CellMouseClick);
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn3.HeaderText = "EtnLampSettnig_Id";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // txt_ChargeMissionMaxNum
            // 
            this.txt_ChargeMissionMaxNum.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_ChargeMissionMaxNum.Location = new System.Drawing.Point(352, 24);
            this.txt_ChargeMissionMaxNum.Name = "txt_ChargeMissionMaxNum";
            this.txt_ChargeMissionMaxNum.Size = new System.Drawing.Size(118, 26);
            this.txt_ChargeMissionMaxNum.TabIndex = 5;
            this.txt_ChargeMissionMaxNum.Text = "0";
            this.txt_ChargeMissionMaxNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_ChargeMissionMaxNum.Click += new System.EventHandler(this.txt_ChargeMissionMaxNum_Click);
            // 
            // btn_ChargeMssionBackup
            // 
            this.btn_ChargeMssionBackup.Location = new System.Drawing.Point(520, 16);
            this.btn_ChargeMssionBackup.Name = "btn_ChargeMssionBackup";
            this.btn_ChargeMssionBackup.Size = new System.Drawing.Size(156, 41);
            this.btn_ChargeMssionBackup.TabIndex = 6;
            this.btn_ChargeMssionBackup.Text = "Back-Up Save File";
            this.btn_ChargeMssionBackup.UseVisualStyleBackColor = true;
            this.btn_ChargeMssionBackup.Click += new System.EventHandler(this.btn_BackUpSaveFile_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(194, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(193, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Charging MaxNum";
            // 
            // ChargeMissionScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 534);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ChargeMissionScreen";
            this.Text = "AutoMission_TowerLampScreen";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_ChargeMission)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dtg_ChargeMission;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.TextBox txt_ChargeMissionMaxNum;
        private System.Windows.Forms.Button btn_ChargeMssionBackup;
        private System.Windows.Forms.Label label2;
    }
}