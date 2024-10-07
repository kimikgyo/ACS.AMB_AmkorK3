using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using log4net;

namespace INA_ACS_Server
{
    public partial class ETNTowerLamp : Form
    {
        private readonly static ILog EventLogger = LogManager.GetLogger("Event"); //Function 실행관련 Log
#if AMT_TEST
        public unsafe bool Tcp_Qu_RW(int iPort, byte* pbIp, byte* pbData) => true;
#else
        [DllImport("Ex64_dllc.dll")]
        public static unsafe extern bool Tcp_Qu_RW(int iPort, byte* pbIp, byte* pbData);
#endif

        public ETNTowerLamp()
        {
            InitializeComponent();
        }


        public void AlarmLampOn()
        {
            button2_Click(this, null);
        }
        public void AlarmLampOff()
        {
            button3_Click(this, null);
        }

        unsafe public void ETNTowerLamp_1_AlarmLampOn()
        {
            const byte D_not = 100;            // Don't care  // Do not change before state
            const byte C_lampoff = 0;
            const byte C_lampon = 1;
            const byte C_lampblink = 2;


            bool b_chk = false;
            int iPort, i = 0;
            string m_str;
            byte* c_pIdata = stackalloc byte[10];
            byte* c_pIpadd = stackalloc byte[6];

            c_pIdata[0] = 1;		// 1-write  0-read
            c_pIdata[1] = 0;
            //sound 25ea model group select   0-4:
            //c_pIdata[1]  = 3;	


            c_pIdata[2] = C_lampon;         // lamp1 RED
            c_pIdata[3] = C_lampblink;		// lamp2 Yellow
            c_pIdata[4] = D_not;		// lamp3 Green
            c_pIdata[5] = C_lampon;			// lamp4 Blue
            c_pIdata[6] = C_lampblink;		// lamp4 White
            c_pIdata[7] = 1;                // so

            c_pIpadd[0] = 10;
            c_pIpadd[1] = 141;
            c_pIpadd[2] = 130;
            c_pIpadd[3] = 30;

            iPort = 5555;

            b_chk = Tcp_Qu_RW(iPort, c_pIpadd, c_pIdata);


            m_str = " ";
            if (b_chk)
            {
                m_str = "  [ETNTowerLamp_1_AlarmLampOn = Success send] ";

            }
            else m_str = "  [ETNTowerLamp_1_AlarmLampOn = Send  Error] ";

            text1.Text = m_str;
            EventLogger.Info($"{m_str}");
        }

        unsafe public void ETNTowerLamp_1_AlarmLampOff()
        {
            const byte D_not = 100;            // Don't care  // Do not change before state
            const byte C_lampoff = 0;
            const byte C_lampon = 1;
            const byte C_lampblink = 2;


            bool b_chk = false;
            int iPort, i = 0;
            string m_str;
            byte* c_pIdata = stackalloc byte[10];
            byte* c_pIpadd = stackalloc byte[6];

            c_pIdata[0] = 1;		// 1-write  0-read
            c_pIdata[1] = 0;
            //sound 25ea model group select   0-4:
            //c_pIdata[1]  = 3;	


            c_pIdata[2] = C_lampoff;        // lamp1 RED
            c_pIdata[3] = C_lampoff;		// lamp2 Yellow
            c_pIdata[4] = C_lampoff;		// lamp3 Green
            c_pIdata[5] = C_lampoff;		// lamp4 Blue
            c_pIdata[6] = C_lampoff;		// lamp4 White
            c_pIdata[7] = 0;                // sound

            c_pIpadd[0] = 10;
            c_pIpadd[1] = 141;
            c_pIpadd[2] = 130;
            c_pIpadd[3] = 30;

            iPort = 5555;

            b_chk = Tcp_Qu_RW(iPort, c_pIpadd, c_pIdata);


            m_str = " ";
            if (b_chk)
            {
                m_str = "  [ETNTowerLamp_1_AlarmLampOff = All off] ";
            }
            else m_str = " [ETNTowerLamp_1_AlarmLampOff = Send  Error] ";

            text1.Text = m_str;
            EventLogger.Info($"{m_str}");
        }

        unsafe public void ETNTowerLamp_2_AlarmLampOn()
        {
            const byte D_not = 100;            // Don't care  // Do not change before state
            const byte C_lampoff = 0;
            const byte C_lampon = 1;
            const byte C_lampblink = 2;


            bool b_chk = false;
            int iPort, i = 0;
            string m_str;
            byte* c_pIdata = stackalloc byte[10];
            byte* c_pIpadd = stackalloc byte[6];

            c_pIdata[0] = 1;		// 1-write  0-read
            c_pIdata[1] = 0;
            //sound 25ea model group select   0-4:
            //c_pIdata[1]  = 3;	

            c_pIdata[2] = C_lampon;         // lamp1 RED
            c_pIdata[3] = C_lampblink;		// lamp2 Yellow
            c_pIdata[4] = D_not;		// lamp3 Green
            c_pIdata[5] = C_lampon;			// lamp4 Blue
            c_pIdata[6] = C_lampblink;		// lamp4 White
            c_pIdata[7] = 1;                // Sound 1~5

            //Ip 주소
            c_pIpadd[0] = 10;
            c_pIpadd[1] = 141;
            c_pIpadd[2] = 141;
            c_pIpadd[3] = 34;

            //Port
            iPort = 5555;


            b_chk = Tcp_Qu_RW(iPort, c_pIpadd, c_pIdata);

            m_str = " ";
            if (b_chk)
            {
                m_str = "  [ETNTowerLamp_2_AlarmLampOn = Success send] ";
            }
            else m_str = "  [ETNTowerLamp_2_AlarmLampOn = Send  Error] ";


            text1.Text = m_str;
            EventLogger.Info($"{m_str}");
        }

        unsafe public void ETNTowerLamp_2_AlarmLampOff()
        {
            const byte D_not = 100;            // Don't care  // Do not change before state
            const byte C_lampoff = 0;
            const byte C_lampon = 1;
            const byte C_lampblink = 2;


            bool b_chk = false;
            int iPort, i = 0;
            string m_str;
            byte* c_pIdata = stackalloc byte[10];
            byte* c_pIpadd = stackalloc byte[6];

            c_pIdata[0] = 1;		// 1-write  0-read
            c_pIdata[1] = 0;
            //sound 25ea model group select   0-4:
            //c_pIdata[1]  = 3;	


            c_pIdata[2] = C_lampoff;        // lamp1 RED
            c_pIdata[3] = C_lampoff;		// lamp2 Yellow
            c_pIdata[4] = C_lampoff;		// lamp3 Green
            c_pIdata[5] = C_lampoff;		// lamp4 Blue
            c_pIdata[6] = C_lampoff;		// lamp4 White
            c_pIdata[7] = 0;				// sound

            //Ip 주소
            c_pIpadd[0] = 10;
            c_pIpadd[1] = 141;
            c_pIpadd[2] = 141;
            c_pIpadd[3] = 34;

            //Port
            iPort = 5555;


            b_chk = Tcp_Qu_RW(iPort, c_pIpadd, c_pIdata);
            m_str = " ";

            if (b_chk)
            {
                m_str = "  [ETNTowerLamp_2_AlarmLampOff = All off] ";
            }
            else m_str = " [ETNTowerLamp_2_AlarmLampOff = Send  Error] ";
            text1.Text = m_str;
            EventLogger.Info($"{m_str}");
        }

        unsafe public void ETNTowerLamp_3_AlarmLampOn()
        {
            const byte D_not = 100;            // Don't care  // Do not change before state
            const byte C_lampoff = 0;
            const byte C_lampon = 1;
            const byte C_lampblink = 2;


            bool b_chk = false;
            int iPort, i = 0;
            string m_str;
            byte* c_pIdata = stackalloc byte[10];
            byte* c_pIpadd = stackalloc byte[6];

            c_pIdata[0] = 1;		// 1-write  0-read
            c_pIdata[1] = 0;
            //sound 25ea model group select   0-4:
            //c_pIdata[1]  = 3;	

            c_pIdata[2] = C_lampon;         // lamp1 RED
            c_pIdata[3] = C_lampblink;		// lamp2 Yellow
            c_pIdata[4] = D_not;		// lamp3 Green
            c_pIdata[5] = C_lampon;			// lamp4 Blue
            c_pIdata[6] = C_lampblink;		// lamp4 White
            c_pIdata[7] = 1;				// Sound 1~5

            //Ip주소
            c_pIpadd[0] = 10;
            c_pIpadd[1] = 141;
            c_pIpadd[2] = 141;
            c_pIpadd[3] = 65;

            //Port
            iPort = 5555;

            b_chk = Tcp_Qu_RW(iPort, c_pIpadd, c_pIdata);


            m_str = " ";
            if (b_chk)
            {
                m_str = "  [ETNTowerLamp_3_AlarmLampOn = Success send] ";

            }
            else m_str = "  [ETNTowerLamp_3_AlarmLampOn = Send  Error] ";

            text1.Text = m_str;
            EventLogger.Info($"{m_str}");


        }
        unsafe public void ETNTowerLamp_3_AlarmLampOff()
        {
            const byte D_not = 100;            // Don't care  // Do not change before state
            const byte C_lampoff = 0;
            const byte C_lampon = 1;
            const byte C_lampblink = 2;


            bool b_chk = false;
            int iPort, i = 0;
            string m_str;
            byte* c_pIdata = stackalloc byte[10];
            byte* c_pIpadd = stackalloc byte[6];

            c_pIdata[0] = 1;		// 1-write  0-read
            c_pIdata[1] = 0;
            //sound 25ea model group select   0-4:
            //c_pIdata[1]  = 3;	


            c_pIdata[2] = C_lampoff;        // lamp1 RED
            c_pIdata[3] = C_lampoff;		// lamp2 Yellow
            c_pIdata[4] = C_lampoff;		// lamp3 Green
            c_pIdata[5] = C_lampoff;		// lamp4 Blue
            c_pIdata[6] = C_lampoff;		// lamp4 White
            c_pIdata[7] = 0;				// sound

            //Ip주소
            c_pIpadd[0] = 10;
            c_pIpadd[1] = 141;
            c_pIpadd[2] = 141;
            c_pIpadd[3] = 65;

            //Port
            iPort = 5555;

            b_chk = Tcp_Qu_RW(iPort, c_pIpadd, c_pIdata);
            m_str = " ";
            if (b_chk)
            {
                m_str = "  [ETNTowerLamp_3_AlarmLampOff = All off] ";

            }
            else m_str = " [ETNTowerLamp_3_AlarmLampOff = Send  Error] ";
            text1.Text = m_str;
            EventLogger.Info($"{m_str}");

        }

        unsafe public void ETNTowerLamp_1_AlarmLampStatus()
        {
            bool b_chk = false;
            int iPort, i = 0;
            string m_str;
            byte* c_pIdata = stackalloc byte[10];
            byte* c_pIpadd = stackalloc byte[6];

            c_pIdata[0] = 0;        // 1-write  0-read

            c_pIpadd[0] = 192;
            c_pIpadd[1] = 168;
            c_pIpadd[2] = 1;
            c_pIpadd[3] = 111;

            iPort = 5555;

            b_chk = Tcp_Qu_RW(iPort, c_pIpadd, c_pIdata);

            m_str = " ";
            if (b_chk)
            {

                if (c_pIdata[2] == 0) m_str += " R-OFF/";
                else if (c_pIdata[2] == 1) m_str += " R-ON/";
                else m_str += " R-BLINK/";

                if (c_pIdata[7] == 0) m_str += " Sound-OFF/";
                else m_str += " Sound-ON/";

            }
            else m_str = "  [Send  Error] ";
        }

        unsafe public void ETNTowerLamp_2_AlarmLampStatus()
        {
            bool b_chk = false;
            int iPort, i = 0;
            string m_str;
            byte* c_pIdata = stackalloc byte[10];
            byte* c_pIpadd = stackalloc byte[6];

            c_pIdata[0] = 0;        // 1-write  0-read

            c_pIpadd[0] = 192;
            c_pIpadd[1] = 168;
            c_pIpadd[2] = 1;
            c_pIpadd[3] = 112;


            iPort = 5555;

            b_chk = Tcp_Qu_RW(iPort, c_pIpadd, c_pIdata);


            m_str = " ";
            if (b_chk)
            {

                if (c_pIdata[2] == 0) m_str += " R-OFF/";
                else if (c_pIdata[2] == 1) m_str += " R-ON/";
                else m_str += " R-BLINK/";

                if (c_pIdata[7] == 0) m_str += " Sound-OFF/";
                else m_str += " Sound-ON/";


            }
            else m_str = "  [Send  Error] ";

            text1.Text = m_str;
        }

        unsafe public void ETNTowerLamp_3_AlarmLampStatus()
        {
            bool b_chk = false;
            int iPort, i = 0;
            string m_str;
            byte* c_pIdata = stackalloc byte[10];
            byte* c_pIpadd = stackalloc byte[6];

            c_pIdata[0] = 0;        // 1-write  0-read

            c_pIpadd[0] = 192;
            c_pIpadd[1] = 168;
            c_pIpadd[2] = 1;
            c_pIpadd[3] = 113;


            iPort = 5555;

            b_chk = Tcp_Qu_RW(iPort, c_pIpadd, c_pIdata);
            m_str = " ";
            if (b_chk)
            {
                if (c_pIdata[2] == 0) m_str += " R-OFF/";
                else if (c_pIdata[2] == 1) m_str += " R-ON/";
                else m_str += " R-BLINK/";

                if (c_pIdata[7] == 0) m_str += " Sound-OFF/";
                else m_str += " Sound-ON/";

            }
            else m_str = "  [Send  Error] ";

            text1.Text = m_str;
        }


        unsafe private void button1_Click(object sender, EventArgs e)   //정보 읽어오기
        {

            bool b_chk = false;
            int iPort, i = 0;
            string m_str;
            byte* c_pIdata = stackalloc byte[10];
            byte* c_pIpadd = stackalloc byte[6];

            c_pIdata[0] = 0;        // 1-write  0-read

            c_pIpadd[0] = 192;
            c_pIpadd[1] = 168;
            c_pIpadd[2] = 1;
            c_pIpadd[3] = 102;


            iPort = 5555;

            b_chk = Tcp_Qu_RW(iPort, c_pIpadd, c_pIdata);


            m_str = " ";
            if (b_chk)
            {

                if (c_pIdata[2] == 0) m_str += " R-OFF/";
                else if (c_pIdata[2] == 1) m_str += " R-ON/";
                else m_str += " R-BLINK/";

                if (c_pIdata[3] == 0) m_str += " Y-OFF/";
                else if (c_pIdata[3] == 1) m_str += " Y-ON/";
                else m_str += " Y-BLINK/";

                if (c_pIdata[7] == 0) m_str += " Sound-OFF/";
                else m_str += " Sound-ON/";

            }
            else m_str = "  [Send  Error] ";

            text1.Text = m_str;

        }

        unsafe private void button2_Click(object sender, EventArgs e)   //On
        {
            const byte D_not = 100;            // Don't care  // Do not change before state
            const byte C_lampoff = 0;
            const byte C_lampon = 1;
            const byte C_lampblink = 2;


            bool b_chk = false;
            int iPort, i = 0;
            string m_str;
            byte* c_pIdata = stackalloc byte[10];
            byte* c_pIpadd = stackalloc byte[6];

            c_pIdata[0] = 1;		// 1-write  0-read
            c_pIdata[1] = 0;
            //sound 25ea model group select   0-4:
            //c_pIdata[1]  = 3;	

            c_pIdata[2] = C_lampon;         // lamp1 RED
            c_pIdata[3] = C_lampblink;		// lamp2 Yellow
            c_pIdata[4] = D_not;		// lamp3 Green
            c_pIdata[5] = C_lampon;			// lamp4 Blue
            c_pIdata[6] = C_lampblink;		// lamp4 White
            c_pIdata[7] = 1;				// Sound 1~5

            c_pIpadd[0] = 192;
            c_pIpadd[1] = 168;
            c_pIpadd[2] = 1;
            c_pIpadd[3] = 102;

            iPort = 5555;

            b_chk = Tcp_Qu_RW(iPort, c_pIpadd, c_pIdata);


            m_str = " ";
            if (b_chk)
            {
                m_str = "  [Success send] ";

            }
            else m_str = "  [Send  Error] ";

            text1.Text = m_str;

        }

        unsafe private void button3_Click(object sender, EventArgs e)   //Off
        {
            const byte D_not = 100;            // Don't care  // Do not change before state
            const byte C_lampoff = 0;
            const byte C_lampon = 1;
            const byte C_lampblink = 2;


            bool b_chk = false;
            int iPort, i = 0;
            string m_str;
            byte* c_pIdata = stackalloc byte[10];
            byte* c_pIpadd = stackalloc byte[6];

            c_pIdata[0] = 1;		// 1-write  0-read
            c_pIdata[1] = 0;
            //sound 25ea model group select   0-4:
            //c_pIdata[1]  = 3;	


            c_pIdata[2] = C_lampoff;        // lamp1 RED
            c_pIdata[3] = C_lampoff;		// lamp2 Yellow
            c_pIdata[4] = C_lampoff;		// lamp3 Green
            c_pIdata[5] = C_lampoff;		// lamp4 Blue
            c_pIdata[6] = C_lampoff;		// lamp4 White
            c_pIdata[7] = 0;				// sound

            c_pIpadd[0] = 192;
            c_pIpadd[1] = 168;
            c_pIpadd[2] = 1;
            c_pIpadd[3] = 102;

            iPort = 5555;

            b_chk = Tcp_Qu_RW(iPort, c_pIpadd, c_pIdata);


            m_str = " ";
            if (b_chk)
            {
                m_str = "  [All off] ";

            }
            else m_str = " [Send  Error] ";

            text1.Text = m_str;


        }


    }
}
