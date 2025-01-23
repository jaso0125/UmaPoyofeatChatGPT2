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
            // �����f�[�^�̃��[�h (��: ���t�Ɋ�Â��ă��[�X�����擾)
            var today = DateTime.Today.ToString("yyyyMMdd");
            LoadRaceInfo(today);
        }

        private void btnSearchRaceInfo_Click(object sender, EventArgs e)
        {
            // �I�����ꂽ���t�Ń��[�X�����擾
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
                lblRaceInfo.Text = "�Y�����郌�[�X��񂪌�����܂���";
                return;
            }

            var firstRaceInfo = raceInfos[0];

            var horceRaces = _horseRaceService.GetHorseRaceByRaceId(firstRaceInfo.RaceId);

            dataGridView1.DataSource = horceRaces.Select(r => new
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

            lblRaceInfo.Text = $"{firstRaceInfo.RaceCourse} ���n�� {firstRaceInfo.RaceNumber} {firstRaceInfo.RaceName} {firstRaceInfo.StartTime} ���� {firstRaceInfo.Distance}";
            lblCondition.Text = $"�V��F{firstRaceInfo.Weather} �ŁF{firstRaceInfo.ShibaTrackCondition} �_�F{firstRaceInfo.DirtTrackCondition}";

            listBox1.SelectedIndex = 0;
        }
    }
}