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
        private static string _endpoint;
        private static string _port;
        private static string _address;
        
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
            
            _port = DotNetEnv.Env.GetString("PORT");
            Console.WriteLine("ENV : " + DotNetEnv.Env.GetString("ENV"));
            switch (DotNetEnv.Env.GetString("ENV"))
            {
                case "PROD":
                    Console.WriteLine("Is Prod");
                    _endpoint = DotNetEnv.Env.GetString("ENDPOINT_PROD");
                    _address = $"{_endpoint}:{_port}";
                    break;
                case "DEV":
                    Console.WriteLine("Is Dev");
                    _endpoint = DotNetEnv.Env.GetString("ENDPOINT_DEV");
                    _address = $"{_endpoint}:{_port}";
                    break;
                case "LOCAL":
                    Console.WriteLine("Is Local");
                    _endpoint = DotNetEnv.Env.GetString("ENDPOINT_LOCAL");
                    _address = $"{_endpoint}:{_port}";
                    break;
            }
            Console.WriteLine($"Address : {_address}");
            WebSocketServer webSocketServer = new WebSocketServer(_address);
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