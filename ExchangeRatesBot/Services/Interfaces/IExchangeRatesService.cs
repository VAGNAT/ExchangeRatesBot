using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRatesBot.Services.Interfaces
{
    public interface IExchangeRatesService
    {        
        Dictionary<string, string> Currencies { get; }
        Task<ExchangeRate> GetExchangeRatesAsync(string currency, DateTime date);
        string GetCurrenciesForPattern();
        string GetCurrenciesRepresentation();
        string GetCurrencyRepresentation(string curruncy);

    }
}
