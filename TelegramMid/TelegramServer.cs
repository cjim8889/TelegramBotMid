﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TelegramMid.Context;
using TelegramMid.Controller;
using TelegramMid.Model;
using TelegramMid.Utility;

namespace TelegramMid
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

            AddControllers<IControllerBase>();
        }

        public void AddController<T>()
        {
            var target = Activator.CreateInstance<T>();
            ControllerLoader.LoadToContext<T>(telegramContext, target);

        }

        public void AddControllers<I>()
        {
            var typeNames = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(I).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => x.FullName).ToList();

            foreach(var typeName in typeNames)
            {
                var type = Type.GetType(typeName);
                var target = Activator.CreateInstance(type);

                ControllerLoader.LoadToContext(telegramContext, type, target);
            }
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

            foreach (long receiverId in messageObj.Receivers)
            {
                tasks.Add(telegramContext.SendMessage(messageObj.Content, receiverId));
            }
            Task.WaitAll(tasks.ToArray());
        }

        public void Run()
        {
            var tasks = new List<Task>();
            tasks.Add(Task.Run(() => mqContext.Channel.BasicConsume(queue: configuration.GetSection("Mq:Key").Value, autoAck: true, consumer: mqConsumer)));
            tasks.Add(Task.Run(() =>
            {
                telegramContext.TelegramBotClient.StartReceiving();
                Thread.Sleep(int.MaxValue);
            }));


            Task.WaitAll(tasks.ToArray());
        }
    }
}