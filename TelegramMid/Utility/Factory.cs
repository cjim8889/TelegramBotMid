using System;
using System.Collections.Generic;
using System.Reflection;
using TelegramMid.Controller;

namespace TelegramMid.Utility
{
    class MissingDependencyException : Exception
    {
    }
    class Factory
    {
        public static Dictionary<string, object> dependencyDict = new Dictionary<string, object>();

        public static void AddDependency<T>(T obj)
        {
            dependencyDict.Add(typeof(T).FullName, obj);
        }

        public static void AddDependency<T>()
        {
            dependencyDict.Add(typeof(T).FullName, InstanceInstantiate<T>());
        }

        public static object InstanceInstantiate<T>()
        {
            return InstanceInstantiate(typeof(T));
        }

        public static TelegramServer InstantiateServer()
        {
            var server = (TelegramServer)InstanceInstantiate<TelegramServer>();

            server.AddControllers<IControllerBase>();

            return server;
        }

        public static object InstanceInstantiate(Type type)
        {
            ConstructorInfo[] constructorInfos = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            var info = constructorInfos[0];
            //Currently can only handle one constructor
            //Can not correctly handle interdependency
            //Maybe using GetUninitializedObject in the future?

            var paramsInfo = info.GetParameters();
            var constructorParams = ConstructDependencyParams(paramsInfo);

            var target = Activator.CreateInstance(type, constructorParams);

            return target;
        }

        private static object[] ConstructDependencyParams(ParameterInfo[] parameterInfos)
        {
            var tempList = new List<object>();

            foreach (var paramInfo in parameterInfos)
            {
                if (!dependencyDict.ContainsKey(paramInfo.ParameterType.FullName))
                {
                    throw new MissingDependencyException();
                }

                tempList.Add(dependencyDict.GetValueOrDefault(paramInfo.ParameterType.FullName));
            }

            return tempList.ToArray();
        }
    }
}
