using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmaPoyofeatChatGPT2.Models;

namespace UmaPoyofeatChatGPT2.Services
{
    public class RaceService : IRaceService
    {
        private readonly UmaPoyofeatChatGpt2Context _dbContext;

        public RaceService(UmaPoyofeatChatGpt2Context dbContext)
        {
            _dbContext = dbContext;
        }

        public List<HorseRace> GetAllHorseRaces()
        {
            return _dbContext.HorseRaces.ToList();
        }

        public HorseRace? GetHorseRaceById(int id)
        {
            return _dbContext.HorseRaces.Find(id);
        }

        public void AddHorseRace(HorseRace horseRace)
        {
            _dbContext.HorseRaces.Add(horseRace);
            _dbContext.SaveChanges();
        }

        public void UpdateHorseRace(HorseRace horseRace)
        {
            _dbContext.HorseRaces.Update(horseRace);
            _dbContext.SaveChanges();
        }

        public void DeleteHorseRace(int id)
        {
            var horseRace = _dbContext.HorseRaces.Find(id);
            if (horseRace != null)
            {
                _dbContext.HorseRaces.Remove(horseRace);
                _dbContext.SaveChanges();
            }
        }
    }
}