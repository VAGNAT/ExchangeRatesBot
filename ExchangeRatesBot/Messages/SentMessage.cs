using ExchangeRatesBot.Messages.Interfaces;
using ExchangeRatesBot.Resourses;
using System;
using System.Globalization;

namespace ExchangeRatesBot.Messages
{
    public class SentMessage : ISentMessage
    {
        
        public string GetDateMoreCurrentDateText()
        {
            return string.Format(MessageResourse.DateMoreCurrentDate, DateTime.Now.ToShortDateString());
        }

        public string GetExchangeRatesText(DateTime date, string currency, decimal rate)
        {
            return string.Format(MessageResourse.ExchangeRates, date.ToShortDateString(), currency,
                rate.ToString("C", CultureInfo.CreateSpecificCulture("uk-UA")));                
        }

        public string GetIncorrectDateText()
        {
            return MessageResourse.IncorrectDate;
        }

        public string GetIncorrectFormatMessagesText()
        {
            return MessageResourse.IncorrectFormatMessage;
        }

        public string GetServiceIsUnavailableText()
        {
            return MessageResourse.ServiceIsUnavailableText;
        }

        public string GetStartText()
        {
            return MessageResourse.Start;
        }

        public string GetUnknownUpdateTypeMessage()
        {
            return MessageResourse.UnknownUpdate;
        }

        public string GetHelpText(string currencies)
        {
            return string.Format(MessageResourse.Help, currencies);            
        }
    }
}
