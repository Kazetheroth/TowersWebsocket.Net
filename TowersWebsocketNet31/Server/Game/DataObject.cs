using System.Collections.Generic;
using TowersWebsocketNet31.Server.Game.EntityData;
using TowersWebsocketNet31.Server.Game.EquipmentData;

namespace TowersWebsocketNet31.Server.Game
{
    public static class DataObject
    {
        public static int nbEntityInScene;
        
        public static MonsterList MonsterList;
        public static EquipmentList EquipmentList;
        //public static CardList CardList;
        //public static List<Material> MaterialsList;
        
        public static List<Monster> monsterInScene = new List<Monster>();
        //public static Dictionary<int, PlayerPrefab> playerInScene = new Dictionary<int, PlayerPrefab>();
        public static List<Entity> invocationsInScene = new List<Entity>();

        //public static List<GameObject> objectInScene = new List<GameObject>();
    }
}