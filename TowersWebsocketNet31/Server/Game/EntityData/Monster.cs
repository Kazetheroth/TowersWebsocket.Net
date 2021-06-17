using System;
using System.Collections.Generic;
using System.Diagnostics;
using TowersWebsocketNet31.Server.Game.Controller;
using TowersWebsocketNet31.Server.Game.EquipmentData;
using TowersWebsocketNet31.Server.Game.Mechanics;

namespace TowersWebsocketNet31.Server.Game.EntityData
{
    public enum MonsterType
    {
        Tank,
        Support,
        Distance,
        Cac
    }
    
    public class Monster: Entity
    {
        public int id { get; set; }
        public string mobName { get; set; }
        public int nbWeapon { get; set; }
        public int weaponOriginalId { get; set; }

        public Family family { get; set; }

        public TypeWeapon constraint { get; set; }

        public MonsterType monsterType { get; set; }

        public void SetConstraint(TypeWeapon nconstraint)
        {
            constraint = nconstraint;
        }

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

        public void InitOriginalWeapon()
        {
            if (nbWeapon == 0)
            {
                return;
            }

            // TODO : get weapon
            Weapon weapon = DataObject.EquipmentList.GetWeaponWithId(weaponOriginalId);

            this.weapon = weapon;
        }
    }
}
