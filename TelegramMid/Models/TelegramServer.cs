using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TelegramMid.Context;
using TelegramMid.Controller;

namespace TelegramMid.Models
{
    class TelegramServer
    {
        private readonly IConfiguration configuration;
        private readonly MqContext mqContext;
        private readonly TelegramContext telegramContext;
        private readonly EventingBasicConsumer mqConsumer;

        public TelegramServer(IConfiguration configuration, MqContext mqContext, TelegramContext telegramContext)
        {
            this.configuration = configuration;
            this.mqContext = mqContext;
            this.telegramContext = telegramContext;

            mqConsumer = CreateEventConsumer();

            mqConsumer.Received += OnMqMessageReceived;

            new ControllerLoader<MainController>(new MainController()).LoadToContext(telegramContext);
        }


        private EventingBasicConsumer CreateEventConsumer()
        {
            return new EventingBasicConsumer(mqContext.Channel);
        }

        private void OnMqMessageReceived(object model, BasicDeliverEventArgs ea)
        {
            Console.WriteLine("Message Received from Mq");
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            var messageObj = JsonConvert.DeserializeObject<MqMessage>(message);

            var tasks = new List<Task>();

            foreach (int receiverId in messageObj.Receivers)
            {
                tasks.Add(telegramContext.SendMessage(messageObj.Content, receiverId));
            }
            Task.WaitAll(tasks.ToArray());
        }

        public void Run()
        {
            var tasks = new List<Task>();
            tasks.Add(Task.Run(() => mqContext.Channel.BasicConsume(queue: "test", autoAck: true, consumer: mqConsumer)));
            tasks.Add(Task.Run(() =>
            {
                telegramContext.TelegramBotClient.StartReceiving();
                Thread.Sleep(int.MaxValue);
            }));


            Task.WaitAll(tasks.ToArray());
        }
    }
}
