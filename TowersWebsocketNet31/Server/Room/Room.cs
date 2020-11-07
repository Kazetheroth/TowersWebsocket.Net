using System;
using System.Collections.Generic;
using System.Timers;
using WebSocketSharp.Server;

namespace TowersWebsocketNet31.Server.Room
{
    public class Room
    {
        private int id;
        private string name;
        private string password;
        private int roomOwner;
        private int maxPlayers;
        private string mode;
        private bool isRanking;
        private bool isPublic;
        private bool isLaunched;
        private bool hasEnded;
        private List<Player.Player> playerList;
        private string stage;
        private int timerValue;

        public Room(int id, string name, string password, int roomOwner, int maxPlayers, string mode, bool isRanking, bool isPublic, bool isLaunched, bool hasEnded, List<Player.Player> playerList, string stage)
        {
            this.id = id;
            this.name = name;
            this.password = password;
            this.roomOwner = roomOwner;
            this.maxPlayers = maxPlayers;
            this.mode = mode;
            this.isRanking = isRanking;
            this.isPublic = isPublic;
            this.isLaunched = isLaunched;
            this.hasEnded = hasEnded;
            this.playerList = playerList;
            this.stage = stage;
        }

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Password
        {
            get => password;
            set => password = value;
        }

        public int RoomOwner
        {
            get => roomOwner;
            set => roomOwner = value;
        }

        public int MaxPlayers
        {
            get => maxPlayers;
            set => maxPlayers = value;
        }

        public string Mode
        {
            get => mode;
            set => mode = value;
        }

        public bool IsRanking
        {
            get => isRanking;
            set => isRanking = value;
        }

        public bool IsPublic
        {
            get => isPublic;
            set => isPublic = value;
        }

        public bool IsLaunched
        {
            get => isLaunched;
            set => isLaunched = value;
        }

        public bool HasEnded
        {
            get => hasEnded;
            set => hasEnded = value;
        }

        public List<Player.Player> PlayerList
        {
            get => playerList;
            set => playerList = value;
        }

        public void StartChooseRoleTimer(TowersWebsocket session)
        {
            int nbReady = 0;
            foreach (Player.Player player in PlayerList)
            {
                nbReady = player.GameLoaded ? nbReady + 1 : nbReady;
            }

            if (nbReady == 2)
            {
                foreach (Player.Player player in PlayerList)
                {
                    session.SendToTarget("{\"callbackMessages\":{\"message\":\"LoadGame\"}}", player.Id);
                }
                stage = "roleTimer";
                timerValue = 100;
                var timer = new System.Timers.Timer(1000);
                timer.Elapsed += OnTimedEvent;
                timer.Enabled = true;
            }
        }
        
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (stage == "roleTimer" || stage == "defenseTimer")
                timerValue--;
            else
                timerValue++;

            if (timerValue <= 0)
                return;
            WebSocketServiceHost webSocketServiceHost;
            Program.webSocketServer.WebSocketServices.TryGetServiceHost("/websocket", out webSocketServiceHost);
            //'{"_TARGET":"ALL", "_ROOMID":"' + room + '", "callbackMessages":{"' + timer + '":' + str(seconds) + '}}'
            string callback = "{\"callbackMessages\":{\"" + stage + "\":" + timerValue + "}}";
            Console.WriteLine(callback);
            foreach (Player.Player player in PlayerList)
            {
                webSocketServiceHost.Sessions.SendTo(callback, player.Id);
            }
            
        }
    }
}