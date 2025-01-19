using Microsoft.Extensions.Configuration;

namespace UmaPoyofeatChatGPT2.Common
{
    public static class AppSettingsManager
    {
        private static IConfigurationRoot _configuration;

        static AppSettingsManager()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // プロジェクトの実行ディレクトリを基準に設定
                .AddJsonFile("AppSettings/appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }

        public static T GetSection<T>(string sectionName) where T : new()
        {
            var section = new T();
            _configuration.GetSection(sectionName).Bind(section);
            return section;
        }

        public static string GetConnectionString(string name)
        {
            return _configuration.GetConnectionString(name) ?? throw new InvalidOperationException($"Connection string '{name}' was not found.");
        }
    }
}