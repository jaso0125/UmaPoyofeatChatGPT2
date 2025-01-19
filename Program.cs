using Microsoft.Extensions.Configuration;
using Serilog;

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
                // アプリケーションのメインロジック
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