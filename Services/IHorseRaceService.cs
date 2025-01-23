using UmaPoyofeatChatGPT2.Models;

namespace UmaPoyofeatChatGPT2.Services
{
    public interface IHorseRaceService
    {
        List<HorseRace> GetAllHorseRaces();

        HorseRace? GetHorseRaceById(int id);

        List<HorseRace> GetHorseRaceByRaceId(string raceId);

        void AddHorseRace(HorseRace horseRace);

        void UpdateHorseRace(HorseRace horseRace);

        void DeleteHorseRace(int id);
    }
}