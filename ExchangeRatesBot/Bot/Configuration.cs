using Microsoft.Extensions.Configuration;

namespace ExchangeRatesBot.Bot
{
    internal static class Configuration
    {        
        private readonly static string _token;
        public static string BotToken => _token;
        static Configuration()
        {
            var configuration = new ConfigurationBuilder().AddUserSecrets<BotExchangeRate>().Build();
            _token = configuration.GetSection("TokenExchangeRate").Value;
        }
    }
}
