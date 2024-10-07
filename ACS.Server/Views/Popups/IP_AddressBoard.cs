using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace INA_ACS_Server.UI
{
    public partial class IP_AddressBoard : Form
    {
        string return_Value = string.Empty;

        public string Return_Value
        {
            get { return return_Value; }
            set { return_Value = value; }
        }


        public IP_AddressBoard()
        {
            InitializeComponent();
        }

        public IP_AddressBoard(string textbox_Value)
        {
            InitializeComponent();
            txt_Inputtext.Text = textbox_Value;
        }

        private void UIFunc_KEYPRESS(object sender, EventArgs e)
        {
            string name = ((Button)sender).Name;

            switch (name)
            {
                case "btn_No1":
                    txt_Inputtext.Text = txt_Inputtext.Text + "1";
                    break;
                case "btn_No2":
                    txt_Inputtext.Text = txt_Inputtext.Text + "2";
                    break;
                case "btn_No3":
                    txt_Inputtext.Text = txt_Inputtext.Text + "3";
                    break;
                case "btn_No4":
                    txt_Inputtext.Text = txt_Inputtext.Text + "4";
                    break;
                case "btn_No5":
                    txt_Inputtext.Text = txt_Inputtext.Text + "5";
                    break;
                case "btn_No6":
                    txt_Inputtext.Text = txt_Inputtext.Text + "6";
                    break;
                case "btn_No7":
                    txt_Inputtext.Text = txt_Inputtext.Text + "7";
                    break;
                case "btn_No8":
                    txt_Inputtext.Text = txt_Inputtext.Text + "8";
                    break;
                case "btn_No9":
                    txt_Inputtext.Text = txt_Inputtext.Text + "9";
                    break;
                case "btn_No0":
                    txt_Inputtext.Text = txt_Inputtext.Text + "0";
                    break;


                case "btn_Q":
                    txt_Inputtext.Text = txt_Inputtext.Text + "Q";
                    break;
                case "btn_W":
                    txt_Inputtext.Text = txt_Inputtext.Text + "W";
                    break;
                case "btn_E":
                    txt_Inputtext.Text = txt_Inputtext.Text + "E";
                    break;
                case "btn_R":
                    txt_Inputtext.Text = txt_Inputtext.Text + "R";
                    break;
                case "btn_T":
                    txt_Inputtext.Text = txt_Inputtext.Text + "T";
                    break;
                case "btn_Y":
                    txt_Inputtext.Text = txt_Inputtext.Text + "Y";
                    break;
                case "btn_U":
                    txt_Inputtext.Text = txt_Inputtext.Text + "U";
                    break;
                case "btn_I":
                    txt_Inputtext.Text = txt_Inputtext.Text + "I";
                    break;
                case "btn_O":
                    txt_Inputtext.Text = txt_Inputtext.Text + "O";
                    break;
                case "btn_P":
                    txt_Inputtext.Text = txt_Inputtext.Text + "P";
                    break;


                case "btn_A":
                    txt_Inputtext.Text = txt_Inputtext.Text + "A";
                    break;
                case "btn_S":
                    txt_Inputtext.Text = txt_Inputtext.Text + "S";
                    break;
                case "btn_D":
                    txt_Inputtext.Text = txt_Inputtext.Text + "D";
                    break;
                case "btn_F":
                    txt_Inputtext.Text = txt_Inputtext.Text + "F";
                    break;
                case "btn_G":
                    txt_Inputtext.Text = txt_Inputtext.Text + "G";
                    break;
                case "btn_H":
                    txt_Inputtext.Text = txt_Inputtext.Text + "H";
                    break;
                case "btn_J":
                    txt_Inputtext.Text = txt_Inputtext.Text + "J";
                    break;
                case "btn_K":
                    txt_Inputtext.Text = txt_Inputtext.Text + "K";
                    break;
                case "btn_L":
                    txt_Inputtext.Text = txt_Inputtext.Text + "L";
                    break;


                case "btn_Z":
                    txt_Inputtext.Text = txt_Inputtext.Text + "Z";
                    break;
                case "btn_X":
                    txt_Inputtext.Text = txt_Inputtext.Text + "X";
                    break;
                case "btn_C":
                    txt_Inputtext.Text = txt_Inputtext.Text + "C";
                    break;
                case "btn_V":
                    txt_Inputtext.Text = txt_Inputtext.Text + "V";
                    break;
                case "btn_B":
                    txt_Inputtext.Text = txt_Inputtext.Text + "B";
                    break;
                case "btn_N":
                    txt_Inputtext.Text = txt_Inputtext.Text + "N";
                    break;
                case "btn_M":
                    txt_Inputtext.Text = txt_Inputtext.Text + "M";
                    break;

                case "btn_UNDERBAR":
                    txt_Inputtext.Text = txt_Inputtext.Text + "_";
                    break;

                case "btn_DOT":
                    txt_Inputtext.Text = txt_Inputtext.Text + ".";
                    break;

                case "btn_Backspace":
                    int lenght = txt_Inputtext.TextLength;
                    if(lenght >0) txt_Inputtext.Text = txt_Inputtext.Text.Remove(lenght - 1);
                    break;

                case "btn_ENTER":
                    return_Value = txt_Inputtext.Text;
                    DialogResult = DialogResult.OK;
                    break;
            }
        }

        private void txt_Inputtext_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txt_Inputtext.Text = "";
        }


       
    }
}
