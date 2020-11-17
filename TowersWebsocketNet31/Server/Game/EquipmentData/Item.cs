namespace TowersWebsocketNet31.Server.Game.EquipmentData
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    // Class needed to Equipement and Material
    public class Item
    {
        public int id { get; set; } = -1;

        public Rarity rarity { get; set; } = Rarity.Common;

        // In percent
        public int lootRate { get; set; } = 100;
        
        // Used by editor
        public string tempPathSprite;
    }
}
