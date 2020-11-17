﻿namespace TowersWebsocketNet31.Server.Game.EntityData
{
    public enum EquipmentType
    {
        WEAPON,
        ARMOR
    }

    //Class for equipements
    public abstract class Equipement
    {
        public int cost { get; set; }
        public string equipmentName { get; set; }
        
        public EquipmentType equipmentType;
    }
}