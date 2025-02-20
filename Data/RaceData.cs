using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmaPoyofeatChatGPT2.Models;

namespace UmaPoyofeatChatGPT2.Data
{
    public class RaceData
    {
        [JsonProperty("レース情報")]
        public RaceInformation RaceInformation { get; set; } = new RaceInformation();
    }

    public class Win5RaceData
    {
        [JsonProperty("Win5レース情報")]
        public List<RaceInformation> RaceInformations { get; set; } = new List<RaceInformation>();
    }

    public partial class RaceInformation
    {
        [JsonProperty("レースID")]
        public string RaceId { get; set; } = string.Empty;

        [JsonProperty("開催年月日")]
        public string Date { get; set; } = string.Empty;

        [JsonProperty("開催競馬場")]
        public string RaceCourse { get; set; } = string.Empty;

        [JsonProperty("レース番号")]
        public string? RaceNumber { get; set; }

        [JsonProperty("レース名")]
        public string? RaceName { get; set; }

        [JsonProperty("発送時刻")]
        public string StartTime { get; set; } = string.Empty;

        [JsonProperty("距離")]
        public string Distance { get; set; } = string.Empty;

        [JsonProperty("天候")]
        public string Weather { get; set; } = string.Empty;

        [JsonProperty("芝馬場")]
        public string ShibaTrackCondition { get; set; } = string.Empty;

        [JsonProperty("ダート馬場")]
        public string DirtTrackCondition { get; set; } = string.Empty;

        public bool? IsWin5Race { get; set; } = false;

        [JsonProperty("競走馬情報")]
        public List<HorseRaceInfo>? Horses { get; set; } = new List<HorseRaceInfo>();

        public static RaceInformation ConvertToRaceInformation(RaceInfo raceInfo)
        {
            var raceInformation = new RaceInformation
            {
                RaceId = raceInfo.RaceId,
                Date = raceInfo.Date,
                RaceCourse = raceInfo.RaceCourse,
                RaceNumber = raceInfo.RaceNumber,
                RaceName = raceInfo.RaceName,
                StartTime = raceInfo.StartTime,
                Distance = raceInfo.Distance,
                Weather = raceInfo.Weather,
                ShibaTrackCondition = raceInfo.ShibaTrackCondition,
                DirtTrackCondition = raceInfo.DirtTrackCondition,
                IsWin5Race = raceInfo.IsWin5Race,
                Horses = new List<HorseRaceInfo>()
            };
            return raceInformation;
        }

        public static HorseRaceInfo ConvertToHorceRaceInfo(HorseRace horseRace)
        {
            var horseRaceInfo = new HorseRaceInfo
            {
                RaceId = horseRace.RaceId,
                KettoNum = horseRace.KettoNum,
                Wakuban = horseRace.Wakuban,
                Umaban = horseRace.Umaban,
                HorseName = horseRace.HorseName,
                GenderAge = horseRace.GenderAge,
                Kinryo = horseRace.Kinryo,
                Jockey = horseRace.Jockey,
                WeightChange = horseRace.WeightChange ?? "",
                TrainingTime = horseRace.TrainingTime,
                TrainerComment = horseRace.TrainerComment,
            };
            return horseRaceInfo;
        }

        public static PastRaceInfo ConvertToPastRaceInfo(PastRace pastRace)
        {
            var pastRaceInfo = new PastRaceInfo
            {
                KettoNum = pastRace.KettoNum,
                Date = pastRace.Date,
                Racecourse = pastRace.Racecourse,
                Weather = pastRace.Weather,
                RaceName = pastRace.RaceName,
                NumberOfHorses = pastRace.NumberOfHorses,
                PostPosition = pastRace.PostPosition,
                HorseNumber = pastRace.HorseNumber,
                Odds = pastRace.Odds,
                Popularity = pastRace.Popularity,
                FinishPosition = pastRace.FinishPosition,
                Jockey = pastRace.Jockey,
                Weight = pastRace.Weight,
                Distance = pastRace.Distance,
                TrackCondition = pastRace.TrackCondition,
                Time = pastRace.StartTime,
                Margin = pastRace.Margin,
                RunningOrder = pastRace.RunningOrder,
                WeightChange = pastRace.WeightChange,
                TrainerComment = pastRace.TrainerComment,
            };
            return pastRaceInfo;
        }
    }

    public class HorseRaceInfo
    {
        [JsonProperty("レースID")]
        public string RaceId { get; set; } = string.Empty;

        [JsonProperty("血統番号")]
        public string KettoNum { get; set; } = string.Empty;

        [JsonProperty("枠番")]
        public string Wakuban { get; set; } = string.Empty;

        [JsonProperty("馬番")]
        public string Umaban { get; set; } = string.Empty;

        [JsonProperty("馬名")]
        public string HorseName { get; set; } = string.Empty;

        [JsonProperty("性齢")]
        public string GenderAge { get; set; } = string.Empty;

        [JsonProperty("斤量")]
        public string Kinryo { get; set; } = string.Empty;

        [JsonProperty("騎手")]
        public string Jockey { get; set; } = string.Empty;

        [JsonProperty("馬体重増減")]
        public string WeightChange { get; set; } = string.Empty;

        [JsonProperty("調教タイム")]
        public string? TrainingTime { get; set; }

        [JsonProperty("厩舎コメント")]
        public string? TrainerComment { get; set; }

        [JsonProperty("過去レース情報")]
        public List<PastRaceInfo>? PastRaces { get; set; }
    }

    public class PastRaceInfo
    {
        [JsonProperty("血統番号")]
        public string KettoNum { get; set; } = string.Empty;

        [JsonProperty("開催年月日")]
        public string? Date { get; set; }

        [JsonProperty("開催競馬場")]
        public string? Racecourse { get; set; }

        [JsonProperty("天候")]
        public string? Weather { get; set; }

        [JsonProperty("レース名")]
        public string? RaceName { get; set; }

        [JsonProperty("頭数")]
        public string? NumberOfHorses { get; set; }

        [JsonProperty("枠番")]
        public string? PostPosition { get; set; }

        [JsonProperty("馬番")]
        public string? HorseNumber { get; set; }

        [JsonProperty("オッズ")]
        public string? Odds { get; set; }

        [JsonProperty("人気")]
        public string? Popularity { get; set; }

        [JsonProperty("着順")]
        public string? FinishPosition { get; set; }

        [JsonProperty("騎手")]
        public string? Jockey { get; set; }

        [JsonProperty("斤量")]
        public string? Weight { get; set; }

        [JsonProperty("距離")]
        public string? Distance { get; set; }

        [JsonProperty("馬場")]
        public string? TrackCondition { get; set; }

        [JsonProperty("タイム")]
        public string? Time { get; set; }

        [JsonProperty("一着との差")]
        public string? Margin { get; set; }

        [JsonProperty("通過順")]
        public string? RunningOrder { get; set; }

        [JsonProperty("馬体重増減")]
        public string? WeightChange { get; set; }

        [JsonProperty("厩舎コメント")]
        public string? TrainerComment { get; set; }
    }
}