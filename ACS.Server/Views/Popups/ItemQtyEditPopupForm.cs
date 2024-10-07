using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INA_ACS_Server.Views.Popups
{
    public partial class ItemQtyEditPopupForm : Form
    {
        public int ReturnValue { get; set; }

        public ItemQtyEditPopupForm(string callName, string itemCD, string itemNM, int itemQtyAvail, int itemQtyOut)
        {
            InitializeComponent();
            this.Text = DateTime.Now.ToString();
            txt_CallName.Text = callName;
            txt_ItemCD.Text = $"{itemCD} ({itemNM})";
            txt_ItemQtyAvail.Text = itemQtyAvail.ToString();
            txt_ItemOutQty.Text = itemQtyOut.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int.TryParse(txt_ItemQtyAvail.Text.Trim(), out int allQty);
            int.TryParse(txt_ItemOutQty.Text.Trim(), out int value);

            if (value >= 0 && value <= allQty)
            {
                ReturnValue = value;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("invalid input value.");
                DialogResult = DialogResult.None;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
