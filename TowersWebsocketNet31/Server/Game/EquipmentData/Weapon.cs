using System.Collections.Generic;
using TowersWebsocketNet31.Server.Game.Controller;
using TowersWebsocketNet31.Server.Game.EntityData;
using TowersWebsocketNet31.Server.Game.Mechanics;

namespace TowersWebsocketNet31.Server.Game.EquipmentData
{
    public enum TypeWeapon
    {
        Distance,
        Cac
    }
    
    public enum CategoryWeapon
    {
        SHORT_SWORD,
        LONG_SWORD,
        SPEAR,
        AXE,
        TWO_HAND_AXE,
        HAMMER,
        HALBERD,
        MACE,
        BOW,
        STAFF,
        DAGGER,
        TRIDENT,
        RIFLE,
        CROSSBOW,
        SLING,
        HANDGUN
    }

    public class Weapon : Equipment
    {
        public int id { get; set; }
        public CategoryWeapon category { get; set; }
        public TypeWeapon type { get; set; }
        public int damage { get; set; }
        public float attSpeed { get; set; }

        public string animationToPlay { get; set; }
        public string spellOfBasicAttack { get; set; }

        public List<string> warriorSpells { get; set; }
        public List<string> mageSpells { get; set; }
        public List<string> rogueSpells { get; set; }
        public List<string> rangerSpells { get; set; }

        public Spell basicAttack { get; set; }
        
        public Entity wielder { get; set; }

        public void InitPlayerSkill(Classes classe)
        {
            switch (classe)
            {
                case Classes.Warrior:
                    InitWeaponSpellWithJson(warriorSpells);
                    break;
                case Classes.Mage:
                    InitWeaponSpellWithJson(mageSpells);
                    break;
                case Classes.Rogue:
                    InitWeaponSpellWithJson(rogueSpells);
                    break;
                case Classes.Ranger:
                    InitWeaponSpellWithJson(rangerSpells);
                    break;
            }
        }

        public void InitWeapon()
        {
            InitBasicAttack();
        }

        public void InitBasicAttack()
        {
            Spell spell = SpellController.LoadSpellByName(spellOfBasicAttack);

            if (spell != null)
            {
                wielder.basicAttack = spell;
            }
        }

        public void InitWeaponSpellWithJson(List<string> spellsToFind)
        {
            foreach (string spellString in spellsToFind)
            {
                Spell spell = SpellController.LoadSpellByName(spellString);

                if (spell == null)
                {
                    continue;
                }

                wielder.spells.Add(spell);
            }
        }
    }
}