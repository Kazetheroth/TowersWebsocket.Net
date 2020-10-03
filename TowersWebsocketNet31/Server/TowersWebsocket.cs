using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

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
            Console.WriteLine(e.Data);
            Message newMessage;
            if (JsonSerializer.Deserialize<Message>(e.Data) != null)
            {
                newMessage = JsonSerializer.Deserialize<Message>(e.Data);
                var callback = OnMessageArgs(ref newMessage);
                if (callback != null)
                {
                    /*** ALL TARGET ***/
                    if (newMessage._TARGET == TargetMessage.Target[(int)TargetType.All])
                    {
                        var playerList = rooms.Find(r => r.Name == newMessage._ROOMID)?.PlayerList;
                        if (playerList != null)
                        {
                            foreach (var player in playerList)
                            {
                                Sessions.SendTo(callback, player.Id);
                            } 
                        }
                    }
                    /*** OTHERS TARGET ***/
                    else if (newMessage._TARGET == TargetMessage.Target[(int) TargetType.Others])
                    {
                        var playerList = rooms.Find(r => r.Name == newMessage._ROOMID)?.PlayerList;
                        if (playerList != null)
                        {
                            foreach (var player in playerList)
                            {
                                if (player.Id != ID)
                                {
                                    Sessions.SendTo(callback, player.Id);
                                }
                            }
                        }
                    }
                    /*** SELF TARGET ***/
                    else if (newMessage._TARGET == TargetMessage.Target[(int)TargetType.Self])
                    {
                        Sessions.SendTo(callback, ID);
                    }
                    /*** ONLY_ONE TARGET ***/
                    else if (newMessage._TARGET == TargetMessage.Target[(int)TargetType.OnlyOne])
                    {
                        var playerList = rooms.Find(r => r.Name == newMessage._ROOMID)?.PlayerList;
                        if (playerList != null)
                        {
                            foreach (var player in playerList)
                            {
                                if (player.AuthToken == newMessage._ARGS[0].tokenTarget)
                                {
                                    Sessions.SendTo(callback, player.Id);
                                }
                            }
                        }
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