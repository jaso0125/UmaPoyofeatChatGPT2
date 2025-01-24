using Markdig;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using UmaPoyofeatChatGPT2.AppSettings;
using UmaPoyofeatChatGPT2.Data;

namespace UmaPoyofeatChatGPT2.Services
{
    public class NoteService
    {
        public static async Task<string> PutNote(string predictionText, NoteConfig noteConfig)
        {
            try
            {
                var loginUrl = noteConfig.LoginUrl;
                var loginData = new
                {
                    login = noteConfig.UserName, // NoteのログインIDをここに入力
                    password = noteConfig.Password // Noteのパスワードをここに入力
                };
                var headers = new MediaTypeWithQualityHeaderValue("application/json");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(headers);

                    var loginContent = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");
                    var loginResponse = await client.PostAsync(loginUrl, loginContent);

                    if (loginResponse.IsSuccessStatusCode)
                    {
                        var cookies = loginResponse.Headers.GetValues("Set-Cookie");
                        var cookieContainer = new System.Net.CookieContainer();

                        foreach (var cookie in cookies)
                        {
                            cookieContainer.SetCookies(new Uri("https://note.com"), cookie);
                        }
                        var handler = new HttpClientHandler() { CookieContainer = cookieContainer };

                        using (var clientWithCookies = new HttpClient(handler))
                        {
                            // 記事ID作成
                            var postCreateUrl = noteConfig.PostCreateUrl;

                            var postCreateData = new FormUrlEncodedContent(new[]
    {
                            new KeyValuePair<string, string?>("template_key", null)
                        });
                            var postCreateResponse = await clientWithCookies.PostAsync(postCreateUrl, postCreateData);

                            var createResponseData = await postCreateResponse.Content.ReadAsStringAsync();
                            var createDataJson = JObject.Parse(createResponseData);

                            var textNote = JsonConvert.DeserializeObject<TextNote>(createDataJson["data"]!.ToString());

                            // 記事投稿
                            var putUrl = $"{postCreateUrl}/{textNote!.Id}";
                            var chatGPTResponseData = JsonConvert.DeserializeObject<ChatGPTResponse>(predictionText);
                            var markDown = $"{chatGPTResponseData!.Body}";
                            var html = Markdown.ToHtml(markDown)
                                .Replace("）", "）<br>")
                                .Replace("出走馬の印と評価</h3>", "出走馬の印と評価</h3><br>")
                                .Replace("最終予想</h3>", "最終予想</h3><br>");

                            var payload = new
                            {
                                status = "published",
                                name = $"{chatGPTResponseData.Title}",
                                free_body = html,
                                magazine_ids = new[] { 5824493 },
                                hashtags = new[] { "#中央競馬", "#中央競馬予想" }
                            };

                            var putContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                            var putResponse = await clientWithCookies.PutAsync(putUrl, putContent);

                            var responseData = await putResponse.Content.ReadAsStringAsync();
                            var dataJson = JObject.Parse(responseData);

                            return dataJson["data"]?["note_url"]?.ToString()!;
                        }
                    }
                    else
                    {
                        return "Login failed.";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"エラー{ex.Message}";
            }
        }
    }
}