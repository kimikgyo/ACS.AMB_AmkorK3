
namespace INA_ACS_Server
{
    partial class ProductNameInfoScreen
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
            this.dtg_ProductNameInfo = new System.Windows.Forms.DataGridView();
            this.DGV_WiseTowerLampMonitoring_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_ProductNameInfo_MaxNum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_ProductNameInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dtg_ProductNameInfo
            // 
            this.dtg_ProductNameInfo.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dtg_ProductNameInfo.AllowUserToAddRows = false;
            this.dtg_ProductNameInfo.AllowUserToDeleteRows = false;
            this.dtg_ProductNameInfo.AllowUserToResizeColumns = false;
            this.dtg_ProductNameInfo.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtg_ProductNameInfo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dtg_ProductNameInfo.BackgroundColor = System.Drawing.Color.White;
            this.dtg_ProductNameInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_ProductNameInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dtg_ProductNameInfo.ColumnHeadersHeight = 50;
            this.dtg_ProductNameInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtg_ProductNameInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DGV_WiseTowerLampMonitoring_Id});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_ProductNameInfo.DefaultCellStyle = dataGridViewCellStyle9;
            this.dtg_ProductNameInfo.EnableHeadersVisualStyles = false;
            this.dtg_ProductNameInfo.GridColor = System.Drawing.Color.LightGray;
            this.dtg_ProductNameInfo.Location = new System.Drawing.Point(9, 97);
            this.dtg_ProductNameInfo.MultiSelect = false;
            this.dtg_ProductNameInfo.Name = "dtg_ProductNameInfo";
            this.dtg_ProductNameInfo.ReadOnly = true;
            this.dtg_ProductNameInfo.RowHeadersVisible = false;
            this.dtg_ProductNameInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtg_ProductNameInfo.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dtg_ProductNameInfo.RowTemplate.Height = 23;
            this.dtg_ProductNameInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtg_ProductNameInfo.Size = new System.Drawing.Size(726, 752);
            this.dtg_ProductNameInfo.TabIndex = 13;
            this.dtg_ProductNameInfo.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtg_ProductNameInfo_CellEndEdit);
            this.dtg_ProductNameInfo.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dtg_ProductNameInfo_CellMouseClick);
            // 
            // DGV_WiseTowerLampMonitoring_Id
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_WiseTowerLampMonitoring_Id.DefaultCellStyle = dataGridViewCellStyle8;
            this.DGV_WiseTowerLampMonitoring_Id.HeaderText = "Index";
            this.DGV_WiseTowerLampMonitoring_Id.Name = "DGV_WiseTowerLampMonitoring_Id";
            this.DGV_WiseTowerLampMonitoring_Id.ReadOnly = true;
            this.DGV_WiseTowerLampMonitoring_Id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // txt_ProductNameInfo_MaxNum
            // 
            this.txt_ProductNameInfo_MaxNum.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_ProductNameInfo_MaxNum.Location = new System.Drawing.Point(360, 39);
            this.txt_ProductNameInfo_MaxNum.Name = "txt_ProductNameInfo_MaxNum";
            this.txt_ProductNameInfo_MaxNum.Size = new System.Drawing.Size(118, 26);
            this.txt_ProductNameInfo_MaxNum.TabIndex = 15;
            this.txt_ProductNameInfo_MaxNum.Text = "0";
            this.txt_ProductNameInfo_MaxNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_ProductNameInfo_MaxNum.Click += new System.EventHandler(this.txt_ProductNameInfo_MaxNum_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(132, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 25);
            this.label1.TabIndex = 14;
            this.label1.Text = "ProductNameInfo_MaxNum";
            // 
            // ProductNameInfoScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 861);
            this.Controls.Add(this.txt_ProductNameInfo_MaxNum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtg_ProductNameInfo);
            this.Name = "ProductNameInfoScreen";
            this.Text = "ProductNameInfoScreen";
            ((System.ComponentModel.ISupportInitialize)(this.dtg_ProductNameInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dtg_ProductNameInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_WiseTowerLampMonitoring_Id;
        private System.Windows.Forms.TextBox txt_ProductNameInfo_MaxNum;
        private System.Windows.Forms.Label label1;
    }
}