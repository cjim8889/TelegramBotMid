using System;
using System.Collections.Generic;
using System.Text;
using TelegramMid.Attribute;

namespace TelegramMid.Controller
{
    interface IControllerBase
    {
    }
    class MainController : IControllerBase
    {

        public MainController()
        {

        }


        [Command("start")]
        public string Start(string[] arguments, long chatId)
        {
            return $"Hi, 这里是Telegram Push Service Bot\n你的ID是: {chatId}";
        }


        [Command("wocao")]
        public string f(string[] arguments, long chatId)
        {
            return "Cao ni ma";
        }
    }
}
