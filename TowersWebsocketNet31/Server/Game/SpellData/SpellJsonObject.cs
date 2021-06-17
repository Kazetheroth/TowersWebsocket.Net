﻿using System.Collections.Generic;
 namespace TowersWebsocketNet31.Server.Game.SpellData
{
    public class SpellListObject
    {
        public List<SpellJsonObject> skills;
    }

    public class SpellJsonObject
    {
        public string id;
        public string name;
    }
}