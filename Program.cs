using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using UmaPoyofeatChatGPT2.Models;
using UmaPoyofeatChatGPT2.Service;

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

            var logFilePath = configuration["Logging:File:Path"] ?? "logs/log-.txt";

            // �T�[�r�X�R���N�V�������쐬
            var serviceCollection = new ServiceCollection();

            // �ڑ�������i�K�X appsettings.json ����ǂݎ�鏈����ǉ��j
            var connectionString = configuration["ConnectionStrings:DefaultConnection"];

            // DbContext ��o�^
            serviceCollection.AddDbContext<UmaPoyofeatChatGpt2Context>(options => options.UseSqlServer(connectionString));

            // �K�v�ȑ��̃T�[�r�X���o�^�i��: ���̑��̈ˑ��֌W�j
            serviceCollection.AddTransient<ExampleService>();

            // �T�[�r�X�v���o�C�_�[���\�z
            var serviceProvider = serviceCollection.BuildServiceProvider();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .WriteTo.File(
                    logFilePath,
                    rollingInterval: RollingInterval.Day
                )
                .CreateLogger();

            Log.Information("Application started");

            try
            {
                // �A�v���P�[�V�����̃��C�����W�b�N
                Log.Information("Running the application...");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unhandled exception occurred.");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}