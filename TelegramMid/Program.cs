using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using TelegramMid.Models;

namespace TelegramMid
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            var factory = new ConnectionFactory();
            factory.Uri = new Uri(configuration.GetSection("Mq:ConnectionString").Value);

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "test",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                var type = new { article = Article.Empty};
                var messageObj = JsonConvert.DeserializeAnonymousType(message, type);




                Console.WriteLine(" [x] Received {0}", messageObj.article.Title);

            };



            channel.BasicConsume(queue: "test", autoAck: true, consumer: consumer);
        }
    }
}
