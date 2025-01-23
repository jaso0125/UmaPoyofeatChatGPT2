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
                    Wakuban = r.Wakuban,
                    Umaban = r.Umaban,
                    Bamei = r.HorseName,
                    SexBarei = r.GenderAge,
                    Kinryo = r.Kinryo,
                    KisyuName = r.Jockey,
                    Bataijyu = r.WeightChange,
                    Time = r.TrainingTime,
                    Comment = r.TrainerComment
                })
                .ToList();

            // DataGridView にデータを表示
            dataGridView1.DataSource = races;

            // データがなければラベルにメッセージを表示
            lblRaceInfo.Text = races.Any() ? "レース情報が取得されました" : "該当するレース情報が見つかりません";
        }
    }
}