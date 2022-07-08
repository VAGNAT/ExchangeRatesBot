using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Moq;
using ExchangeRatesBot.Services.Interfaces;
using ExchangeRatesBot.Messages.Interfaces;
using ExchangeRatesBot.Handlers.Interfaces;
using ExchangeRatesBot.Messages;
using ExchangeRatesBot.Services;

namespace ExchangeRatesBot.Handlers.Tests
{
    [TestClass()]
    public class HandlerTextMessageTests
    {
        private Mock<IExchangeRatesService> _serviceMock;
        private Mock<ISentMessage> _sentMessagesMock;
        private IHandlerTextMessage _handler;
        private IExchangeRatesService _service;
        private ISentMessage _sentMessages;


        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            _sentMessages = new SentMessage();
            _service = new ExchangeRatesService();
            _serviceMock = new Mock<IExchangeRatesService>();
            _sentMessagesMock = new Mock<ISentMessage>();
            _handler = new HandlerTextMessage(_serviceMock.Object, _sentMessagesMock.Object);
        }

        #region testConstructor

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandlerTextMessage_Constructor_FirstParametrNull()
        {
            new HandlerTextMessage(null, _sentMessagesMock.Object);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandlerTextMessage_Constructor_SecondParametrNull()
        {
            new HandlerTextMessage(_serviceMock.Object, null);
        }
        #endregion
        [TestMethod()]
        public void HandleHelpTest_Dependency()
        {
            //act
            _handler.HandleHelp();

            //assert
            _serviceMock.Verify((x) => x.GetCurrenciesRepresentation(), Times.Once);
            _sentMessagesMock.Verify((x) => x.GetHelpText(It.IsAny<string>()), Times.Once);
        }              

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "testData.xml",
            "message",
            DataAccessMethod.Sequential)]
        [TestMethod()]
        public async Task GetExchangeRatesTest_IncorrectMessage()
        {
            //arrange
            string expected = _sentMessages.GetIncorrectFormatMessagesText();
            string message = Convert.ToString(TestContext.DataRow["incorrect"]);
            _serviceMock.Setup((x) => x.GetCurrenciesForPattern()).Returns(_service.GetCurrenciesForPattern());
            _sentMessagesMock.Setup((x) => x.GetIncorrectFormatMessagesText()).Returns(_sentMessages.GetIncorrectFormatMessagesText());

            //act
            string actual = await _handler.HandleExchangeRatesAsync(message);

            //assert
            _serviceMock.Verify((x) => x.GetCurrenciesForPattern(), Times.Once);
            _sentMessagesMock.Verify((x) => x.GetIncorrectFormatMessagesText(), Times.Once);
            Assert.AreEqual(expected, actual);
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "testData.xml",
            "message",
            DataAccessMethod.Sequential)]
        [TestMethod()]
        public async Task GetExchangeRatesTest_CorrectMessageWithoutDate()
        {
            //arrange
            string message = Convert.ToString(TestContext.DataRow["correctWithoutDate"]);
            _serviceMock.Setup((x) => x.GetCurrenciesForPattern()).Returns(_service.GetCurrenciesForPattern());
            _serviceMock.Setup((x) => x.GetExchangeRatesAsync(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(Task.FromResult(new ExchangeRate { SaleRateNB = 1 }));
            _serviceMock.Setup((x) => x.GetCurrencyRepresentation(It.IsAny<string>())).Returns(It.IsAny<string>());

            //act
            await _handler.HandleExchangeRatesAsync(message);

            //assert
            _serviceMock.Verify((x) => x.GetCurrenciesForPattern(), Times.Once);
            _sentMessagesMock.Verify((x) => x.GetIncorrectDateText(), Times.Once);
            _sentMessagesMock.Verify((x) => x.GetExchangeRatesText(It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<decimal>()), Times.Once);            
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "testData.xml",
            "message",
            DataAccessMethod.Sequential)]
        [TestMethod()]
        public async Task GetExchangeRatesTest_CorrectMessage()
        {
            //arrange
            string message = Convert.ToString(TestContext.DataRow["correct"]);
            _serviceMock.Setup((x) => x.GetCurrenciesForPattern()).Returns(_service.GetCurrenciesForPattern());
            _serviceMock.Setup((x) => x.GetExchangeRatesAsync(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(Task.FromResult(new ExchangeRate { SaleRateNB = 1 }));
            _serviceMock.Setup((x) => x.GetCurrencyRepresentation(It.IsAny<string>())).Returns(It.IsAny<string>());

            //act
            await _handler.HandleExchangeRatesAsync(message);

            //assert
            _serviceMock.Verify((x) => x.GetCurrenciesForPattern(), Times.Once);
            _sentMessagesMock.Verify((x) => x.GetExchangeRatesText(It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<decimal>()), Times.Once);
        }
                
        [TestMethod()]
        public async Task GetExchangeRatesTest_CorrectMessageDateMoreCurrent()
        {
            //arrange
            string message = $"usd {DateTime.Now.AddDays(1).ToShortDateString()}";
            _serviceMock.Setup((x) => x.GetCurrenciesForPattern()).Returns(_service.GetCurrenciesForPattern());
            _serviceMock.Setup((x) => x.GetExchangeRatesAsync(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(Task.FromResult(new ExchangeRate { SaleRateNB = 1 }));
            _serviceMock.Setup((x) => x.GetCurrencyRepresentation(It.IsAny<string>())).Returns(It.IsAny<string>());

            //act
            await _handler.HandleExchangeRatesAsync(message);

            //assert
            _serviceMock.Verify((x) => x.GetCurrenciesForPattern(), Times.Once);
            _sentMessagesMock.Verify((x) => x.GetDateMoreCurrentDateText(), Times.Once);
            _sentMessagesMock.Verify((x) => x.GetExchangeRatesText(It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<decimal>()), Times.Once);
        }

        [TestMethod()]
        public async Task GetExchangeRatesTest_CorrectMessageNoData()
        {
            //arrange
            string expected = _sentMessages.GetServiceIsUnavailableText();
            string message = $"usd {DateTime.MinValue.ToShortDateString()}";
            _serviceMock.Setup((x) => x.GetCurrenciesForPattern()).Returns(_service.GetCurrenciesForPattern());
            _serviceMock.Setup((x) => x.GetExchangeRatesAsync(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(Task.FromResult(new ExchangeRate()));
            _sentMessagesMock.Setup((x) => x.GetServiceIsUnavailableText()).Returns(_sentMessages.GetServiceIsUnavailableText());

            //act
            string actual = await _handler.HandleExchangeRatesAsync(message);

            //assert
            _serviceMock.Verify((x) => x.GetCurrenciesForPattern(), Times.Once);
            _sentMessagesMock.Verify((x) => x.GetServiceIsUnavailableText(), Times.Once);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void HandleStartTest_Dependency()
        {
            //act
            _handler.HandleStart();

            //assert
            _sentMessagesMock.Verify((x) => x.GetStartText(), Times.Once);
        }

        [TestMethod()]
        public void UnknownUpdateTypeMessageTest_Dependency()
        {
            //act
            _handler.UnknownUpdateTypeMessage();

            //assert
            _sentMessagesMock.Verify((x) => x.GetUnknownUpdateTypeMessage(), Times.Once);
        }
    }
}