using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace INA_ACS_Server.UI
{
    public partial class NumPadForm : Form
    {
        private string drawNo = string.Empty;
        private string inputValue = string.Empty;
        private string reciveValue = string.Empty;
        private double Min_value = 0;
        private double Max_value = 0;
        private string Tag_name = string.Empty;
        private string range_type = string.Empty;

        private double Limit00 = 0;
        private double Limit01 = 0;
        private double Restart00 = 0;
        private double Restart01 = 0;
        private int C_Value_index = 0;

        public NumPadForm(string recValue, string Tag_name, string Range_type)
        {
            InitializeComponent();
            reciveValue = recValue;
            this.Tag_name = Tag_name;
            range_type = Range_type;

            SubFunc_LimitValueDefinebyRangType(Range_type);
        }

        public NumPadForm(string recValue, string Tag_name, string sMin, string sMax)
        {
            InitializeComponent();
            this.reciveValue = recValue;
            this.Tag_name = Tag_name;

            Min_value = Math.Round(double.Parse(sMin), 1);
            Max_value = Math.Round(double.Parse(sMax), 1);

            txt_Min.Text = Min_value.ToString();
            txt_Max.Text = Max_value.ToString();
        }

        public string InputValue
        {
            get { return inputValue; }
            set { inputValue = value; }
        }

        private void SubFunc_LimitValueDefinebyRangType(string range_type)
        {
            switch (range_type)
            {
                case "RT1":
                    Min_value = 0.1;
                    Max_value = 10;
                    break;

                case "RT2":
                    Min_value = 0;
                    Max_value = 10000;
                    break;

                case "RT3":
                    Min_value = 1;
                    Max_value = 5000;
                    break;

                case "RT4":
                    Min_value = 0;
                    Max_value = 35000;
                    break;

                case "RT5":
                    Min_value = 0;
                    Max_value = 600;
                    break;

                case "RT6":
                    Min_value = -10000;
                    Max_value = 150000;
                    break;

                case "RT7":
                    Min_value = 0;
                    Max_value = 360;
                    break;

                case "RT8":
                    Min_value = 0;
                    Max_value = 3950000;
                    break;

                case "RT9":
                    Min_value = 0;
                    Max_value = 5;
                    break;

                case "RT10":
                    Min_value = 0;
                    Max_value = 150000;
                    break;

                case "RT11":
                    Min_value = -5;
                    Max_value = 5;
                    break;

                case "RT12":
                    Min_value = 0;
                    Max_value = 60000;
                    break;

                case "RT13":
                    Min_value = 0;
                    Max_value = 1200;
                    break;

                case "RT14":
                    Min_value = 0;
                    Max_value = 50000;
                    break;

                case "RT15":
                    Min_value = 0;
                    Max_value = 100;
                    break;

                case "RT16":
                    Min_value = 0;
                    Max_value = 500000;
                    break;

                case "RT17":
                    Min_value = 0;
                    Max_value = 200;
                    break;

                case "RT18":
                    Min_value = 0;
                    Max_value = 20;
                    break;

                case "RT19":
                    Min_value = 0;
                    Max_value = 59;
                    break;

                case "RT20":
                    Min_value = 1;
                    Max_value = 5;
                    break;

                case "RT21":
                    Min_value = 0;
                    Max_value = 200;
                    break;

                case "RT26":
                    Min_value = 2;
                    Max_value = 3;
                    break;

                case "RT27":
                    Min_value = -10000;
                    Max_value = 10000;
                    break;

                case "RT28":
                    Min_value = 0;
                    Max_value = 1000;
                    break;

                case "RT29":
                    Min_value = 1;
                    Max_value = 3;
                    break;

                case "RT30":
                    Min_value = 1;
                    Max_value = 8;
                    break;

                case "RT31":
                    Min_value = 0;
                    Max_value = 100;
                    break;

                //0~1440 1일 분으로 계산하였을때
                case "RT32":
                    Min_value = 0;
                    Max_value = 1440;
                    break;

                case "RT33":
                    Min_value = 0;
                    Max_value = 1;
                    break;

                default:
                    Min_value = -150000;
                    Max_value = 150000;
                    break;
            }

            txt_Min.Text = Min_value.ToString();
            txt_Max.Text = Max_value.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            inputValue = string.Empty;
            Close();
        }

        void KeyClick(string strKeyClick)
        {
            switch (strKeyClick)
            {
                default:
                    drawNo += strKeyClick;
                    break;
            }

            txtValue.Text = drawNo;
        }

        private void btnSet_Click(object sender, EventArgs e)
        {

            if (txtValue.Text.Equals(".") || txtValue.Text.Contains("%")) return;
            if (txtValue.Text.StartsWith("."))
            {
                MessageBox.Show("입력된 값을 확인하십시오!");
                return;
            }
            double return_value = 0;

            inputValue = txtValue.Text.Trim();
            if (txtValue.Text.Length == 0) inputValue = "0";
            if (txtGiho.Text.Equals("-")) inputValue = "-" + inputValue;

            return_value = double.Parse(inputValue);

            if (return_value >= Min_value && return_value <= Max_value)
            {
                if (subCheckException(return_value)) DialogResult = DialogResult.OK;
                else MessageBox.Show("값이 입력되지 않았습니다.");
            }
            else
            {
                MessageBox.Show("입력범위를 초과하였습니다!!");
            }
        }

        //예외 처리를 하기 위한 부분
        private bool subCheckException(double value)
        {
            double Restart_value = 0;
            double Limit_value = 0;
            double Temp_Value = value;


            if (C_Value_index == 0) return true; // 일반 적인 경우에는 참 값을 반환 한다.
            else
            {
                switch (C_Value_index)
                {
                    case 1:
                        Limit_value = Temp_Value * Math.Pow(10, Limit01);
                        Restart_value = Restart00 * Math.Pow(10, Restart01);
                        break;
                    case 2:
                        Limit_value = Limit00 * Math.Pow(10, Temp_Value);
                        Restart_value = Restart00 * Math.Pow(10, Restart01);
                        break;
                    case 3:
                        Limit_value = Limit00 * Math.Pow(10, Limit01);
                        Restart_value = Temp_Value * Math.Pow(10, Restart01);
                        break;
                    case 4:
                        Limit_value = Limit00 * Math.Pow(10, Limit01);
                        Restart_value = Restart00 * Math.Pow(10, Temp_Value);
                        break;
                }

                // 일반적으로 값이 올바르면 참값을 리턴한다.
                if (Restart_value < Limit_value) return true;
                else return false;
            }
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            KeyClick("7");
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            KeyClick("8");
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            KeyClick("9");
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            KeyClick("4");
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            KeyClick("5");
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            KeyClick("6");
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            KeyClick("1");
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            KeyClick("2");
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            KeyClick("3");
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            KeyClick("0");
        }

        private void btnPoint_Click(object sender, EventArgs e)
        {
            if (drawNo.Contains(".")) MessageBox.Show("소수점이 입력되어있습니다.");
            else KeyClick(".");
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            int x, y;
            x = y = drawNo.Length;
            x -= 1;
            y -= 2;

            if (drawNo.Length > 0)
            {
                if (x == 0)
                    txtValue.Text = drawNo = string.Empty;
                else
                {
                    if (drawNo[y] == '.')
                        txtValue.Text = drawNo = drawNo.Remove(y, 2);
                    else
                        txtValue.Text = drawNo = drawNo.Remove(x, 1);
                }
            }
        }

        private void NumpadForm_Load(object sender, EventArgs e)
        {
            txtValue.Text = reciveValue;
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            txtGiho.Text = "-";
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            if (txtGiho.Text.Equals("+")) txtGiho.Text = "-";
            else txtGiho.Text = "+";
        }
    }
}
