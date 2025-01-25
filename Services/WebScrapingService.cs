using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UmaPoyofeatChatGPT2.Common;
using UmaPoyofeatChatGPT2.Data;
using UmaPoyofeatChatGPT2.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace UmaPoyofeatChatGPT2.Services
{
    public class WebScrapingService
    {
        private readonly AppSettings.AppSettings _appSettings;
        private readonly ApiService _apiService;
        private HttpClient _httpClient;

        public WebScrapingService()
        {
            _appSettings = AppSettingsManager.GetSection<AppSettings.AppSettings>("AppSettings");
            _apiService = new ApiService(_appSettings.ApiSettings.BaseUrl);
            _httpClient = new HttpClient(new HttpClientHandler { UseCookies = true });

            // EncodingProvider を登録
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public async Task<string> UpsertRaceInfoAsync(string raceId)
        {
            // ログイン処理
            await LoginAsync();

            // レース詳細ページURLを生成
            var raceDetailUrl = $"https://race.netkeiba.com/race/shutuba.html?race_id={raceId}";

            // レース詳細情報を取得
            var detailPageBytes = await _httpClient.GetByteArrayAsync(raceDetailUrl);
            var detailPageHtml = Encoding.GetEncoding("euc-jp").GetString(detailPageBytes);
            var detailDoc = new HtmlDocument();
            detailDoc.LoadHtml(detailPageHtml);

            var horseRows = detailDoc.DocumentNode.SelectNodes(".//html/body/div[1]/div[3]/div[2]/table/tr");

            if (horseRows == null)
            {
                return $"このレースは存在しません。";
            }

            // 競走馬情報を取得
            var trainerTimeDic = await GetTrainingTimeAsync(raceId);
            var trainerCommentDic = await GetTrainerCommentAsync(raceId);

            // 各レースの馬毎のデータの追加・更新
            var horseRaces = await _apiService.GetHorseRacesByRaceIdAsync(raceId);

            var horseRacesList = new List<HorseRace>();
            foreach (var row in horseRows)
            {
                var kettoNum = ExtractKettoNum(row.SelectSingleNode("td[4]/div/div/span/a").GetAttributeValue("href", ""));
                var horseName = row.SelectSingleNode("td[4]").InnerText.Trim();

                var horseRace = new HorseRace
                {
                    RaceId = raceId,
                    Wakuban = row.SelectSingleNode("td[1]").InnerText.Trim(),
                    Umaban = row.SelectSingleNode("td[2]").InnerText.Trim(),
                    HorseName = horseName,
                    KettoNum = kettoNum,
                    GenderAge = row.SelectSingleNode("td[5]").InnerText.Trim(),
                    Kinryo = row.SelectSingleNode("td[6]").InnerText.Trim(),
                    Jockey = row.SelectSingleNode("td[7]").InnerText.Trim(),
                    WeightChange = row.SelectSingleNode("td[9]").InnerText.Trim(),
                    TrainingTime = trainerTimeDic.ContainsKey(horseName) ? trainerTimeDic[horseName] : "",
                    TrainerComment = trainerCommentDic.ContainsKey(horseName) ? trainerCommentDic[horseName] : "",
                };
                horseRacesList.Add(horseRace);
            }
            await _apiService.UpsertHorseRacesAsync(horseRacesList);

            return $"レース情報、馬毎のレース情報のデータ更新完了 raceId:{raceId}";
        }

        public async Task<string> UpsertRaceInfosAsync(string date)
        {
            try
            {
                // ログイン処理
                await LoginAsync();

                var raceInfos = new List<RaceInfo>();
                var url = $"https://race.netkeiba.com/top/race_list.html?kaisai_date={date}";

                // Playwrightのセットアップ
                using var playwright = await Playwright.CreateAsync();
                await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
                var page = await browser.NewPageAsync();

                // タイムアウトを60秒に設定
                page.SetDefaultTimeout(90000);

                // 指定したURLに移動
                await page.GotoAsync(url);
                var isRaceTopRaceExists = await page.QuerySelectorAsync("#RaceTopRace") != null;

                if (!isRaceTopRaceExists)
                {
                    return "本日開催のレースがありません。";
                }

                await page.WaitForSelectorAsync("#RaceTopRace");

                // 競馬場ごとのレース情報を取得
                var jyoSections = await page.EvaluateAsync<string[][]>(@"
                Array.from(document.querySelectorAll('.RaceList_DataList')).map(section => {
                    const title = section.querySelector('.RaceList_DataTitle').innerText.trim();
                    const races = Array.from(section.querySelectorAll('dd ul li a')).map(node => node.innerText.trim());

                    const weatherElement = section.querySelector('.Weather span');
                    const weatherClass = weatherElement ? weatherElement.className : 'Weather01'; // デフォルトで 'Weather01' = '晴'
                    const weather = weatherClass.includes('Weather01') ? '晴' :
                                    weatherClass.includes('Weather02') ? '曇' :
                                    weatherClass.includes('Weather03') ? '雨' :
                                    weatherClass.includes('Weather04') ? '雪' : '不明';

                    const shibaElement = section.querySelector('.Shiba');
                    const shibaCondition = shibaElement ? shibaElement.innerText.split('：')[1] : '良';

                    const dirtElement = section.querySelector('.Da');
                    const dirtCondition = dirtElement ? dirtElement.innerText.split('：')[1] : '良';

                    return [title, weather, shibaCondition, dirtCondition, ...races];
                });
            ");

                foreach (var section in jyoSections)
                {
                    var titleParts = section[0].Split(' ');
                    var kaiji = titleParts[0].Replace("回", "").PadLeft(2, '0');
                    var jyoName = titleParts[1].Trim();
                    var eventDay = titleParts[2].Replace("日目", "").Trim().PadLeft(2, '0');
                    var weather = section[1];
                    var shibaCondition = section[2];
                    var dirtCondition = section[3];
                    var raceNodes = section.Skip(4).ToArray();
                    var raceCourseCode = Util.raceCourseCodes.TryGetValue(jyoName, out string? value) ? value : "00";
                    var raceDetails = raceNodes.Where(x => !string.IsNullOrEmpty(x));

                    foreach (var raceNode in raceDetails)
                    {
                        var info = RaceTopInfo.ParseRaceInfo(raceNode);
                        info.JyoName = jyoName;

                        // RaceNumberから数値部分を抽出
                        var raceNumOnly = Regex.Match(info.RaceNum, @"\d+").Value;
                        var raceId = GenerateRaceId(date, raceCourseCode, kaiji, eventDay, raceNumOnly);

                        // レース情報の追加・更新
                        var raceInfo = await _apiService.GetRaceInfoByRaceIdAsync(raceId);

                        if (raceInfo == null)
                        {
                            raceInfo = new RaceInfo
                            {
                                RaceId = raceId,
                                Date = date,
                                RaceCourse = info.JyoName,
                                RaceNumber = info.RaceNum,
                                RaceName = info.RaceTitle,
                                StartTime = info.RaceTime,
                                Distance = info.RaceDistance,
                                Weather = weather,
                                ShibaTrackCondition = shibaCondition,
                                DirtTrackCondition = dirtCondition,
                            };
                        }

                        await _apiService.UpsertRaceInfoAsync(raceInfo);

                        // レース詳細ページURLを生成
                        var raceDetailUrl = $"https://race.netkeiba.com/race/shutuba.html?race_id={raceId}";

                        // レース詳細情報を取得
                        var detailPageBytes = await _httpClient.GetByteArrayAsync(raceDetailUrl);
                        var detailPageHtml = Encoding.GetEncoding("euc-jp").GetString(detailPageBytes);
                        var detailDoc = new HtmlDocument();
                        detailDoc.LoadHtml(detailPageHtml);

                        var horseRows = detailDoc.DocumentNode.SelectNodes(".//html/body/div[1]/div[3]/div[2]/table/tr");

                        if (horseRows == null)
                        {
                            return $"このレースは存在しません。日時：{date},競馬場：{raceCourseCode},レースNo：{info.RaceNum} ";
                        }

                        // 競走馬情報を取得
                        var trainerTimeDic = await GetTrainingTimeAsync(raceId);
                        var trainerCommentDic = await GetTrainerCommentAsync(raceId);

                        // 各レースの馬毎のデータの追加・更新
                        var horseRaces = await _apiService.GetHorseRacesByRaceIdAsync(raceId);

                        var horseRacesList = new List<HorseRace>();
                        foreach (var row in horseRows)
                        {
                            var kettoNum = ExtractKettoNum(row.SelectSingleNode("td[4]/div/div/span/a").GetAttributeValue("href", ""));
                            var horseName = row.SelectSingleNode("td[4]").InnerText.Trim();

                            var horseRace = new HorseRace
                            {
                                RaceId = raceId,
                                Wakuban = row.SelectSingleNode("td[1]").InnerText.Trim(),
                                Umaban = row.SelectSingleNode("td[2]").InnerText.Trim(),
                                HorseName = horseName,
                                KettoNum = kettoNum,
                                GenderAge = row.SelectSingleNode("td[5]").InnerText.Trim(),
                                Kinryo = row.SelectSingleNode("td[6]").InnerText.Trim(),
                                Jockey = row.SelectSingleNode("td[7]").InnerText.Trim(),
                                WeightChange = row.SelectSingleNode("td[9]").InnerText.Trim(),
                                TrainingTime = trainerTimeDic.ContainsKey(horseName) ? trainerTimeDic[horseName] : "",
                                TrainerComment = trainerCommentDic.ContainsKey(horseName) ? trainerCommentDic[horseName] : "",
                            };
                            horseRacesList.Add(horseRace);
                        }
                        await _apiService.UpsertHorseRacesAsync(horseRacesList);
                    }
                }
                return "レース情報、馬毎のレース情報のデータ更新完了";
            }
            catch (Exception ex)
            {
                return $"エラー:{ex.Message}";
            }
        }

        #region ログイン処理

        public async Task LoginAsync()
        {
            var loginData = new Dictionary<string, string>
        {
            { "login_id", _appSettings.NetKeiba.UserName },
            { "pswd", _appSettings.NetKeiba.Password },
            { "pid", "login" },
            { "action", "auth" },
        };

            var loginContent = new FormUrlEncodedContent(loginData);
            var loginResponse = await _httpClient.PostAsync(_appSettings.NetKeiba.LoginUrl, loginContent);
            loginResponse.EnsureSuccessStatusCode();
        }

        #endregion ログイン処理

        /// <summary>
        /// race_idを生成
        /// 例: 202405021001
        /// </summary>
        /// <param name="date"></param>
        /// <param name="raceCourseCode"></param>
        /// <param name="eventDay"></param>
        /// <param name="raceNum"></param>
        /// <returns></returns>
        private string GenerateRaceId(string date, string raceCourseCode, string kaiji, string eventDay, string raceNum)
        {
            var year = date.Substring(0, 4);

            // 例：202405021001
            return $"{year}{raceCourseCode}{kaiji}{eventDay}{raceNum.PadLeft(2, '0')}";
        }

        #region 調教タイム

        /// <summary>
        /// 調教タイムをディクショナリに格納
        /// </summary>
        /// <param name="raceId"></param>
        /// <returns></returns>
        private async Task<Dictionary<string, string>> GetTrainingTimeAsync(string raceId)
        {
            var trainingUrl = $"https://race.netkeiba.com/race/oikiri.html?race_id={raceId}";
            var trainingPageBytes = await _httpClient.GetByteArrayAsync(trainingUrl);
            var trainingPage = Encoding.GetEncoding("euc-jp").GetString(trainingPageBytes);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(trainingPage);

            var trainingTimesDict = new Dictionary<string, string>();

            if (htmlDoc.DocumentNode.SelectNodes("//td[@class='Horse_Info fc']/div") == null) { return trainingTimesDict; }

            var horseNames = htmlDoc.DocumentNode.SelectNodes("//td[@class='Horse_Info fc']/div").Select(x => x.InnerText).ToList();
            var trainingNodes = htmlDoc.DocumentNode.SelectNodes("//td[@class='TrainingTimeData txt_l']/ul/li").Select(x => x.InnerText.Trim()).ToList();

            if (trainingNodes != null)
            {
                for (int i = 0; i < horseNames.Count; i++)
                {
                    var startIndex = i * 5;
                    if (startIndex + 5 <= trainingNodes.Count)
                    {
                        var trainingTimesList = trainingNodes.Skip(startIndex).Take(5).ToList();
                        var trainingTimes = string.Join(",", trainingTimesList);
                        trainingTimesDict[horseNames[i]] = trainingTimes;
                    }
                }
            }
            return trainingTimesDict;
        }

        #endregion 調教タイム

        #region 厩舎コメント

        /// <summary>
        /// 厩舎コメントをディクショナリに格納
        /// </summary>
        /// <param name="raceId"></param>
        /// <returns></returns>
        private async Task<Dictionary<string, string>> GetTrainerCommentAsync(string raceId)
        {
            var commentUrl = $"https://race.netkeiba.com/race/comment.html?race_id={raceId}";
            var commentPageBytes = await _httpClient.GetByteArrayAsync(commentUrl);
            var commentPage = Encoding.GetEncoding("euc-jp").GetString(commentPageBytes);
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(commentPage);

            var trainingCommentsDict = new Dictionary<string, string>();
            var horseNames = new List<string>();
            var horseNamesNodes = htmlDoc.DocumentNode.SelectNodes("//td[@class='Horse_Name']/a");

            if (horseNamesNodes != null)
            {
                horseNames = horseNamesNodes.Select(x => x.InnerText).ToList();
            }

            var trainerComments = new List<string>();
            var trainerCommentsNodes = htmlDoc.DocumentNode.SelectNodes("//dl[@class='Comment_Cell']");
            if (trainerCommentsNodes != null)
            {
                trainerComments = trainerCommentsNodes.Select(x => RemoveQuestionPrefix(x.InnerText).Trim()).ToList();
            }

            for (int i = 0; i < horseNames.Count; i++)
            {
                trainingCommentsDict[horseNames[i]] = trainerComments[i];
            }

            return trainingCommentsDict;
        }

        #endregion 厩舎コメント

        /// <summary>
        ///  特定のパターンを削除するメソッド
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string RemoveQuestionPrefix(string input)
        {
            var pattern = @"\nＱ　.*?\n";
            return Regex.Replace(input, pattern, string.Empty).Trim();
        }

        /// <summary>
        /// 血統番号を取得
        /// </summary>
        /// <param name="href"></param>
        /// <returns></returns>
        private string ExtractKettoNum(string href)
        {
            var match = Regex.Match(href, @"horse/(\d+)");
            return match.Success ? match.Groups[1].Value : string.Empty;
        }
    }
}