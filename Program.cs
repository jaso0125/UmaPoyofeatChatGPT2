using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using UmaPoyofeatChatGPT2.Models;
using UmaPoyofeatChatGPT2.Services;

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

            // サービスコレクションを作成
            var serviceCollection = new ServiceCollection();

            // 接続文字列（適宜 appsettings.json から読み取る処理を追加）
            var connectionString = configuration["ConnectionStrings:DefaultConnection"];

            // DbContext を登録
            serviceCollection.AddDbContext<UmaPoyofeatChatGpt2Context>(options => options.UseSqlServer(connectionString));

            // サービスの登録
            serviceCollection.AddScoped<IHorseRaceService, HorseRaceService>();
            serviceCollection.AddScoped<IPastRaceService, PastRaceService>();
            serviceCollection.AddScoped<IRaceInfoService, RaceInfoService>();
            serviceCollection.AddScoped<IResultService, ResultService>();

            // サービスプロバイダーを構築
            var serviceProvider = serviceCollection.BuildServiceProvider();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Form1 を依存性注入で生成
            var form = ActivatorUtilities.CreateInstance<Form1>(serviceProvider);
            Application.Run(form);
        }
    }
}