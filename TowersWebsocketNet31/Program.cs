using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using TowersWebsocketNet31.Chat;
using TowersWebsocketNet31.Server;
using TowersWebsocketNet31.Server.Player;
using TowersWebsocketNet31.Server.Room;
using WebSocketSharp.Server;

namespace TowersWebsocketNet31
{
    public static class Program
    {
        private static string _endpoint;
        private static string _port;
        private static string _address;
        public static List<Player> players = new List<Player>();
        public static List<Room> rooms = new List<Room>();
        public static WebSocketServer webSocketServer;

        public static void Main(string[] args)
        {
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if (isWindows)
            {
                using (var stream = File.OpenRead("towers.env"))
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
            webSocketServer = new WebSocketServer(_address); 
            webSocketServer.AddWebSocketService<TowersWebsocket>("/websocket");
            webSocketServer.AddWebSocketService<ChatWebsocket>("/chat");

            
            
            webSocketServer.Start();
            
            rooms.Add(new Room(0, "GENERAL", null, 0, 500, "public", false, true, true, true, new List<Player>(), null));
            rooms.Add(new Room(1, "MatchmakingWaitinglist", null, 0, 500, "public", false, true, true, true, new List<Player>(), null));

            Console.WriteLine("Websocket Server started!");
            Console.ReadKey(true);
            Console.WriteLine("Websocket Server stoped!");
            webSocketServer.Stop();
        }
    }
}