using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using ACS.Common.DTO;
using static ExceptionFilterUtility;

namespace ACS.RobotApi
{
    public class FleetApi : IFleetApi, IDisposable
    {
        private readonly ILog _logger;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerSettings _settings;


        public Uri BaseAddress => _httpClient.BaseAddress;

        public FleetApi(ILog logger, string ip, double timeout, JsonSerializerSettings settings = null)
        {
            _logger = logger;
            _httpClient = MakeHttpClient(ip, timeout);
            _settings = settings ?? new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
        }

        private HttpClient MakeHttpClient(string ip, double timeout)
        {
            var httpClient = new HttpClient();
            string auth = string.Format("{0}:{1}", "distributor", "distributor".ToSHA256());
            string accessToken = auth.ToBase64Encode();
            //string accessToken = "ZGlzdHJpYnV0b3I6NjJmMmYwZjFlZmYxMGQzMTUyYzk1ZjZmMDU5NjU3NmU0ODJiYjhlNDQ4MDY0MzNmNGNmOTI5NzkyODM0YjAxNA==";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", accessToken);
            httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en_US"));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);
#if MIRDEMO
            string uriString = $"http://localhost:5000/api/v2.0.0/";
#else
            string uriString = $"http://{ip.TrimEnd('/')}/api/v2.0.0/";
#endif
            httpClient.BaseAddress = new Uri(uriString);

            return httpClient;
        }

        public async Task<List<int>> GetRobotIdsAsync()
        {
            try
            {
                var robotInfos = await _httpClient.GetFromJsonAsync<List<FleetRobotInfoResponse>>("robots");
                return new List<int>(robotInfos.Select(x => x.id));
            }
            catch (Exception ex) when (True(() => _logger.Error(ex)))
            {
                return null;
            }
        }

        public async Task<FleetRobotInfoResponse> GetRobotByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<FleetRobotInfoResponse>($"robots/{id}");
            }
            catch (Exception ex) when (True(() => _logger.Error(ex)))
            {
                return null;
            }
        }

        public async Task<List<MissionResponse>> GetMissionsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<MissionResponse>>("missions");
            }
            catch (Exception ex) when (True(() => _logger.Error(ex)))
            {
                return null;
            }
        }

        public async Task<List<MissionSchedulerSimpleResponse>> GetMissionSchedulerAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<MissionSchedulerSimpleResponse>>("mission_scheduler");
            }
            catch (Exception ex) when (True(() => _logger.Error(ex)))
            {
                return null;
            }
        }

        public async Task<MissionSchedulerSimpleResponse> PostMissionSchedulerAsync(object value)
        {
            if (!AcceptFilterUtility.WriteAccepted) { _logger.Error($"-- API NOT ALLOWED. [{nameof(PostMissionSchedulerAsync)}] --"); return null; }

            try
            {
                var response = await _httpClient.PostAsJsonAsync("mission_scheduler", value);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<MissionSchedulerSimpleResponse>(jsonResponse);
            }
            catch (Exception ex) when (True(() => _logger.Error(ex)))
            {
                return null;
            }
        }

        public async Task<MissionSchedulerDetailResponse> GetMissionSchedulerByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<MissionSchedulerDetailResponse>($"mission_scheduler/{id}");
            }
            catch (Exception ex) when (True(() => _logger.Error(ex)))
            {
                return null;
            }
        }

        public async Task<bool> DeleteMissionSchedulerAsync()
        {
            if (!AcceptFilterUtility.WriteAccepted) { _logger.Error($"-- API NOT ALLOWED. [{nameof(DeleteMissionSchedulerAsync)}] --"); return false; }

            try
            {
                var response = await _httpClient.DeleteFromJsonAsync("mission_scheduler");
                return true;
            }
            catch (Exception ex) when (True(() => _logger.Error(ex)))
            {
                return false;
            }
        }

        public async Task<bool> DeleteMissionSchedulerByIdAsync(int id)
        {
            if (!AcceptFilterUtility.WriteAccepted) { _logger.Error($"-- API NOT ALLOWED. [{nameof(DeleteMissionSchedulerByIdAsync)}] --"); return false; }

            try
            {
                var response = await _httpClient.DeleteFromJsonAsync($"mission_scheduler/{id}");
                return true;
            }
            catch (Exception ex) when (True(() => _logger.Error(ex)))
            {
                return false;
            }
        }

        public async Task<List<FleetMapSimpleResponse>> GetMapsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<FleetMapSimpleResponse>>("maps");
            }
            catch (Exception ex) when (True(() => _logger.Error(ex)))
            {
                return null;
            }
        }

        public async Task<FleetMapDetailResponse> GetMapByIdAsync(string guid)
        {
            try
            {
                var newMap = await _httpClient.GetFromJsonAsync<FleetMapDetailResponse>($"maps/{guid}");

                // decode map image
                using (var ms = new System.IO.MemoryStream())
                {
                    byte[] mapDecodedBytes = Convert.FromBase64String(newMap.base_map);  //Fleet Var 3.0사용
                    //byte[] mapDecodedBytes = Convert.FromBase64String(newMap.map);  //Fleet Var 2.0사용
                    ms.Write(mapDecodedBytes, 0, mapDecodedBytes.Length);
                    newMap.Image = System.Drawing.Image.FromStream(ms);
                }

                return newMap;
            }
            catch (Exception ex) when (True(() => _logger.Error(ex)))
            {
                return null;
            }
        }

        public async Task<List<FleetPositionSimpleResponse>> GetPositionsAsync(string guid)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<FleetPositionSimpleResponse>>($"maps/{guid}/positions");
            }
            catch (Exception ex) when (True(() => _logger.Error(ex)))
            {
                return null;
            }
        }

        public async Task<FleetPositionDetailResponse> GetPositionByIdAsync(string guid)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<FleetPositionDetailResponse>($"positions/{guid}");
            }
            catch (Exception ex) when (True(() => _logger.Error(ex)))
            {
                return null;
            }
        }

        public override string ToString()
        {
            return $"BaseAddress={_httpClient.BaseAddress.AbsoluteUri}";
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
