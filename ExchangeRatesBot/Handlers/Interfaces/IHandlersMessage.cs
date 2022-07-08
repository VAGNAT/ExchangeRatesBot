using ExchangeRatesBot.Messages;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ExchangeRatesBot.Handlers.Interfaces
{
    public interface IHandlersMessage
    {
        Task<SentMessageModel> BotOnMessageReceivedAsync(Update update);
        SentMessageModel UnknownUpdateHandlerAsync();
    }
}
