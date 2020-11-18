using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TowersWebsocketNet31.Chat;
using TowersWebsocketNet31.Server;
using TowersWebsocketNet31.Server.Account;
using TowersWebsocketNet31.Server.Game;
using TowersWebsocketNet31.Server.Room;
using WebSocketSharp.Server;

namespace TowersWebsocketNet31
{
    public static class Program
    {
        private static string _endpoint;
        private static string _port;
        private static string _address;
        public static List<Account> players = new List<Account>();
        public static List<Room> rooms = new List<Room>();
        public static WebSocketServer webSocketServer;

        public static async Task Main(string[] args)
        {
            //await TestMain.RunTestAsyncOneEffect();

            await DataObject.InitDictionary();

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
                string path = "/app/server/TowersWebsocketNet31/towers.env";
                Console.WriteLine($"Address : {path}");
                DotNetEnv.Env.Load(path);
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
            
            rooms.Add(new Room(0, "GENERAL", null, 0, 500, "public", false, true, true, true, new List<Account>(), null));
            rooms.Add(new Room(1, "MatchmakingWaitinglist", null, 0, 500, "public", false, true, true, true, new List<Account>(), null));

            Console.WriteLine("Websocket Server started!");
            Console.ReadKey(true);
            Console.WriteLine("Websocket Server stoped!");
            webSocketServer.Stop();
        }
    }
}