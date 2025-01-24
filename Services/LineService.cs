using Newtonsoft.Json;
using System.Text;
using UmaPoyofeatChatGPT2.AppSettings;

namespace UmaPoyofeatChatGPT2.Services
{
    public class LineService
    {
        private readonly LineConfig _lineConfig;

        public LineService(LineConfig lineConfig)
        {
            _lineConfig = lineConfig;
        }

        public async Task SendMessageAsync(string message)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_lineConfig.ChannelAccessToken}");

                var data = new
                {
                    to = _lineConfig.UserId,
                    messages = new[]
                    {
                        new
                        {
                            type = "text",
                            text = message
                        }
                    }
                };

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(_lineConfig.RequestUri, content);
                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Response status code does not indicate success: {response.StatusCode}. Response content: {responseContent}");
                }
            }
        }
    }
}