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
            var today = DateTime.Today.ToString("yyyy-MM-dd");
            LoadRaceInfo(today);
        }

        private void btnSearchRaceInfo_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            // �I�����ꂽ���t�Ń��[�X�����擾
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

            lblRaceInfo.Text = horceRaces.Any() ? $"���[�X��񂪎擾����܂���" : "�Y�����郌�[�X��񂪌�����܂���";
        }

        private void LoadRaceInfo(string date)
        {
            var races = _horseRaceService.GetAllHorseRaces()
                .Where(r => r.RaceId.StartsWith(date)) // ���t�Ńt�B���^�����O
                .Select(r => new
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
                })
                .ToList();

            // DataGridView �Ƀf�[�^��\��
            dataGridView1.DataSource = races;

            // �f�[�^���Ȃ���΃��x���Ƀ��b�Z�[�W��\��
            lblRaceInfo.Text = races.Any() ? "���[�X��񂪎擾����܂���" : "�Y�����郌�[�X��񂪌�����܂���";
        }
    }
}