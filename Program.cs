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

            // サービスコレクションを作成
            var serviceCollection = new ServiceCollection();

            // 接続文字列（適宜 appsettings.json から読み取る処理を追加）
            var connectionString = configuration["ConnectionStrings:DefaultConnection"];

            // DbContext を登録
            serviceCollection.AddDbContext<UmaPoyofeatChatGpt2Context>(options => options.UseSqlServer(connectionString));

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