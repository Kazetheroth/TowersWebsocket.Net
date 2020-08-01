using System;
using System.Collections.Generic;
using TowersWebsocketNet31.Server;
using TowersWebsocketNet31.Server.Player;
using WebSocketSharp.Server;

namespace TowersWebsocketNet31
{
    public class Program
    {
        public static string ENDPOINT;
        public static string PORT;
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load(".env");
            switch (DotNetEnv.Env.GetString("ENV"))
            {
                case "PROD":
                    ENDPOINT = DotNetEnv.Env.GetString("ENDPOINT_PROD");
                    break;
                case "DEV":
                    ENDPOINT = DotNetEnv.Env.GetString("ENDPOINT_DEV");
                    break;
                case "LOCAL":
                    ENDPOINT = DotNetEnv.Env.GetString("ENDPOINT_LOCAL");
                    break;
            }

            PORT = DotNetEnv.Env.GetString("PORT");

            WebSocketServer webSocketServer = new WebSocketServer($"{ENDPOINT}:{PORT}");
            webSocketServer.AddWebSocketService<TowersWebsocket>("/websocket");
            
            webSocketServer.Start();
            Console.WriteLine("Websocket Server started!");
            Console.ReadKey(true);
            Console.WriteLine("Websocket Server stoped!");
            webSocketServer.Stop();
        }
    }
}