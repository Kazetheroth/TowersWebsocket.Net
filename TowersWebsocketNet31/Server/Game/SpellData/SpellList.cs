using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using TowersWebsocketNet31.Server.Game.Mechanics;

namespace TowersWebsocketNet31.Server.Game.SpellData
{
    public class SpellList
    {
        public class SpellInfo
        {
            public string filename;
            public int id;
            public string spellName;
            public Spell spell;
        }
        
        // NameSpell - Spell
        public List<SpellInfo> SpellInfos;

        public SpellList(string json)
        {
            SpellInfos = new List<SpellInfo>();
            
            InitSpellDictionnary(json);
        }

        public Spell GetSpellByName(string name)
        {
            SpellInfo spellInfo;
            if (String.IsNullOrEmpty(name) || (spellInfo = SpellInfos.Find(info => info.spellName == name)) == null)
            {
                return null;
            }

            return Utils.Clone(spellInfo.spell);
        }

        public Spell GetSpellById(int id)
        {
            SpellInfo spellInfo = SpellInfos.Find(info => info.id == id);

            return spellInfo != null ? Utils.Clone(spellInfo.spell) : null;
        }
        
        private void InitSpellDictionnary(string json)
        {
            try
            {
                Console.WriteLine("Before deserialization spell list");
                SpellListObject spellList = JsonSerializer.Deserialize<SpellListObject>(json);
                Console.WriteLine("After deserialization spell list");
                Console.WriteLine(spellList);

                if (spellList != null)
                {
                    Console.WriteLine(spellList.skills);
                    Console.WriteLine(spellList.skills.Count);
                }
                else
                {
                    Console.WriteLine("Liste null - ahah");
                    return;
                }

                foreach (SpellJsonObject spellJsonObject in spellList.skills)
                {
                    DownloadSpell(spellJsonObject.name);
                    Spell currentSpell = LoadSpellByName(spellJsonObject.name);

                    if (currentSpell == null)
                    {
                        Console.WriteLine("spell not found");
                        continue;
                    }

                    int id = Int32.Parse(spellJsonObject.id);
                    currentSpell.id = id;

                    SpellInfo spellInfo = new SpellInfo
                    {
                        filename = spellJsonObject.name,
                        id = id,
                        spellName = currentSpell.nameSpell,
                        spell = currentSpell
                    };
                    
                    SpellInfos.Add(spellInfo);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                Console.WriteLine(json);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Data);
            }
        }
        
        private static Spell LoadSpellByName(string filenameSpell)
        {
            string path = "/app/server/TowersWebsocketNet31/Data/SpellsJson/" + filenameSpell + ".json";
            Spell spell = FindSpellWithPath(path);

            return spell;
        }
        
        private static Spell FindSpellWithPath(string tempPath)
        {
            Spell spell = null;
            string jsonSpell;

            try
            {
                jsonSpell = File.ReadAllText(tempPath);
                spell = JsonSerializer.Deserialize<Spell>(jsonSpell);
                spell = Utils.Clone(spell);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cant import spell for path : " + tempPath);
                Console.WriteLine(e.Message);
            }

            return spell;
        }

        private void DownloadSpell(string filename)
        {
            using var client = new WebClient();
            
            Console.WriteLine("data : " + Directory.Exists("/app/server/TowersWebsocketNet31/Data/"));
            Console.WriteLine("spell : " + Directory.Exists("/app/server/TowersWebsocketNet31/Data/SpellsJson/"));

            string [] fileEntries = Directory.GetFiles("/app/server/TowersWebsocketNet31/Data/SpellsJson/");
            Console.WriteLine("count before : " + fileEntries.Length);

            client.DownloadFile(new Uri($@"https://www.towers.heolia.eu/data/spell/{filename}.json"), 
                "/app/server/TowersWebsocketNet31/Data/SpellsJson/" + filename + ".json");
            
            string [] fileEntries2 = Directory.GetFiles("/app/server/TowersWebsocketNet31/Data/SpellsJson/");
            Console.WriteLine("count before : " + fileEntries2.Length);
        }
    }
}