using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using WebSocketSharp;
using WebSocketSharp.Server;
using static TowersWebsocketNet31.Server.TargetMessage;

namespace TowersWebsocketNet31.Server
{
    
    public class TowersWebsocket : WebSocketBehavior
    {
        public static List<Player.Player> players = new List<Player.Player>();
        public static List<Room.Room> rooms = new List<Room.Room>();
        
        protected override void OnOpen()
        {
            base.OnOpen();
            var newPlayer = new Player.Player(ID);
            players.Add(newPlayer);
            Console.WriteLine($"New connection->{ID}");
        }

        protected override void OnMessage (MessageEventArgs e)
        {
            string callback;
            
            Console.WriteLine(e.Data);
            Message newMessage;
            if (JsonSerializer.Deserialize<Message>(e.Data) != null)
            {
                newMessage = JsonSerializer.Deserialize<Message>(e.Data);
                callback = OnMessageArgs(ref newMessage);
                if (callback != null)
                {
                    if (newMessage._TARGET == TargetMessage.Target[(int)TargetType.All])
                    {
                        
                    }
                    else if (newMessage._TARGET == TargetMessage.Target[(int) TargetType.Others])
                    {
                        
                    }
                    else if (newMessage._TARGET == TargetMessage.Target[(int)TargetType.Self])
                    {
                        
                    }
                    else if (newMessage._TARGET == TargetMessage.Target[(int)TargetType.OnlyOne])
                    {
                        
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        string OnMessageArgs(ref Message newMessage)
        {
            string callback = null;
            if (newMessage._METHOD != null)
            {
                switch (newMessage._METHOD)
                {
                    case "setIdentity":
                        Console.WriteLine("ID : " + ID);
                        callback = players.Find(x => x.Id == ID)?.SetIndentity(newMessage._ARGS[0].tokenPlayer, newMessage._ROOMID);
                        Console.WriteLine(callback);
                        Sessions.SendTo(callback, ID);
                        Sessions.SendTo("{\"callbackMessages\":{\"message\":\"Identity Set\"}}", ID);
                        break;
                    default:
                        break;
                }
            }
            return callback;
        }
    }
}