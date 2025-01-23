using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmaPoyofeatChatGPT2.Util
{
    public class Util
    {
        public LineConfig? LineConfig { get; set; }
        public TwitterConfig? TwitterConfig { get; set; }
        public ConnectionStrings? ConnectionStrings { get; set; }
        public OpenAI? OpenAI { get; set; }

        public static Util LoadFromFile(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                string json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<Util>(json);
            }
        }
    }

    public class LineConfig
    {
        public string ChannelAccessToken { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }

    public class TwitterConfig
    {
        public string AccessToken { get; set; } = string.Empty;
        public string AccessTokenSecret { get; set; } = string.Empty;
        public string ConsumerKey { get; set; } = string.Empty;
        public string ConsumerSecret { get; set; } = string.Empty;
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; } = string.Empty;
    }

    public class OpenAI
    {
        public string ApiKey { get; set; } = string.Empty;
    }
}