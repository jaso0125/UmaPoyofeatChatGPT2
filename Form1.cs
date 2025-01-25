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
        private const string ResponseExample = "{\r\n  \"title\": \"2024�N5��5�� �������n�� 11R NHK�}�C��C(G1)\",\r\n  \"body\": \"### ���[�X���\\n- **�J�Ó�**: 2024�N5��5��\\n- **�J�Ë��n��**: �������n��\\n- **���[�X��**: NHK�}�C��C(G1)\\n- **����**: ��1600m\\n- **�V��**: ��\\n- **�Ŕn����**: ��\\n\\n### �o���n�̈�ƕ]��\\n#### 16. **�W�����^���}���^�� �i�n��: 16, �R��: ��c����j**\\n- **��**: ��\\n- **�]��**: �H���܂ł�3���ɓ���ȂǁA���肵�����͂������Ă��܂��B�܂��A����1600m�̕���ł��O���̎��т����邱�Ƃ���A�ŗL�͌��ł��B\\n\\n#### 14. **�A�X�R���s�`�F�[�m �i�n��: 14, �R��: �����[���j**\\n- **��**: ��\\n- **�]��**: ��_�W���x�i�C��F�ł͏������A���ԏ܂ł��D������ȂǁA�m���Ȏ��͂��������Ă��܂��B�����R�[�X�̓K�����������߁A��ʑ��������҂���܂��B\\n\\n#### 3. **�f�B�X�y�����c�@ �i�n��: 3, �R��: �L�����x�j**\\n- **��**: ��\\n- **�]��**: �A�[�����g��C����������ȂǁA�����̂���n�ł��B�n�̏d�����肵�A�����������ł��邽�߁A�v���ӂł��B\\n\\n#### 6. **���W���I�� �i�n��: 6, �R��: �ˍ�\�j**\\n- **��**: ��\\n- **�]��**: �����n�ł��H��������S�苭��������A�����̎�1600m�ł̎��т�����܂��B�X���[�Y�ȋ��n���ł���΍D������\���������ł��B\\n\\n#### 12. **�S���o�f�J�[�u�[�X �i�n��: 12, �R��: �����C���j**\\n- **��**: ��\\n- **�]��**: �T�E�W�A���r�ARC�ł͏��������߂Ă���A���͔n�ł��B�����t����Ԃ��ǂ��ƕ]�����Ă���_���v���X�ޗ��ł��B\\n\\n#### 1. **�_�m���}�b�L�����[ �i�n��: 1, �R��: �k���F�j**\\n- **��**: ��\\n- **�]��**: �����ł̃^�t�ȋ��n���������Ȃ�����������Ă���A�C���ʂ̐����������܂��B������1600m�ł��|�e���V�����𔭊�����ΗL�͂ł��B\\n\\n### �ŏI�\�z\\n16. **�W�����^���}���^��**�i���j\\n14. **�A�X�R���s�`�F�[�m**�i���j\\n3. **�f�B�X�y�����c�@**�i���j\\n6. **���W���I��**�i���j\\n12. **�S���o�f�J�[�u�[�X**�i���j\\n1. **�_�m���}�b�L�����[**�i���j\"\r\n}";

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
                toolStripStatusLabel1.Text = "���[�X���X�g���擾��...";

                // �I�����ꂽ���t�Ń��[�X�����擾
                var selectedDate = dateTimePicker1.Value.ToString("yyyyMMdd");
                var raceInfos = await _apiService.GetRaceInfosByDateAsync(selectedDate);

                LoadRaceInfo(raceInfos);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�G���[���������܂���: {ex.Message}");
            }
            finally
            {
                toolStripStatusLabel1.Text = "����";
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
                lblRaceInfo.Text = "�Y�����郌�[�X��񂪌�����܂���";
                return;
            }

            var firstRaceInfo = raceInfos[0];

            var horseRaces = await _apiService.GetHorseRacesByRaceIdAsync(firstRaceInfo.RaceId);

            BindHorseDataToGridView(horseRaces);

            lblRaceInfo.Text = $"{firstRaceInfo.RaceCourse} ���n�� {firstRaceInfo.RaceNumber} {firstRaceInfo.RaceName} {firstRaceInfo.StartTime} ���� {firstRaceInfo.Distance}";
            lblCondition.Text = $"�V��F{firstRaceInfo.Weather} �ŁF{firstRaceInfo.ShibaTrackCondition} �_�F{firstRaceInfo.DirtTrackCondition}";

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

                        lblRaceInfo.Text = $"{selectedRace.RaceCourse}���n�� {selectedRace.RaceNumber} {selectedRace.RaceName} {selectedRace.StartTime} {selectedRace.Distance}";
                        lblCondition.Text = $"�V��F{selectedRace.Weather} �ŁF{selectedRace.ShibaTrackCondition} �_�F{selectedRace.DirtTrackCondition}";

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
                �g = r.Wakuban,
                �n�� = r.Umaban,
                �n�� = r.HorseName,
                ���� = r.GenderAge,
                �җ� = r.Kinryo,
                �R�薼 = r.Jockey,
                �n�̏d = r.WeightChange,
                �\�z�� = "",
                �����^�C�� = r.TrainingTime,
                �X�ɃR�����g = r.TrainerComment
            }).ToList();
        }

        private async void btnUpdateRaces_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "�f�[�^�o�^��...";
            var selectedDate = dateTimePicker1.Value.ToString("yyyyMMdd");
            toolStripStatusLabel1.Text = await _webScrapingService.UpsertRaceInfosAsync(selectedDate);
        }

        private async void btnPredict_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Text = "�\�z��...";
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
                          content = $"���n�̗\�z����B���ꂩ��`����JSON�`���̏��ŁA�ǂ̔n��3���ȓ��ɂȂ肻�������q�ϓI�Ȉӌ��������ċ����ė~�����B" +
                          $"���͂Ō����ƕ]�����܂Ƃ߂āB���ƁA�X�ɃR�����g�̓��e�����̂܂ܗ��p���Ȃ��ŁB�����^�C���A���n��⋗���Ƃ̑����A������߂āA�l���ǂ݂����Ȃ�悤�ȕ\���ɂ��Ă˂ł����5�`10�s�̕��͂ɂ������B�������(1�̂�)�Z(1�̂�)��(1�̂�)��(1�̂݁�(������)�ň���ĕ\�����āA�\�z����n��1�`6���ɂ����߂ĂˁB" +
                          $"��t���鏇�Ԃ͕K�������Z�������������ƂȂ�悤�ɂ��Ă�" +
                          $"���҂���n�����Ȃ��ꍇ�́A�����Ɉ�t���Ȃ��Ă������̂Ń��[�X�ɂ���Ă͗\�z������1����2���ł��悢�B���ɖ������̃��[�X��"+
                          $"�o�͂�JSON�`����title��body�ŏo�͂��ė~�����B" +
                          $"�񓚗�F{ResponseExample}" +
                          $"{Environment.NewLine}" +
                          $"�����^�C��(TrainingTime)�͈ȉ��̃��[���ŕ���ł���B" +
                          $"{Environment.NewLine}" +
                          $"������i�S�[���O����t�Z���āj6F�A5F�A4F�A3F�A1F�̃^�C���B"+
                          $"{Environment.NewLine}" +
                          $"()���̃^�C���̓��b�v�\���B���̏ꍇ�A6F-5F�A5F-4F�A4F-3F�A3F-2F�A2F-1F�̃^�C����()���ɕ\������܂�" +
                          $"{Environment.NewLine}"+
                          $"����Ɖ񓚂̍Ō�Ɉȉ��̉񓚗�̂悤�ɂ܂Ƃ߂ė~����(�擪�̍ŏ��̐����͔n��)"+
                          $"### �ŏI�\�z\\n#### 16. **�W�����^���}���^��**�i���j\\n#### 14. **�A�X�R���s�`�F�[�m**�i���j\\n#### 3. **�f�B�X�y�����c�@**�i���j\\n#### 6. **���W���I��**�i���j\\n#### 12. **�S���o�f�J�[�u�[�X**�i���j\\n#### 1. **�_�m���}�b�L�����[**�i���j"
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

                toolStripStatusLabel1.Text = "�\�z����";
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = $"�G���[�F{ex.Message}";
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
                if (line.Contains('��')) markMap[ExtractHorseName(line)] = "��";
                else if (line.Contains('��')) markMap[ExtractHorseName(line)] = "��";
                else if (line.Contains('��')) markMap[ExtractHorseName(line)] = "��";
                else if (line.Contains('��')) markMap[ExtractHorseName(line)] = "��";
                else if (line.Contains('��')) markMap[ExtractHorseName(line)] = "��";
            }

            var gridViewModels = new List<GridViewModel>();

            for (var i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var gridViewModel = new GridViewModel
                {
                    �g = dataGridView1.Rows[i].Cells[0].Value?.ToString()!,
                    �n�� = dataGridView1.Rows[i].Cells[1].Value?.ToString()!,
                    �n�� = dataGridView1.Rows[i].Cells[2].Value?.ToString()!,
                    ���� = dataGridView1.Rows[i].Cells[3].Value?.ToString()!,
                    �җ� = dataGridView1.Rows[i].Cells[4].Value?.ToString()!,
                    �R�薼 = dataGridView1.Rows[i].Cells[5].Value?.ToString()!,
                    �n�̏d = dataGridView1.Rows[i].Cells[6].Value?.ToString()!,
                    �����^�C�� = dataGridView1.Rows[i].Cells[8].Value?.ToString(),
                    �X�ɃR�����g = dataGridView1.Rows[i].Cells[9].Value?.ToString(),
                };

                var horseName = dataGridView1.Rows[i].Cells[2].Value?.ToString();
                if (horseName != null && markMap.ContainsKey(horseName))
                {
                    gridViewModel.�\�z�� = markMap[horseName];
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
            var rows = gridViewModels.Where(r => r.�\�z�� != null).OrderBy(r =>
            {
                var mark = r.�\�z��;
                var order = mark switch
                {
                    "��" => 1,
                    "��" => 2,
                    "��" => 3,
                    "��" => 4,
                    "��" => 5,
                    _ => 6
                };
                return (order, int.Parse(r.�n��));
            }).ToList();

            dataGridView1.DataSource = null;

            dataGridView1.DataSource = rows.Select(r => new GridViewModel
            {
                �g = r.�g,
                �n�� = r.�n��,
                �n�� = r.�n��,
                ���� = r.����,
                �җ� = r.�җ�,
                �R�薼 = r.�R�薼,
                �n�̏d = r.�n�̏d,
                �\�z�� = r.�\�z��,
                �����^�C�� = r.�����^�C��,
                �X�ɃR�����g = r.�X�ɃR�����g
            }).ToList();
        }

        private async void btnPutnote_Click(object sender, EventArgs e)
        {
            var result = await NoteService.PutNote(richTextBoxHorseInfo.Text, _appSettings.NoteConfig);

            lblnoteURL.Text = result;
            toolStripStatusLabel1.Text = $"Note���e���� {result}";
        }

        private async void btnLINE_Click(object sender, EventArgs e)
        {
            var channelAccessToken = _appSettings.LineConfig.ChannelAccessToken;
            var lineUserId = _appSettings.LineConfig.UserId;

            var notifier = new LineService(_appSettings.LineConfig);

            var messages = new List<string> { $"{lblRaceInfo.Text}�ˁI�\�z���Ă���B{Environment.NewLine}" };

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    var umaban = row.Cells[1].Value?.ToString();
                    var horseName = row.Cells[2].Value?.ToString();
                    var prediction = row.Cells[7].Value?.ToString();

                    if (!string.IsNullOrEmpty(umaban) && !string.IsNullOrEmpty(horseName) && !string.IsNullOrEmpty(prediction))
                    {
                        var message = $"{umaban}�� {horseName} {prediction}";
                        messages.Add(message);
                    }
                }
            }

            var combinedMessage = string.Join("\n", messages);
            combinedMessage += $"{Environment.NewLine}{Environment.NewLine}�\�z���Ă���Ă񂾂��I���肪�����v���I�J�X���I";
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
                        var message = $"{umaban}�� {horseName} {prediction}";
                        messages.Add(message);
                    }
                }
            }
            var combinedMessage = string.Join("\n", messages);

            var tweetMessage = TweetService.MakeTweetMessage(lblRaceInfo.Text, combinedMessage, lblnoteURL.Text);

            var tweetService = new TweetService(_appSettings.TwitterConfig);
            var id = await tweetService.Tweet(tweetMessage);

            richTextBoxTweetMessage.Text = tweetMessage;
            toolStripStatusLabel1.Text = $"X�Ƀ|�X�g���� {id}";
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