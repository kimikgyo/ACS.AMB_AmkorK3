
namespace INA_ACS_Server
{
    partial class MapMonitoring
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
            this.T_Panel = new System.Windows.Forms.TableLayoutPanel();
            this.ucMapView1 = new ACS.RobotMap.UCMapView();
            this.ucMapView2 = new ACS.RobotMap.UCMapView();
            this.ucMapView3 = new ACS.RobotMap.UCMapView();
            this.T_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // T_Panel
            // 
            this.T_Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.T_Panel.ColumnCount = 3;
            this.T_Panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.T_Panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.T_Panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.T_Panel.Controls.Add(this.ucMapView1, 0, 0);
            this.T_Panel.Controls.Add(this.ucMapView2, 1, 0);
            this.T_Panel.Controls.Add(this.ucMapView3, 2, 0);
            this.T_Panel.Location = new System.Drawing.Point(12, 12);
            this.T_Panel.Name = "T_Panel";
            this.T_Panel.RowCount = 1;
            this.T_Panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.T_Panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.T_Panel.Size = new System.Drawing.Size(872, 558);
            this.T_Panel.TabIndex = 1;
            // 
            // ucMapView1
            // 
            this.ucMapView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucMapView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucMapView1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucMapView1.Location = new System.Drawing.Point(3, 3);
            this.ucMapView1.MapID = null;
            this.ucMapView1.MapName = null;
            this.ucMapView1.Name = "ucMapView1";
            this.ucMapView1.Size = new System.Drawing.Size(284, 552);
            this.ucMapView1.TabIndex = 0;
            // 
            // ucMapView2
            // 
            this.ucMapView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucMapView2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucMapView2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucMapView2.Location = new System.Drawing.Point(293, 3);
            this.ucMapView2.MapID = null;
            this.ucMapView2.MapName = null;
            this.ucMapView2.Name = "ucMapView2";
            this.ucMapView2.Size = new System.Drawing.Size(284, 552);
            this.ucMapView2.TabIndex = 1;
            // 
            // ucMapView3
            // 
            this.ucMapView3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucMapView3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucMapView3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucMapView3.Location = new System.Drawing.Point(583, 3);
            this.ucMapView3.MapID = null;
            this.ucMapView3.MapName = null;
            this.ucMapView3.Name = "ucMapView3";
            this.ucMapView3.Size = new System.Drawing.Size(286, 552);
            this.ucMapView3.TabIndex = 2;
            // 
            // MapMonitoring
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 582);
            this.Controls.Add(this.T_Panel);
            this.Name = "MapMonitoring";
            this.Text = "Map Monitoring";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MapMonitoring_FormClosing);
            this.Enter += new System.EventHandler(this.MapMonitoring_Enter);
            this.Leave += new System.EventHandler(this.MapMonitoring_Leave);
            this.T_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ACS.RobotMap.UCMapView ucMapView1;
        private System.Windows.Forms.TableLayoutPanel T_Panel;
        private ACS.RobotMap.UCMapView ucMapView2;
        private ACS.RobotMap.UCMapView ucMapView3;
    }
}