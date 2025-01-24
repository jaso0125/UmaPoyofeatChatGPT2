using Newtonsoft.Json;
using System.Text;
using UmaPoyofeatChatGPT2.Models;

namespace UmaPoyofeatChatGPT2.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(string baseAddress)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };
        }

        #region HorseRace関連

        public async Task<List<HorseRace>> GetHorseRacesByRaceIdAsync(string raceId)
        {
            var response = await _httpClient.GetAsync($"HorseRace/{raceId}");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<HorseRace>>(jsonString) ?? new List<HorseRace>();
        }

        public async Task UpsertHorseRacesAsync(List<HorseRace> horseRaces)
        {
            var jsonContent = JsonConvert.SerializeObject(horseRaces);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("HorseRace/Upsert", content);
            response.EnsureSuccessStatusCode();
        }

        #endregion HorseRace関連

        #region RaceInfo関連

        public async Task<List<RaceInfo>> GetRaceInfosByDateAsync(string date)
        {
            var response = await _httpClient.GetAsync($"RaceInfo/ByDate/{date}");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<RaceInfo>>(jsonString) ?? new List<RaceInfo>();
        }

        public async Task<RaceInfo> GetRaceInfoByRaceIdAsync(string raceId)
        {
            var response = await _httpClient.GetAsync($"RaceInfo/ByRaceId/{raceId}");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RaceInfo>(jsonString) ?? new RaceInfo();
        }

        public async Task<RaceInfo> GetRaceInfoByDateRaceCourseRaceNumberAsync(string date, string raceCource, string raceNumber)
        {
            var response = await _httpClient.GetAsync($"RaceInfo/{date}/{raceCource}/{raceNumber}");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RaceInfo>(jsonString) ?? new RaceInfo();
        }

        public async Task UpsertRaceInfoAsync(RaceInfo raceInfo)
        {
            var jsonContent = JsonConvert.SerializeObject(raceInfo);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("RaceInfo/Upsert/RaceInfo", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpsertRaceInfosAsync(List<RaceInfo> raceInfos)
        {
            var jsonContent = JsonConvert.SerializeObject(raceInfos);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("RaceInfo/Upsert/RaceInfos", content);
            response.EnsureSuccessStatusCode();
        }

        #endregion RaceInfo関連

        #region PastRace関連

        public async Task<List<PastRace>> GetPastRacesAsync(string kettoNum)
        {
            var response = await _httpClient.GetAsync($"PastRace/{kettoNum}");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<PastRace>>(jsonString) ?? new List<PastRace>();
        }

        #endregion PastRace関連
    }
}