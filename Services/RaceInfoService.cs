using UmaPoyofeatChatGPT2.Models;

namespace UmaPoyofeatChatGPT2.Services
{
    public class RaceInfoService : IRaceInfoService
    {
        private readonly UmaPoyofeatChatGpt2Context _dbContext;

        public RaceInfoService(UmaPoyofeatChatGpt2Context dbContext)
        {
            _dbContext = dbContext;
        }

        public List<RaceInfo> GetAllRaceInfos()
        {
            return _dbContext.RaceInfos.ToList();
        }

        public RaceInfo? GetRaceInfoById(string raceId)
        {
            return _dbContext.RaceInfos.FirstOrDefault(ri => ri.RaceId == raceId);
        }

        public void AddRaceInfo(RaceInfo raceInfo)
        {
            _dbContext.RaceInfos.Add(raceInfo);
            _dbContext.SaveChanges();
        }

        public void UpdateRaceInfo(RaceInfo raceInfo)
        {
            _dbContext.RaceInfos.Update(raceInfo);
            _dbContext.SaveChanges();
        }

        public void DeleteRaceInfo(string raceId)
        {
            var raceInfo = _dbContext.RaceInfos.FirstOrDefault(ri => ri.RaceId == raceId);
            if (raceInfo != null)
            {
                _dbContext.RaceInfos.Remove(raceInfo);
                _dbContext.SaveChanges();
            }
        }
    }
}