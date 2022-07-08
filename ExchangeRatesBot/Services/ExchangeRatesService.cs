using ExchangeRatesBot.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRatesBot.Services
{
    public class ExchangeRatesService: IExchangeRatesService
    {
        private readonly string url = "https://api.privatbank.ua/p24api/exchange_rates?json&date=";
        private readonly Dictionary<string, string> _currencies;
        public Dictionary<string, string> Currencies => _currencies;
        
        public ExchangeRatesService()
        {
            _currencies = new Dictionary<string, string>
            {
                { "USD", "US dollar" },
                { "EUR", "euro" },                
                { "CHF", "Swiss franc" },
                { "GBP", "British pound" },
                { "PLZ", "Polish zloty" },
                { "SEK", "Swedish krona" },
                { "XAU", "gold" },
                { "CAD", "Canadian dollar" }
            };
        }

        public async Task<ExchangeRate> GetExchangeRatesAsync(string currency, DateTime date)
        {
            HttpClient http = new HttpClient();
            HttpResponseMessage response = await http.GetAsync(url + date.ToShortDateString());
            string content = await response.Content.ReadAsStringAsync();
            ExchangeRateModel exchangeRateModel = JsonConvert.DeserializeObject<ExchangeRateModel>(content);
            List<ExchangeRate> exchangeRates = exchangeRateModel.ExchangeRate.Where(m => m.Currency == currency).ToList();
            return exchangeRates.Count > 0 ? exchangeRates[0] : new ExchangeRate();
        }

        public string GetCurrenciesForPattern()
        {
            return _currencies.Select(CurrencyForPattern).Aggregate((s1, s2) => $"{s1}|{s2}");
        }

        public string GetCurrenciesRepresentation()
        {
            return _currencies.Select(CurrencyRepresentation).Aggregate((s1, s2) => $"{s1}\n{s2}");
        }

        public string GetCurrencyRepresentation(string curruncy)
        {
            return _currencies[curruncy];
        }

        private string CurrencyRepresentation(KeyValuePair<string, string> currency)
        {
            return $"{currency.Key} {currency.Value}";
        }

        private string CurrencyForPattern(KeyValuePair<string, string> currency)
        {
            return $"{currency.Key}";
        }
    }
}
