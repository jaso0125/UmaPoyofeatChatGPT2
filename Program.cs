using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UmaPoyofeatChatGPT2.Models;

namespace UmaPoyofeatChatGPT2
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("AppSettings/appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            // �T�[�r�X�R���N�V�������쐬
            var serviceCollection = new ServiceCollection();

            // �ڑ�������i�K�X appsettings.json ����ǂݎ�鏈����ǉ��j
            var connectionString = configuration["ConnectionStrings:DefaultConnection"];

            // DbContext ��o�^
            serviceCollection.AddDbContext<UmaPoyofeatChatGpt2Context>(options => options.UseSqlServer(connectionString));

            // �T�[�r�X�v���o�C�_�[���\�z
            var serviceProvider = serviceCollection.BuildServiceProvider();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Form1 ���ˑ��������Ő���
            var form = ActivatorUtilities.CreateInstance<Form1>(serviceProvider);
            Application.Run(form);
        }
    }
}