using System.Threading.Tasks;
using ExchangeRatesBot.Bot;
using ExchangeRatesBot.Handlers;
using ExchangeRatesBot.Messages;
using ExchangeRatesBot.Services;
using Telegram.Bot;
using Microsoft.Extensions.DependencyInjection;
using ExchangeRatesBot.Services.Interfaces;
using ExchangeRatesBot.Messages.Interfaces;
using ExchangeRatesBot.Handlers.Interfaces;
using ExchangeRatesBot.Bot.Interfaces;
using Microsoft.Extensions.Logging;

namespace ExchangeRatesBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder => builder.AddConsole())
                .AddSingleton<IExchangeRatesService, ExchangeRatesService>()
                .AddSingleton<ISentMessage, SentMessage>()
                .AddSingleton<IHandlerTextMessage, HandlerTextMessage>()
                .AddSingleton<IHandlersMessage, HandlersMessage>()
                .AddSingleton<ITelegramBotClient>(x => new TelegramBotClient(Configuration.BotToken))
                .AddSingleton<IBot, BotExchangeRate>()
                .BuildServiceProvider();

            await serviceProvider.GetService<IBot>().StartBotAsync();
        }
    }
}
