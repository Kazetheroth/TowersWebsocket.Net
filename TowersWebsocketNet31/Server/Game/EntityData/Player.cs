using Games.Global.Armors;
using TowersWebsocketNet31.Server.Game.Controller;
using TowersWebsocketNet31.Server.Game.EquipmentData;
using TowersWebsocketNet31.Server.Game.Mechanics;

namespace TowersWebsocketNet31.Server.Game.EntityData
{
    public enum Classes
    {
        Warrior,
        Ranger,
        Rogue,
        Mage
    }

    public class Player : Entity
    {
        public Armor helmetArmor { get; set; }
        public Armor chestplateArmor { get; set; }
        public Armor leggingsArmor { get; set; }

        public Classes mainClass { get; set; }
        
        
        public Player(Classes classe, CategoryWeapon categoryWeapon)
        {
            mainClass = classe;
            typeEntity = TypeEntity.ALLIES;

            isPlayer = true;

            switch(classe)
            {
                case Classes.Mage:
                    att = 0;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    attSpeed = 0;
                    basicDefense = SpellController.LoadSpellByName("BasicDefenseMage");
                    break;
                case Classes.Warrior:
                    att = 0;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    attSpeed = 0;
                    nbCharges = 4;
                    basicDefense = SpellController.LoadSpellByName("BasicDefenseWarrior");
                    break;
                case Classes.Rogue:
                    att = 0;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    attSpeed = 0;
                    basicDefense = SpellController.LoadSpellByName("BasicDefenseRogue");
                    break;
                case Classes.Ranger:
                    att = 0;
                    def = 2;
                    speed = 10;
                    hp = 50;
                    ressource1 = 50;
                    attSpeed = 0;
                    basicDefense = SpellController.LoadSpellByName("BasicDefenseRanger");
                    break;
            }

            initialAtt = att;
            initialDef = def;
            initialHp = hp;
            initialSpeed = speed;
            initialAttSpeed = attSpeed;
            initialRessource1 = ressource1;
            initialMagicalDef = magicalDef;
            initialPhysicalDef = physicalDef;

            InitStatBasedOnOriginal();
            
            int idWeapon = GetIdWeaponFromCategory(categoryWeapon);
            InitWeapon(idWeapon);

            SpellController.CastPassiveSpell(this);
        }

        public void ResetSpellCooldownAndStatus()
        {
            basicAttack.isOnCooldown = false;
            basicDefense.isOnCooldown = false;
            
            foreach (Spell spell in spells)
            {
                spell.isOnCooldown = false;
            }

            underEffects.Clear();
            damageDealExtraEffect.Clear();
            damageReceiveExtraEffect.Clear();
            activeSpellComponents.Clear();

            SpellController.CastPassiveSpell(this);
        }

        public void ResetStats()
        {
            switch(mainClass)
            {
                case Classes.Mage:
                    att = initialAtt;
                    def = initialDef;
                    speed = initialSpeed;
                    hp = initialHp;
                    ressource1 = initialRessource1;
                    attSpeed = initialAttSpeed;
                    physicalDef = initialPhysicalDef;
                    magicalDef = initialMagicalDef;
                    break;
                case Classes.Warrior:
                    att = initialAtt;
                    def = initialDef;
                    speed = initialSpeed;
                    hp = initialHp;
                    ressource1 = initialRessource1;
                    attSpeed = initialAttSpeed;
                    physicalDef = initialPhysicalDef;
                    magicalDef = initialMagicalDef;
                    break;
                case Classes.Rogue:
                    att = initialAtt;
                    def = initialDef;
                    speed = initialSpeed;
                    hp = initialHp;
                    ressource1 = initialRessource1;
                    attSpeed = initialAttSpeed;
                    physicalDef = initialPhysicalDef;
                    magicalDef = initialMagicalDef;
                    break;
                case Classes.Ranger:
                    att = initialAtt;
                    def = initialDef;
                    speed = initialSpeed;
                    hp = initialHp;
                    ressource1 = initialRessource1;
                    attSpeed = initialAttSpeed;
                    physicalDef = initialPhysicalDef;
                    magicalDef = initialMagicalDef;
                    break;
            }

            ResetSpellCooldownAndStatus();
        }

        private int GetIdWeaponFromCategory(CategoryWeapon categoryWeapon)
        {
            switch(categoryWeapon)
            {
                case CategoryWeapon.SHORT_SWORD:
                    return 1;
                case CategoryWeapon.BOW:
                    return 2;
                case CategoryWeapon.SPEAR:
                    return 3;
                case CategoryWeapon.DAGGER:
                    return 4;
                case CategoryWeapon.STAFF:
                    return 5;
            }

            return 0;
        }

        public void InitWeapon(int idWeapon)
        {
            Weapon weapon = new Weapon();
            weapon.InitPlayerSkill(mainClass);
            // TODO : Add init weapon => change basic attack spell

            weapons.Add(weapon);
        }
    }
}