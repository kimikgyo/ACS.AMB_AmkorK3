
namespace ACS.RobotMap
{
    partial class UCMapView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnMapDownload = new System.Windows.Forms.Button();
            this.chkCustomMap = new System.Windows.Forms.CheckBox();
            this.lbl_ClickPosInfo = new System.Windows.Forms.Label();
            this.cb_DisplayInfo = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnMapDownload
            // 
            this.btnMapDownload.Location = new System.Drawing.Point(241, 4);
            this.btnMapDownload.Name = "btnMapDownload";
            this.btnMapDownload.Size = new System.Drawing.Size(109, 23);
            this.btnMapDownload.TabIndex = 294;
            this.btnMapDownload.Text = "download map";
            this.btnMapDownload.UseVisualStyleBackColor = true;
            this.btnMapDownload.Visible = false;
            // 
            // chkCustomMap
            // 
            this.chkCustomMap.AutoSize = true;
            this.chkCustomMap.Location = new System.Drawing.Point(101, 4);
            this.chkCustomMap.Margin = new System.Windows.Forms.Padding(1, 4, 1, 4);
            this.chkCustomMap.Name = "chkCustomMap";
            this.chkCustomMap.Size = new System.Drawing.Size(84, 18);
            this.chkCustomMap.TabIndex = 293;
            this.chkCustomMap.Text = "custom map";
            this.chkCustomMap.UseVisualStyleBackColor = true;
            // 
            // lbl_ClickPosInfo
            // 
            this.lbl_ClickPosInfo.AutoSize = true;
            this.lbl_ClickPosInfo.Location = new System.Drawing.Point(202, 5);
            this.lbl_ClickPosInfo.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lbl_ClickPosInfo.Name = "lbl_ClickPosInfo";
            this.lbl_ClickPosInfo.Size = new System.Drawing.Size(35, 14);
            this.lbl_ClickPosInfo.TabIndex = 292;
            this.lbl_ClickPosInfo.Text = "label1";
            this.lbl_ClickPosInfo.Visible = false;
            // 
            // cb_DisplayInfo
            // 
            this.cb_DisplayInfo.AutoSize = true;
            this.cb_DisplayInfo.BackColor = System.Drawing.Color.Transparent;
            this.cb_DisplayInfo.ForeColor = System.Drawing.Color.Black;
            this.cb_DisplayInfo.Location = new System.Drawing.Point(1, 4);
            this.cb_DisplayInfo.Margin = new System.Windows.Forms.Padding(1, 4, 1, 4);
            this.cb_DisplayInfo.Name = "cb_DisplayInfo";
            this.cb_DisplayInfo.Size = new System.Drawing.Size(84, 18);
            this.cb_DisplayInfo.TabIndex = 291;
            this.cb_DisplayInfo.Text = "display info.";
            this.cb_DisplayInfo.UseVisualStyleBackColor = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(1, 30);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(1, 4, 1, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(449, 493);
            this.pictureBox1.TabIndex = 290;
            this.pictureBox1.TabStop = false;
            // 
            // UCMapView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.btnMapDownload);
            this.Controls.Add(this.chkCustomMap);
            this.Controls.Add(this.lbl_ClickPosInfo);
            this.Controls.Add(this.cb_DisplayInfo);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UCMapView";
            this.Size = new System.Drawing.Size(451, 527);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMapDownload;
        private System.Windows.Forms.CheckBox chkCustomMap;
        private System.Windows.Forms.Label lbl_ClickPosInfo;
        private System.Windows.Forms.CheckBox cb_DisplayInfo;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
