using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace TowersWebsocketNet31.Server.Game.EntityData
{
    [Serializable]
    public class GroupsMonsterList
    {
        public List<GroupsJsonObject> groups { get; set; }
    }

    [Serializable]
    public class RawMonsterList
    {
        public List<MobJsonObject> monsters;
    }
    public class MonsterList
    {
        //private GameObject[] monsterGameObjects;

        public List<GroupsMonster> groupsList;
        public List<Monster> monsterList;
        
        public MonsterList(string json)
        {
            groupsList = new List<GroupsMonster>();
            monsterList = new List<Monster>();

            InitMonsterList(json);
        }

        public GroupsMonster GetGroupsMonsterById(int id)
        {
            return Utils.Clone(groupsList.First(group => group.id == id));
        }

        public Monster GetMonsterById(int id)
        {
            Monster cloneMonster = Utils.Clone(monsterList.First(monster => monster.id == id));
            cloneMonster.typeEntity = TypeEntity.MOB;
            return cloneMonster;
        }

        private void InitMonsterList(string json)
        {
            try
            {
                GroupsMonsterList mobsList = JsonSerializer.Deserialize<GroupsMonsterList>(json);
                
                if (mobsList == null)
                {
                    return;
                }

                foreach (GroupsJsonObject groupsJson in mobsList.groups)
                {
                    groupsList.Add(groupsJson.ConvertToMonsterGroups());
                    foreach (MobJsonObject mob in groupsJson.monsterList)
                    {
                        Monster monster = mob.ConvertToMonster();

                        if (!monsterList.Exists(monsterAdded => monsterAdded.id == monster.id))
                        {
                            monsterList.Add(monster);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR at the creation of MonsterList");
                Console.WriteLine(json);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Data);
            }
        }
        
    }
}