
namespace INA_ACS_Server
{
    partial class WiseModuleManualScreen
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
            this.dtg_WiseModuleManual = new System.Windows.Forms.DataGridView();
            this.DGV_EtnLampSettnig_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DisplayTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dtg_WiseModuleManual)).BeginInit();
            this.SuspendLayout();
            // 
            // dtg_WiseModuleManual
            // 
            this.dtg_WiseModuleManual.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dtg_WiseModuleManual.AllowUserToAddRows = false;
            this.dtg_WiseModuleManual.AllowUserToDeleteRows = false;
            this.dtg_WiseModuleManual.AllowUserToResizeColumns = false;
            this.dtg_WiseModuleManual.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtg_WiseModuleManual.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dtg_WiseModuleManual.BackgroundColor = System.Drawing.Color.White;
            this.dtg_WiseModuleManual.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_WiseModuleManual.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dtg_WiseModuleManual.ColumnHeadersHeight = 50;
            this.dtg_WiseModuleManual.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtg_WiseModuleManual.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DGV_EtnLampSettnig_Id});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_WiseModuleManual.DefaultCellStyle = dataGridViewCellStyle4;
            this.dtg_WiseModuleManual.EnableHeadersVisualStyles = false;
            this.dtg_WiseModuleManual.GridColor = System.Drawing.Color.LightGray;
            this.dtg_WiseModuleManual.Location = new System.Drawing.Point(7, 12);
            this.dtg_WiseModuleManual.MultiSelect = false;
            this.dtg_WiseModuleManual.Name = "dtg_WiseModuleManual";
            this.dtg_WiseModuleManual.ReadOnly = true;
            this.dtg_WiseModuleManual.RowHeadersVisible = false;
            this.dtg_WiseModuleManual.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtg_WiseModuleManual.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dtg_WiseModuleManual.RowTemplate.Height = 23;
            this.dtg_WiseModuleManual.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtg_WiseModuleManual.Size = new System.Drawing.Size(663, 837);
            this.dtg_WiseModuleManual.TabIndex = 11;
            this.dtg_WiseModuleManual.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dtg_WiseModuleManual_CellMouseClick);
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
            // WiseModuleManualScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 861);
            this.Controls.Add(this.dtg_WiseModuleManual);
            this.Name = "WiseModuleManualScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WiseTowerLampManualScreen";
            ((System.ComponentModel.ISupportInitialize)(this.dtg_WiseModuleManual)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dtg_WiseModuleManual;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_EtnLampSettnig_Id;
        private System.Windows.Forms.Timer DisplayTimer;
    }
}