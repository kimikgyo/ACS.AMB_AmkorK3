using System;
using System.Windows.Forms;



namespace INA_ACS_Server.UI
{
    public partial class InputTextBoxForm : Form
    {
        public string InputValue = string.Empty;

        public InputTextBoxForm(string initialValue)
        {
            InitializeComponent();
            this.InputValue = initialValue;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.txtInput.Text = this.InputValue;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            InputValue = txtInput.Text.Trim();
            Close();
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
