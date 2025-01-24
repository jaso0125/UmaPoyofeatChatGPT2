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
            var selectedDate = dateTimePicker1.Value.ToString("yyyyMMdd");
            toolStripStatusLabel1.Text = await _webScrapingService.UpsertRaceInfosAsync(selectedDate);
        }
    }
}