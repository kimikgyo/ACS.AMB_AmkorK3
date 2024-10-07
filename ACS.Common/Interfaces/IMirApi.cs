using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ACS.Common.DTO;

namespace ACS.RobotApi
{
    public interface IMirApi
    {
        Uri BaseAddress { get; }

        Task<RobotStatusResponse> GetStatusAsync();
        Task<GetHookStatusResponse> GetHookStatusAsync();

        Task<List<MissionResponse>> GetMissionsAsync();

        Task<List<MissionQueueSimpleResponse>> GetMissionQueueAsync();
        Task<MissionQueueSimpleResponse> PostMissionQueueAsync(object value);
        Task<MissionQueueDetailResponse> GetMissionQueueByIdAsync(int id);
        Task<bool> DeleteMissionQueueAsync();
        Task<bool> DeleteMissionQueueByIdAsync(int id);

        List<RegisterResponse> GetRegisters();
        RegisterResponse GetRegisterById(int id);
        RegisterResponse PutRegisterById(int id, int value);

        Task<List<RegisterResponse>> GetRegistersAsync();
        Task<RegisterResponse> GetRegisterByIdAsync(int id);
        Task<RegisterResponse> PutRegisterByIdAsync(int id, int value);

        Task<List<MirMapSimpleResponse>> GetMapsAsync();
        Task<MirMapDetailResponse> GetMapByIdAsync(string guid);

        Task<List<MirPositionSimpleResponse>> GetPositionsAsync(string mapGuid);
        Task<MirPositionDetailResponse> GetPositionByIdAsync(string guid);
    }
}