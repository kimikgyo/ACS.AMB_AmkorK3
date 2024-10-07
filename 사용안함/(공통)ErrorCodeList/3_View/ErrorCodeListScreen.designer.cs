
namespace INA_ACS_Server
{
    partial class ErrorCodeListScreen
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
            this.dtg_ErrorCodeList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_ErrorCodeListBackup = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_ErrorPositionGroup = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_ErrorCodeList)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txt_ErrorPositionGroup);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.dtg_ErrorCodeList);
            this.groupBox2.Controls.Add(this.btn_ErrorCodeListBackup);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1121, 554);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ErrorCodeList";
            // 
            // dtg_ErrorCodeList
            // 
            this.dtg_ErrorCodeList.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dtg_ErrorCodeList.AllowUserToAddRows = false;
            this.dtg_ErrorCodeList.AllowUserToDeleteRows = false;
            this.dtg_ErrorCodeList.AllowUserToResizeColumns = false;
            this.dtg_ErrorCodeList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtg_ErrorCodeList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dtg_ErrorCodeList.BackgroundColor = System.Drawing.Color.White;
            this.dtg_ErrorCodeList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_ErrorCodeList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dtg_ErrorCodeList.ColumnHeadersHeight = 50;
            this.dtg_ErrorCodeList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtg_ErrorCodeList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_ErrorCodeList.DefaultCellStyle = dataGridViewCellStyle4;
            this.dtg_ErrorCodeList.EnableHeadersVisualStyles = false;
            this.dtg_ErrorCodeList.GridColor = System.Drawing.Color.LightGray;
            this.dtg_ErrorCodeList.Location = new System.Drawing.Point(6, 63);
            this.dtg_ErrorCodeList.MultiSelect = false;
            this.dtg_ErrorCodeList.Name = "dtg_ErrorCodeList";
            this.dtg_ErrorCodeList.ReadOnly = true;
            this.dtg_ErrorCodeList.RowHeadersVisible = false;
            this.dtg_ErrorCodeList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtg_ErrorCodeList.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dtg_ErrorCodeList.RowTemplate.Height = 23;
            this.dtg_ErrorCodeList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtg_ErrorCodeList.Size = new System.Drawing.Size(1109, 485);
            this.dtg_ErrorCodeList.TabIndex = 10;
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
            // btn_ErrorCodeListBackup
            // 
            this.btn_ErrorCodeListBackup.Location = new System.Drawing.Point(580, 16);
            this.btn_ErrorCodeListBackup.Name = "btn_ErrorCodeListBackup";
            this.btn_ErrorCodeListBackup.Size = new System.Drawing.Size(156, 41);
            this.btn_ErrorCodeListBackup.TabIndex = 6;
            this.btn_ErrorCodeListBackup.Text = "Back-Up Save File";
            this.btn_ErrorCodeListBackup.UseVisualStyleBackColor = true;
            this.btn_ErrorCodeListBackup.Click += new System.EventHandler(this.btn_BackUpSaveFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(163, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 19);
            this.label1.TabIndex = 11;
            this.label1.Text = "Error Position Group";
            // 
            // txt_ErrorPositionGroup
            // 
            this.txt_ErrorPositionGroup.AccessibleDescription = "RT15";
            this.txt_ErrorPositionGroup.AccessibleName = "";
            this.txt_ErrorPositionGroup.BackColor = System.Drawing.Color.White;
            this.txt_ErrorPositionGroup.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_ErrorPositionGroup.Location = new System.Drawing.Point(336, 25);
            this.txt_ErrorPositionGroup.Name = "txt_ErrorPositionGroup";
            this.txt_ErrorPositionGroup.ReadOnly = true;
            this.txt_ErrorPositionGroup.Size = new System.Drawing.Size(194, 25);
            this.txt_ErrorPositionGroup.TabIndex = 605;
            this.txt_ErrorPositionGroup.Text = "0";
            this.txt_ErrorPositionGroup.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_ErrorPositionGroup.Click += new System.EventHandler(this.txt_ErrorPositionGroup_Click);
            // 
            // ErrorCodeListScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1162, 578);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ErrorCodeListScreen";
            this.Text = "AutoMission_TowerLampScreen";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_ErrorCodeList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dtg_ErrorCodeList;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Button btn_ErrorCodeListBackup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_ErrorPositionGroup;
    }
}