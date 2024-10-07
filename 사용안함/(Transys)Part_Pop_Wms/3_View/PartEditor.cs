using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
namespace INA_ACS_Server
{
    public partial class PartEditor : Form
    {
        public PartModel EditDataModel;

        public PartEditor()
        {
            InitializeComponent();

        }
        private void Form2_Load(object sender, EventArgs e)
        {
            this.AcceptButton = btn_OK;
            this.CancelButton = btn_Cancel;

            PartUI_Init();
        }

        private void PartUI_Init()
        {
            txt_PartUI_LINE_CD.Text = EditDataModel.LINE_CD;
            txt_PartUI_POST_CD.Text = EditDataModel.POST_CD.ToString();
            txt_PartUI_OUT_Q.Text = EditDataModel.OUT_Q.ToString();
            txt_PartUI_PART_CD.Text = EditDataModel.PART_CD;
            txt_PartUI_PART_NM.Text = EditDataModel.PART_NM;
            cmb_PartUI_COMM_PO.Text = EditDataModel.COMM_PO.ToString();
            txt_PartUI_COMM_ANG.Text = EditDataModel.COMM_ANG.ToString();
            cmb_PartUI_NP_MODE.Text = string.IsNullOrWhiteSpace(EditDataModel.NP_MODE) ? "N" : EditDataModel.NP_MODE;
            txt_PartUI_NP_OUT_Q.Text = EditDataModel.NP_OUT_Q.ToString();
            txt_PartUI_NP_PART_CD.Text = EditDataModel.NP_PART_CD;
            txt_PartUI_NP_PART_NM.Text = EditDataModel.NP_PART_NM;
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            EditPartlist();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void cmb_PartUI_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Chang_Mode = ((ComboBox)sender).Name;
            string name = Chang_Mode.Replace("cmb_PartUI_", "");

            switch (name)
            {
                case "COMM_PO":
                    if (cmb_PartUI_COMM_PO.Text == "Y")
                    {
                        txt_PartUI_COMM_ANG.Enabled = true;
                    }
                    else
                    {
                        txt_PartUI_COMM_ANG.Enabled = false;
                    }

                    break;

                case "NP_MODE":
                    if (cmb_PartUI_NP_MODE.Text == "Y")
                    {
                        txt_PartUI_NP_OUT_Q.Enabled = true;
                        txt_PartUI_NP_PART_CD.Enabled = true;
                        txt_PartUI_NP_PART_NM.Enabled = true;
                    }
                    else
                    {
                        txt_PartUI_NP_OUT_Q.Enabled = false;
                        txt_PartUI_NP_PART_CD.Enabled = false;
                        txt_PartUI_NP_PART_NM.Enabled = false;
                    }
                    break;
            }
        }

        private void EditPartlist()
        {
            int.TryParse(txt_PartUI_POST_CD.Text.Trim(), out int postCD);
            int.TryParse(txt_PartUI_OUT_Q.Text.Trim(), out int outQ1);
            int.TryParse(txt_PartUI_NP_OUT_Q.Text, out int outQ2);


            EditDataModel.LINE_CD = txt_PartUI_LINE_CD.Text.Trim();
            EditDataModel.POST_CD = postCD;
            EditDataModel.COMM_PO = cmb_PartUI_COMM_PO.Text.Trim();
            EditDataModel.COMM_ANG = Convert.ToInt32(txt_PartUI_COMM_ANG.Text.Trim());

            EditDataModel.OUT_Q = outQ1;
            EditDataModel.PART_CD = txt_PartUI_PART_CD.Text.Trim();
            EditDataModel.PART_NM = txt_PartUI_PART_NM.Text.Trim();

            EditDataModel.NP_MODE = cmb_PartUI_NP_MODE.Text.Trim();

            EditDataModel.NP_OUT_Q = outQ2;
            EditDataModel.NP_PART_CD = txt_PartUI_NP_PART_CD.Text.Trim();
            EditDataModel.NP_PART_NM = txt_PartUI_NP_PART_NM.Text.Trim();

            DialogResult = DialogResult.OK;
        }


    }
}
