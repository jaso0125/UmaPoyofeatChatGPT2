using UmaPoyofeatChatGPT2.Models;

namespace UmaPoyofeatChatGPT2.Services
{
    public class PastRaceService : IPastRaceService
    {
        private readonly UmaPoyofeatChatGpt2Context _dbContext;

        public PastRaceService(UmaPoyofeatChatGpt2Context dbContext)
        {
            _dbContext = dbContext;
        }

        public List<PastRace> GetAllPastRaces()
        {
            return _dbContext.PastRaces.ToList();
        }

        public PastRace? GetPastRaceById(int id)
        {
            return _dbContext.PastRaces.Find(id);
        }

        public void AddPastRace(PastRace pastRace)
        {
            _dbContext.PastRaces.Add(pastRace);
            _dbContext.SaveChanges();
        }

        public void UpdatePastRace(PastRace pastRace)
        {
            _dbContext.PastRaces.Update(pastRace);
            _dbContext.SaveChanges();
        }

        public void DeletePastRace(int id)
        {
            var pastRace = _dbContext.PastRaces.Find(id);
            if (pastRace != null)
            {
                _dbContext.PastRaces.Remove(pastRace);
                _dbContext.SaveChanges();
            }
        }

        public List<PastRace> GetPastRacesByKettoNum(string horseId)
        {
            return _dbContext.PastRaces
                .Where(pr => pr.KettoNum == horseId)
                .ToList();
        }
    }
}