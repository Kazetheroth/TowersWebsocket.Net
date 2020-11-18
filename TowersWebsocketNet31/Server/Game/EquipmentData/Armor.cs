﻿using System.Collections.Generic;
 using TowersWebsocketNet31.Server.Game.EntityData;
 using TowersWebsocketNet31.Server.Game.EquipmentData;
 using TowersWebsocketNet31.Server.Game.Mechanics;

 namespace Games.Global.Armors
{
    public enum CategoryArmor
    {
        HELMET,
        CHESTPLATE,
        LEGGINGS
    }
    
    public class Armor : Equipment
    {
        public int def;
        public CategoryArmor armorCategory;

        public List<TypeEffect> effects;
    } 
}