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
        public static readonly string[] Target = {
            "ALL",
            "ONLY_ONE",
            "SELF",
            "OTHERS"
        };
    }
    public class Utils
    {
        public static string InvokeStringMethod(string typeName, string methodName, string stringParam)
        {
            // Get the Type for the class
            Type calledType = Type.GetType(typeName);

            // Invoke the method itself. The string returned by the method winds up in s.
            // Note that stringParam is passed via the last parameter of InvokeMember,
            // as an array of Objects.
            String s = (String)calledType.InvokeMember(
                methodName,
                BindingFlags.InvokeMethod | BindingFlags.Public | 
                BindingFlags.Static,
                null,
                null,
                new Object[] { stringParam });

            // Return the string that was returned by the called method.
            return s;
        }
    }
}