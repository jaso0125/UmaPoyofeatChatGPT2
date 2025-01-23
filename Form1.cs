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
            var today = DateTime.Today.ToString("yyyy-MM-dd");
            LoadRaceInfo(today);
        }

        private void btnSearchRaceInfo_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            // 選択された日付でレース情報を取得
            var selectedDate = dateTimePicker1.Value.ToString("yyyyMMdd");

            var raceInfos = _raceInfoService.GetHorseRacesByDate(selectedDate);

            foreach (var raceInfo in raceInfos)
            {
                listBox1.Items.Add($"{raceInfo.RaceCourse} {raceInfo.RaceNumber}");
            }
            dataGridView1.Rows.Clear();

            if (raceInfos.Count == 0) { return; }

            var firstRaceList = raceInfos[0];

            var horceRaces = _horseRaceService.GetHorseRaceByRaceId(firstRaceList.RaceId);

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

            lblRaceInfo.Text = horceRaces.Any() ? $"レース情報が取得されました" : "該当するレース情報が見つかりません";
        }

        private void LoadRaceInfo(string date)
        {
            var races = _horseRaceService.GetAllHorseRaces()
                .Where(r => r.RaceId.StartsWith(date)) // 日付でフィルタリング
                .Select(r => new
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
                })
                .ToList();

            // DataGridView にデータを表示
            dataGridView1.DataSource = races;

            // データがなければラベルにメッセージを表示
            lblRaceInfo.Text = races.Any() ? "レース情報が取得されました" : "該当するレース情報が見つかりません";
        }
    }
}