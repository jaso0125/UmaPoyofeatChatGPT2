using Newtonsoft.Json;

namespace UmaPoyofeatChatGPT2.Data
{
    public class TextNote
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; } = string.Empty;

        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("body")]
        public string Body { get; set; } = string.Empty;

        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("publish_at")]
        public DateTime? PublishAt { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; } = string.Empty;

        [JsonProperty("can_publish")]
        public bool CanPublish { get; set; }

        [JsonProperty("can_update")]
        public bool CanUpdate { get; set; }

        [JsonProperty("can_read")]
        public bool CanRead { get; set; }
    }
}