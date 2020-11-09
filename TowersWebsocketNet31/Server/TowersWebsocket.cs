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
            if (JsonSerializer.Deserialize<Message>(e.Data) != null)
            {
                var newMessage = JsonSerializer.Deserialize<Message>(e.Data);
                
                if (newMessage.GRID != null)
                {
                    var playerList = Program.rooms.Find(r => r.Name == newMessage._ROOMID)?.PlayerList;
                    if (playerList != null)
                    {
                        foreach (Player.Player player in playerList)
                        {
                            if (player.Id != ID)
                            {
                                SendToTarget("{\"GRID\":\"" + newMessage.GRID + "\"}", player.Id);
                            }
                        }
                    }
                    return;
                }
                
                Callbacks callback = OnMessageArgs(ref newMessage);
                if (callback != null)
                {
                    /*** ALL TARGET ***/
                    if (newMessage._TARGET == TargetMessage.Target[(int)TargetType.All])
                    {
                        SendToAll(newMessage, callback);
                    }
                    /*** OTHERS TARGET ***/
                    else if (newMessage._TARGET == TargetMessage.Target[(int) TargetType.Others])
                    {
                        SendToOthers(newMessage, callback);
                    }
                    /*** SELF TARGET ***/
                    else if (newMessage._TARGET == TargetMessage.Target[(int)TargetType.Self])
                    {
                        SendToSelf(newMessage, callback);
                    }
                    /*** ONLY_ONE TARGET ***/
                    else if (newMessage._TARGET == TargetMessage.Target[(int)TargetType.OnlyOne])
                    {
                        SendToTarget(newMessage, callback);
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
                        SendToTarget("{\"callbackMessages\":{\"message\":\"DEATH\"}}", pl.Id);
                    }
                }
            }
            Program.players.Remove(player);
        }

        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
            Player.Player player = Program.players.Find(p => p.Id == ID);
            Room.Room playerRoom = Program.rooms.Find(r => r.Name == player?.RoomId);
            if (playerRoom != null && playerRoom.Name != "GENERAL" && playerRoom.Name != "MatchmakingWaitinglist")
            {
                foreach (Player.Player pl in playerRoom.PlayerList)
                {
                    if (ID != pl.Id)
                    {
                        SendToTarget("{\"callbackMessages\":{\"message\":\"DEATH\"}}", pl.Id);
                    }
                }
            }
            Program.players.Remove(player);
        }

        private void SendToAll(Message newMessage, Callbacks callback)
        {
            List<Player.Player> playerList = Program.rooms.Find(r => r.Name == newMessage._ROOMID)?.PlayerList;
            if (playerList != null)
            {
                foreach (Player.Player player in playerList)
                {
                    foreach (string returnCallbackMessage in callback.callbacks)
                    {
                        SendToTarget(returnCallbackMessage, player.Id);
                    }
                } 
            }
        }

        private void SendToSelf(Message newMessage, Callbacks callback)
        {
            foreach (string returnCallbackMessage in callback.callbacks)
            {
                SendToTarget(returnCallbackMessage, ID);
            }
        }

        private void SendToOthers(Message newMessage, Callbacks callback)
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
                            SendToTarget(returnCallbackMessage, player.Id);
                        }
                    }
                }
            }
        }

        private void SendToTarget(Message newMessage, Callbacks callback)
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
                            SendToTarget(returnCallbackMessage, player.Id);
                        }
                    }
                }
            }
        }

        Callbacks OnMessageArgs(ref Message newMessage)
        {
            Callbacks callback = new Callbacks(new List<string>());
            if (newMessage._METHOD != null)
            {
                Message message = newMessage;
                switch (newMessage._METHOD)
                {
                    case "setIdentity":
                        callback.callbacks.Add(Program.players.Find(x => x.Id == ID)?.SetIndentity(newMessage._ARGS[0].tokenPlayer, newMessage._ROOMID));
                        callback.callbacks.Add("{\"callbackMessages\":{\"message\":\"Identity Set\"}}");
                        break;
                    case "joinWaitingRanked":
                        callback.callbacks.Add(Program.players.Find(x => x.Id == ID)?.JoinWaitingRanked(newMessage._ARGS[0].room));
                        Console.WriteLine($"Room : {Program.rooms[0].Name} - {Program.rooms[0].PlayerList.Count}");
                        Console.WriteLine($"Room : {Program.rooms[1].Name} - {Program.rooms[1].PlayerList.Count}");
                        break;
                    case "getRankedMatch":
                        //Console.WriteLine(newMessage._ARGS[0]);
                        if (!Program.players.Find(x => x.Id == ID)?.JoinMatchRanked(this) == true)
                            callback.callbacks.Add("{\"callbackMessages\":{\"message\":\"Searching Match\"}}");
                        break;
                    case "setGameLoaded":
                        Program.players.Find(x => x.Id == ID)?.SetGameLoaded();
                        Program.rooms.Find(x => x.Name == message._ROOMID)?.StartPhase(this, "roleTimer", "LoadGame", 100);
                        break;
                    case "setDefenseReady":
                        Program.players.Find(x => x.Id == ID)?.SetDefenseReady();
                        Program.rooms.Find(x => x.Name == message._ROOMID)?.StartPhase(this, "defenseTimer", "StartDefense", 120);
                        break;
                    case "setAttackReady":
                        Program.players.Find(x => x.Id == ID)?.SetAttackReady();
                        Program.rooms.Find(x => x.Name == message._ROOMID)?.StartPhase(this, "attackTimer", "StartAttack", 0);
                        break;
                    default:
                        break;
                }
            }

            callback = callback.callbacks.Count > 0 ? callback : null;
            return callback;
        }

        public void SendToTarget(string returnCallbackMessage, string id)
        {
            Console.WriteLine($"Sending callback : {returnCallbackMessage}\nTo : {id}");
            Sessions.SendTo(returnCallbackMessage, id);
        }
    }
}