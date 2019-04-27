using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramMid.Attribute
{
    class CommandAttribute : System.Attribute
    {
        public CommandAttribute(string commandName)
        {
            CommandName = commandName;
        }

        public string CommandName { get; set; }
    }
}
