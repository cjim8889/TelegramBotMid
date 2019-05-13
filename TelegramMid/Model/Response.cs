using System;
using System.Collections.Generic;
using System.Text;
using TelegramMid.Context;

namespace TelegramMid.Model
{

    interface IResponse
    {
        void SendResponse(TelegramContext telegramContext);
    }

    abstract class AbstractResponse : IResponse
    {
        public abstract void SendResponse(TelegramContext telegramContext);
        public abstract long ChatId { get; set; }

        public AbstractResponse()
        {
            ChatId = 0;
        }
    }


    class ResponseFactory
    {
        public static TextResponse NewTextResponse(string response, long chatId)
        {
            return new TextResponse() { ResponseText = response, ChatId = chatId };
        }

        public static TextResponse NewTextResponse(string response)
        {
            return new TextResponse() { ResponseText = response };
        }
    }

    class TextResponse : AbstractResponse
    {
        public string ResponseText { get; set; }
        public override long ChatId { get; set; }

        public override async void SendResponse(TelegramContext telegramContext)
        {
            await telegramContext.SendMessage(ResponseText, ChatId);
        }
    }
}
