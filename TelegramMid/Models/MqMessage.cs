using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramMid.Models
{
    class MqMessage
    {
        public string Content { get; set; }
        public List<int> Receivers { get; set; }
    }
}
