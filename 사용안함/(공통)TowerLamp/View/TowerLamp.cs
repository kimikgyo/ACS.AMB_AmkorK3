using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace INA_ACS_Server
{
    public partial class TowerLamp : Form
    {
        [DllImport("Ux64_dllc.dll")]
        public static extern void Usb_Qu_Open();
        [DllImport("Ux64_dllc.dll")]
        public static extern void Usb_Qu_Close();
        [DllImport("Ux64_dllc.dll")]
        public static unsafe extern bool Usb_Qu_write(byte Q_index, byte Q_type, byte* pQ_data);
        [DllImport("Ux64_dllc.dll")]
        public static extern int Usb_Qu_Getstate();


        const byte C_lampoff = 0;
        const byte C_lampon = 1;
        const byte C_lampblink = 2;
        const byte C_D_not = 100;  //  // Don't care  // Do not change before state



        public TowerLamp()
        {
            InitializeComponent();
        }

        public void AlarmLampOn()
        {
            button4_Click(this, null);
        }

        public void AlarmLampOff()
        {
            button5_Click(this, null);
        }

        public void AllLampOff()
        {
            button3_Click(this, null);
        }




        unsafe private void button2_Click(object sender, EventArgs e)
        {
            bool bchk = false;
            byte* bbb = stackalloc byte[6];

            bbb[0] = C_lampon;
            bbb[1] = C_lampblink;
            bbb[2] = C_D_not;
            bbb[3] = C_lampon;
            bbb[4] = C_lampblink;

            bbb[5] = 3; // sound select


            bchk = Usb_Qu_write(0, 0, bbb);
            if (bchk) text1.Text = "succes write";
            else text1.Text = "write error";
        }

        unsafe private void button3_Click(object sender, EventArgs e)
        {
            bool bchk = false;
            byte* bbb = stackalloc byte[6];

            bbb[0] = C_lampoff;
            bbb[1] = C_lampoff;
            bbb[2] = C_lampoff;
            bbb[3] = C_lampoff;
            bbb[4] = C_lampoff;

            bbb[5] = 0; // sound off


            bchk = Usb_Qu_write(0, 0, bbb);
            if (bchk) text1.Text = "All off";
            else text1.Text = "write error";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i;
            string m_str;

            i = Usb_Qu_Getstate();
            m_str = " Read Connect Usb  -->  ";

            if ((i & 0x01) == 1) m_str += "Index0(0), ";
            else m_str += "Index0(X), ";
            if ((i & 0x02) == 2) m_str += "Index1(O), ";
            else m_str += "Index1(X), ";
            if ((i & 0x04) == 4) m_str += "Index2(O), ";
            else m_str += "Index2(X), ";
            if ((i & 0x08) == 8) m_str += "Index3(O), ";
            else m_str += "Index3(X), ";

            text1.Text = m_str;
        }

        unsafe private void button4_Click(object sender, EventArgs e)
        {
            bool bchk = false;
            byte* bbb = stackalloc byte[6];

            bbb[0] = C_lampon;
            bbb[1] = C_lampblink;
            bbb[2] = C_D_not;
            bbb[3] = C_lampon;
            bbb[4] = C_lampblink;

            bbb[5] = 3; // sound select


            bchk = Usb_Qu_write(0, 0, bbb);
            if (bchk) text1.Text = "succes write";
            else text1.Text = "write error";
        }

        unsafe private void button5_Click(object sender, EventArgs e)
        {
            bool bchk = false;
            byte* bbb = stackalloc byte[6];

            bbb[0] = C_lampon;
            bbb[1] = C_lampoff;
            bbb[2] = C_D_not;
            bbb[3] = C_lampon;
            bbb[4] = C_lampblink;

            bbb[5] = 0; // sound off


            bchk = Usb_Qu_write(0, 0, bbb);
            if (bchk) text1.Text = "succes write";
            else text1.Text = "write error";
        }
    }
}
