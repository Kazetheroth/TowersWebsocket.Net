using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using TowersWebsocketNet31.Server;
using TowersWebsocketNet31.Server.Player;
using WebSocketSharp.Server;

namespace TowersWebsocketNet31
{
    public class Program
    {
        public static string ENDPOINT;
        public static string PORT;
        public static string ADDRESS;
        
        public static void Main(string[] args)
        {
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if (isWindows)
            {
                using (var stream = File.OpenRead("../../../towers.env"))
                {
                    DotNetEnv.Env.Load(stream);
                }
            }
            else
            {
                DotNetEnv.Env.Load("./towers.env");
            }
            
            PORT = DotNetEnv.Env.GetString("PORT");
            Console.WriteLine("ENV : " + DotNetEnv.Env.GetString("ENV"));
            switch (DotNetEnv.Env.GetString("ENV"))
            {
                case "PROD":
                    Console.WriteLine("Is Prod");
                    ENDPOINT = DotNetEnv.Env.GetString("ENDPOINT_PROD");
                    ADDRESS = $"{ENDPOINT}:{PORT}";
                    break;
                case "DEV":
                    Console.WriteLine("Is Dev");
                    ENDPOINT = DotNetEnv.Env.GetString("ENDPOINT_DEV");
                    ADDRESS = $"{ENDPOINT}:{PORT}";
                    break;
                case "LOCAL":
                    Console.WriteLine("Is Local");
                    ENDPOINT = DotNetEnv.Env.GetString("ENDPOINT_LOCAL");
                    ADDRESS = ENDPOINT;
                    break;
            }
            Console.WriteLine($"Address : {ADDRESS}");
            WebSocketServer webSocketServer = new WebSocketServer(ADDRESS);
            webSocketServer.AddWebSocketService<TowersWebsocket>("/websocket");
            //Console.WriteLine(webSocketServer.WebSocketServices["websoket"].Type);
            
            webSocketServer.Start();
            Console.WriteLine("Websocket Server started!");
            Console.ReadKey(true);
            Console.WriteLine("Websocket Server stoped!");
            webSocketServer.Stop();
        }
    }
}