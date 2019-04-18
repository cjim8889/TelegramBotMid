using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramMid.Models
{
    public class Article
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public DateTime PublishedDate { get; set; }

        public static Article Empty
        {
            get
            {
                return new Article();
            }
        }
    }
}
