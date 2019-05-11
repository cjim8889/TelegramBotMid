using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramMid.Model
{
    class MqMessage
    {
        public string Content { get; set; }
        public List<long> Receivers { get; set; }
    }
}
