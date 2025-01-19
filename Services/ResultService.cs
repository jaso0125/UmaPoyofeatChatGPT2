using UmaPoyofeatChatGPT2.Models;

namespace UmaPoyofeatChatGPT2.Services
{
    public class ResultService : IResultService
    {
        private readonly UmaPoyofeatChatGpt2Context _dbContext;

        public ResultService(UmaPoyofeatChatGpt2Context dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Result> GetAllResults()
        {
            return _dbContext.Results.ToList();
        }

        public Result? GetResultById(int id)
        {
            return _dbContext.Results.Find(id);
        }

        public void AddResult(Result result)
        {
            _dbContext.Results.Add(result);
            _dbContext.SaveChanges();
        }

        public void UpdateResult(Result result)
        {
            _dbContext.Results.Update(result);
            _dbContext.SaveChanges();
        }

        public void DeleteResult(int id)
        {
            var result = _dbContext.Results.Find(id);
            if (result != null)
            {
                _dbContext.Results.Remove(result);
                _dbContext.SaveChanges();
            }
        }

        public List<Result> GetResultsByRaceId(string raceId)
        {
            return _dbContext.Results
                .Where(r => r.RaceId == raceId)
                .ToList();
        }
    }
}