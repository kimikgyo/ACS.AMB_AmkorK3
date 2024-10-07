namespace INA_ACS_Server
{
    partial class UserNumberForm
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
            this.btnpassword = new System.Windows.Forms.Button();
            this.txt_password = new System.Windows.Forms.TextBox();
            this.btnLogin2 = new System.Windows.Forms.Button();
            this.txt_UserNumber = new System.Windows.Forms.TextBox();
            this.btnLogin = new DevExpress.XtraEditors.SimpleButton();
            this.C_Radio1 = new INA_ACS_Server.RJRadioButton();
            this.C_Radio2 = new INA_ACS_Server.RJRadioButton();
            this.SuspendLayout();
            // 
            // btnpassword
            // 
            this.btnpassword.AccessibleName = "";
            this.btnpassword.BackColor = System.Drawing.SystemColors.Control;
            this.btnpassword.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnpassword.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnpassword.Location = new System.Drawing.Point(131, 418);
            this.btnpassword.Name = "btnpassword";
            this.btnpassword.Size = new System.Drawing.Size(85, 90);
            this.btnpassword.TabIndex = 154;
            this.btnpassword.Text = "비밀번호 설정";
            this.btnpassword.UseVisualStyleBackColor = false;
            this.btnpassword.Visible = false;
            // 
            // txt_password
            // 
            this.txt_password.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_password.Location = new System.Drawing.Point(30, 384);
            this.txt_password.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_password.Name = "txt_password";
            this.txt_password.Size = new System.Drawing.Size(186, 29);
            this.txt_password.TabIndex = 152;
            this.txt_password.UseSystemPasswordChar = true;
            // 
            // btnLogin2
            // 
            this.btnLogin2.AccessibleName = "";
            this.btnLogin2.BackColor = System.Drawing.SystemColors.Control;
            this.btnLogin2.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin2.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnLogin2.Location = new System.Drawing.Point(12, 420);
            this.btnLogin2.Name = "btnLogin2";
            this.btnLogin2.Size = new System.Drawing.Size(85, 90);
            this.btnLogin2.TabIndex = 150;
            this.btnLogin2.Text = "Login";
            this.btnLogin2.UseVisualStyleBackColor = false;
            this.btnLogin2.Visible = false;
            // 
            // txt_UserNumber
            // 
            this.txt_UserNumber.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_UserNumber.Location = new System.Drawing.Point(30, 335);
            this.txt_UserNumber.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_UserNumber.Name = "txt_UserNumber";
            this.txt_UserNumber.Size = new System.Drawing.Size(186, 29);
            this.txt_UserNumber.TabIndex = 148;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(481, 474);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(118, 36);
            this.btnLogin.TabIndex = 155;
            this.btnLogin.Text = "sign in";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // C_Radio1
            // 
            this.C_Radio1.AutoSize = true;
            this.C_Radio1.BordercheckedColor = System.Drawing.Color.MediumSlateBlue;
            this.C_Radio1.BorderunCheckedColor = System.Drawing.Color.Black;
            this.C_Radio1.CheckedColor = System.Drawing.Color.MediumSlateBlue;
            this.C_Radio1.Location = new System.Drawing.Point(30, 283);
            this.C_Radio1.MinimumSize = new System.Drawing.Size(0, 21);
            this.C_Radio1.Name = "C_Radio1";
            this.C_Radio1.Size = new System.Drawing.Size(119, 21);
            this.C_Radio1.TabIndex = 156;
            this.C_Radio1.TabStop = true;
            this.C_Radio1.Text = "rjRadioButton1";
            this.C_Radio1.UnCheckedColor = System.Drawing.Color.Black;
            this.C_Radio1.UseVisualStyleBackColor = true;
            // 
            // C_Radio2
            // 
            this.C_Radio2.AutoSize = true;
            this.C_Radio2.BordercheckedColor = System.Drawing.Color.MediumSlateBlue;
            this.C_Radio2.BorderunCheckedColor = System.Drawing.Color.Black;
            this.C_Radio2.CheckedColor = System.Drawing.Color.MediumSlateBlue;
            this.C_Radio2.Location = new System.Drawing.Point(30, 307);
            this.C_Radio2.MinimumSize = new System.Drawing.Size(0, 21);
            this.C_Radio2.Name = "C_Radio2";
            this.C_Radio2.Size = new System.Drawing.Size(119, 21);
            this.C_Radio2.TabIndex = 157;
            this.C_Radio2.TabStop = true;
            this.C_Radio2.Text = "rjRadioButton1";
            this.C_Radio2.UnCheckedColor = System.Drawing.Color.Black;
            this.C_Radio2.UseVisualStyleBackColor = true;
            // 
            // UserNumberForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 520);
            this.Controls.Add(this.C_Radio2);
            this.Controls.Add(this.C_Radio1);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnpassword);
            this.Controls.Add(this.txt_password);
            this.Controls.Add(this.btnLogin2);
            this.Controls.Add(this.txt_UserNumber);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserNumberForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.TopMost = true;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UserNumberForm_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnpassword;
        private System.Windows.Forms.TextBox txt_password;
        private System.Windows.Forms.Button btnLogin2;
        private System.Windows.Forms.TextBox txt_UserNumber;
        private DevExpress.XtraEditors.SimpleButton btnLogin;
        private RJRadioButton C_Radio1;
        private RJRadioButton C_Radio2;
    }
}