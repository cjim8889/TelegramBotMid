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

            var consumer = new EventingBasicConsumer(mqContext.Channel);


            telegramContext.RegisterCommand("wocao", Program.testCommand);


            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);



                var messageObj = JsonConvert.DeserializeObject<MqMessage>(message);

                //await telegramContext.SendMessage(tgMessage, 883936683);


            };


            var task = Task.Run(() => mqContext.Channel.BasicConsume(queue: "test", autoAck: true, consumer: consumer));
            var tgtask = Task.Run(() => {

                telegramContext.TelegramBotClient.StartReceiving();
                Thread.Sleep(int.MaxValue);

            });


            Console.WriteLine("Hello");
            
        }

        public static string testCommand(string[] arguments)
        {

            if (arguments != null)
            {
                return arguments[0];
            }

            return "No arguments";
        }
    }
}
