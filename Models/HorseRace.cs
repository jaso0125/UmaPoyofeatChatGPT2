using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UmaPoyofeatChatGPT2.Models
{
    [Table("HorseRace")]
    public class HorseRace
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HorseRaceInfoId { get; set; } // 主キー

        [Required]
        [MaxLength(50)]
        public string RaceId { get; set; } = null!; // レースID

        [Required]
        [MaxLength(50)]
        public string KettoNum { get; set; } = null!; // 血統番号

        [Required]
        [MaxLength(50)]
        public string Wakuban { get; set; } = null!; // 枠番

        [Required]
        [MaxLength(50)]
        public string Umaban { get; set; } = null!; // 馬番

        [Required]
        [MaxLength(100)]
        public string HorseName { get; set; } = null!; // 馬名

        [Required]
        [MaxLength(50)]
        public string GenderAge { get; set; } = null!; // 性別・年齢

        [Required]
        [MaxLength(50)]
        public string Kinryo { get; set; } = null!; // 負担重量

        [Required]
        [MaxLength(100)]
        public string Jockey { get; set; } = null!; // 騎手名

        [MaxLength(50)]
        public string? WeightChange { get; set; } // 馬体重変化

        [MaxLength(200)]
        public string? TrainingTime { get; set; } // 調教タイム

        [MaxLength(500)]
        public string? TrainerComment { get; set; } // 調教師コメント
    }
}