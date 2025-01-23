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
            // �����f�[�^�̃��[�h (��: ���t�Ɋ�Â��ă��[�X�����擾)
            var today = DateTime.Today.ToString("yyyy-MM-dd");
            LoadRaceInfo(today);
        }

        private void btnSearchRaceInfo_Click(object sender, EventArgs e)
        {
            // �I�����ꂽ���t�Ń��[�X�����擾
            var selectedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            LoadRaceInfo(selectedDate);
        }

        private void LoadRaceInfo(string date)
        {
            var races = _raceService.GetAllHorseRaces()
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