using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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

        private const string ResponseWin5RaceExample = "{\r\n  \"title\": \"2024年5月5日 Win5の予想\",\r\n  \"body\": \"### 対象レース情報\\n- **開催日**: 2024年5月5日\\n- **1レース目**:中山9R 若潮S\\n **2レース目**:中京10R 豊川特別\\n **3レース目**:中山10R ジャニュアリ\\n **4レース目**:中京11R 日経新春杯\\n **5レース目**:中山11R 京成杯\\n ### レースごとの1着予想馬\\n #### 中山9R 若潮S\\n * 予想1： 1. **タシット （馬番: 1, 騎手: 横山和）**\\n     * **評価**： 中山1600mでの安定した成績を誇り、前走も2着と好走。調教タイムも優秀で、さらに上積みを期待できる条件。\\n * 予想2： 6. **ワンダイレクト （馬番: 6, 騎手: 藤岡佑）**\\n     * **評価**： 前走の不運なレースを払拭し、調教でも好タイムを記録。中山コースとも相性が良く、巻き返しに期待がかかる。\\n * 予想3： 9. **ニシノライコウ （馬番: 9, 騎手: 菅原明）**\\n     * **評価**： 中山1600mの実績は確か。久々でも崩れない気性はプラス要素。再び上位争いに加われる状況。\\n #### 中京10R 豊川特別 \\n  * 予想1： 2. **ドゥータップ （馬番: 2, 騎手: 松山）**\\n     * **評価**：前走は昇級初戦で2着に健闘し、上昇中の力強さがあります。中京1200mで良績があり、この条件なら引き続き好走が期待できそうです。\\n #### 中山10R ジャニュアリ\\n  * 予想1： 7. **ロードアウォード （馬番: 7, 騎手: ルメール）**\\n     * **評価**：前走は厳しい展開で3着まで持ち込む粘りを見せた実力馬。千二の距離に対応でき、中山コースが得意で期待大。調教もタイムが安定しており、上昇気配が伺える。\\n  * 予想2： 15. **ジュンウィンダム （馬番: 15, 騎手: 三浦）**\\n     * **評価**：前走はオープンの中でも上位に食い込み、自在性のある競馬を展開。距離が合っており、好位差しのしたたかな立ち回りが光ります。調教も順調です。\\n  * 予想3： 11. **ケイアイアニラ （馬番: 11, 騎手: キング）**\\n     * **評価**：前走フェアウェルSで勝利、時計も良く今回のレースに向けた調整も良好。スピード面での持ち味を発揮すれば一発がある要注意馬。\\n  * 予想4： 16. **スターターン （馬番: 16, 騎手: 戸崎圭）**\\n     * **評価**：軽いダート巧者として知られ、今回も中山コースでの変わり身を期待。ペルセウスSでの善戦を考慮。\\n   #### 中京11R 日経新春杯\\n  * 予想1： 3.  **ヴェルトライゼンデ （馬番: 3, 騎手: Aルメート）**\\n     * **評価**：一度叩かれた脚が力強さを増しており、舞台適性も高く、地力勝負に期待したい。重賞でも粘り強さを見せているため首位候補。\\n   #### 中山11R 京成杯\\n  * 予想1： 9. **キングノジョー （馬番: 9, 騎手: ルメール）**\\n     * **評価**：新馬戦での完勝は鮮烈で、長い距離をしっかりと走り切るスタミナを持っています。中山芝2000mの適性も高そうで、さらに名手ルメールの手腕も期待できるばかりです。\\n  * 予想2： 2. **ニシノエージェント （馬番: 2, 騎手: 津村）**\\n     * **評価**：切れる脚を持ち、競馬を進める上での巧みさが光ります。中山コースでの経験もあり、相性の良さが上位進出のポイントになるでしょう。\\n  * 予想3： 5. **ガルダイア （馬番: 5, 騎手: 杉原）**\\n     * **評価**：競馬の駆け引きの面で成熟してきた感があります。距離延長の芝2000mでの初戦となりますが、適正を引き出せると期待されます。\\n  * 予想4： 1. **タイセイリコルド （馬番: 1, 騎手: 石橋脩）**\\n     * **評価**：先行してレースを進める脚質を持っており、中山コースのタイトさを味方にできそうです。調教タイムも好感触で安定した走りに期待できます。\\n  * 予想5： 7. **コスモストーム （馬番: 7, 騎手: 秋山稔）**\\n     * **評価**：芝での初戦とは違い今度は条件が合う形に。距離適性が試される一戦だが、成長を示せば大きな結果を勝ち取ることができそうです。\\n ### WIN5組み合わせ数\\n 3 × 1 × 4 × 1 × 5 = 60通り\" \r\n}";

        public RaceData RaceData { get; set; } = new RaceData();
        public Win5RaceData Win5RaceData { get; set; } = new Win5RaceData();

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

                if (UmapoyoTab.SelectedIndex == 0)
                {
                    var raceInfos = await _apiService.GetRaceInfosByDateAsync(selectedDate);
                    LoadRaceInfo(raceInfos);
                }
                else if (UmapoyoTab.SelectedIndex == 1)
                {
                    var win5RaceInfos = await _apiService.GetWin5RaceInfosByDateAsync(selectedDate);
                    Win5RaceData.RaceInformations = win5RaceInfos.Select(x => RaceInformation.ConvertToRaceInformation(x)).ToList();
                    LoadWin5RaceInfo(win5RaceInfos);
                }
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

        private async void LoadWin5RaceInfo(List<RaceInfo> win5RafeInfos)
        {
            dataGridView2.DataSource = null;

            if (win5RafeInfos.Count == 0)
            {
                lblRaceInfo.Text = "該当するレース情報が見つかりません";
                return;
            }

            dataGridView2.DataSource = win5RafeInfos.Select(r => new GridViewModelWin5
            {
                レースNo = r.RaceNumber!,
                競馬場 = r.RaceCourse,
                レース名 = r.RaceName!,
                発走時刻 = r.StartTime,
                予想1 = "",
                予想2 = "",
                予想3 = "",
                予想4 = "",
                予想5 = "",
            }).ToList();
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

        private async void btnUpdateRace_Click(object sender, EventArgs e)
        {
            var selectedItem = listBox1.SelectedItem?.ToString();
            var selectedDate = dateTimePicker1.Value.ToString("yyyyMMdd");

            if (selectedItem == null) return;

            if (listBox1.SelectedItem != null)
            {
                var raceCourse = selectedItem.Split(' ')[0];
                var raceNumber = selectedItem.Split(' ')[1];

                var selectedRace = await _apiService.GetRaceInfoByDateRaceCourseRaceNumberAsync(hiddenDateLabel.Text, raceCourse, raceNumber);

                toolStripStatusLabel1.Text = await _webScrapingService.UpsertRaceInfoAsync(selectedRace.RaceId);
            }
        }

        private void UmapoyoTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UmapoyoTab.SelectedIndex == 0)
            {
                listBox1.Enabled = true;
                btnPredict.Enabled = true;
                Win5PredictButton.Enabled = false;
            }
            else if (UmapoyoTab.SelectedIndex == 1)
            {
                listBox1.Enabled = false;
                btnPredict.Enabled = false;
                Win5PredictButton.Enabled = true;
            }
        }

        private async void Win5PredictButton_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Text = "Win5レース予想中...";

                if (Win5RaceData.RaceInformations.Count != 0)
                {
                    foreach (var raceInformation in Win5RaceData.RaceInformations)
                    {
                        var horseRaces = await _apiService.GetHorseRacesByRaceIdAsync(raceInformation.RaceId);

                        if (horseRaces.Count != 0)
                        {
                            raceInformation.Horses = horseRaces.Select(x => RaceInformation.ConvertToHorceRaceInfo(x)).ToList();
                            foreach (var horseRace in raceInformation.Horses)
                            {
                                var pastRaces = await _apiService.GetPastRacesAsync(horseRace.KettoNum);
                                horseRace.PastRaces = pastRaces.Select(x => RaceInformation.ConvertToPastRaceInfo(x)).Take(3).ToList();
                            }
                        }
                    }
                }

                var requestUrl = "https://api.openai.com/v1/chat/completions";
                var data = JsonConvert.SerializeObject(Win5RaceData, Formatting.Indented);
                var jsonContent = JsonConvert.SerializeObject(data);

                var requestBody = new
                {
                    model = "gpt-4o",
                    messages = new[]
    {
                    new { role = "system",
                          content = $"競馬のWIN5レースの予想だよ。これから伝えるJSON形式のWIN5対象5レースの情報で、どの馬が1着になりそうかを客観的な意見を交えて教えて欲しい。" +
                          $"文章で見解と評価をまとめて。あと、厩舎コメントの内容をそのまま流用しないで。調教タイム、競馬場や距離との相性、もからめて、人が読みたくなるような表現にしてねできれば5～10行の文章にしたい。1レースにつき予想する馬1～5頭におさめてね。" +
                          $"期待する馬がいない場合は、無理に5頭予想しなくていいでレースによっては予想しり馬の頭数は1頭や2頭でもよい。"+
                          $"出力はJSON形式でtitleとbodyで出力して欲しい。" +
                          $"回答例：{ResponseWin5RaceExample}"
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

                var gridViewModels = UpdateWin5PredictionData(chatCompletion.Choices[0].Message?.Content!);

                toolStripStatusLabel1.Text = "Win5レース予想完了...";
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = $"エラー:{ex.Message}";
            }
        }

        private List<GridViewModelWin5> UpdateWin5PredictionData(string predictionText)
        {
            var pattern = @"[`{}\""\[\]]|title: |body: |json";
            var replaceText = Regex.Replace(predictionText, pattern, "");

            var lines = replaceText.Split("\\n");

            return new List<GridViewModelWin5>();
        }
    }
}