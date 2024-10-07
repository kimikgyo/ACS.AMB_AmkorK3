
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace INA_ACS_Server.UI
{
    public partial class MiR_Error_PopupForm : Form
    {
        private MainForm mainForm;

        private string reciveValue = string.Empty;

        public MiR_Error_PopupForm(string recValue)
        {
            InitializeComponent();
            this.reciveValue = recValue;
            subFuncMSG_Display(reciveValue);

        }

        void subFuncMSG_Display(string MiR_Error_Text)
        {
            lbl_MiR_Error.Text = MiR_Error_Text;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
            
        }

    }
}
