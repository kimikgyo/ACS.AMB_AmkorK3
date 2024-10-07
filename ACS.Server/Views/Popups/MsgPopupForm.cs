using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace INA_ACS_Server.UI
{
    public partial class MsgPopupForm : Form
    {
        private string reciveValue = string.Empty;

        public MsgPopupForm(string recValue)
        {
            InitializeComponent();
            this.reciveValue = recValue;
            subFuncMSG_Display(reciveValue);
        }

        void subFuncMSG_Display(string MSG_Text)
        {
            lbl_text.Text = MSG_Text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
