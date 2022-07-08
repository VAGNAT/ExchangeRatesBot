using ExchangeRatesBot.Handlers.Interfaces;
using ExchangeRatesBot.Messages;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ExchangeRatesBot.Handlers
{
    public class HandlersMessage : IHandlersMessage
    {
        private readonly IHandlerTextMessage _handlerMessage;

        public HandlersMessage(IHandlerTextMessage handlerMessage)
        {
            _handlerMessage = handlerMessage ?? throw new ArgumentNullException(nameof(handlerMessage));
        }

        public async Task<SentMessageModel> BotOnMessageReceivedAsync(Update update)
        {
            Message message = update.Message ?? update.EditedMessage;
            SentMessageModel sentMessage = new SentMessageModel();
            if (message.Type == MessageType.Text)
            {
                switch (message.Text.Trim().ToLower())
                {
                    case "/start":
                        sentMessage.Message = _handlerMessage.HandleStart();
                        break;
                    case "/help":
                        sentMessage.Message = _handlerMessage.HandleHelp();
                        break;
                    default:
                        sentMessage.Message = await _handlerMessage.HandleExchangeRatesAsync(message.Text.Trim());
                        sentMessage.Reply = true;
                        break;
                }
            }
            else
            {
                sentMessage.Message = _handlerMessage.UnknownUpdateTypeMessage();
                sentMessage.Reply = true;
            }
            return sentMessage;
        }

        public SentMessageModel UnknownUpdateHandlerAsync()
        {
            SentMessageModel sentMessage = new SentMessageModel();
            sentMessage.Message = "I cannot process this type. Use the \"/help\" command for more information.";
            return sentMessage;
        }

        
    }
}
