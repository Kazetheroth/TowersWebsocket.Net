using System;
using System.Collections;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Serialization.Json;
using TowersWebsocketNet31.Server.Game;
using TowersWebsocketNet31.Server.Game.EntityData;
using TowersWebsocketNet31.Server.Game.EquipmentData;
using TowersWebsocketNet31.Server.Game.Models;


namespace TowersWebsocketNet31.Server.Account
{
    public class Account
    {
        private string id;
        private string roomId;
        private string authToken;
        private bool gameLoaded;
        private bool defenseEnd;
        private bool defenseReady;
        private bool attackReady;
        private bool isBot;

        private GameInstance currentGameInstance;
        private List<Deck> decks;
        private Dictionary<int, int> cardInCollection;

        public Account(string id)
        {
            this.id = id;
        }
        public Account(string id, string roomId, string authToken)
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

        public List<Deck> Decks
        {
            get => decks;
            set => decks = value;
        }

        public Dictionary<int, int> CardInCollection
        {
            get => cardInCollection;
            set => cardInCollection = value;
        }

        public string SetIndentity(string playerToken, string room)
        {
            AuthToken = playerToken;
            roomId = room;
            bool searching = true;
            while (searching)
            {
                Account account = Program.rooms.Find(x => x.Name == roomId)?.PlayerList.Find(y => y.AuthToken == playerToken);
                searching = account != null;
                if (searching)
                {
                    Program.rooms.Find(x => x.Name == roomId)?.PlayerList.Remove(account);
                }
            }

            DataObject.LoadDeckAndCollection(this);

            Console.WriteLine("==================================================================");
            Console.WriteLine(decks.Count);
            Console.WriteLine(cardInCollection.Count);
            
            Program.rooms.Find(x => x.Name == roomId)?.PlayerList.Add(this);
            Console.WriteLine(id);
            return "{\"callbackMessages\":{\"message\":\"Identity\", \"identity\":{\"classes\":"+ -1 +", \"weapon\":"+ -1 +"}}}";
        }

        public string JoinWaitingRanked(string room)
        {
            Program.rooms.Find(x => x.Name == roomId)?.PlayerList.Remove(this);
            roomId = room;
            Program.rooms.Find(x => x.Name == roomId)?.PlayerList.Add(this);
            return "{\"callbackMessages\":{\"message\":\"WaitingForRanked\"}}";
        }
        public string QuitMatchmaking(string room)
        {
            Program.rooms.Find(x => x.Name == roomId)?.PlayerList.Remove(this);
            roomId = room;
            Program.rooms.Find(x => x.Name == roomId)?.PlayerList.Add(this);
            return "{\"callbackMessages\":{\"message\":\"LeavingMatchmaking\"}}";
        }
        public bool JoinMatchRanked(TowersWebsocket session)
        {
            if (Program.rooms.Find(x => x.Name == roomId)?.PlayerList.Count >= 2)
            {
                Account opponent = Program.rooms.Find(x => x.Name == roomId)?.PlayerList
                    .Find(y => y.authToken != this.authToken);
                if (opponent != null)
                {
                    var api = new RestClient("https://www.towers.heolia.eu");
                    var request = new RestRequest("api/v1/account/rankedJoin");
                    request.AddParameter("gameToken", "-dx+L-oGb:EDJ,kGcP(7lVTe^0?9nv");
                    var response = api.Post(request);
                    var content = response.Content;
                    
                    Console.WriteLine(content);
                    if (!content.Contains("SearchingMatch"))
                    {
                        try
                        {
                            Console.WriteLine(content);
                            List<Account> players = new List<Account>();
                            players.Add(this);
                            players.Add(opponent);
                            Program.rooms.Add(new Room.Room(Program.rooms.Count, content, null, 0, 2, "1v1", true, true, true, false, players, null));
                            string callback = "{\"callbackMessages\":{\"room\":\"" + content + "\", \"message\":\"MatchStart\"}}";
                            session.SendToTarget(callback, id);
                            session.SendToTarget(callback, opponent.id);
                            return true;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                }
            }
            return false;
        }

        public void SetGameLoaded()
        {
            gameLoaded = true;
            defenseReady = false;
            attackReady = false;
        }

        public void SetDefenseReady()
        {
            defenseReady = true;
            attackReady = false;
        }
        public void SetAttackReady()
        {
            defenseReady = false;
            attackReady = true;
        }

        public void InitGameInstance(string classes, string weapon, string equipmentDeck, string monsterDeck)
        {
            Classes wantedClass = Enum.Parse<Classes>(classes);
            CategoryWeapon typeWeapon = Enum.Parse<CategoryWeapon>(weapon);
            int idEquipmentDeck = Int32.Parse(equipmentDeck);
            int idMonsterDeck = Int32.Parse(monsterDeck);
            int idPlayer = 1;

            currentGameInstance = new GameInstance(idPlayer, wantedClass, typeWeapon, idEquipmentDeck, idMonsterDeck, Program.rooms.Find(x => x.Name == roomId)?.GameGrid);
        }
    }
}