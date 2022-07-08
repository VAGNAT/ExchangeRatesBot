using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRatesBot.Services.Tests
{
    [TestClass()]
    public class ExchangeRatesServiceTests
    {
        private ExchangeRatesService _service;
        [TestInitialize]
        public void TestInitialize()
        {
            _service = new ExchangeRatesService();
        }
        [TestMethod()]
        public void GetCurrenciesForPatternTest()
        {
            //arange
            string expectedCurrencies = "";
            foreach (var item in _service.Currencies)
            {
                if (expectedCurrencies.Length > 0)
                {
                    expectedCurrencies += "|";
                }
                expectedCurrencies += item.Key;
            }

            //act
            string actualCurrencies = _service.GetCurrenciesForPattern();

            //assert
            Assert.AreEqual(expectedCurrencies, actualCurrencies);
        }

        [TestMethod()]
        public void GetCurrenciesRepresentationTest()
        {
            //arange
            string expectedCurrencies = "";
            foreach (var item in _service.Currencies)
            {
                if (expectedCurrencies.Length > 0)
                {
                    expectedCurrencies += "\n";
                }
                expectedCurrencies += item.Key + " " + item.Value;
            }

            //act
            string actualCurrencies = _service.GetCurrenciesRepresentation();

            //assert
            Assert.AreEqual(expectedCurrencies, actualCurrencies);
        }
    }
}