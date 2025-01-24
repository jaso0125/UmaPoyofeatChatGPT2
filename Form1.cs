using UmaPoyofeatChatGPT2.Common;
using UmaPoyofeatChatGPT2.Models;
using UmaPoyofeatChatGPT2.Services;

namespace UmaPoyofeatChatGPT2
{
    public partial class Form1 : Form
    {
        private readonly ApiService _apiService;
        private readonly WebScrapingService _webScrapingService;

        public Form1()
        {
            InitializeComponent();
            var appSettings = AppSettingsManager.GetSection<AppSettings.AppSettings>("AppSettings");

            _apiService = new ApiService(appSettings.ApiSettings.BaseUrl);
            _webScrapingService = new WebScrapingService();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblCondition.Text = "";
            lblRaceInfo.Text = "";
            toolStripStatusLabel1.Text = "";
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
            dataGridView1.DataSource = horseRaces.Select(r => new
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
            var selectedDate = dateTimePicker1.Value.ToString("yyyyMMdd");
            toolStripStatusLabel1.Text = await _webScrapingService.UpsertRaceInfosAsync(selectedDate);
        }
    }
}