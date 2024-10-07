using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using ACS.RobotMap;

namespace INA_ACS_Server
{
    public partial class MapMonitoring : Form
    {
        private float mapScale = 0.9f;
        private Point mouseFirstLocation = new Point(0, 0);
        private Point mouseMoveOffset = new Point(0, 0);
        private MapReadDtoQueue<MapReadDto> _queue;
        private List<IMapReadService> _readServices;
        private readonly MainForm main;
        private readonly IUnitOfWork uow;

        public MapMonitoring(MainForm mainForm, UnitOfWork uow, MapReadDtoQueue<MapReadDto> queue, List<IMapReadService> readServices)
        {
            InitializeComponent();

            this.main = mainForm;
            this.uow = uow;

            _queue = queue;
            _readServices = readServices;

            this.FormBorderStyle = FormBorderStyle.None; //윈도우(상단) 테두리 제거 source code
            this.BackColor = main.skinColor;

            Init();
        }


        private void MapMonitoring_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing) // 사용자가 ALT-F4 누르거나 x 버튼 눌러서 창을 닫으려 할때
                e.Cancel = true;
        }

        // 화면 진입시 초기화
        public void Init()
        {
            ConfigData.FleetPositions = uow.FleetPositions.DBGetAll();
            var DBMap = uow.FloorMapIDConfigs.GetAll();

            Task task2 = Task.Run(async () =>
            {
                while (true)
                {
                    while (_queue.TryDequeue(out MapReadDto item))
                    {
                        if (_readServices.Count >= 1)
                        {
                            if (_readServices[0].MapGuid == item.Map.Guid)
                            {
                                ucMapView1.Map = item.Map;
                                ucMapView1.RobotInfoList = item.RobotInfo;

                                var MapData = DBMap.FirstOrDefault(x => x.MapID == item.Map.Guid);
                                using (var ms = new System.IO.MemoryStream())
                                {
                                    byte[] mapDecodedBytes = Convert.FromBase64String(MapData.MapImageData);  //Fleet Var 3.0사용
                                                                                                         //byte[] mapDecodedBytes = Convert.FromBase64String(newMap.map);  //Fleet Var 2.0사용
                                    ms.Write(mapDecodedBytes, 0, mapDecodedBytes.Length);
                                    ucMapView1.mapImageFromDB = System.Drawing.Image.FromStream(ms);
                                }
                            }
                        }

                        if (_readServices.Count >= 2)
                        {
                            if (_readServices[1].MapGuid == item.Map.Guid)
                            {
                                ucMapView2.Map = item.Map;
                                ucMapView2.RobotInfoList = item.RobotInfo;

                                var MapData = DBMap.FirstOrDefault(x => x.MapID == item.Map.Guid);
                                using (var ms = new System.IO.MemoryStream())
                                {
                                    byte[] mapDecodedBytes = Convert.FromBase64String(MapData.MapImageData);  //Fleet Var 3.0사용
                                                                                                              //byte[] mapDecodedBytes = Convert.FromBase64String(newMap.map);  //Fleet Var 2.0사용
                                    ms.Write(mapDecodedBytes, 0, mapDecodedBytes.Length);
                                    ucMapView2.mapImageFromDB = System.Drawing.Image.FromStream(ms);
                                }
                            }
                        }

                        if (_readServices.Count >= 3)
                        {
                            if (_readServices[2].MapGuid == item.Map.Guid)
                            {
                                ucMapView3.Map = item.Map;
                                ucMapView3.RobotInfoList = item.RobotInfo;

                                var MapData = DBMap.FirstOrDefault(x => x.MapID == item.Map.Guid);
                                using (var ms = new System.IO.MemoryStream())
                                {
                                    byte[] mapDecodedBytes = Convert.FromBase64String(MapData.MapImageData);  //Fleet Var 3.0사용
                                                                                                              //byte[] mapDecodedBytes = Convert.FromBase64String(newMap.map);  //Fleet Var 2.0사용
                                    ms.Write(mapDecodedBytes, 0, mapDecodedBytes.Length);
                                    ucMapView3.mapImageFromDB = System.Drawing.Image.FromStream(ms);
                                }
                            }
                        }
                    }
                }
            });


            if (_readServices.Count >= 1)
            {
                ucMapView1.Init(
                        apiClient: null,
                        queue: _queue,
                        mapID: _readServices[0].MapGuid,
                        mapName: _readServices[0].MapName,
                        mapScale: mapScale,
                        mouseFirstLocation: mouseFirstLocation,
                        mouseMoveOffset: mouseMoveOffset);

                ucMapView1.Dock = DockStyle.Fill;
                ucMapView1.StartLoop();
            }

            if (_readServices.Count >= 2)
            {
                ucMapView2.Init(
                        apiClient: null,
                        queue: _queue,
                        mapID: _readServices[1].MapGuid,
                        mapName: _readServices[1].MapName,
                        mapScale: mapScale,
                        mouseFirstLocation: mouseFirstLocation,
                        mouseMoveOffset: mouseMoveOffset);

                ucMapView2.Dock = DockStyle.Fill;
                ucMapView2.StartLoop();
            }

            if (_readServices.Count >= 3)
            {
                ucMapView3.Init(
                        apiClient: null,
                        queue: _queue,
                        mapID: _readServices[2].MapGuid,
                        mapName: _readServices[2].MapName,
                        mapScale: mapScale,
                        mouseFirstLocation: mouseFirstLocation,
                        mouseMoveOffset: mouseMoveOffset);

                ucMapView3.Dock = DockStyle.Fill;
                ucMapView3.StartLoop();
            }
        }

        private void MapMonitoring_Enter(object sender, EventArgs e)
        {
            ConfigData.FleetPositions = uow.FleetPositions.DBGetAll();

            Task task2 = Task.Run(async () =>
            {
                while(true)
                {
                    while (_queue.TryDequeue(out MapReadDto item))
                    {
                        if (_readServices.Count >= 1)
                        {
                            if (_readServices[0].MapGuid == item.Map.Guid)
                            {
                                ucMapView1.Map = item.Map;
                                ucMapView1.RobotInfoList = item.RobotInfo;
                            }
                        }

                        if (_readServices.Count >= 2)
                        {
                            if (_readServices[1].MapGuid == item.Map.Guid)
                            {
                                ucMapView2.Map = item.Map;
                                ucMapView2.RobotInfoList = item.RobotInfo;
                            }
                        }

                        if (_readServices.Count >= 3)
                        {
                            if (_readServices[2].MapGuid == item.Map.Guid)
                            {
                                ucMapView3.Map = item.Map;
                                ucMapView3.RobotInfoList = item.RobotInfo;
                            }
                        }
                    }
                }
            });


            if (_readServices.Count >= 1)
            {
                ucMapView1.Init(
                        apiClient: null,
                        queue: _queue,
                        mapID: _readServices[0].MapGuid,
                        mapName: _readServices[0].MapName,
                        mapScale: mapScale,
                        mouseFirstLocation: mouseFirstLocation,
                        mouseMoveOffset: mouseMoveOffset);

                ucMapView1.Dock = DockStyle.Fill;
                ucMapView1.StartLoop();
            }

            if (_readServices.Count >= 2)
            {
                ucMapView2.Init(
                        apiClient: null,
                        queue: _queue,
                        mapID: _readServices[1].MapGuid,
                        mapName: _readServices[1].MapName,
                        mapScale: mapScale,
                        mouseFirstLocation: mouseFirstLocation,
                        mouseMoveOffset: mouseMoveOffset);

                ucMapView2.Dock = DockStyle.Fill;
                ucMapView2.StartLoop();
            }

            if (_readServices.Count >= 3)
            {
                ucMapView3.Init(
                        apiClient: null,
                        queue: _queue,
                        mapID: _readServices[2].MapGuid,
                        mapName: _readServices[2].MapName,
                        mapScale: mapScale,
                        mouseFirstLocation: mouseFirstLocation,
                        mouseMoveOffset: mouseMoveOffset);

                ucMapView3.Dock = DockStyle.Fill;
                ucMapView3.StartLoop();
            }
        }

        private void MapMonitoring_Leave(object sender, EventArgs e)
        {
            if (_readServices.Count >= 1)
                ucMapView1.StopLoop();

            if (_readServices.Count >= 2)
                ucMapView2.StopLoop();

            if (_readServices.Count >= 3)
                ucMapView3.StopLoop();
        }
    }
}
