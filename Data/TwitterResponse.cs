using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmaPoyofeatChatGPT2.Data
{
    public class TwitterResponse
    {
        public TweetData Data { get; set; } = new TweetData();
    }

    public class TweetData
    {
        [JsonProperty("edit_history_tweet_ids")]
        public List<string> EditHistoryTweetIds { get; set; } = new List<string>();

        public string Id { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}