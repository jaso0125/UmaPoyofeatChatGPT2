using UmaPoyofeatChatGPT2.Services;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace UmaPoyofeatChatGPT2
{
    public partial class Form1 : Form
    {
        private readonly IHorseRaceService _horseRaceService;
        private readonly IRaceInfoService _raceInfoService;

        public Form1(IHorseRaceService raceService, IRaceInfoService raceInfoService)
        {
            _horseRaceService = raceService;
            _raceInfoService = raceInfoService;
            InitializeComponent();
            lblCondition.Text = "";
            lblRaceInfo.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 初期データのロード (例: 日付に基づいてレース情報を取得)
            var today = DateTime.Today.ToString("yyyyMMdd");
            LoadRaceInfo(today);
        }

        private void btnSearchRaceInfo_Click(object sender, EventArgs e)
        {
            // 選択された日付でレース情報を取得
            var selectedDate = dateTimePicker1.Value.ToString("yyyyMMdd");
            LoadRaceInfo(selectedDate);
        }

        private void LoadRaceInfo(string date)
        {
            listBox1.Items.Clear();

            var raceInfos = _raceInfoService.GetHorseRacesByDate(date);

            foreach (var raceInfo in raceInfos)
            {
                listBox1.Items.Add($"{raceInfo.RaceCourse} {raceInfo.RaceNumber}");
            }
            dataGridView1.Rows.Clear();

            if (raceInfos.Count == 0)
            {
                lblRaceInfo.Text = "該当するレース情報が見つかりません";
                return;
            }

            var firstRaceInfo = raceInfos[0];

            var horceRaces = _horseRaceService.GetHorseRaceByRaceId(firstRaceInfo.RaceId);

            dataGridView1.DataSource = horceRaces.Select(r => new
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

            lblRaceInfo.Text = $"{firstRaceInfo.RaceCourse} 競馬場 {firstRaceInfo.RaceNumber} {firstRaceInfo.RaceName} {firstRaceInfo.StartTime} 発走 {firstRaceInfo.Distance}";
            lblCondition.Text = $"天候：{firstRaceInfo.Weather} 芝：{firstRaceInfo.ShibaTrackCondition} ダ：{firstRaceInfo.DirtTrackCondition}";

            listBox1.SelectedIndex = 0;
        }
    }
}