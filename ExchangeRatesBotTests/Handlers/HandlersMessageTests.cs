using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using ExchangeRatesBot.Handlers.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Threading;

namespace ExchangeRatesBot.Handlers.Tests
{
    [TestClass()]
    public class HandlersMessageTests
    {
        private Mock<IHandlerTextMessage> _handlerMessageMock;
        private Mock<ITelegramBotClient> _botMock;
        private IHandlersMessage _handlers;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            _handlerMessageMock = new Mock<IHandlerTextMessage>();
            _botMock = new Mock<ITelegramBotClient>();
            _handlers = new HandlersMessage(_handlerMessageMock.Object);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandlersBot_Constructor()
        {
            new HandlersMessage(null);
        }

        [TestMethod()]
        public void BotOnMessageReceivedAsyncTest_UnknownMessage()
        {
            //arrange
            Update update = new Update() { Message = new Message() };

            //act
            _handlers.BotOnMessageReceivedAsync(update);

            //assert
            _handlerMessageMock.Verify((x) => x.UnknownUpdateTypeMessage(), Times.Once);
        }

        [TestMethod()]
        public void BotOnMessageReceivedAsyncTest_StartMessage()
        {
            //arrange
            Update update = new Update() { Message = new Message() { Text = "/Start" } };

            //act
            _handlers.BotOnMessageReceivedAsync(update);

            //assert
            _handlerMessageMock.Verify((x) => x.HandleStart(), Times.Once);
        }

        [TestMethod()]
        public void BotOnMessageReceivedAsyncTest_HelpMessage()
        {
            //arrange
            Update update = new Update() { Message = new Message() { Text = "/Help" } };

            //act
            _handlers.BotOnMessageReceivedAsync(update);

            //assert
            _handlerMessageMock.Verify((x) => x.HandleHelp(), Times.Once);
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "testData.xml",
            "message",
            DataAccessMethod.Sequential)]
        [TestMethod()]
        public void BotOnMessageReceivedAsyncTest_CorrectMessage()
        {
            //arrange
            Update update = new Update() { Message = new Message() { Text = Convert.ToString(TestContext.DataRow["correct"]) } };

            //act
            _handlers.BotOnMessageReceivedAsync(update);

            //assert
            _handlerMessageMock.Verify((x) => x.HandleExchangeRatesAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod()]
        public void BotOnMessageReceivedAsyncTest_NotTextMessage()
        {
            //arrange
            Update update = new Update() { Message = new Message() };

            //act
            _handlers.BotOnMessageReceivedAsync(update);

            //assert
            _handlerMessageMock.Verify((x) => x.UnknownUpdateTypeMessage(), Times.Once);
        }
    }
}