using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using TelegramMid.Attribute;
using TelegramMid.Context;

namespace TelegramMid.Controller
{
    interface IControllerBase
    {
    }
    class MainController : IControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly TelegramContext telegramContext;
        public MainController(TelegramContext telegramContext, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.telegramContext = telegramContext;
        }

        [Command("start")]
        public string Start(Message message)
        {
            return $"Hi, 这里是Telegram Push Service Bot\n你的ID是: {message.From.Id}\n注册服务请访问https://push.oxifus.com\n\n\n";
        }


        [Command("wocao")]
        public string f(Message message)
        {
            return "Cao ni ma";
        }

        [Command("version")]
        public async void Version(Message message)
        {
            await telegramContext.SendMessage("v0.0.1", message.Chat.Id);
        }
    }
}
