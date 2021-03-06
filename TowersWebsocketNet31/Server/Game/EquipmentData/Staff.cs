﻿using System;
using System.Collections.Generic;

namespace TowersWebsocketNet31.Server.Game.EquipmentData
{
    [Serializable]
    public class Staff : Weapon
    {
        public Staff()
        {
           animationToPlay = "doingShortSwordAttack";
           spellOfBasicAttack = "StaffBasicAttack";
            
           warriorSpells = new List<string>();
           warriorSpells.Add("WarriorStaff1");
           warriorSpells.Add("WarriorStaff2");
           warriorSpells.Add("WarriorStaff3");

           mageSpells = new List<string>();
           mageSpells.Add("MageStaff1");
           mageSpells.Add("MageStaff2");
           mageSpells.Add("MageStaff3");

           rangerSpells = new List<string>();
           rangerSpells.Add("RangerStaff1");
           rangerSpells.Add("RangerStaff2");
           rangerSpells.Add("RangerStaff3");

           rogueSpells = new List<string>();
           rogueSpells.Add("RogueStaff1");
           rogueSpells.Add("RogueStaff2");
           rogueSpells.Add("RogueStaff3");
        }
    }
}