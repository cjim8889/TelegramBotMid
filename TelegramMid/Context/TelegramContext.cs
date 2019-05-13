using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace TelegramMid.Context
{
    class TelegramContext
    {
        public event Core.Dispatcher.MessageEventHandler OnMessage;
        public TelegramContext(IConfiguration configuration)
        {
            TelegramBotClient = new TelegramBotClient(configuration.GetSection("Telegram_Token").Value);
            TelegramBotClient.OnMessage += Bot_OnMessage;
        }

        ~TelegramContext()
        {
            TelegramBotClient.StopReceiving();
        }

        public ITelegramBotClient TelegramBotClient { get; }

        private void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var message = e.Message;

            OnMessage(message);
            //    Console.WriteLine($"Received message from {e.Message.From.FirstName} {e.Message.From.LastName}");
            //    if (messageText.StartsWith('/'))
            //    {
            //        var messageBody = messageText.Substring(1);
            //        var messageSplit = messageBody.Split(' ', 2);
            //        if (messageSplit.Length > 1)
            //        {
            //            await CommandHandler(messageSplit[0], messageSplit[1].Split(' '), chatId);
            //        }
            //        else
            //        {
            //            await CommandHandler(messageSplit[0], null, chatId);
            //        }
            //    }
        }

        //private async Task CommandHandler(string command, string[] arguments, long chatId)
        //{
         
        //    if (!commandTable.ContainsKey(command))
        //    {
        //        await SendMessage("Unsupported Command", chatId);
        //        return;
        //    }

        //    string returnMessageText = commandTable.GetValueOrDefault(command)(arguments, chatId);
        //    await SendMessage(returnMessageText, chatId);
        //}

        //public void RegisterCommand(string command, Func<string[], long, string> func)
        //{
        //    if (!commandTable.ContainsKey(command))
        //    {
        //        commandTable.Add(command, func);
        //    }
        //}

        public async Task SendMessage(string message, long chatId)
        {
            try
            {
                await TelegramBotClient.SendTextMessageAsync(
                  chatId: chatId,
                  text: message
                );


                Console.WriteLine($"Message Sent {chatId}");
            }
            catch(ChatNotFoundException e)
            {
                Console.WriteLine($"Fail to find the chatId {chatId} {e.Message}");
            }           
        }
    }
}
