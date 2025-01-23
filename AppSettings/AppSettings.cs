﻿namespace UmaPoyofeatChatGPT2.AppSettings
{
    public class AppSettings
    {
        public LineConfig LineConfig { get; set; } = new LineConfig();
        public TwitterConfig TwitterConfig { get; set; } = new TwitterConfig();
        public ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();
        public OpenAI OpenAI { get; set; } = new OpenAI();
        public ApiSettings ApiSettings { get; set; } = new ApiSettings();
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

    public class ApiSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
    }
}