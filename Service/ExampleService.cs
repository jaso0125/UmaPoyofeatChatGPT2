using UmaPoyofeatChatGPT2.Models;

namespace UmaPoyofeatChatGPT2.Service
{
    public class ExampleService
    {
        private readonly UmaPoyofeatChatGpt2Context _dbContext;

        public ExampleService(UmaPoyofeatChatGpt2Context dbContext)
        {
            _dbContext = dbContext;
        }

        // Run メソッドを定義
        public void Run()
        {
            // データベースから HorseRaces のデータを取得
            var horseRaces = _dbContext.HorseRaces.Take(10).ToList();
            foreach (var race in horseRaces)
            {
                Console.WriteLine($"Race ID: {race.RaceId}, Horse Name: {race.HorseName}");
            }
        }
    }
}