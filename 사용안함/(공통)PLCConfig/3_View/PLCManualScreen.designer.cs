
namespace INA_ACS_Server
{
    partial class PLCManualScreen
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
            this.dtg_PLCModuleManual = new System.Windows.Forms.DataGridView();
            this.DGV_EtnLampSettnig_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DisplayTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dtg_PLCModuleManual)).BeginInit();
            this.SuspendLayout();
            // 
            // dtg_PLCModuleManual
            // 
            this.dtg_PLCModuleManual.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dtg_PLCModuleManual.AllowUserToAddRows = false;
            this.dtg_PLCModuleManual.AllowUserToDeleteRows = false;
            this.dtg_PLCModuleManual.AllowUserToResizeColumns = false;
            this.dtg_PLCModuleManual.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtg_PLCModuleManual.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dtg_PLCModuleManual.BackgroundColor = System.Drawing.Color.White;
            this.dtg_PLCModuleManual.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_PLCModuleManual.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dtg_PLCModuleManual.ColumnHeadersHeight = 50;
            this.dtg_PLCModuleManual.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtg_PLCModuleManual.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DGV_EtnLampSettnig_Id});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_PLCModuleManual.DefaultCellStyle = dataGridViewCellStyle4;
            this.dtg_PLCModuleManual.EnableHeadersVisualStyles = false;
            this.dtg_PLCModuleManual.GridColor = System.Drawing.Color.LightGray;
            this.dtg_PLCModuleManual.Location = new System.Drawing.Point(7, 12);
            this.dtg_PLCModuleManual.MultiSelect = false;
            this.dtg_PLCModuleManual.Name = "dtg_PLCModuleManual";
            this.dtg_PLCModuleManual.ReadOnly = true;
            this.dtg_PLCModuleManual.RowHeadersVisible = false;
            this.dtg_PLCModuleManual.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtg_PLCModuleManual.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dtg_PLCModuleManual.RowTemplate.Height = 23;
            this.dtg_PLCModuleManual.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtg_PLCModuleManual.Size = new System.Drawing.Size(1427, 399);
            this.dtg_PLCModuleManual.TabIndex = 11;
            this.dtg_PLCModuleManual.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dtg_WiseTowerLampManual_CellMouseClick);
            // 
            // DGV_EtnLampSettnig_Id
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_EtnLampSettnig_Id.DefaultCellStyle = dataGridViewCellStyle3;
            this.DGV_EtnLampSettnig_Id.HeaderText = "EtnLampSettnig_Id";
            this.DGV_EtnLampSettnig_Id.Name = "DGV_EtnLampSettnig_Id";
            this.DGV_EtnLampSettnig_Id.ReadOnly = true;
            this.DGV_EtnLampSettnig_Id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DisplayTimer
            // 
            this.DisplayTimer.Enabled = true;
            this.DisplayTimer.Interval = 1000;
            this.DisplayTimer.Tick += new System.EventHandler(this.DisplayTimer_Tick);
            // 
            // PLCManualScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1446, 423);
            this.Controls.Add(this.dtg_PLCModuleManual);
            this.Name = "PLCManualScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PLC Module ManualScreen";
            ((System.ComponentModel.ISupportInitialize)(this.dtg_PLCModuleManual)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dtg_PLCModuleManual;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_EtnLampSettnig_Id;
        private System.Windows.Forms.Timer DisplayTimer;
    }
}