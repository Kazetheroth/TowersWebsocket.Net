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

        protected override void OnOpen()
        {
            base.OnOpen();
            Player.Player newPlayer = new Player.Player(ID);
            Program.players.Add(newPlayer);
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
                foreach (var rooms in Program.rooms)
                {
                    Console.WriteLine($"Room : {rooms.Name}");
                    Console.WriteLine($"Room PlayerList Lenght : {rooms.PlayerList.Count}");
                }
                
                if (callback != null)
                {
                    /*** ALL TARGET ***/
                    if (newMessage._TARGET == TargetMessage.Target[(int)TargetType.All])
                    {
                        List<Player.Player> playerList = Program.rooms.Find(r => r.Name == newMessage._ROOMID)?.PlayerList;
                        if (playerList != null)
                        {
                            foreach (Player.Player player in playerList)
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
                        var playerList = Program.rooms.Find(r => r.Name == newMessage._ROOMID)?.PlayerList;
                        if (playerList != null)
                        {
                            foreach (Player.Player player in playerList)
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
                        var playerList = Program.rooms.Find(r => r.Name == newMessage._ROOMID)?.PlayerList;
                        if (playerList != null)
                        {
                            foreach (Player.Player player in playerList)
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
            Player.Player player = Program.players.Find(p => p.Id == ID);
            Room.Room playerRoom = Program.rooms.Find(r => r.Name == player?.RoomId);
            if (playerRoom != null && playerRoom.Name != "GENERAL" && playerRoom.Name != "MatchmakingWaitinglist")
            {
                foreach (Player.Player pl in playerRoom.PlayerList)
                {
                    if (ID != pl.Id)
                    {
                        Sessions.SendTo("{\"callbackMessages\":{\"message\":\"DEATH\"}}", pl.Id);
                    }
                }
            }
            Program.players.Remove(player);
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
                        callback.callbacks.Add(Program.players.Find(x => x.Id == ID)?.SetIndentity(newMessage._ARGS[0].tokenPlayer, newMessage._ROOMID));
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