﻿ namespace TowersWebsocketNet31.Server.Game.EquipmentData
{
    public enum EquipmentType
    {
        WEAPON,
        ARMOR
    }

    //Class for equipements
    public abstract class Equipment : Item
    {
        public int cost { get; set; }
        public string equipmentName { get; set; }
        
        public EquipmentType equipmentType;
    }
}