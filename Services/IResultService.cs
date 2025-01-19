using UmaPoyofeatChatGPT2.Models;

namespace UmaPoyofeatChatGPT2.Services
{
    public interface IResultService
    {
        List<Result> GetAllResults();

        Result? GetResultById(int id);

        void AddResult(Result result);

        void UpdateResult(Result result);

        void DeleteResult(int id);

        List<Result> GetResultsByRaceId(string raceId);
    }
}