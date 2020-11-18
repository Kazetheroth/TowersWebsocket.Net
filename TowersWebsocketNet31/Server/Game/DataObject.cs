using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using TowersWebsocketNet31.Server.Game.EntityData;
using TowersWebsocketNet31.Server.Game.EquipmentData;
using TowersWebsocketNet31.Server.Game.Models;

namespace TowersWebsocketNet31.Server.Game
{
    public static class DataObject
    {   
        public static MonsterList MonsterList;
        public static EquipmentList EquipmentList;
        public static CardList CardList;

        public static async Task InitDictionary()
        {
            await CollectEquipment();
            await CollectMonster();
            await CollectCard();
        }

        private static async Task CollectEquipment()
        {
            RestClient api = new RestClient("https://www.towers.heolia.eu");
            RestRequest request = new RestRequest("services/game/equipment/list.php");
            var response = api.Post(request);
            var content = response.Content;

            if (response.IsSuccessful)
            {
                EquipmentList = new EquipmentList(content);
            }
            else
            {
                Console.WriteLine(response.Content);
            }
        }

        private static async Task CollectMonster()
        {
            RestClient api = new RestClient("https://www.towers.heolia.eu");
            RestRequest request = new RestRequest("services/game/group/list.php");
            var response = api.Post(request);
            var content = response.Content;

            if (response.IsSuccessful)
            {
                MonsterList = new MonsterList(content);
            }
            else
            {
                Console.WriteLine(response.Content);
            }
        }

        private static async Task CollectCard()
        {
            RestClient api = new RestClient("https://www.towers.heolia.eu");
            RestRequest request = new RestRequest("services/game/card/list.php");
            var response = api.Post(request);
            var content = response.Content;

            if (response.IsSuccessful)
            {
                CardList = new CardList(content);
            }
            else
            {
                Console.WriteLine(response.Content);
            }
        }

        public static void LoadDeckAndCollection(Account.Account account)
        {   
            RestClient api = new RestClient("https://www.towers.heolia.eu");
            RestRequest request = new RestRequest("services/game/card/listCardDeck.php");
            request.AddParameter("deckOwner", account.AuthToken);
            var response = api.Post(request);
            var content = response.Content;

            if (response.IsSuccessful)
            {
                CardList.InitDeck(account, content);
            }
            else
            {
                Console.WriteLine(response.Content);
            }

            api = new RestClient("https://www.towers.heolia.eu");
            request = new RestRequest("services/game/card/listAccountCollection.php");
            request.AddParameter("collectionOwner", account.AuthToken);
            response = api.Post(request);
            content = response.Content;

            if (response.IsSuccessful)
            {
                CardList.InitCollection(account, content);
            }
            else
            {
                Console.WriteLine(response.Content);
            }
        }
    }
}