﻿using TowersWebsocketNet31.Server.Game.SpellData;

namespace TowersWebsocketNet31.Server.Game.EntityData
{   
    public class Classes
    {
        public int id { get; set; }

        public string name { get; set; }
        public int def { get; set; }
        public int hp { get; set; }
        public int att { get; set; }
        public int speed { get; set; }
        public int attSpeed { get; set; }
        public int ressource { get; set; }

        public Spell defenseSpell { get; set; }
    }
}