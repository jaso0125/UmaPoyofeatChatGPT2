using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmaPoyofeatChatGPT2.Common
{
    public class Util
    {
        public static Dictionary<string, string> raceCourseCodes = new()
        {
            {"札幌", "01"},
            {"函館", "02"},
            {"福島", "03"},
            {"新潟", "04"},
            {"東京", "05"},
            {"中山", "06"},
            {"中京", "07"},
            {"京都", "08"},
            {"阪神", "09"},
            {"小倉", "10"}
        };
    }
}