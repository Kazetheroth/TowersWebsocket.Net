using System.Collections.Generic;
using TowersWebsocketNet31.Server.Game.EquipmentData;

namespace TowersWebsocketNet31.Server.Game.EntityData
{
    public enum Family
    {
        Demon,
        Human,
        Plant,
        Insect,
        Statue,
        Undead,
        Gobelin,
        Beast,
        Elementary,
        DivineCreature,
        Dwarf,
        Elf
    }

    public class MonstersInGroup
    {
        public Monster monster { get; set; }
        public int nbMonster { get; set; }

        public void SetMonster(Monster nMonster)
        {
            monster = nMonster;
        }

        public Monster GetMonster()
        {
            return Utils.Clone(monster);
        }

        public int GetMonsterId()
        {
            int id = -1;

            if (monster != null)
            {
                id = monster.id;
            }

            return id;
        }
    }

    public class GroupsMonster
    {
        public const int DEFAULT_RADIUS = 1;

        public int id { get; set; }
        public Family family { get; set; }
        public int cost { get; set; }
        public string name { get; set; }
        public int radius { get; set; } = DEFAULT_RADIUS;
        public bool hasKey { get; set; }

        public List<MonstersInGroup> monstersInGroupList { get; set; } = new List<MonstersInGroup>();
    }
}
