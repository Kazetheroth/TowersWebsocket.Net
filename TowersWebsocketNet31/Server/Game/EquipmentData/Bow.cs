using System.Collections.Generic;

namespace TowersWebsocketNet31.Server.Game.EquipmentData
{
    public class Bow: Weapon
    {
        public Bow()
        {
           animationToPlay = "doingBowAttack";
           spellOfBasicAttack = "BowBasicAttack";

           warriorSpells = new List<string>();
           warriorSpells.Add("WarriorBow1");
           warriorSpells.Add("WarriorBow2");
           warriorSpells.Add("WarriorBow3");

           mageSpells = new List<string>();
           mageSpells.Add("MageBow1");
           mageSpells.Add("MageBow2");
           mageSpells.Add("MageBow3");

           rangerSpells = new List<string>();
           rangerSpells.Add("RangerBow1");
           rangerSpells.Add("RangerBow2");
           rangerSpells.Add("RangerBow3");

           rogueSpells = new List<string>();
           rogueSpells.Add("RogueBow1");
           rogueSpells.Add("RogueBow2");
           rogueSpells.Add("RogueBow3");
        }
    }
}
