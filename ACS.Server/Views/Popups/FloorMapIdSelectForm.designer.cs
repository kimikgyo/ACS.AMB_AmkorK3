﻿namespace INA_ACS_Server.UI
{
    partial class FloorMapIdSelectForm
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.cbo_FloorMapId_Select = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Location = new System.Drawing.Point(470, 143);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(153, 96);
            this.btnCancel.TabIndex = 274;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(470, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(153, 96);
            this.button1.TabIndex = 274;
            this.button1.Text = "Set";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(27, 65);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(196, 24);
            this.label24.TabIndex = 276;
            this.label24.Text = "Floor MapId Select";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbo_FloorMapId_Select
            // 
            this.cbo_FloorMapId_Select.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbo_FloorMapId_Select.FormattingEnabled = true;
            this.cbo_FloorMapId_Select.Items.AddRange(new object[] {
            "-"});
            this.cbo_FloorMapId_Select.Location = new System.Drawing.Point(31, 92);
            this.cbo_FloorMapId_Select.Name = "cbo_FloorMapId_Select";
            this.cbo_FloorMapId_Select.Size = new System.Drawing.Size(375, 32);
            this.cbo_FloorMapId_Select.TabIndex = 275;
            // 
            // FloorMapIdSelectForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(655, 251);
            this.ControlBox = false;
            this.Controls.Add(this.label24);
            this.Controls.Add(this.cbo_FloorMapId_Select);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FloorMapIdSelectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Floor MapId Select Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ComboBox cbo_FloorMapId_Select;
    }
}