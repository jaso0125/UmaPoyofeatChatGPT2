using UmaPoyofeatChatGPT2.Models;

namespace UmaPoyofeatChatGPT2.Services
{
    public interface IRaceInfoService
    {
        List<RaceInfo> GetAllRaceInfos();

        RaceInfo? GetRaceInfoById(string raceId);

        List<RaceInfo> GetHorseRacesByDate(string date);

        RaceInfo? GetRaceInfoByDateRaceCourseRaceNumber(string date, string raceCourse, string Raceumber);

        void AddRaceInfo(RaceInfo raceInfo);

        void UpdateRaceInfo(RaceInfo raceInfo);

        void DeleteRaceInfo(string raceId);
    }
}