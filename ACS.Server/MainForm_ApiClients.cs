using ACS.RobotApi;
using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace INA_ACS_Server
{
    public partial class MainForm
    {
        private readonly static ILog ApiLogger = LogManager.GetLogger("ApiEvent");

        private IFleetApi _fleetApi;
        private List<IMirApi> _mirApiList;


        private IFleetApi InitFleetApiClient(ILog logger)
        {
            string ip = ConfigData.sFleet_IP_Address_SV;
            //double timeoutMS = double.Parse(ConfigData.sFleet_ResponseTime) * 1000;
            double timeoutMS = 3000;

           IFleetApi client = new FleetApi(logger, ip, timeoutMS);
            return client;
        }


        private List<IMirApi> InitMirApiClients(ILog logger)
        {
            var newApiClients = new List<IMirApi>();
            var robots = uow.Robots.GetAll();

            for (int i = 0; i < robots.Count; i++)
            {

                //Config 데이터 없을경우 에러발생됨
                if (string.IsNullOrWhiteSpace(ConfigData.RobotIPAddress[i])) ConfigData.RobotIPAddress[i] = "localhost:5000";

                string ip = ConfigData.RobotIPAddress[i];
                //double timeoutMS =  Double.Parse(ConfigData.sFleet_ResponseTime) * 1000;
                double timeoutMS = 2000;

                IMirApi client = new MirApi(logger, ip, timeoutMS);
                newApiClients.Add(client);

                // 로봇 객체에 정보 입력해준다
                robots[i].RobotIp = ip;     // config file에서 읽어들인 ip로 할당한다
                robots[i].Api = client;     // robot api 통신하는 객체

                // DB 로봇 정보 Table에 IP 업데이트해준다
                uow.Robots.UpdateIpAddress(robots[i].Id, ip);
            }

            // robot API write 금지
            //AcceptFilterUtility.SetAcceptFilterFunction(() => false); // 데모 모드에서는 write API 는 무조건 null 리턴한다
            //MessageBox.Show("MIRDEMO API write 금지 모드\n\n쓰기API는 항상 NULL리턴함", "ACS", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            return newApiClients;
        }

    }
}
