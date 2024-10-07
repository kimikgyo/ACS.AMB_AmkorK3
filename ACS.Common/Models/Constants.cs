using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public static class Constants
    {
        public static char SensorSendSTX = (char)0xA2;
        public static char SensorRecvDA = (char)0xFE;
        public static char SensorRecvSA = (char)0x00;
        public static char SensorRecvFCMD = (char)0xA3;
        public static char STX = (char)0x02;
        public static char ETX = (char)0x03;
        public static string Header = "<01#";
        public static string CR = "\r";  //0x0D

    }
}
