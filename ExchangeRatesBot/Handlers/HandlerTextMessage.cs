using ExchangeRatesBot.Handlers.Interfaces;
using ExchangeRatesBot.Messages.Interfaces;
using ExchangeRatesBot.Services;
using ExchangeRatesBot.Services.Interfaces;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExchangeRatesBot.Handlers
{
    public class HandlerTextMessage : IHandlerTextMessage
    {
        private readonly IExchangeRatesService _exchangeRatesService;
        private readonly ISentMessage _sentMessages;

        public HandlerTextMessage(IExchangeRatesService exchangeRatesService, ISentMessage sentMessages)
        {
            _exchangeRatesService = exchangeRatesService ?? throw new ArgumentNullException(nameof(exchangeRatesService));
            _sentMessages = sentMessages ?? throw new ArgumentNullException(nameof(sentMessages));
        }

        public async Task<string> HandleExchangeRatesAsync(string message)
        {
            string patternCurrency = @"\s*(" + _exchangeRatesService.GetCurrenciesForPattern() + @")\s*";
            Regex regexCurrency = new Regex(patternCurrency, RegexOptions.IgnoreCase);
            if (!regexCurrency.IsMatch(message))
            {
                return _sentMessages.GetIncorrectFormatMessagesText();
            }
            string currency = regexCurrency.Match(message).ToString().Trim().ToUpper();

            string patternDate = @"(\s|[a-z]|[а-я])";
            Regex regexDate = new Regex(patternDate, RegexOptions.IgnoreCase);
            string resultDate = regexDate.Replace(message, "");
            DateTime.TryParse(resultDate, out DateTime date);
            string resultMessage = "";
            if (date == DateTime.MinValue)
            {
                date = DateTime.Now.AddDays(-1);
                resultMessage = _sentMessages.GetIncorrectDateText();
            }
            else if (date > DateTime.Now)
            {
                date = DateTime.Now.AddDays(-1);
                resultMessage = _sentMessages.GetDateMoreCurrentDateText();
            }

            ExchangeRate exchangeRate = await _exchangeRatesService.GetExchangeRatesAsync(currency, date);
            if (exchangeRate.SaleRateNB <= 0)
            {
                return _sentMessages.GetServiceIsUnavailableText();
            }

            resultMessage += _sentMessages.GetExchangeRatesText(date, _exchangeRatesService.GetCurrencyRepresentation(currency),
                exchangeRate.SaleRateNB);
            return resultMessage;
        }

        public string HandleHelp()
        {
            return _sentMessages.GetHelpText(_exchangeRatesService.GetCurrenciesRepresentation());
        }

        public string HandleStart()
        {
            return _sentMessages.GetStartText();
        }

        public string UnknownUpdateTypeMessage()
        {
            return _sentMessages.GetUnknownUpdateTypeMessage();
        }
    }
}
