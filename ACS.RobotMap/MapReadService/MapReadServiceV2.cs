using ACS.RobotApi;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.RobotMap
{
    public class DummyMapReadService : IMapReadService
    {
        public string MapGuid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string MapName { get; set; }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
