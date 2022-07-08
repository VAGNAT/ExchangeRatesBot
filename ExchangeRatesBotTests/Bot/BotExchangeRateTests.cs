using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using Telegram.Bot;
using ExchangeRatesBot.Handlers.Interfaces;
using Microsoft.Extensions.Logging;

namespace ExchangeRatesBot.Bot.Tests
{
    [TestClass()]
    public class BotExchangeRateTests
    {
        private Mock<ITelegramBotClient> _botClientMock;
        private Mock<ILoggerFactory> _loggerMock;
        private Mock<IHandlersMessage> _handlersMock;
        private BotExchangeRate _bot;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            _botClientMock = new Mock<ITelegramBotClient>();
            _loggerMock = new Mock<ILoggerFactory>();
            _handlersMock = new Mock<IHandlersMessage>();
            _bot = new BotExchangeRate(_botClientMock.Object, _loggerMock.Object, _handlersMock.Object);
        }

        #region testConstructor

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BotExchangeRate_Constructor_FirstParametrNull()
        {
            new BotExchangeRate(null, _loggerMock.Object, _handlersMock.Object);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BotExchangeRate_Constructor_SecondParametrNull()
        {
            new BotExchangeRate(_botClientMock.Object, null, _handlersMock.Object);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BotExchangeRate_Constructor_ThirdParametrNull()
        {
            new BotExchangeRate(_botClientMock.Object, _loggerMock.Object, null);
        }


        #endregion               
    }
}