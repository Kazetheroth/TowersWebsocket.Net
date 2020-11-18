using System;
using System.Reflection;

namespace TowersWebsocketNet31.Server
{
    public enum TargetType
    {
        All,
        OnlyOne,
        Self,
        Others
    }

    public static class TargetMessage
    {
        public static readonly string[] Target =
        {
            "ALL",
            "ONLY_ONE",
            "SELF",
            "OTHERS"
        };
    }

    public static class Utils
    {
        public static string GenerateStages()
        {
            string result = "";
            Random rnd = new Random();
            for (int i = 0; i < 5; i++)
            {
                result += rnd.Next(0, 1).ToString();
            }

            return result;
        }

        public static string InvokeStringMethod(string typeName, string methodName, string stringParam)
        {
            // Get the Type for the class
            Type calledType = Type.GetType(typeName);

            // Invoke the method itself. The string returned by the method winds up in s.
            // Note that stringParam is passed via the last parameter of InvokeMember,
            // as an array of Objects.
            String s = (String) calledType.InvokeMember(
                methodName,
                BindingFlags.InvokeMethod | BindingFlags.Public |
                BindingFlags.Static,
                null,
                null,
                new Object[] {stringParam});

            // Return the string that was returned by the called method.
            return s;
        }

        public static T Clone<T>(T origin) where T : new()
        {
            T clone = new T();
            PropertyInfo[] propertyInfos = origin.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.GetType().IsArray)
                {
                    Array array = (Array) propertyInfo.GetValue(origin);
                    Array targetArray = Array.CreateInstance(propertyInfo.GetType(), array.Length);

                    for (int i = 0; i < array.Length; ++i)
                    {
                        object o = array.GetValue(i);

                        if (o.GetType().GetConstructor(Type.EmptyTypes) != null)
                        {
                            targetArray.SetValue(Clone(o), i);
                        }
                        else
                        {
                            targetArray.SetValue(o, i);
                        }

                    }

                    propertyInfo.SetValue(clone, targetArray);
                }
                else
                {
                    propertyInfo.SetValue(clone, propertyInfo.GetValue(origin));
                }
            }

            return clone;
        }
    }

    public abstract class ObjectParsed
    {
        public abstract void InsertValue(string key, string value);

        public abstract void DoSomething();
    }
}