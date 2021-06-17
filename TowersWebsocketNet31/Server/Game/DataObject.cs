using System;
using System.Threading.Tasks;
using RestSharp;
using TowersWebsocketNet31.Server.Game.EntityData;
using TowersWebsocketNet31.Server.Game.EquipmentData;
using TowersWebsocketNet31.Server.Game.Models;
using TowersWebsocketNet31.Server.Game.SpellData;

namespace TowersWebsocketNet31.Server.Game
{
    public static class DataObject
    {   
        public static ClassesList ClassesList;
        public static SpellList SpellList;
        public static CategoryWeaponList CategoryWeaponList;
        public static MonsterList MonsterList;
        public static EquipmentList EquipmentList;
        public static CardList CardList;

        public static async Task InitDictionary()
        {
            await CollectSpell();
            await CollectCategoryWeapon();
            await CollectClasses();
            await CollectEquipment();
            await CollectClassesEquipment();
            await CollectMonster();
            await CollectCard();
        }


        private static async Task CollectSpell()
        {
            RestClient api = new RestClient("https://www.towers.heolia.eu");
            RestRequest request = new RestRequest("services/game/skill/list.php");
            var response = api.Post(request);
            var content = response.Content;

            if (response.IsSuccessful)
            {
                SpellList = new SpellList(content);
            }
            else
            {
                Console.WriteLine("Error at Collect spell");
                Console.WriteLine(response.Content);
            }
        }

        private static async Task CollectCategoryWeapon()
        {
            RestClient api = new RestClient("https://www.towers.heolia.eu");
            RestRequest request = new RestRequest("services/game/category/list.php");
            var response = api.Post(request);
            var content = response.Content;

            if (response.IsSuccessful)
            {
                CategoryWeaponList = new CategoryWeaponList(content);
            }
            else
            {
                Console.WriteLine("Error at Collect category");
                Console.WriteLine(response.Content);
            }
        }

        private static async Task CollectClasses()
        {
            RestClient api = new RestClient("https://www.towers.heolia.eu");
            RestRequest request = new RestRequest("services/game/classes/list.php");
            var response = api.Post(request);
            var content = response.Content;

            if (response.IsSuccessful)
            {
                ClassesList = new ClassesList(content);
            }
            else
            {
                Console.WriteLine("Error at Collect classes");
                Console.WriteLine(response.Content);
            }
        }

        private static async Task CollectClassesEquipment()
        {
            RestClient api = new RestClient("https://www.towers.heolia.eu");
            RestRequest request = new RestRequest("services/game/classes_category/list.php");
            var response = api.Post(request);
            var content = response.Content;

            if (response.IsSuccessful)
            {
                ClassesList.InitClassesCategorySpells(content);
            }
            else
            {
                Console.WriteLine("Error at Collect classes / equipment");
                Console.WriteLine(response.Content);
            }
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
                Console.WriteLine("Error at Collect equipment");
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
                Console.WriteLine("Error at Collect monster");
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
                Console.WriteLine("Error at Collect card");
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
                Console.WriteLine("Error at Collect deck and collection");
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
                Console.WriteLine("Error at Collect collection");
                Console.WriteLine(response.Content);
            }
        }
    }
}