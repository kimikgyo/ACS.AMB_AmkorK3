
namespace INA_ACS_Server
{
    partial class PositionScreen
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
            this.dtg_PositionArea = new System.Windows.Forms.DataGridView();
            this.DGV_PositionSettnig_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_PositionDataMaxNum = new System.Windows.Forms.TextBox();
            this.btn_POSBackUpSaveFile = new System.Windows.Forms.Button();
            this.btn_BackUpGetFile = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_PositionArea)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtg_PositionArea
            // 
            this.dtg_PositionArea.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dtg_PositionArea.AllowUserToAddRows = false;
            this.dtg_PositionArea.AllowUserToDeleteRows = false;
            this.dtg_PositionArea.AllowUserToResizeColumns = false;
            this.dtg_PositionArea.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtg_PositionArea.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dtg_PositionArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtg_PositionArea.BackgroundColor = System.Drawing.Color.White;
            this.dtg_PositionArea.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(89)))), ((int)(((byte)(96)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_PositionArea.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dtg_PositionArea.ColumnHeadersHeight = 50;
            this.dtg_PositionArea.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtg_PositionArea.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DGV_PositionSettnig_Id});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(52)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_PositionArea.DefaultCellStyle = dataGridViewCellStyle9;
            this.dtg_PositionArea.EnableHeadersVisualStyles = false;
            this.dtg_PositionArea.GridColor = System.Drawing.Color.LightGray;
            this.dtg_PositionArea.Location = new System.Drawing.Point(6, 56);
            this.dtg_PositionArea.MultiSelect = false;
            this.dtg_PositionArea.Name = "dtg_PositionArea";
            this.dtg_PositionArea.ReadOnly = true;
            this.dtg_PositionArea.RowHeadersVisible = false;
            this.dtg_PositionArea.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(52)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtg_PositionArea.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dtg_PositionArea.RowTemplate.Height = 23;
            this.dtg_PositionArea.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtg_PositionArea.Size = new System.Drawing.Size(1133, 450);
            this.dtg_PositionArea.TabIndex = 0;
            this.dtg_PositionArea.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtg_PositionArea_CellEndEdit);
            this.dtg_PositionArea.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dtg_PositionArea_CellMouseClick);
            // 
            // DGV_PositionSettnig_Id
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_PositionSettnig_Id.DefaultCellStyle = dataGridViewCellStyle8;
            this.DGV_PositionSettnig_Id.HeaderText = "PositionSettnig_Id";
            this.DGV_PositionSettnig_Id.Name = "DGV_PositionSettnig_Id";
            this.DGV_PositionSettnig_Id.ReadOnly = true;
            this.DGV_PositionSettnig_Id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(207, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Position Data MaxNum";
            // 
            // txt_PositionDataMaxNum
            // 
            this.txt_PositionDataMaxNum.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_PositionDataMaxNum.Location = new System.Drawing.Point(407, 20);
            this.txt_PositionDataMaxNum.Name = "txt_PositionDataMaxNum";
            this.txt_PositionDataMaxNum.Size = new System.Drawing.Size(141, 26);
            this.txt_PositionDataMaxNum.TabIndex = 2;
            this.txt_PositionDataMaxNum.Text = "0";
            this.txt_PositionDataMaxNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_PositionDataMaxNum.Click += new System.EventHandler(this.txt_PositionDataMaxNum_Click);
            // 
            // btn_POSBackUpSaveFile
            // 
            this.btn_POSBackUpSaveFile.Location = new System.Drawing.Point(622, 14);
            this.btn_POSBackUpSaveFile.Name = "btn_POSBackUpSaveFile";
            this.btn_POSBackUpSaveFile.Size = new System.Drawing.Size(156, 41);
            this.btn_POSBackUpSaveFile.TabIndex = 3;
            this.btn_POSBackUpSaveFile.Text = "Back-Up Save File";
            this.btn_POSBackUpSaveFile.UseVisualStyleBackColor = true;
            this.btn_POSBackUpSaveFile.Click += new System.EventHandler(this.btn_POSBackUpSaveFile_Click);
            // 
            // btn_BackUpGetFile
            // 
            this.btn_BackUpGetFile.Location = new System.Drawing.Point(796, 14);
            this.btn_BackUpGetFile.Name = "btn_BackUpGetFile";
            this.btn_BackUpGetFile.Size = new System.Drawing.Size(166, 41);
            this.btn_BackUpGetFile.TabIndex = 4;
            this.btn_BackUpGetFile.Text = "BackUpGetFile";
            this.btn_BackUpGetFile.UseVisualStyleBackColor = true;
            this.btn_BackUpGetFile.Visible = false;
            this.btn_BackUpGetFile.Click += new System.EventHandler(this.btn_BackUpGetFile_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btn_BackUpGetFile);
            this.groupBox1.Controls.Add(this.txt_PositionDataMaxNum);
            this.groupBox1.Controls.Add(this.btn_POSBackUpSaveFile);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(3, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1145, 521);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PositionArea";
            // 
            // PositionScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 534);
            this.Controls.Add(this.dtg_PositionArea);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PositionScreen";
            this.Text = "PositionScreen";
            ((System.ComponentModel.ISupportInitialize)(this.dtg_PositionArea)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dtg_PositionArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_PositionSettnig_Id;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_PositionDataMaxNum;
        private System.Windows.Forms.Button btn_POSBackUpSaveFile;
        private System.Windows.Forms.Button btn_BackUpGetFile;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}