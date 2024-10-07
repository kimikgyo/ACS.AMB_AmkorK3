using System;
using System.Windows.Forms;
using System.Drawing;
using INA_ACS_Server.UI;



namespace INA_ACS_Server.UI
{
    public partial class LoginForm : Form
    {
        public string Login_UserName = string.Empty;
        public string Login_UserPassword = string.Empty;
        public int Update_index = 0;

        public LoginForm()
        {
            InitializeComponent();

            this.AcceptButton = btnLogin;
            this.CancelButton = btnCancel;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            txtUserName.Text = "MAINT";
            //txtUserName.Text = "INATECH";
            //txtUserPassword.Text = "INATECH";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login_UserName = txtUserName.Text.Trim();
            Login_UserPassword = txtUserPassword.Text.Trim();
            Update_index = 1;
            DialogResult = DialogResult.Yes;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }

        private void VirtualKeyBoard_Click(object sender, EventArgs e)
        {
            VirtualKeyBoard Vkeyboard;
            if (((TextBox)sender).Text.Length > 0) Vkeyboard = new VirtualKeyBoard(((TextBox)sender).Text.ToString());
            else Vkeyboard = new VirtualKeyBoard();
            DialogResult result = Vkeyboard.ShowDialog();

            if (result == DialogResult.OK)
            {
                ((TextBox)sender).Text = Vkeyboard.Return_Value.ToString();
                Vkeyboard.Close();
            }
        }

        private void PasswordKeyBoard_Click(object sender, EventArgs e)
        {
            LoginKeypad Loginkeyboard;

            if (((TextBox)sender).Text.Length > 0) Loginkeyboard = new LoginKeypad(((TextBox)sender).Text.ToString());
            else Loginkeyboard = new LoginKeypad();
            DialogResult result = Loginkeyboard.ShowDialog();

            if (result == DialogResult.OK)
            {
                ((TextBox)sender).Text = Loginkeyboard.Return_Value.ToString();
                Loginkeyboard.Close();
            }
        }
    }
}
