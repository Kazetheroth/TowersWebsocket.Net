using System;
using System.Collections.Generic;
using System.Text.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace TowersWebsocketNet31.Server
{
    public class TowersWebsocket : WebSocketBehavior
    {
        public static List<Player.Player> players = new List<Player.Player>();
        
        protected override void OnOpen()
        {
            base.OnOpen();
            var newPlayer = new Player.Player(ID, "GENERAL", "Dabm1X6cnLnjGllVAiRi");
            players.Add(newPlayer);
            Console.WriteLine($"New connection->{newPlayer.AuthToken}");
        }

        protected override void OnMessage (MessageEventArgs e)
        {
            Console.WriteLine(e.Data);
            if (e.Data.Contains("GRID"))
            {
                
            }
            else
            {
                Message newMessage = new Message();
                if (JsonSerializer.Deserialize<Message>(e.Data) != null)
                {
                    newMessage = JsonSerializer.Deserialize<Message>(e.Data);
                }
                //string id = players.Find(player => player.AuthToken.Equals(newMessage._SENDER))?.Id;
                //Send ($"Message received : USER:{newMessage._SENDER}, DATA:{newMessage._GRID}, WSID: {id}");
            }
        }
    }
}