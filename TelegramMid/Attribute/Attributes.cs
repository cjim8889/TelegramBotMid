using System;
using System.Collections.Generic;
using System.Text;
using TelegramMid.Core;

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

    class TypeAttribute : System.Attribute
    {
        public TypeAttribute(DispatcherType type)
        {
            Type = type;
        }

        public DispatcherType Type { get; set; }
    }
}
