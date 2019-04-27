using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using TelegramMid.Models;
using TelegramMid.Context;
using System.Threading.Tasks;
using System.Threading;
using TelegramMid.Controller;
using System.Reflection;
using TelegramMid.Attributes;
using System.Linq;
using System.Linq.Expressions;

namespace TelegramMid
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables("Tg_");
            ;


            var configuration = builder.Build();
            var mqContext = new MqContext(configuration);
            var telegramContext = new TelegramContext(configuration);

            var server = new TelegramServer(configuration, mqContext, telegramContext);

            server.Run();



        }

        

        public static string testCommand(string[] arguments, long chatId)
        {

            if (arguments != null)
            {
                return arguments[0];
            }

            return "No arguments";
        }
    }
}
