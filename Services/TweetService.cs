using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using UmaPoyofeatChatGPT2.AppSettings;
using UmaPoyofeatChatGPT2.Data;

namespace UmaPoyofeatChatGPT2.Services
{
    public class TweetService
    {
        private readonly TwitterConfig _tweetConfig;

        public TweetService(TwitterConfig tweetConfig)
        {
            _tweetConfig = tweetConfig;
        }

        public async Task<long> Tweet(string tweetMessage)
        {
            // リクエスト送信するHTTPクライアントの作成
            using (var httpClient = new HttpClient())
            {
                // OAuth認証ヘッダーを設定
                var oauth = new OAuthHelper(_tweetConfig.ConsumerKey, _tweetConfig.ConsumerSecret, _tweetConfig.AccessToken, _tweetConfig.AccessTokenSecret);
                var authHeader = oauth.GenerateAuthorizationHeader(_tweetConfig.RequestUri, "POST", tweetMessage);

                httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authHeader);

                // JSONコンテントを準備
                var body = new { text = tweetMessage };
                var jsonContent = JsonConvert.SerializeObject(body);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // POSTリクエストを送信
                var response = await httpClient.PostAsync(_tweetConfig.RequestUri, content);

                // レスポンスを読み込む
                var responseString = await response.Content.ReadAsStringAsync();

                var jsonString = JsonConvert.DeserializeObject<TwitterResponse>(responseString);

                var hasId = long.TryParse(jsonString!.Data.Id, out long id);

                return hasId ? id : 1514180366646480898;
            }
        }

        public static string MakeTweetMessage(string title, string data, string noteURL)
        {
            var text = $"【予想公開】{Environment.NewLine}" +
                $"{title}の予想をお届けします！{Environment.NewLine}{Environment.NewLine}" +
                $"{data}{Environment.NewLine}{Environment.NewLine}" +
                $"詳しい予想はこちら👇{Environment.NewLine}" +
                $"👉{noteURL}{Environment.NewLine}{Environment.NewLine}" +
                $"よかったらいいねもよろしくです🙌{Environment.NewLine}{Environment.NewLine}" +
                $"#競馬予想 #うまぽよ #AI競馬予想 ";

            return text;
        }

        public class OAuthHelper
        {
            private string consumerKey;
            private string consumerSecret;
            private string accessToken;
            private string accessTokenSecret;
            private const string signatureMethod = "HMAC-SHA1";
            private const string version = "1.0";

            public OAuthHelper(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
            {
                this.consumerKey = consumerKey;
                this.consumerSecret = consumerSecret;
                this.accessToken = accessToken;
                this.accessTokenSecret = accessTokenSecret;
            }

            public string GenerateAuthorizationHeader(string url, string method, string tweetContent)
            {
                var timeStamp = GenerateTimeStamp();
                var nonce = GenerateNonce();

                // Authorizationヘッダーに含める必要のあるすべてのパラメーターを収集
                var parameters = new SortedDictionary<string, string>
                {
                    { "oauth_consumer_key", consumerKey },
                    { "oauth_nonce", nonce },
                    { "oauth_signature_method", signatureMethod },
                    { "oauth_timestamp", timeStamp },
                    { "oauth_token", accessToken },
                    { "oauth_version", version }
                };

                // 署名の生成
                var signature = GenerateSignature(url, method, parameters, tweetContent);
                parameters.Add("oauth_signature", signature);

                // Authorizationヘッダーの構築
                var headerBuilder = new StringBuilder("OAuth ");
                foreach (var parameter in parameters)
                {
                    headerBuilder.AppendFormat("{0}=\"{1}\", ", parameter.Key, Uri.EscapeDataString(parameter.Value));
                }

                // 尾のコンマとスペースを取り除く
                headerBuilder.Length -= 2;

                return headerBuilder.ToString();
            }

            private string GenerateSignature(string url, string method, SortedDictionary<string, string> parameters, string tweetContent)
            {
                var sigString = string.Join("&", parameters.Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));
                var fullSigData = $"{method.ToUpper()}&{Uri.EscapeDataString(url)}&{Uri.EscapeDataString(sigString)}";

                using (var hasher = new HMACSHA1(Encoding.ASCII.GetBytes($"{Uri.EscapeDataString(consumerSecret)}&{Uri.EscapeDataString(accessTokenSecret)}")))
                {
                    var signature = Convert.ToBase64String(hasher.ComputeHash(Encoding.ASCII.GetBytes(fullSigData)));
                    return signature;
                }
            }

            private static string GenerateNonce()
            {
                return Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            }

            private static string GenerateTimeStamp()
            {
                TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return Convert.ToInt64(timeSpan.TotalSeconds).ToString();
            }
        }
    }
}