using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using UmaPoyofeatChatGPT2.Common;
using UmaPoyofeatChatGPT2.Data;
using UmaPoyofeatChatGPT2.Models;
using UmaPoyofeatChatGPT2.Services;

namespace UmaPoyofeatChatGPT2
{
    public partial class Form1 : Form
    {
        private readonly ApiService _apiService;
        private readonly AppSettings.AppSettings _appSettings = AppSettingsManager.GetSection<AppSettings.AppSettings>("AppSettings");
        private readonly WebScrapingService _webScrapingService;
        private const string ResponseExample = "{\r\n  \"title\": \"2024年5月5日 東京競馬場 11R NHKマイルC(G1)\",\r\n  \"body\": \"### レース情報\\n- **開催日**: 2024年5月5日\\n- **開催競馬場**: 東京競馬場\\n- **レース名**: NHKマイルC(G1)\\n- **距離**: 芝1600m\\n- **天候**: 晴\\n- **芝馬場状態**: 良\\n\\n### 出走馬の印と評価\\n#### 16. **ジャンタルマンタル （馬番: 16, 騎手: 川田将雅）**\\n- **印**: ◎\\n- **評価**: 皐月賞でも3着に入るなど、安定した実力を持っています。また、東京1600mの舞台でも前走の実績があることから、最有力候補です。\\n\\n#### 14. **アスコリピチェーノ （馬番: 14, 騎手: ルメール）**\\n- **印**: ○\\n- **評価**: 阪神ジュベナイルFでは勝利し、桜花賞でも好走するなど、確実な実力を顕示しています。東京コースの適性も高いため、上位争いが期待されます。\\n\\n#### 3. **ディスペランツァ （馬番: 3, 騎手: 鮫島克駿）**\\n- **印**: ▲\\n- **評価**: アーリントンCを勝利するなど、勢いのある馬です。馬体重も安定し、調教も順調であるため、要注意です。\\n\\n#### 6. **ロジリオン （馬番: 6, 騎手: 戸崎圭）**\\n- **印**: △\\n- **評価**: 強い馬でも食い下がる粘り強さがあり、東京の芝1600mでの実績もあります。スムーズな競馬ができれば好走する可能性が高いです。\\n\\n#### 12. **ゴンバデカーブース （馬番: 12, 騎手: モレイラ）**\\n- **印**: △\\n- **評価**: サウジアラビアRCでは勝利を収めており、実力馬です。調教師が状態が良いと評価している点もプラス材料です。\\n\\n#### 1. **ダノンマッキンリー （馬番: 1, 騎手: 北村友）**\\n- **印**: ☆\\n- **評価**: 中京でのタフな競馬を強いられながらも勝利しており、気性面の成長も見られます。東京芝1600mでもポテンシャルを発揮すれば有力です。\\n\\n### 最終予想\\n16. **ジャンタルマンタル**（◎）\\n14. **アスコリピチェーノ**（○）\\n3. **ディスペランツァ**（▲）\\n6. **ロジリオン**（△）\\n12. **ゴンバデカーブース**（△）\\n1. **ダノンマッキンリー**（☆）\"\r\n}";

        public RaceData RaceData { get; set; } = new RaceData();

        public Form1()
        {
            InitializeComponent();

            _apiService = new ApiService(_appSettings.ApiSettings.BaseUrl);
            _webScrapingService = new WebScrapingService();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblCondition.Text = "";
            lblRaceInfo.Text = "";
            toolStripStatusLabel1.Text = "";
            hiddenDateLabel.Text = dateTimePicker1.Value.ToString("yyyyMMdd");
        }

        private async void btnSearchRaceInfo_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Text = "レースリストを取得中...";

                // 選択された日付でレース情報を取得
                var selectedDate = dateTimePicker1.Value.ToString("yyyyMMdd");
                var raceInfos = await _apiService.GetRaceInfosByDateAsync(selectedDate);

                LoadRaceInfo(raceInfos);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラーが発生しました: {ex.Message}");
            }
            finally
            {
                toolStripStatusLabel1.Text = "完了";
            }
        }

        private async void LoadRaceInfo(List<RaceInfo> raceInfos)
        {
            dataGridView1.DataSource = null;
            listBox1.Items.Clear();

            foreach (var raceInfo in raceInfos)
            {
                listBox1.Items.Add($"{raceInfo.RaceCourse} {raceInfo.RaceNumber}");
            }

            if (raceInfos.Count == 0)
            {
                lblRaceInfo.Text = "該当するレース情報が見つかりません";
                return;
            }

            var firstRaceInfo = raceInfos[0];

            var horseRaces = await _apiService.GetHorseRacesByRaceIdAsync(firstRaceInfo.RaceId);

            BindHorseDataToGridView(horseRaces);

            lblRaceInfo.Text = $"{firstRaceInfo.RaceCourse} 競馬場 {firstRaceInfo.RaceNumber} {firstRaceInfo.RaceName} {firstRaceInfo.StartTime} 発走 {firstRaceInfo.Distance}";
            lblCondition.Text = $"天候：{firstRaceInfo.Weather} 芝：{firstRaceInfo.ShibaTrackCondition} ダ：{firstRaceInfo.DirtTrackCondition}";

            listBox1.SelectedIndex = 0;
        }

        private async void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            var selectedItem = listBox1.SelectedItem?.ToString();
            if (selectedItem == null) return;

            if (listBox1.SelectedItem != null)
            {
                var raceCourse = selectedItem.Split(' ')[0];
                var raceNumber = selectedItem.Split(' ')[1];

                var selectedRace = await _apiService.GetRaceInfoByDateRaceCourseRaceNumberAsync(hiddenDateLabel.Text, raceCourse, raceNumber);

                if (selectedRace != null)
                {
                    var horseRaces = await _apiService.GetHorseRacesByRaceIdAsync(selectedRace.RaceId);
                    if (horseRaces.Count != 0)
                    {
                        BindHorseDataToGridView(horseRaces);

                        lblRaceInfo.Text = $"{selectedRace.RaceCourse}競馬場 {selectedRace.RaceNumber} {selectedRace.RaceName} {selectedRace.StartTime} {selectedRace.Distance}";
                        lblCondition.Text = $"天候：{selectedRace.Weather} 芝：{selectedRace.ShibaTrackCondition} ダ：{selectedRace.DirtTrackCondition}";

                        RaceData.RaceInformation = RaceInformation.ConvertToRaceInformation(selectedRace);
                        RaceData.RaceInformation.Horses = horseRaces.Select(x => RaceInformation.ConvertToHorceRaceInfo(x)).ToList();
                        foreach (var horseRace in RaceData.RaceInformation.Horses)
                        {
                            var pastRaces = await _apiService.GetPastRacesAsync(horseRace.KettoNum);
                            horseRace.PastRaces = pastRaces.Select(x => RaceInformation.ConvertToPastRaceInfo(x)).ToList();
                        }
                    }
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            hiddenDateLabel.Text = dateTimePicker1.Value.ToString("yyyyMMdd");
        }

        private void BindHorseDataToGridView(List<HorseRace> horseRaces)
        {
            dataGridView1.DataSource = horseRaces.Select(r => new GridViewModel
            {
                枠 = r.Wakuban,
                馬番 = r.Umaban,
                馬名 = r.HorseName,
                性齢 = r.GenderAge,
                斤量 = r.Kinryo,
                騎手名 = r.Jockey,
                馬体重 = r.WeightChange,
                予想印 = "",
                調教タイム = r.TrainingTime,
                厩舎コメント = r.TrainerComment
            }).ToList();
        }

        private async void btnUpdateRaces_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "データ登録中...";
            var selectedDate = dateTimePicker1.Value.ToString("yyyyMMdd");
            toolStripStatusLabel1.Text = await _webScrapingService.UpsertRaceInfosAsync(selectedDate);
        }

        private async void btnPredict_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Text = "予想中...";
                richTextBoxHorseInfo.Clear();

                var requestUrl = "https://api.openai.com/v1/chat/completions";
                var data = JsonConvert.SerializeObject(RaceData, Formatting.Indented);
                var jsonContent = JsonConvert.SerializeObject(data);

                var requestBody = new
                {
                    model = "gpt-4o",
                    messages = new[]
                    {
                    new { role = "system",
                          content = $"競馬の予想だよ。これから伝えるJSON形式の情報で、どの馬が3着以内になりそうかを客観的な意見を交えて教えて欲しい。" +
                          $"文章で見解と評価をまとめて。あと、厩舎コメントの内容をそのまま流用しないで。調教タイム、競馬場や距離との相性、もからめて、人が読みたくなるような表現にしてねできれば5～10行の文章にしたい。それを◎(1個のみ)〇(1個のみ)▲(1個のみ)△(1個のみ☆(複数可)で印つけて表現して、予想する馬を1～6頭におさめてね。" +
                          $"印付ける順番は必ず◎→〇→▲→△→☆となるようにしてね" +
                          $"期待する馬がいない場合は、無理に印付けなくてもいいのでレースによっては予想頭数は1頭や2頭でもよい。特に未勝利のレースは"+
                          $"出力はJSON形式でtitleとbodyで出力して欲しい。" +
                          $"回答例：{ResponseExample}" +
                          $"{Environment.NewLine}" +
                          $"調教タイム(TrainingTime)は以下のルールで並んでいる。" +
                          $"{Environment.NewLine}" +
                          $"左から（ゴール前から逆算して）6F、5F、4F、3F、1Fのタイム。"+
                          $"{Environment.NewLine}" +
                          $"()内のタイムはラップ表示。その場合、6F-5F、5F-4F、4F-3F、3F-2F、2F-1Fのタイムが()内に表示されます" +
                          $"{Environment.NewLine}"+
                          $"それと回答の最後に以下の回答例のようにまとめて欲しい(先頭の最初の数字は馬番)"+
                          $"### 最終予想\\n#### 16. **ジャンタルマンタル**（◎）\\n#### 14. **アスコリピチェーノ**（○）\\n#### 3. **ディスペランツァ**（▲）\\n#### 6. **ロジリオン**（△）\\n#### 12. **ゴンバデカーブース**（△）\\n#### 1. **ダノンマッキンリー**（☆）"
                          }
                    ,
                    new { role = "user", content = $"{jsonContent}" },
                    }
                };
                var jsonRequestBody = JsonConvert.SerializeObject(requestBody);

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(requestUrl),
                    Headers = { { "Authorization", $"Bearer {_appSettings.OpenAI.ApiKey}" } },
                    Content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json")
                };

                var response = await new HttpClient().SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                var chatCompletion = JsonConvert.DeserializeObject<ChatCompletion>(responseBody);

                if (chatCompletion == null) { chatCompletion = new ChatCompletion(); }
                chatCompletion.Choices ??= new List<Choice> { new Choice() };

                var predictionText = chatCompletion.Choices[0].Message?.Content;
                richTextBoxHorseInfo.Text = predictionText!.Replace("```", "").Replace("json", "");

                var gridViewModels = UpdatePredictionMarks(chatCompletion.Choices[0].Message?.Content!);
                SortDataGridViewByPredictionMarks(gridViewModels);

                toolStripStatusLabel1.Text = "予想完了";
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = $"エラー：{ex.Message}";
            }
        }

        private List<GridViewModel> UpdatePredictionMarks(string predictionText)
        {
            var pattern = @"[`{}\""\[\]]|title: |body: |json";
            var replaceText = Regex.Replace(predictionText, pattern, "");

            var lines = replaceText.Split("\\n");
            var markMap = new Dictionary<string, string>();

            foreach (var line in lines)
            {
                if (line.Contains('◎')) markMap[ExtractHorseName(line)] = "◎";
                else if (line.Contains('○')) markMap[ExtractHorseName(line)] = "○";
                else if (line.Contains('▲')) markMap[ExtractHorseName(line)] = "▲";
                else if (line.Contains('△')) markMap[ExtractHorseName(line)] = "△";
                else if (line.Contains('☆')) markMap[ExtractHorseName(line)] = "☆";
            }

            var gridViewModels = new List<GridViewModel>();

            for (var i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var gridViewModel = new GridViewModel
                {
                    枠 = dataGridView1.Rows[i].Cells[0].Value?.ToString()!,
                    馬番 = dataGridView1.Rows[i].Cells[1].Value?.ToString()!,
                    馬名 = dataGridView1.Rows[i].Cells[2].Value?.ToString()!,
                    性齢 = dataGridView1.Rows[i].Cells[3].Value?.ToString()!,
                    斤量 = dataGridView1.Rows[i].Cells[4].Value?.ToString()!,
                    騎手名 = dataGridView1.Rows[i].Cells[5].Value?.ToString()!,
                    馬体重 = dataGridView1.Rows[i].Cells[6].Value?.ToString()!,
                    調教タイム = dataGridView1.Rows[i].Cells[8].Value?.ToString(),
                    厩舎コメント = dataGridView1.Rows[i].Cells[9].Value?.ToString(),
                };

                var horseName = dataGridView1.Rows[i].Cells[2].Value?.ToString();
                if (horseName != null && markMap.ContainsKey(horseName))
                {
                    gridViewModel.予想印 = markMap[horseName];
                }
                gridViewModels.Add(gridViewModel);
            }
            return gridViewModels;
        }

        private string ExtractHorseName(string line)
        {
            var match = Regex.Match(line, @"\*\*(.*?)\*\*");
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        private void SortDataGridViewByPredictionMarks(List<GridViewModel> gridViewModels)
        {
            var rows = gridViewModels.Where(r => r.予想印 != null).OrderBy(r =>
            {
                var mark = r.予想印;
                var order = mark switch
                {
                    "◎" => 1,
                    "○" => 2,
                    "▲" => 3,
                    "△" => 4,
                    "☆" => 5,
                    _ => 6
                };
                return (order, int.Parse(r.馬番));
            }).ToList();

            dataGridView1.DataSource = null;

            dataGridView1.DataSource = rows.Select(r => new GridViewModel
            {
                枠 = r.枠,
                馬番 = r.馬番,
                馬名 = r.馬名,
                性齢 = r.性齢,
                斤量 = r.斤量,
                騎手名 = r.騎手名,
                馬体重 = r.馬体重,
                予想印 = r.予想印,
                調教タイム = r.調教タイム,
                厩舎コメント = r.厩舎コメント
            }).ToList();
        }

        private async void btnPutnote_Click(object sender, EventArgs e)
        {
            var result = await NoteService.PutNote(richTextBoxHorseInfo.Text, _appSettings.NoteConfig);

            lblnoteURL.Text = result;
            toolStripStatusLabel1.Text = $"Note投稿完了 {result}";
        }

        private async void btnLINE_Click(object sender, EventArgs e)
        {
            var channelAccessToken = _appSettings.LineConfig.ChannelAccessToken;
            var lineUserId = _appSettings.LineConfig.UserId;

            var notifier = new LineService(_appSettings.LineConfig);

            var messages = new List<string> { $"{lblRaceInfo.Text}ね！予想してやるよ。{Environment.NewLine}" };

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    var umaban = row.Cells[1].Value?.ToString();
                    var horseName = row.Cells[2].Value?.ToString();
                    var prediction = row.Cells[7].Value?.ToString();

                    if (!string.IsNullOrEmpty(umaban) && !string.IsNullOrEmpty(horseName) && !string.IsNullOrEmpty(prediction))
                    {
                        var message = $"{umaban}番 {horseName} {prediction}";
                        messages.Add(message);
                    }
                }
            }

            var combinedMessage = string.Join("\n", messages);
            combinedMessage += $"{Environment.NewLine}{Environment.NewLine}予想してやってんだぞ！ありがたく思え！カスが！";
            await notifier.SendMessageAsync(combinedMessage);
        }

        private async void btnX_Click(object sender, EventArgs e)
        {
            var messages = new List<string>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    var umaban = row.Cells[1].Value?.ToString();
                    var horseName = row.Cells[2].Value?.ToString();
                    var prediction = row.Cells[7].Value?.ToString();

                    if (!string.IsNullOrEmpty(umaban) && !string.IsNullOrEmpty(horseName) && !string.IsNullOrEmpty(prediction))
                    {
                        var message = $"{umaban}番 {horseName} {prediction}";
                        messages.Add(message);
                    }
                }
            }
            var combinedMessage = string.Join("\n", messages);

            var tweetMessage = TweetService.MakeTweetMessage(lblRaceInfo.Text, combinedMessage, lblnoteURL.Text);

            var tweetService = new TweetService(_appSettings.TwitterConfig);
            var id = await tweetService.Tweet(tweetMessage);

            richTextBoxTweetMessage.Text = tweetMessage;
            toolStripStatusLabel1.Text = $"Xにポスト完了 {id}";
        }

        private void btnOrePuro_Click(object sender, EventArgs e)
        {
            var autoRunPath = @"C:\UmaPoyo\UMAAuto\bin\Release\UMAAuto";

            var argURLData = $"{RaceData.RaceInformation.RaceId}";

            var datas = "";
            var dataGridContents = new List<DataGridContent>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    var dataGridContent = new DataGridContent();

                    dataGridContent.Umaban = row.Cells[1].Value?.ToString()!;
                    dataGridContent.Bamei = row.Cells[2].Value?.ToString()!;
                    dataGridContent.Mark = row.Cells[7].Value?.ToString()!;

                    dataGridContents.Add(dataGridContent);
                }
            }
            var orePuroDataList = dataGridContents.Where(x => x.Mark != "").ToList();

            foreach (var item in orePuroDataList)
            {
                datas += item.Umaban + ",";
            }
            datas = datas.TrimEnd(',');

            var processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = autoRunPath;
            processStartInfo.Arguments = $"{argURLData} {datas}";

            var p = Process.Start(processStartInfo);
        }
    }
}