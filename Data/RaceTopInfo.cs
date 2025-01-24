namespace UmaPoyofeatChatGPT2.Data
{
    public class RaceTopInfo
    {
        public string JyoName { get; set; } = string.Empty;
        public string RaceNum { get; set; } = string.Empty;
        public string RaceTitle { get; set; } = string.Empty;
        public string RaceTime { get; set; } = string.Empty;
        public string RaceDistance { get; set; } = string.Empty;
        public int NumberOfHorses { get; set; }

        public static RaceTopInfo ParseRaceInfo(string raceInfo)
        {
            var lines = raceInfo.Split('\n');
            var raceTimeAndDistance = lines[2].Split(' ');

            return new RaceTopInfo
            {
                JyoName = "",
                RaceNum = lines[0],
                RaceTitle = lines[1],
                RaceTime = raceTimeAndDistance[0],
                RaceDistance = raceTimeAndDistance[1],
                NumberOfHorses = int.Parse(raceTimeAndDistance[2].Replace("頭", ""))
            };
        }
    }
}