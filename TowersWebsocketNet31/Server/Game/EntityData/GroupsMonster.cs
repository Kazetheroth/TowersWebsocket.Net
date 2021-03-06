﻿using System.Collections.Generic;
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

        public List<MonstersInGroup> monstersInGroupList { get; set; } = new List<MonstersInGroup>();

        public void InitSpecificEquipment(Monster monster, List<int> equipment)
        {
            int nbWeaponFound = 0;

            if (equipment != null)
            {
                if (monster.constraint == TypeWeapon.Cac)
                {
                    if (equipment[0] != 0)
                    {
                        if (monster.InitWeapon(equipment[0]))
                        {
                            nbWeaponFound++;
                        }
                    }
                }
                else if (monster.constraint == TypeWeapon.Distance)
                {
                    if (equipment[1] != 0)
                    {
                        if (monster.InitWeapon(equipment[1]))
                        {
                            nbWeaponFound++;
                        }
                    }
                
                    // TODO init armor with : equipment[2] / equipment[3] / equipment[4]
                }
            }

            if (nbWeaponFound == 0)
            {
                monster.InitOriginalWeapon();
            }
        }

//        public bool IsMonsterListDifferentToOtherList(List<MonstersInGroup> comparedMonstersInGroups)
//        {
//            if (monstersInGroupList.Count != comparedMonstersInGroups.Count)
//            {
//                return true;
//            }
//
//            bool doesntFindMonster = false;
//            foreach (MonstersInGroup monstersInGroup in monstersInGroupList)
//            {
//                doesntFindMonster = !comparedMonstersInGroups.Exists(monster => monster.GetMonsterId() == monstersInGroup.GetMonsterId());
//                
//                if (doesntFindMonster)
//                {
//                    break;
//                }
//            }
//
//            return doesntFindMonster;
//        }
    }
}
