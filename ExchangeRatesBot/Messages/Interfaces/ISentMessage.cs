using System;

namespace ExchangeRatesBot.Messages.Interfaces
{
    public interface ISentMessage
    {
        string GetHelpText(string currencies);
        string GetStartText();
        string GetUnknownUpdateTypeMessage();
        string GetIncorrectFormatMessagesText();
        string GetIncorrectDateText();
        string GetDateMoreCurrentDateText();
        string GetServiceIsUnavailableText();
        string GetExchangeRatesText(DateTime date, string currency, decimal rate);
    }
}
