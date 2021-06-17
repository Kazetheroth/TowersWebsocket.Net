using Games.Global.Armors;
using TowersWebsocketNet31.Server.Game.Controller;
using TowersWebsocketNet31.Server.Game.EquipmentData;
using TowersWebsocketNet31.Server.Game.Mechanics;
using TowersWebsocketNet31.Server.Game.SpellData;

namespace TowersWebsocketNet31.Server.Game.EntityData
{
    public class Player : Entity
    {
        public Armor helmetArmor { get; set; }
        public Armor chestplateArmor { get; set; }
        public Armor leggingsArmor { get; set; }

        private Classes mainClass;

        public Player(int idClasses, int idCategoryWeapon)
        {
            mainClass = DataObject.ClassesList.GetClassesFromId(idClasses);
            typeEntity = TypeEntity.ALLIES;

            isPlayer = true;

            InitStatBasedOnOriginal();
            
            weapon = DataObject.EquipmentList.GetFirstWeaponFromIdCategory(idCategoryWeapon);

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

            ClearUnderEffect();
            damageDealExtraEffect.Clear();
            damageReceiveExtraEffect.Clear();
            activeSpellComponents.Clear();

            SpellController.CastPassiveSpell(this);
        }

        public void ResetStats()
        {
            if (mainClass != null)
            {
                att = initialAtt = mainClass.att;
                def = initialDef = mainClass.def;
                speed = initialSpeed = mainClass.speed;
                hp = initialHp = mainClass.hp;
                attSpeed = initialAttSpeed = mainClass.attSpeed;
                ressource1 = initialRessource1 = mainClass.ressource;

                basicDefense = mainClass.defenseSpell;
            }

            initialMagicalDef = magicalDef;
            initialPhysicalDef = physicalDef;

            ResetSpellCooldownAndStatus();
        }
    }
}