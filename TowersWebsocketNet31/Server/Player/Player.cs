using System;

namespace TowersWebsocketNet31.Server.Player
{
    public class Player
    {
        private string id;
        private string roomId;
        private string authToken;
        private bool gameLoaded;
        private bool defenseEnd;
        private bool defenseReady;
        private bool attackReady;
        private bool isBot;

        public Player(string id)
        {
            this.id = id;
        }
        public Player(string id, string roomId, string authToken)
        {
            this.id = id;
            this.roomId = roomId;
            this.authToken = authToken;
        }

        public string Id
        {
            get => id;
            set => id = value;
        }

        public string RoomId
        {
            get => roomId;
            set => roomId = value;
        }

        public string AuthToken
        {
            get => authToken;
            set => authToken = value;
        }

        public bool GameLoaded
        {
            get => gameLoaded;
            set => gameLoaded = value;
        }

        public bool DefenseEnd
        {
            get => defenseEnd;
            set => defenseEnd = value;
        }

        public bool DefenseReady
        {
            get => defenseReady;
            set => defenseReady = value;
        }

        public bool AttackReady
        {
            get => attackReady;
            set => attackReady = value;
        }

        public bool IsBot
        {
            get => isBot;
            set => isBot = value;
        }
        
        public string SetIndentity(string playerToken, string room)
        {
            AuthToken = playerToken;
            roomId = room;
            Console.WriteLine(id);
            return "{\"callbackMessages\":{\"message\":\"Identity\", \"identity\":{\"classes\":"+ -1 +", \"weapon\":"+ -1 +"}}}";
        }
        
    }
}