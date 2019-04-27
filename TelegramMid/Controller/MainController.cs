using System;
using System.Collections.Generic;
using System.Text;
using TelegramMid.Attribute;

namespace TelegramMid.Controller
{
    class MainController
    {

        public MainController()
        {

        }


        [Command("test")]
        public string Test(string[] arguments, long chatId)
        {
            return "Hello Test";
        }


        [Command("wocao")]
        public string f(string[] arguments, long chatId)
        {
            return "Cao ni ma";
        }
    }
}
