using System;
using System.Collections.Generic;
using System.Text.Json;
using RestSharp;
using TowersWebsocketNet31.Server.Game.EntityData;
using TowersWebsocketNet31.Server.Game.EquipmentData;

namespace TowersWebsocketNet31.Server.Game.Models
{
    [Serializable]
    public class CardJsonList
    {
        public List<CardJsonObject> cards { get; set; }
    }
    
    [Serializable]
    public class CardJsonObject
    {
        public string id { get; set; }
        public string monsterGroupId { get; set; }
        public string weaponId { get; set; }

        public Card ConvertToCard()
        {
            Card card = new Card
            {
                id = Int32.Parse(id),
                /*GroupsMonster = monsterGroupId != null ? DataObject.MonsterList.GetGroupsMonsterById(Int32.Parse(monsterGroupId)) : null,
                Weapon = weaponId != null ? DataObject.EquipmentList.GetWeaponWithId(Int32.Parse(weaponId)) : null*/
            };

            return card;
        }
    }
    public class Card
    {
        public int id { get; set; }

        public GroupsMonster GroupsMonster { get; set; }
        public Weapon Weapon { get; set; }
    }
}