using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using ACS.RobotApi;

namespace ACS.RobotMap
{
    public class MirMapReadService : IMapReadService
    {
        private readonly ILog _logger;
        private readonly IMirApi _mirApi;
        private readonly IList<IMirApi> _mirApiList;
        private readonly MapReadDtoQueue<MapReadDto> _queue;
        private bool _bStopFlag = false;

        public string MapGuid { get; set; }
        public string MapName { get; set; }

        public MirMapReadService(MapReadDtoQueue<MapReadDto> queue, ILog logger, IMirApi mirApi, IList<IMirApi> mirApiList_ForRobotStatusReading)
        {
            this._queue = queue;
            this._logger = logger;
            this._mirApi = mirApi;
            this._mirApiList = mirApiList_ForRobotStatusReading;
        }

        public void Start()
        {
            Task.Run(() => Loop());
        }

        public void Stop()
        {
            _bStopFlag = true;
        }

        protected async Task Loop()
        {
            bool bMapLoaded = false;
            FleetMap cachedMap = null;

            while (!_bStopFlag)
            {
                try
                {
                    if (!bMapLoaded) // 맵 로드되기 전에는 맵,포지션,로봇상태 모두 읽는다
                    {
                        // get data
                        var newMap = await MirGetMapAsync(MapGuid);
                        var newStatus = await MirGetRobotStatusAsync();

                        // queue data
                        if (newMap != null && newStatus != null)
                        {
                            _queue.Enqueue(new MapReadDto
                            {
                                Map = newMap,
                                RobotInfo = newStatus,
                                CreatedTime = DateTime.Now
                            });
                            // keep map
                            cachedMap = newMap;

                            bMapLoaded = true;
                        }
                    }
                    else // 맵 로드된 후에는 로봇상태만 읽는다
                    {
                        // get data
                        var newStatus = await MirGetRobotStatusAsync();

                        // queue data
                        if (cachedMap != null && newStatus != null)
                        {
                            _queue.Enqueue(new MapReadDto
                            {
                                Map = cachedMap,
                                RobotInfo = newStatus,
                                CreatedTime = DateTime.Now
                            });
                        }
                    }

                    // delay
                    //Debug.WriteLine($"{GetType().Name} QueueCount = {_queue.Count}");
                    await Task.Delay(1000);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {GetType().Name}.{nameof(Loop)}() 예외발생! - {ex.Message}");
                }
            }
        }








        private async Task<FleetMap> MirGetMapAsync(string mapGuid)
        {
            var newMap = new FleetMap();

            var mirMapData = await _mirApi.GetMapByIdAsync(mapGuid);
            if (mirMapData != null)
            {
                mirMapData.MapToFleetMap(newMap);

                newMap.Positions = await MirGetPositionsAsync(mapGuid);
                return newMap;
            }
            return null;
        }

        private async Task<List<FleetPosition>> MirGetPositionsAsync(string mapGuid)
        {
            var newPositions = new List<FleetPosition>();

            var mirPositionSimpleDTOs = await _mirApi.GetPositionsAsync(mapGuid);
            if (mirPositionSimpleDTOs != null)
            {
                // Robot Map 상에서 ACS 사용하는 Position Name ACS로 설정한후 Name이 ACS로설정되어있는 것을 찾아 표시한다
                // 포지션 너무많아서 임시로 100개만 읽는다...
                foreach (string pos_id in mirPositionSimpleDTOs.Select(p => p.guid).Take(100))
                {
                    var mirPositionDetailDTO = await _mirApi.GetPositionByIdAsync(pos_id);
                    if (mirPositionDetailDTO != null)
                    {
                        newPositions.Add(new FleetPosition()
                        {
                            Name = mirPositionDetailDTO.name,
                            Guid = mirPositionDetailDTO.guid,
                            PosX = mirPositionDetailDTO.pos_x,
                            PosY = mirPositionDetailDTO.pos_y,
                            Orientation = mirPositionDetailDTO.orientation,
                            MapID = mirPositionDetailDTO.map_id,
                            TypeID = mirPositionDetailDTO.type_id,
                        });
                    }
                };
            }
            return newPositions;
        }

        private async Task<List<FleetRobot>> MirGetRobotStatusAsync()
        {
            var fleetRobots = new List<FleetRobot>();

            foreach (IMirApi x in _mirApiList)
            {
                var fleetRobot = new FleetRobot();

                var robotStatus = await x.GetStatusAsync();
                if (robotStatus != null)
                {
                    robotStatus.MapToFleetRobot(fleetRobot);
                    fleetRobots.Add(fleetRobot);
                }
            }

            return fleetRobots;
        }
    }
}
