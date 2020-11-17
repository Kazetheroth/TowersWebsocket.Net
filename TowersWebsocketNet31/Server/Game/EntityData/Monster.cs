using System;
using System.Collections.Generic;
using System.Diagnostics;
using TowersWebsocketNet31.Server.Game.Controller;
using TowersWebsocketNet31.Server.Game.EquipmentData;
using TowersWebsocketNet31.Server.Game.Mechanics;

namespace TowersWebsocketNet31.Server.Game.EntityData
{
    public class SpellList
    {
        public string id;
        public string name;
    }
    
    public class Monster: Entity
    {
        public int id { get; set; }
        public string mobName { get; set; }
        public int nbWeapon { get; set; }
        public int weaponOriginalId { get; set; }

        public Family family { get; set; }

        public TypeWeapon constraint { get; set; }

        public List<SpellList> spellsName { get; set; }

        public void InitMonster()
        {
            if (constraint == TypeWeapon.Distance)
            {
                BehaviorType = BehaviorType.Distance;
            } else if (constraint == TypeWeapon.Cac)
            {
                BehaviorType = BehaviorType.Melee;
            }

            AttackBehaviorType = AttackBehaviorType.Random;
        }

        public bool InitWeapon(int idWeapon)
        {
            if (weapons.Count < nbWeapon)
            {
                // TODO : load weapon
//                Weapon weapon = DataObject.EquipmentList.GetWeaponWithId(idWeapon);
                Weapon weapon = new Weapon();

                if (constraint != weapon.type)
                {
                    return false;
                }

                weapons.Add(weapon);

                return true;
            }

            SpellController.CastPassiveSpell(this);

            return false;
        }

        public void InitOriginalWeapon()
        {
            if (nbWeapon == 0)
            {
                return;
            }

            // TODO : get weapon
//            Weapon weapon = DataObject.EquipmentList.GetWeaponWithId(weaponOriginalId);
            Weapon weapon = new Weapon();

            weapons.Add(weapon);
        }

        public void InitSpells()
        {
            foreach (SpellList spellName in spellsName)
            {
                Spell spell = SpellController.LoadSpellByName(spellName.name);

                if (spell != null)
                {
                    spells.Add(spell);
                }
            }
        }
    }
}
