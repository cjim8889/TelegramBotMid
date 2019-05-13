using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using TelegramMid.Attribute;
using TelegramMid.Context;
using TelegramMid.Model;

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
        public IResponse Start(Message message)
        {
            return ResponseFactory.NewTextResponse($"Hi, 这里是Telegram Push Service Bot\n你的ID是: {message.From.Id}\n注册服务请访问https://push.oxifus.com\n\n\n");
        }


        [Command("wocao")]
        public IResponse wocao()
        {
            return ResponseFactory.NewTextResponse("Mei mao bing");
        }

        [Command("version")]
        public IResponse Version()
        {
            return ResponseFactory.NewTextResponse($"v{configuration.GetSection("Version").Value}");
        }
    }
}
