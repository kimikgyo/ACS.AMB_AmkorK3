
namespace INA_ACS_Server
{
    partial class PLCMonitoringScreen
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtg_PLCReadDataMonitoring = new System.Windows.Forms.DataGridView();
            this.DGV_WiseTowerLampMonitoring_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DisPlayTimer = new System.Windows.Forms.Timer(this.components);
            this.dtg_PLCWriteDataMonitoring = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_PLCReadDataMonitoring)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_PLCWriteDataMonitoring)).BeginInit();
            this.SuspendLayout();
            // 
            // dtg_PLCReadDataMonitoring
            // 
            this.dtg_PLCReadDataMonitoring.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dtg_PLCReadDataMonitoring.AllowUserToAddRows = false;
            this.dtg_PLCReadDataMonitoring.AllowUserToDeleteRows = false;
            this.dtg_PLCReadDataMonitoring.AllowUserToResizeColumns = false;
            this.dtg_PLCReadDataMonitoring.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtg_PLCReadDataMonitoring.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dtg_PLCReadDataMonitoring.BackgroundColor = System.Drawing.Color.White;
            this.dtg_PLCReadDataMonitoring.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_PLCReadDataMonitoring.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dtg_PLCReadDataMonitoring.ColumnHeadersHeight = 50;
            this.dtg_PLCReadDataMonitoring.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtg_PLCReadDataMonitoring.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DGV_WiseTowerLampMonitoring_Id});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_PLCReadDataMonitoring.DefaultCellStyle = dataGridViewCellStyle4;
            this.dtg_PLCReadDataMonitoring.EnableHeadersVisualStyles = false;
            this.dtg_PLCReadDataMonitoring.GridColor = System.Drawing.Color.LightGray;
            this.dtg_PLCReadDataMonitoring.Location = new System.Drawing.Point(7, 12);
            this.dtg_PLCReadDataMonitoring.MultiSelect = false;
            this.dtg_PLCReadDataMonitoring.Name = "dtg_PLCReadDataMonitoring";
            this.dtg_PLCReadDataMonitoring.ReadOnly = true;
            this.dtg_PLCReadDataMonitoring.RowHeadersVisible = false;
            this.dtg_PLCReadDataMonitoring.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtg_PLCReadDataMonitoring.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dtg_PLCReadDataMonitoring.RowTemplate.Height = 23;
            this.dtg_PLCReadDataMonitoring.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtg_PLCReadDataMonitoring.Size = new System.Drawing.Size(1427, 399);
            this.dtg_PLCReadDataMonitoring.TabIndex = 12;
            // 
            // DGV_WiseTowerLampMonitoring_Id
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_WiseTowerLampMonitoring_Id.DefaultCellStyle = dataGridViewCellStyle3;
            this.DGV_WiseTowerLampMonitoring_Id.HeaderText = "Index";
            this.DGV_WiseTowerLampMonitoring_Id.Name = "DGV_WiseTowerLampMonitoring_Id";
            this.DGV_WiseTowerLampMonitoring_Id.ReadOnly = true;
            this.DGV_WiseTowerLampMonitoring_Id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DisPlayTimer
            // 
            this.DisPlayTimer.Enabled = true;
            this.DisPlayTimer.Tick += new System.EventHandler(this.DisPlayTimer_Tick);
            // 
            // dtg_PLCWriteDataMonitoring
            // 
            this.dtg_PLCWriteDataMonitoring.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dtg_PLCWriteDataMonitoring.AllowUserToAddRows = false;
            this.dtg_PLCWriteDataMonitoring.AllowUserToDeleteRows = false;
            this.dtg_PLCWriteDataMonitoring.AllowUserToResizeColumns = false;
            this.dtg_PLCWriteDataMonitoring.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtg_PLCWriteDataMonitoring.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dtg_PLCWriteDataMonitoring.BackgroundColor = System.Drawing.Color.White;
            this.dtg_PLCWriteDataMonitoring.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_PLCWriteDataMonitoring.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dtg_PLCWriteDataMonitoring.ColumnHeadersHeight = 50;
            this.dtg_PLCWriteDataMonitoring.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtg_PLCWriteDataMonitoring.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_PLCWriteDataMonitoring.DefaultCellStyle = dataGridViewCellStyle9;
            this.dtg_PLCWriteDataMonitoring.EnableHeadersVisualStyles = false;
            this.dtg_PLCWriteDataMonitoring.GridColor = System.Drawing.Color.LightGray;
            this.dtg_PLCWriteDataMonitoring.Location = new System.Drawing.Point(7, 427);
            this.dtg_PLCWriteDataMonitoring.MultiSelect = false;
            this.dtg_PLCWriteDataMonitoring.Name = "dtg_PLCWriteDataMonitoring";
            this.dtg_PLCWriteDataMonitoring.ReadOnly = true;
            this.dtg_PLCWriteDataMonitoring.RowHeadersVisible = false;
            this.dtg_PLCWriteDataMonitoring.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtg_PLCWriteDataMonitoring.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dtg_PLCWriteDataMonitoring.RowTemplate.Height = 23;
            this.dtg_PLCWriteDataMonitoring.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtg_PLCWriteDataMonitoring.Size = new System.Drawing.Size(1427, 399);
            this.dtg_PLCWriteDataMonitoring.TabIndex = 13;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn1.HeaderText = "Index";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PLCMonitoringScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1446, 861);
            this.Controls.Add(this.dtg_PLCWriteDataMonitoring);
            this.Controls.Add(this.dtg_PLCReadDataMonitoring);
            this.Name = "PLCMonitoringScreen";
            this.Text = "PLCModuleMonitoringScreen";
            ((System.ComponentModel.ISupportInitialize)(this.dtg_PLCReadDataMonitoring)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_PLCWriteDataMonitoring)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dtg_PLCReadDataMonitoring;
        private System.Windows.Forms.Timer DisPlayTimer;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_WiseTowerLampMonitoring_Id;
        private System.Windows.Forms.DataGridView dtg_PLCWriteDataMonitoring;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    }
}