using System;
using System.Windows.Forms;



namespace INA_ACS_Server.UI
{
    public partial class PasswordForm : Form
    {
        public string Confirm_Password = string.Empty;

        public PasswordForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            txtConfirmPassword.Text = "";
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            Confirm_Password = txtConfirmPassword.Text.Trim();
            Close();
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
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
