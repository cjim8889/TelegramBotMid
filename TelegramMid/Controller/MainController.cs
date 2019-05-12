using System;
using System.Collections.Generic;
using System.Text;
using TelegramMid.Attribute;
using TelegramMid.Context;

namespace TelegramMid.Controller
{
    interface IControllerBase
    {
    }
    class MainController : IControllerBase
    {

        public MainController(TelegramContext telegramContext)
        {
        }


        [Command("start")]
        public string Start(string[] arguments, long chatId)
        {
            return $"Hi, 这里是Telegram Push Service Bot\n你的ID是: {chatId}\n注册服务请访问https://push.oxifus.com\n\n\n";
        }


        [Command("wocao")]
        public string f(string[] arguments, long chatId)
        {
            return "Cao ni ma";
        }
    }
}
