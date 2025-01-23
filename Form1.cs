using UmaPoyofeatChatGPT2.Services;

namespace UmaPoyofeatChatGPT2
{
    public partial class Form1 : Form
    {
        private readonly IRaceService _raceService;

        public Form1(IRaceService raceService)
        {
            _raceService = raceService;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 初期データのロード (例: 日付に基づいてレース情報を取得)
            var today = DateTime.Today.ToString("yyyy-MM-dd");
            LoadRaceInfo(today);
        }

        private void btnSearchRaceInfo_Click(object sender, EventArgs e)
        {
            // 選択された日付でレース情報を取得
            var selectedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            LoadRaceInfo(selectedDate);
        }

        private void LoadRaceInfo(string date)
        {
            var races = _raceService.GetAllHorseRaces()
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