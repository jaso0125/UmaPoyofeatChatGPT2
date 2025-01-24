using Newtonsoft.Json;

namespace UmaPoyofeatChatGPT2.Data
{
    public class ChatGPTResponse
    {
        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("body")]
        public string Body { get; set; } = string.Empty;
    }
}