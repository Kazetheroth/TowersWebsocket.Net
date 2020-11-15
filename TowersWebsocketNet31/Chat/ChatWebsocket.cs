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
        public static List<Server.Account.Account> players = new List<Server.Account.Account>();
        public static List<Server.Room.Room> rooms = new List<Server.Room.Room>();
        
        protected override void OnOpen()
        {
            base.OnOpen();
            Server.Account.Account newAccount = new Server.Account.Account(ID);
            players.Add(newAccount);
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
                Callbacks callback = OnMessageArgs(ref newMessage);
                if (callback != null)
                {
                    /*** ALL TARGET ***/
                    if (newMessage._TARGET == TargetMessage.Target[(int)TargetType.All])
                    {
                        List<Server.Account.Account> playerList = rooms.Find(r => r.Name == newMessage._ROOMID)?.PlayerList;
                        if (playerList != null)
                        {
                            foreach (Server.Account.Account player in playerList)
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
                            foreach (Server.Account.Account player in playerList)
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
                            foreach (Server.Account.Account player in playerList)
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

        Callbacks OnMessageArgs(ref Message newMessage)
        {
            Callbacks callback = new Callbacks(new List<string>());
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