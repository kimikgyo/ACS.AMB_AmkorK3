namespace INA_ACS_Server.OPWindows
{
    partial class AcsLogViewerScreen
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
            this.AutoDisplay_Timer = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_ACS_LogViewer = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // AutoDisplay_Timer
            // 
            this.AutoDisplay_Timer.Enabled = true;
            this.AutoDisplay_Timer.Interval = 500;
            this.AutoDisplay_Timer.Tick += new System.EventHandler(this.AutoDisplay_Timer_Tick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txt_ACS_LogViewer);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 24);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1662, 920);
            this.groupBox2.TabIndex = 151;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ACS Log Viewer";
            // 
            // txt_ACS_LogViewer
            // 
            this.txt_ACS_LogViewer.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_ACS_LogViewer.Location = new System.Drawing.Point(12, 25);
            this.txt_ACS_LogViewer.Multiline = true;
            this.txt_ACS_LogViewer.Name = "txt_ACS_LogViewer";
            this.txt_ACS_LogViewer.ReadOnly = true;
            this.txt_ACS_LogViewer.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_ACS_LogViewer.Size = new System.Drawing.Size(1644, 889);
            this.txt_ACS_LogViewer.TabIndex = 151;
            this.txt_ACS_LogViewer.WordWrap = false;
            // 
            // AcsLogViewerScreen
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(1680, 956);
            this.Controls.Add(this.groupBox2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AcsLogViewerScreen";
            this.Text = "Form1";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer AutoDisplay_Timer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txt_ACS_LogViewer;
    }
}