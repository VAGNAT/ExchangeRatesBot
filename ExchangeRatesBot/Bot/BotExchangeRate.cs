using ExchangeRatesBot.Bot.Interfaces;
using ExchangeRatesBot.Handlers.Interfaces;
using ExchangeRatesBot.Resourses;
using ExchangeRatesBot.Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ExchangeRatesBot.Bot
{
    public class BotExchangeRate : IBot
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<BotExchangeRate> _logger;
        private readonly IHandlersMessage _handlers;
        private readonly CancellationTokenSource _cts;
        private readonly ReceiverOptions _receiverOptions;

        public BotExchangeRate(ITelegramBotClient botClient, ILoggerFactory loggerFactory, IHandlersMessage handlers)
        {
            _botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
            _logger = loggerFactory.CreateLogger<BotExchangeRate>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
            _cts = new CancellationTokenSource();
            _receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
        }
        public async Task StartBotAsync()
        {            
            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                errorHandler: HandleErrorAsync,
                receiverOptions: _receiverOptions,
                cancellationToken: _cts.Token
            );

            User me = await _botClient.GetMeAsync();
            if (me.Username != null)
            {
                _logger.LogWarning($"{MessageResourse.StartBot}", me.Username);
            }
            
            Console.ReadLine();

            _cts.Cancel();            
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            SentMessageModel sentMessage;

            switch (update.Type)
            {
                case UpdateType.Message:
                    sentMessage = await _handlers.BotOnMessageReceivedAsync(update);
                    break;
                case UpdateType.EditedMessage:
                    sentMessage = await _handlers.BotOnMessageReceivedAsync(update);
                    break;
                default:
                    sentMessage = _handlers.UnknownUpdateHandlerAsync();
                    break;
            }

            if (update.Message != null)
            {
                Message message = update.Message ?? update.EditedMessage;
                long chatId = message.Chat.Id;
                string messageText = message.Text;
                UpdateType updateType = update.Type;
                if (messageText is null)
                {
                    _logger.LogInformation($"{MessageResourse.ReceiveMessage}", updateType, chatId);
                }
                else
                {
                    _logger.LogInformation($"{MessageResourse.ReceiveMessageWithText}", updateType, messageText, chatId);
                }
            }

            Task sender = sentMessage.Reply ? SendMessageReplyAsync(update, sentMessage.Message) : SendMessageAsync(update, sentMessage.Message);

            try
            {
                await sender;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }

        private async Task<Telegram.Bot.Types.Message> SendMessageReplyAsync(Update update, string sentMessage)
        {
            Message message = update.Message ?? update.EditedMessage;
            long chatId = message.Chat.Id;
            return await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: sentMessage,
                replyToMessageId: message.MessageId);
        }

        private async Task<Message> SendMessageAsync(Update update, string sentMessage)
        {
            Message message = update.Message ?? update.EditedMessage;
            long chatId = message.Chat.Id;
            return await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: sentMessage);
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            string errorMessage;
            switch (exception)
            {
                case ApiRequestException apiRequestException:
                    errorMessage = string.Format($"{MessageResourse.ErrorTelegram}", apiRequestException.ErrorCode, apiRequestException.Message);
                    break;
                default:
                    errorMessage = exception.ToString();
                    break;
            }

            _logger.LogError(errorMessage);
            return Task.CompletedTask;
        }
    }
}
