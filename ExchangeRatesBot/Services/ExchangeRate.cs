namespace ExchangeRatesBot.Services
{
    public class ExchangeRate
    {
        public string BaseCurrency { get; set; }        
        public string Currency { get; set; }
        public decimal SaleRateNB { get; set; }
    }
}
