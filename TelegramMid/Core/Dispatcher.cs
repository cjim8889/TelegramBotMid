using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Telegram.Bot.Types;
using TelegramMid.Context;
using TelegramMid.Utility;

namespace TelegramMid.Core
{
    class Method
    {
        public bool IsCommand { get; set; }
        public string Command { get; set; }

        public DispatcherType Type { get; set; }
        public MethodInfo MethodInfo { get;  set; }
    }
    class Dispatcher
    {
        private IList<Method> methods;
        public delegate void MessageEventHandler(Message message);

        private readonly TelegramContext telegramContext;
        public Dispatcher(TelegramContext telegramContext)
        {
            this.telegramContext = telegramContext;
            this.telegramContext.OnMessage += new MessageEventHandler(Dispatch);
            methods = new List<Method>();
        }

        //Always dispatch to the first method that fits
        //Controllers are transient
        //Pattern matching to be implemented
        public void Dispatch(Message message)
        {
            var method = SelectProcedure(message);
            if (method == null)
            {
                return;
            }

            var targetControllerType = method.MethodInfo.DeclaringType;

            var target = Factory.InstanceInstantiate(targetControllerType);

            method.MethodInfo.Invoke(target, new[] { message });
        }

        private Method SelectProcedure(Message message)
        {
            var type = (DispatcherType) message.Type;

            if (type != DispatcherType.Text)
            {
                return methods.Where(m => m.Type == type || (m.Type == DispatcherType.Any && !m.IsCommand)).FirstOrDefault();
            }

            if (message.Text.StartsWith('/'))
            {
                var messageBody = message.Text.Substring(1);
                var messageSplit = messageBody.Split(' ', 2);

                return methods.Where(m => m.IsCommand && m.Command == messageSplit[0]).FirstOrDefault();
            }

            return methods.Where(m => m.Type == type && !m.IsCommand).FirstOrDefault();
        }
        public void LoadInterface<I>()
        {
            var typeNames = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(I).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => x.FullName).ToList();

            foreach (var typeName in typeNames)
            {
                var type = Type.GetType(typeName);

                Loader.LoadToList(methods, type);
            }
        }
    }
}
