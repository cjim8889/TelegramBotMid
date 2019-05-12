using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using TelegramMid.Context;
using TelegramMid.Controller;
using TelegramMid.Utility;

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

            Factory.AddDependency<IConfiguration>(configuration);
            Factory.AddDependency<TelegramContext>();
            Factory.AddDependency<MqContext>();

            var server = Factory.InstantiateServer();

            server.Run();

        }
    }
}
