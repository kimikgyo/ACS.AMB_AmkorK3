using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using ACS.RobotApi;
using INA_ACS_Server;
using ACS.Common.DTO;

namespace ACS.RobotMap
{
    public class FleetMapReadService : IMapReadService
    {
        private readonly ILog _logger;
        private readonly IFleetApi _fleetApi;
        private readonly IUnitOfWork uow;
        private readonly MapReadDtoQueue<MapReadDto> _queue;
        private List<int> _robotIds;
        private bool _bStopFlag = false;
        public string MapGuid { get; set; }
        public string MapName { get; set; }

        public FleetMapReadService(MapReadDtoQueue<MapReadDto> queue, ILog logger, IFleetApi fleetApi, IUnitOfWork uow)
        {
            this._queue = queue;
            this._logger = logger;
            this._fleetApi = fleetApi;
            this.uow = uow;
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
                        //fleet Position Data Add
                        uow.FleetPositions.AllRemove();

                        // get data
                        var newMap = await FleetGetMapAsync(MapGuid);
                        var newStatus = await FleetGetRobotStatusAsync();

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
                            //FleetPosition Db저장
                            FleetMapDataAdd(cachedMap);
                            bMapLoaded = true;
                        }
                    }
                    else // 맵 로드된 후에는 로봇상태만 읽는다
                    {
                        // get data
                        var newStatus = await FleetGetRobotStatusAsync();

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

        /// <summary>
        /// Map ImageData 및 Position정보 데이터베이스 입력
        /// </summary>
        /// <param name="fleetMap"></param> Fleet에서 읽은 Map Data
        private void FleetMapDataAdd(FleetMap fleetMap)
        {
            //Map image Data Updata하기(모니터링 프로그램에서 사용)
            var MapImageAdd = uow.FloorMapIDConfigs.Find(x => x.MapID == fleetMap.Guid).FirstOrDefault();
            if (MapImageAdd != null)
            {
                MapImageAdd.MapImageData = fleetMap.base_map;
                uow.FloorMapIDConfigs.Update(MapImageAdd);
            }

            foreach (var fleetMapPosition in fleetMap.Positions.Where(x=>x.Name.StartsWith("ACS") == false))
            {
                var FleetPosition = new FleetPositionModel
                {
                    Name = fleetMapPosition.Name,
                    Guid = fleetMapPosition.Guid,
                    TypeID = fleetMapPosition.TypeID,
                    MapID = fleetMapPosition.MapID,
                    PosX = fleetMapPosition.PosX,
                    PosY = fleetMapPosition.PosY,
                    Orientation = fleetMapPosition.Orientation

                };
                uow.FleetPositions.Add(FleetPosition);
            }
        }

        private async Task<FleetMap> FleetGetMapAsync(string mapGuid)
        {
            var newMap = new FleetMap();

            var fleetMapData = await _fleetApi.GetMapByIdAsync(mapGuid);
            if (fleetMapData != null)
            {
                fleetMapData.MapToFleetMap(newMap);
                newMap.Positions = await FleetGetPositionsAsync(mapGuid);

                return newMap;
            }
            return null;
        }

        private async Task<List<FleetPosition>> FleetGetPositionsAsync(string mapGuid)
        {
            var newPositions = new List<FleetPosition>();

            var fleetPositionSimpleDTOs = await _fleetApi.GetPositionsAsync(mapGuid);
            if (fleetPositionSimpleDTOs != null)
            {
                foreach (string pos_id in fleetPositionSimpleDTOs.Select(p => p.guid).Take(50)) // 포지션 너무많아서 임시로 50개만 읽는다...
                {
                    var fleetPositionDetailDTO = await _fleetApi.GetPositionByIdAsync(pos_id);
                    if (fleetPositionDetailDTO != null)
                    {
                        newPositions.Add(new FleetPosition()
                        {
                            Name = fleetPositionDetailDTO.name,
                            Guid = fleetPositionDetailDTO.guid,
                            PosX = fleetPositionDetailDTO.pos_x,
                            PosY = fleetPositionDetailDTO.pos_y,
                            Orientation = fleetPositionDetailDTO.orientation,
                            MapID = fleetPositionDetailDTO.map_id,
                            TypeID = fleetPositionDetailDTO.type_id,
                        });
                    }
                };
            }
            return newPositions;
        }

        private async Task<List<FleetRobot>> FleetGetRobotStatusAsync()
        {
            // get robot ids (로봇 등록상태가 변경될 경우, 자동 감지하기 위해 계속 갱신한다)
            _robotIds = await _fleetApi.GetRobotIdsAsync();
            if (_robotIds == null) return null;


            // get status
            var fleetRobots = new List<FleetRobot>();

            foreach (var robot_id in _robotIds)
            {
                var fleetRobot = new FleetRobot();

                var robotInfo = await _fleetApi.GetRobotByIdAsync(robot_id);
                if (robotInfo != null)
                {
                    robotInfo.MapToFleetRobot(fleetRobot);
                    fleetRobots.Add(fleetRobot);
                }
            }

            return fleetRobots;
        }
    }
}
