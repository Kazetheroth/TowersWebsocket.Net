using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using TowersWebsocketNet31.Server;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace TowersWebsocketNet31.Chat
{
    
    public class ChatWebsocket : WebSocketBehavior
    {
        public static List<Server.Player.Player> players = new List<Server.Player.Player>();
        public static List<Server.Room.Room> rooms = new List<Server.Room.Room>();
        
        protected override void OnOpen()
        {
            base.OnOpen();
            Server.Player.Player newPlayer = new Server.Player.Player(ID);
            players.Add(newPlayer);
            Console.WriteLine($"New connection->{ID}");
        }

        protected override void OnMessage (MessageEventArgs e)
        {
            base.OnMessage(e);
            Console.WriteLine($"Received Message : {e.Data}");
            Message newMessage;
            if (JsonSerializer.Deserialize<Message>(e.Data) != null)
            {
                newMessage = JsonSerializer.Deserialize<Message>(e.Data);
                CallbackMessages callback = OnMessageArgs(ref newMessage);
                if (callback != null)
                {
                    /*** ALL TARGET ***/
                    if (newMessage._TARGET == TargetMessage.Target[(int)TargetType.All])
                    {
                        List<Server.Player.Player> playerList = rooms.Find(r => r.Name == newMessage._ROOMID)?.PlayerList;
                        if (playerList != null)
                        {
                            foreach (Server.Player.Player player in playerList)
                            {
                                foreach (string returnCallbackMessage in callback.callbacks)
                                {
                                    Console.WriteLine($"Sending callback : {returnCallbackMessage}\nTo : {player.Id}");
                                    Sessions.SendTo(returnCallbackMessage, player.Id);
                                }
                            } 
                        }
                    }
                    /*** OTHERS TARGET ***/
                    else if (newMessage._TARGET == TargetMessage.Target[(int) TargetType.Others])
                    {
                        var playerList = rooms.Find(r => r.Name == newMessage._ROOMID)?.PlayerList;
                        if (playerList != null)
                        {
                            foreach (Server.Player.Player player in playerList)
                            {
                                if (player.Id != ID)
                                {
                                    foreach (string returnCallbackMessage in callback.callbacks)
                                    {
                                        Console.WriteLine($"Sending callback : {returnCallbackMessage}\nTo : {player.Id}");
                                        Sessions.SendTo(returnCallbackMessage, player.Id);
                                    }
                                }
                            }
                        }
                    }
                    /*** SELF TARGET ***/
                    else if (newMessage._TARGET == TargetMessage.Target[(int)TargetType.Self])
                    {
                        foreach (string returnCallbackMessage in callback.callbacks)
                        {
                            Console.WriteLine($"Sending callback : {returnCallbackMessage}\nTo : {ID}");
                            Sessions.SendTo(returnCallbackMessage, ID);
                        }
                    }
                    /*** ONLY_ONE TARGET ***/
                    else if (newMessage._TARGET == TargetMessage.Target[(int)TargetType.OnlyOne])
                    {
                        var playerList = rooms.Find(r => r.Name == newMessage._ROOMID)?.PlayerList;
                        if (playerList != null)
                        {
                            foreach (Server.Player.Player player in playerList)
                            {
                                if (player.AuthToken == newMessage._ARGS[0].tokenTarget)
                                {
                                    foreach (string returnCallbackMessage in callback.callbacks)
                                    {
                                        Console.WriteLine($"Sending callback : {returnCallbackMessage}\nTo : {player.Id}");
                                        Sessions.SendTo(returnCallbackMessage, player.Id);
                                    }
                                }
                            }
                        }
                    }
                    /*** END OF CONDITIONS ***/
                }
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            
        }

        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
        }

        CallbackMessages OnMessageArgs(ref Message newMessage)
        {
            CallbackMessages callback = new CallbackMessages(new List<string>());
            if (newMessage._METHOD != null)
            {
                switch (newMessage._METHOD)
                {
                    case "setIdentity":
                        Console.WriteLine("ID : " + ID);
                        callback.callbacks.Add(players.Find(x => x.Id == ID)?.SetIndentity(newMessage._ARGS[0].tokenPlayer, newMessage._ROOMID));
                        callback.callbacks.Add("{\"callbackMessages\":{\"message\":\"Identity Set\"}}");
                        break;
                    default:
                        break;
                }
            }
            return callback;
        }
    }
}