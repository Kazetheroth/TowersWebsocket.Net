using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace TowersWebsocketNet31.Server
{
    public class TowersWebsocket : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            base.OnOpen();
            Account.Account newAccount = new Account.Account(ID);
            Program.players.Add(newAccount);
            Console.WriteLine($"New connection->{ID}");
        }

        protected override void OnMessage (MessageEventArgs e)
        {
            base.OnMessage(e);
            Console.WriteLine($"Received Message : {e.Data}");

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            var newMessage = JsonSerializer.Deserialize<Message>(e.Data, options);
            if (newMessage != null)
            {
                LoggerUtils.WriteToLogFile(newMessage._ROOMID, e.Data);
//                if (newMessage.GRID != null)
//                {
//                    var playerList = Program.rooms.Find(r => r.Name == newMessage._ROOMID)?.PlayerList;
//                    if (playerList != null)
//                    {
//                        foreach (Account.Account player in playerList)
//                        {
//                            if (player.Id != ID)
//                            {
//                                SendToTarget("{\"GRID\":\"" + newMessage.GRID + "\"}", player.Id);
//                            }
//                        }
//                    }
//                    return;
//                }
                
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
            Console.WriteLine(e.Code);
            if (Sessions.PingTo(ID))
            {
                Sessions.CloseSession(ID);
            }
            Account.Account account = Program.players.Find(p => p.Id == ID);
            Room.Room playerRoom = Program.rooms.Find(r => r.Name == account?.RoomId);
            if (playerRoom != null && playerRoom.Name != "GENERAL" && playerRoom.Name != "MatchmakingWaitinglist")
            {
                foreach (Account.Account pl in playerRoom.PlayerList)
                {
                    if (ID != pl.Id)
                    {
                        SendToTarget("{\"callbackMessages\":{\"message\":\"DEATH\"}}", pl.Id);
                    }
                }
            }
            Program.rooms.Find(x => playerRoom != null && x.Name == playerRoom.Name)?.PlayerList.Remove(account);
            Program.players.Remove(account);
        }

        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
            Console.WriteLine(e.Exception);
            if (Sessions.PingTo(ID))
            {
                Sessions.CloseSession(ID);
            }
            Account.Account account = Program.players.Find(p => p.Id == ID);
            Room.Room playerRoom = Program.rooms.Find(r => r.Name == account?.RoomId);
            if (playerRoom != null && playerRoom.Name != "GENERAL" && playerRoom.Name != "MatchmakingWaitinglist")
            {
                foreach (Account.Account pl in playerRoom.PlayerList)
                {
                    if (ID != pl.Id)
                    {
                        SendToTarget("{\"callbackMessages\":{\"message\":\"DEATH\"}}", pl.Id);
                    }
                }
            }
            Program.rooms.Find(x => playerRoom != null && x.Name == playerRoom.Name)?.PlayerList.Remove(account);
            Program.players.Remove(account);
        }

        private void SendToAll(Message newMessage, Callbacks callback)
        {
            List<Account.Account> playerList = Program.rooms.Find(r => r.Name == newMessage._ROOMID)?.PlayerList;
            string roomName = Program.rooms.Find(r => r.Name == newMessage._ROOMID)?.Name;
            if (playerList != null)
            {
                foreach (Account.Account player in playerList)
                {
                    foreach (string returnCallbackMessage in callback.callbacks)
                    {
                        LoggerUtils.WriteToLogFile(roomName, returnCallbackMessage);
                        SendToTarget(returnCallbackMessage, player.Id);
                    }
                } 
            }
        }

        private void SendToSelf(Message newMessage, Callbacks callback)
        {
            string roomName = Program.rooms.Find(r => r.Name == newMessage._ROOMID)?.Name;
            foreach (string returnCallbackMessage in callback.callbacks)
            {
                LoggerUtils.WriteToLogFile(roomName, returnCallbackMessage);
                SendToTarget(returnCallbackMessage, ID);
            }
        }

        private void SendToOthers(Message newMessage, Callbacks callback)
        {
            var playerList = Program.rooms.Find(r => r.Name == newMessage._ROOMID)?.PlayerList;
            string roomName = Program.rooms.Find(r => r.Name == newMessage._ROOMID)?.Name;
            if (playerList != null)
            {
                foreach (Account.Account player in playerList)
                {
                    if (player.Id != ID)
                    {
                        foreach (string returnCallbackMessage in callback.callbacks)
                        {
                            LoggerUtils.WriteToLogFile(roomName, returnCallbackMessage);
                            SendToTarget(returnCallbackMessage, player.Id);
                        }
                    }
                }
            }
        }

        private void SendToTarget(Message newMessage, Callbacks callback)
        {
            var playerList = Program.rooms.Find(r => r.Name == newMessage._ROOMID)?.PlayerList;
            string roomName = Program.rooms.Find(r => r.Name == newMessage._ROOMID)?.Name;
            if (playerList != null)
            {
                foreach (Account.Account player in playerList)
                {
                    if (player.AuthToken == newMessage._ARGS[0].tokenTarget)
                    {
                        foreach (string returnCallbackMessage in callback.callbacks)
                        {
                            LoggerUtils.WriteToLogFile(roomName, returnCallbackMessage);
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
                    case "quitMatchmaking":
                        callback.callbacks.Add(Program.players.Find(x => x.Id == ID)?.QuitMatchmaking("GENERAL"));
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
                    case "initGame":
                        Program.players.Find(x => x.Id == ID)?.InitGameInstance(newMessage._ARGS[0].classes, newMessage._ARGS[0].weapon, newMessage._ARGS[0].equipmentDeck, newMessage._ARGS[0].monsterDeck);
                        Program.players.Find(x => x.Id == ID)?.SetDefenseReady();
                        Program.rooms.Find(x => x.Name == message._ROOMID)?.StartPhase(this, "defenseTimer", "StartDefense", 120);
                        break;
                    case "setDefenseReady":
                        Program.players.Find(x => x.Id == ID)?.SetDefenseReady();
                        Program.rooms.Find(x => x.Name == message._ROOMID)?.StartPhase(this, "defenseTimer", "StartDefense", 120);
                        break;
                    case "setAttackReady":
                        Program.players.Find(x => x.Id == ID)?.SetAttackReady();
                        Program.rooms.Find(x => x.Name == message._ROOMID)?.StartPhase(this, "attackTimer", "StartAttack", 0);
                        break;
                    case "waitingForAttackGrid":
                        Program.players.Find(x => x.Id == ID)?.SetWaitingForAttackGrid();
                        Program.players.Find(x => x.Id != ID)?.SetNewGrid(newMessage._ARGS[0].gameGrid);
                        Program.rooms.Find(x => x.Name == message._ROOMID)?.SendAttackGridToPlayer(this, "LoadAttackGrid");
                        break;
                    case "SendDeath":
                        callback.callbacks.Add("{\"callbackMessages\":{\"message\":\"DEATH\"}}");
                        break;
                    case "HasWon":
                        callback.callbacks.Add("{\"callbackMessages\":{\"message\":\"WON\"}}");
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