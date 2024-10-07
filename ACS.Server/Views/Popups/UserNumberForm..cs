using DevExpress.LookAndFeel;
using System;
using System.Drawing;
using System.Windows.Forms;



namespace INA_ACS_Server
{
    public partial class UserNumberForm : Form
    {
        public string UserNumber = string.Empty;
        public string UserPassword = string.Empty;
        public int Update_index = 0;

        public UserNumberForm()
        {
            InitializeComponent();

            this.AcceptButton = btnLogin2;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DesignInit();
            EventInit();
        }

        private void DesignInit()
        {
            this.BackColor = Color.FromArgb(43, 52, 59);

            PictureBox pictureBox = new PictureBox();
            pictureBox.Location = new Point(30, (this.Height / 3));
            pictureBox.Width = (this.Width / 3);
            pictureBox.Height = (this.Height / 2);
            pictureBox.Visible = true;

            Bitmap bitmap = new Bitmap(Properties.Resources.Login_background);
            Graphics graphics = Graphics.FromImage(bitmap);

            graphics.DrawString("ACS_Login", new Font("맑은 고딕", 12), Brushes.White, 50, 30);
            graphics.DrawString("Enter your Username and password to sign in to", new Font("맑은 고딕", 12), Brushes.White, 50, 50);
            graphics.DrawString("the ACS. Your username and password should", new Font("맑은 고딕", 12), Brushes.White, 50, 70);
            graphics.DrawString("be given to you by eigher the acs administrator", new Font("맑은 고딕", 12), Brushes.White, 50, 90);
            graphics.DrawString("If you don't have a username and password,", new Font("맑은 고딕", 12), Brushes.White, 50, 110);
            graphics.DrawString("contact the ACS administrator.", new Font("맑은 고딕", 12), Brushes.White, 50, 130);
            graphics.DrawString("", new Font("맑은 고딕", 12), Brushes.White, 50, 90);

            pictureBox.BackgroundImage = bitmap;
            pictureBox.BackgroundImageLayout = ImageLayout.Stretch;

            this.Controls.Add(pictureBox);

            C_Radio1.Checked = true;

            if (C_Radio1.Checked)
            {
                txt_UserNumber.Visible = true;
                txt_password.Visible = false;
            }
        }

        private void EventInit()
        {
            C_Radio1.Click += C_Radio_Click;
            C_Radio2.Click += C_Radio_Click;
        }

        private void C_Radio_Click(object sender, EventArgs e)
        {
            if (C_Radio1.Checked)
            {
                txt_UserNumber.Visible = true;
                txt_password.Visible = false;
            }

            if (C_Radio2.Checked)
            {
                txt_UserNumber.Visible = true;
                txt_password.Visible = true;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            UserNumber = txt_UserNumber.Text.Trim();
            UserPassword = txt_password.Text.Trim();
            Update_index = 1;
            DialogResult = DialogResult.Yes;
        }

        private void btnpassword_Click(object sender, EventArgs e)
        {
            //CheckUserNumberForm checkUser = new CheckUserNumberForm();
            //checkUser.ShowDialog();
        }

        private void UserNumberForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString("Amkor K3", new Font("고딕", 30, FontStyle.Bold), Brushes.White, (this.Width / 3) - 100, (this.Height / 5));
            e.Graphics.DrawString("Technology®", new Font("고딕", 30, FontStyle.Bold), new SolidBrush(Color.FromArgb(83, 143, 237)), (this.Width / 3) + 100, (this.Height / 5));
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(72, 85, 93)), new Rectangle(30, (this.Height / 3), (this.Width - 80), (this.Height / 2)));

            txt_UserNumber.Location = new Point((this.Width / 3) + 100, (this.Height / 2));
            txt_UserNumber.BackColor = Color.FromArgb(214, 207, 121);
            txt_UserNumber.ForeColor = Color.White;
            txt_UserNumber.Width = 350;
            
            txt_password.Location = new Point((this.Width / 3) + 100, (this.Height / 2) + 60);
            txt_password.BackColor = Color.FromArgb(214, 207, 121);
            txt_password.ForeColor = Color.White;
            txt_password.Width = 350;

            btnLogin.Location = new Point((this.Width / 2) + 200, (this.Height / 2) + 120);
            btnLogin.LookAndFeel.Style = LookAndFeelStyle.Flat;
            btnLogin.LookAndFeel.UseDefaultLookAndFeel = false;
            btnLogin.Appearance.BackColor = Color.FromArgb(0, 183, 245);
            btnLogin.Appearance.Options.UseBackColor = true;
            btnLogin.ForeColor = Color.White;
            btnLogin.Width = 118;

            C_Radio1.Location = new Point((this.Width / 3) + 100, (this.Height / 2) - 60);
            C_Radio1.CheckedColor = Color.White;
            C_Radio1.BordercheckedColor = Color.FromArgb(3, 186, 251);
            C_Radio1.UnCheckedColor = Color.Black;
            C_Radio1.BorderunCheckedColor = Color.Black;
            C_Radio1.BackColor = Color.FromArgb(72, 85, 93);
            C_Radio1.ForeColor = Color.White;
            C_Radio1.Text = "Sign in Only UserNumber";

            C_Radio2.Location = new Point((this.Width / 3) + 300, (this.Height / 2) - 60);
            C_Radio2.CheckedColor = Color.White;
            C_Radio2.BordercheckedColor = Color.FromArgb(3, 186, 251);
            C_Radio2.UnCheckedColor = Color.Black;
            C_Radio2.BorderunCheckedColor = Color.Black;
            C_Radio2.BackColor = Color.FromArgb(72, 85, 93);
            C_Radio2.ForeColor = Color.White;
            C_Radio2.Text = "Sign in using password";
        }
    }
}
