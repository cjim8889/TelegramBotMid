using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramMid.Attributes
{
    class CommandAttribute : Attribute
    {
        public CommandAttribute(string commandName)
        {
            CommandName = commandName;
        }

        public string CommandName { get; set; }
    }
}
