using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ACS.Common.DTO;

namespace ACS.RobotApi
{
    public interface IFleetApi
    {
        Uri BaseAddress { get; }

        Task<List<int>> GetRobotIdsAsync();
        Task<FleetRobotInfoResponse> GetRobotByIdAsync(int id);

        Task<List<MissionResponse>> GetMissionsAsync();

        Task<List<MissionSchedulerSimpleResponse>> GetMissionSchedulerAsync();
        Task<MissionSchedulerSimpleResponse> PostMissionSchedulerAsync(object value);
        Task<MissionSchedulerDetailResponse> GetMissionSchedulerByIdAsync(int id);
        Task<bool> DeleteMissionSchedulerAsync();
        Task<bool> DeleteMissionSchedulerByIdAsync(int id);

        Task<List<FleetMapSimpleResponse>> GetMapsAsync();
        Task<FleetMapDetailResponse> GetMapByIdAsync(string guid);

        Task<List<FleetPositionSimpleResponse>> GetPositionsAsync(string guid);
        Task<FleetPositionDetailResponse> GetPositionByIdAsync(string guid);
    }
}