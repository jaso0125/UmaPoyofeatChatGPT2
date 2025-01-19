using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UmaPoyofeatChatGPT2.Models
{
    [Table("PastRace")]
    public partial class PastRace
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // 主キー

        [Required]
        [MaxLength(50)]
        public string KettoNum { get; set; } = null!; // 血統番号

        [Required]
        [MaxLength(50)]
        public string Date { get; set; } = null!; // レース日付

        [MaxLength(100)]
        public string? Racecourse { get; set; } // 競馬場

        [MaxLength(50)]
        public string? Weather { get; set; } // 天候

        [MaxLength(100)]
        public string? RaceName { get; set; } // レース名

        [MaxLength(50)]
        public string? NumberOfHorses { get; set; } // 出走頭数

        [MaxLength(50)]
        public string? PostPosition { get; set; } // 枠番

        [MaxLength(50)]
        public string? HorseNumber { get; set; } // 馬番

        [MaxLength(50)]
        public string? Odds { get; set; } // オッズ

        [MaxLength(50)]
        public string? FinishPosition { get; set; } // 着順

        [MaxLength(100)]
        public string? Jockey { get; set; } // 騎手名

        [MaxLength(50)]
        public string? Weight { get; set; } // 馬体重

        [MaxLength(50)]
        public string? Distance { get; set; } // 距離

        [MaxLength(50)]
        public string? TrackCondition { get; set; } // 馬場状態

        [MaxLength(50)]
        public string? StartTime { get; set; } // 開始時間

        [MaxLength(50)]
        public string? Margin { get; set; } // 着差

        [MaxLength(50)]
        public string? RunningOrder { get; set; } // 通過順位

        [MaxLength(50)]
        public string? WeightChange { get; set; } // 馬体重変化

        [MaxLength(1000)]
        public string? TrainerComment { get; set; } // 調教師コメント

        [MaxLength(50)]
        public string? Popularity { get; set; } // 人気

        [MaxLength(50)]
        public string? HorseName { get; set; } // 馬名
    }
}