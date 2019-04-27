using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using TelegramMid.Attributes;
using TelegramMid.Context;

namespace TelegramMid
{
    class ControllerLoader<T>
    {
        private readonly T target;
        public ControllerLoader(T target)
        {
            this.target = target;
        }

        private static Delegate CreateDelegate(MethodInfo methodInfo, object target)
        {
            Func<Type[], Type> getType;
            var types = methodInfo.GetParameters().Select(p => p.ParameterType);

            getType = Expression.GetFuncType;
            types = types.Concat(new[] { methodInfo.ReturnType });

            return Delegate.CreateDelegate(getType(types.ToArray()), target, methodInfo.Name);
        }

        public void LoadToContext(TelegramContext context)
        {
            MethodInfo[] methodInfos = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            if (methodInfos.Length == 0)
            {
                return;
            }

            foreach (MethodInfo methodInfo in methodInfos)
            {
                CommandAttribute attr = methodInfo.GetCustomAttribute<CommandAttribute>();
                if (attr == null)
                {
                    continue;
                }

                var func = (Func<string[], long, string>)CreateDelegate(methodInfo, target);

                context.RegisterCommand(attr.CommandName, func);
                Console.WriteLine($"Command {attr.CommandName} loaded");
            }
        }

    }
}
