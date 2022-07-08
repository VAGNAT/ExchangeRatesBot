using System.Threading.Tasks;

namespace ExchangeRatesBot.Handlers.Interfaces
{
    public interface IHandlerTextMessage
    {
        string UnknownUpdateTypeMessage();
        string HandleStart();
        string HandleHelp();
        Task<string> HandleExchangeRatesAsync(string message);
    }
}
