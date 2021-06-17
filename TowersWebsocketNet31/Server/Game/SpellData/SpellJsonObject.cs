﻿using System.Collections.Generic;
 namespace TowersWebsocketNet31.Server.Game.SpellData
{
    public class SpellListObject
    {
        public List<SpellJsonObject> skills { get; set; }
    }

    public class SpellJsonObject
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}