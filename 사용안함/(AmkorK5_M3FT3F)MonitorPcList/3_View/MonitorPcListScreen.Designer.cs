
namespace INA_ACS_Server.Views
{
    partial class MonitorPcListScreen
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
            this.dtg_MonitorPcList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_MonitorPcListMaxNum = new System.Windows.Forms.TextBox();
            this.btn_MonitorPcListBackUp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_MonitorPcList)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtg_MonitorPcList);
            this.groupBox1.Controls.Add(this.txt_MonitorPcListMaxNum);
            this.groupBox1.Controls.Add(this.btn_MonitorPcListBackUp);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1121, 515);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Monitor Pc List";
            // 
            // dtg_MonitorPcList
            // 
            this.dtg_MonitorPcList.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dtg_MonitorPcList.AllowUserToAddRows = false;
            this.dtg_MonitorPcList.AllowUserToDeleteRows = false;
            this.dtg_MonitorPcList.AllowUserToResizeColumns = false;
            this.dtg_MonitorPcList.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtg_MonitorPcList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dtg_MonitorPcList.BackgroundColor = System.Drawing.Color.White;
            this.dtg_MonitorPcList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_MonitorPcList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dtg_MonitorPcList.ColumnHeadersHeight = 50;
            this.dtg_MonitorPcList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtg_MonitorPcList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_MonitorPcList.DefaultCellStyle = dataGridViewCellStyle9;
            this.dtg_MonitorPcList.EnableHeadersVisualStyles = false;
            this.dtg_MonitorPcList.GridColor = System.Drawing.Color.LightGray;
            this.dtg_MonitorPcList.Location = new System.Drawing.Point(6, 63);
            this.dtg_MonitorPcList.MultiSelect = false;
            this.dtg_MonitorPcList.Name = "dtg_MonitorPcList";
            this.dtg_MonitorPcList.ReadOnly = true;
            this.dtg_MonitorPcList.RowHeadersVisible = false;
            this.dtg_MonitorPcList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtg_MonitorPcList.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dtg_MonitorPcList.RowTemplate.Height = 23;
            this.dtg_MonitorPcList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtg_MonitorPcList.Size = new System.Drawing.Size(1109, 446);
            this.dtg_MonitorPcList.TabIndex = 10;
            this.dtg_MonitorPcList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtg_MonitorPcList_CellEndEdit);
            this.dtg_MonitorPcList.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dtg_MonitorPcList_CellMouseClick);
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn2.HeaderText = "EtnLampSettnig_Id";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // txt_MonitorPcListMaxNum
            // 
            this.txt_MonitorPcListMaxNum.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_MonitorPcListMaxNum.Location = new System.Drawing.Point(352, 24);
            this.txt_MonitorPcListMaxNum.Name = "txt_MonitorPcListMaxNum";
            this.txt_MonitorPcListMaxNum.Size = new System.Drawing.Size(118, 26);
            this.txt_MonitorPcListMaxNum.TabIndex = 5;
            this.txt_MonitorPcListMaxNum.Text = "0";
            this.txt_MonitorPcListMaxNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_MonitorPcListMaxNum.Click += new System.EventHandler(this.txt_MonitorPcListMaxNum_Click);
            // 
            // btn_MonitorPcListBackUp
            // 
            this.btn_MonitorPcListBackUp.Location = new System.Drawing.Point(520, 16);
            this.btn_MonitorPcListBackUp.Name = "btn_MonitorPcListBackUp";
            this.btn_MonitorPcListBackUp.Size = new System.Drawing.Size(156, 41);
            this.btn_MonitorPcListBackUp.TabIndex = 6;
            this.btn_MonitorPcListBackUp.Text = "Back-Up Save File";
            this.btn_MonitorPcListBackUp.UseVisualStyleBackColor = true;
            this.btn_MonitorPcListBackUp.Click += new System.EventHandler(this.btn_BackUpSaveFile_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(194, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "MonitorPcListMaxNum";
            // 
            // MonitorPcListScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1146, 539);
            this.Controls.Add(this.groupBox1);
            this.Name = "MonitorPcListScreen";
            this.Text = "MonitorPcListScreen";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_MonitorPcList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dtg_MonitorPcList;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.TextBox txt_MonitorPcListMaxNum;
        private System.Windows.Forms.Button btn_MonitorPcListBackUp;
        private System.Windows.Forms.Label label1;
    }
}