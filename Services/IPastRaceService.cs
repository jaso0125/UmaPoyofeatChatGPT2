using UmaPoyofeatChatGPT2.Models;

namespace UmaPoyofeatChatGPT2.Services
{
    public interface IPastRaceService
    {
        List<PastRace> GetAllPastRaces();

        PastRace? GetPastRaceById(int id);

        void AddPastRace(PastRace pastRace);

        void UpdatePastRace(PastRace pastRace);

        void DeletePastRace(int id);

        List<PastRace> GetPastRacesByKettoNum(string kettoNum);
    }
}