using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmaPoyofeatChatGPT2.Data
{
    public class GridViewModel
    {
        public string 枠 { get; set; } = string.Empty;
        public string 馬番 { get; set; } = string.Empty;
        public string 馬名 { get; set; } = string.Empty;
        public string 性齢 { get; set; } = string.Empty;
        public string 斤量 { get; set; } = string.Empty;
        public string 騎手名 { get; set; } = string.Empty;
        public string? 馬体重 { get; set; } = string.Empty;
        public string? 予想印 { get; set; } = string.Empty;
        public string? 調教タイム { get; set; } = string.Empty;
        public string? 厩舎コメント { get; set; } = string.Empty;
    }

    public class GridViewModelWin5
    {
        public string レースNo { get; set; } = string.Empty;
        public string 競馬場 { get; set; } = string.Empty;
        public string レース名 { get; set; } = string.Empty;
        public string 発走時刻 { get; set; } = string.Empty;
        public string 予想1 { get; set; } = string.Empty;
        public string 予想2 { get; set; } = string.Empty;
        public string 予想3 { get; set; } = string.Empty;
        public string 予想4 { get; set; } = string.Empty;
        public string 予想5 { get; set; } = string.Empty;
    }
}