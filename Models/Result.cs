using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UmaPoyofeatChatGPT2.Models
{
    [Table("Result")]
    public partial class Result
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RaceResultId { get; set; } // 主キー

        [Required]
        [MaxLength(3)]
        public string Result1 { get; set; } = null!; // レース結果(着順)

        [Required]
        [MaxLength(50)]
        public string RaceId { get; set; } = null!; // レースID

        [Required]
        [MaxLength(3)]
        public string RaceNum { get; set; } = null!; // レース番号

        [Required]
        [MaxLength(100)]
        public string RaceName { get; set; } = null!; // レース名

        [Required]
        [MaxLength(50)]
        public string Date { get; set; } = null!; // 日付

        [Required]
        [MaxLength(50)]
        public string JyoName { get; set; } = null!; // 競馬場名

        [Required]
        [MaxLength(50)]
        public string Distance { get; set; } = null!; // 距離 (数値型)

        [MaxLength(50)]
        public string? Wakuban { get; set; } // 枠番

        [MaxLength(50)]
        public string? Umaban { get; set; } // 馬番

        [Required]
        [MaxLength(100)]
        public string HorseName { get; set; } = null!; // 馬名

        [Required]
        [MaxLength(100)]
        public string JockeyName { get; set; } = null!; // 騎手名

        [Required]
        [MaxLength(50)]
        public string Time { get; set; } = null!; // レースタイム

        [Required]
        [MaxLength(50)]
        public string Popularity { get; set; } = null!; // 人気 (数値型)

        [MaxLength(50)]
        public string? CornerPassOrder { get; set; } // コーナー通過順位
    }
}