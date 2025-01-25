using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UmaPoyofeatChatGPT2.Models
{
    [Table("RaceInfo")]
    public partial class RaceInfo
    {
        [Key]
        [MaxLength(50)]
        public string RaceId { get; set; } = null!; // 主キーとしてのレースID

        [Required]
        [MaxLength(10)]
        public string Date { get; set; } = null!; // レース日付

        [Required]
        [MaxLength(50)]
        public string RaceCourse { get; set; } = null!; // 競馬場

        [MaxLength(50)]
        public string? RaceNumber { get; set; } // レース番号

        [MaxLength(255)]
        public string? RaceName { get; set; } // レース名

        [MaxLength(10)]
        public string StartTime { get; set; } = null!; // 開始時間

        [MaxLength(50)]
        public string Distance { get; set; } = null!;// 距離

        [MaxLength(50)]
        public string Weather { get; set; } = null!; // 天候

        [MaxLength(50)]
        public string ShibaTrackCondition { get; set; } = null!; // 芝の馬場状態

        [MaxLength(50)]
        public string DirtTrackCondition { get; set; } = null!; // ダートの馬場状態

        public bool? IsWin5Race { get; set; }

        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}