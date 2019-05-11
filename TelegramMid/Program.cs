using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using TelegramMid.Context;
using TelegramMid.Controller;

namespace TelegramMid
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();


            var configuration = builder.Build();

            var mqContext = new MqContext(configuration);
            var telegramContext = new TelegramContext(configuration);


            var server = new TelegramServer(configuration, mqContext, telegramContext);

            server.Run();





        }

    }
}
