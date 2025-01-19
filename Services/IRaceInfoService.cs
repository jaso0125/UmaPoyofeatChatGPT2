using UmaPoyofeatChatGPT2.Models;

namespace UmaPoyofeatChatGPT2.Services
{
    public interface IRaceInfoService
    {
        List<RaceInfo> GetAllRaceInfos();

        RaceInfo? GetRaceInfoById(string raceId);

        void AddRaceInfo(RaceInfo raceInfo);

        void UpdateRaceInfo(RaceInfo raceInfo);

        void DeleteRaceInfo(string raceId);
    }
}