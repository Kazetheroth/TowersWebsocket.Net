using System.Collections.Generic;
using System.Linq;
using TowersWebsocketNet31.Server.Game.Controller;
using TowersWebsocketNet31.Server.Game.EntityData;
using TowersWebsocketNet31.Server.Game.Mechanics;

namespace TowersWebsocketNet31.Server.Game.SpellData
{
    public class BasicAttackSpell : SpellComponent
    {
        public BasicAttackSpell()
        {
            typeSpell = TypeSpell.BasicAttack;
        }
        
        public override void OnTriggerEnter(Entity enemy)
        {
            bool damageIsNull = (enemy.isIntangible && damageType == DamageType.Physical) ||
                                (enemy.hasAntiSpell && damageType == DamageType.Magical) ||
                                caster.isBlind ||
                                enemy.isUntargeatable ||
                                caster.hasDivineShield;

            List<Effect> effects = caster.damageDealExtraEffect
                .GroupBy(currentEffect => currentEffect.typeEffect)
                .Select(g => g.First())
                .ToList();
            foreach (Effect effect in effects)
            {
                if (effect == null)
                {
                    continue;
                }
                EffectController.ApplyEffect(enemy, effect, caster);
            }

            float damage = caster.att;
            if (caster.isWeak)
            {
                damage /= 2;
            }
        
            if (enemy.hasMirror && damageType == DamageType.Magical)
            {
                caster.TakeDamage(damage * 0.4f, caster, DamageType.Magical, this);
            }

            if (enemy.hasThorn && damageType == DamageType.Physical)
            {
                caster.TakeDamage(damage * 0.4f, caster, DamageType.Physical, this);
            }

            damage = damageIsNull ? 0 : damage;

            enemy.TakeDamage(damage, caster, damageType, this);
        }
    }
}