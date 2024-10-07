
namespace INA_ACS_Server
{
    partial class WiseTowerLampMonitoringScreen
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
            this.dtg_WiseTowerLampMonitoring = new System.Windows.Forms.DataGridView();
            this.DGV_WiseTowerLampMonitoring_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DisPlayTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dtg_WiseTowerLampMonitoring)).BeginInit();
            this.SuspendLayout();
            // 
            // dtg_WiseTowerLampMonitoring
            // 
            this.dtg_WiseTowerLampMonitoring.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dtg_WiseTowerLampMonitoring.AllowUserToAddRows = false;
            this.dtg_WiseTowerLampMonitoring.AllowUserToDeleteRows = false;
            this.dtg_WiseTowerLampMonitoring.AllowUserToResizeColumns = false;
            this.dtg_WiseTowerLampMonitoring.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtg_WiseTowerLampMonitoring.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dtg_WiseTowerLampMonitoring.BackgroundColor = System.Drawing.Color.White;
            this.dtg_WiseTowerLampMonitoring.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_WiseTowerLampMonitoring.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dtg_WiseTowerLampMonitoring.ColumnHeadersHeight = 50;
            this.dtg_WiseTowerLampMonitoring.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtg_WiseTowerLampMonitoring.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DGV_WiseTowerLampMonitoring_Id});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_WiseTowerLampMonitoring.DefaultCellStyle = dataGridViewCellStyle4;
            this.dtg_WiseTowerLampMonitoring.EnableHeadersVisualStyles = false;
            this.dtg_WiseTowerLampMonitoring.GridColor = System.Drawing.Color.LightGray;
            this.dtg_WiseTowerLampMonitoring.Location = new System.Drawing.Point(7, 12);
            this.dtg_WiseTowerLampMonitoring.MultiSelect = false;
            this.dtg_WiseTowerLampMonitoring.Name = "dtg_WiseTowerLampMonitoring";
            this.dtg_WiseTowerLampMonitoring.ReadOnly = true;
            this.dtg_WiseTowerLampMonitoring.RowHeadersVisible = false;
            this.dtg_WiseTowerLampMonitoring.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtg_WiseTowerLampMonitoring.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dtg_WiseTowerLampMonitoring.RowTemplate.Height = 23;
            this.dtg_WiseTowerLampMonitoring.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtg_WiseTowerLampMonitoring.Size = new System.Drawing.Size(998, 837);
            this.dtg_WiseTowerLampMonitoring.TabIndex = 12;
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
            // WiseTowerLampMonitoringScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 861);
            this.Controls.Add(this.dtg_WiseTowerLampMonitoring);
            this.Name = "WiseTowerLampMonitoringScreen";
            this.Text = "WiseTowerLampMonitoringScreen";
            ((System.ComponentModel.ISupportInitialize)(this.dtg_WiseTowerLampMonitoring)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dtg_WiseTowerLampMonitoring;
        private System.Windows.Forms.Timer DisPlayTimer;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_WiseTowerLampMonitoring_Id;
    }
}