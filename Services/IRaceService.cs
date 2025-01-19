using UmaPoyofeatChatGPT2.Models;

namespace UmaPoyofeatChatGPT2.Services
{
    public interface IRaceService
    {
        List<HorseRace> GetAllHorseRaces();

        HorseRace? GetHorseRaceById(int id);

        void AddHorseRace(HorseRace horseRace);

        void UpdateHorseRace(HorseRace horseRace);

        void DeleteHorseRace(int id);
    }
}