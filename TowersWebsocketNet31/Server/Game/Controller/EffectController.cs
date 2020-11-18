using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TowersWebsocketNet31.Server.Game.EntityData;
using TowersWebsocketNet31.Server.Game.Mechanics;

namespace TowersWebsocketNet31.Server.Game.Controller
{
    public class EffectController
    {
        public static readonly List<TypeEffect> ControlEffect = new List<TypeEffect> {TypeEffect.Freezing, TypeEffect.Stun, TypeEffect.Sleep, TypeEffect.Immobilization, TypeEffect.Slow, TypeEffect.Expulsion, 
            TypeEffect.Confusion, TypeEffect.Fear, TypeEffect.Charm };

        public static readonly List<TypeEffect> BuffEffect = new List<TypeEffect> 
        {
            TypeEffect.Heal, TypeEffect.Regen, TypeEffect.Resurrection, TypeEffect.AttackUp, TypeEffect.AttackSpeedUp, TypeEffect.DefenseUp, TypeEffect.DivineShield, TypeEffect.AntiSpell, TypeEffect.Will, TypeEffect.Thorn, TypeEffect.Intangible,
            TypeEffect.Mirror, TypeEffect.SpeedUp, TypeEffect.MagicalDefUp, TypeEffect.PhysicalDefUp, TypeEffect.Purification, TypeEffect.ResourceFill, TypeEffect.Link
        };

        public static void DeleteEffectFromTargetsFound(Effect effect, TargetsFound targetsFound)
        {
            if (targetsFound.targets.Count > 0)
            {
                targetsFound.targets.ForEach(target =>
                {
                    if (target.underEffects.ContainsKey(effect.typeEffect))
                    {
                        StopCurrentEffect(target, target.underEffects[effect.typeEffect]);
                    }
                });
            } 
            else if (targetsFound.target != null)
            {
                if (targetsFound.target.underEffects.ContainsKey(effect.typeEffect))
                {
                    StopCurrentEffect(targetsFound.target, targetsFound.target.underEffects[effect.typeEffect]);
                }
            }
        }

        public static void ApplyEffectFromTargetsFound(Entity origin, Effect effect, TargetsFound targetsFound)
        {
            // TODO : IMPLEMENT CORRECT SRC OF DAMAGES
            if (targetsFound.targets.Count > 0)
            {
                targetsFound.targets.ForEach(target => ApplyEffect(target, effect, origin));
            } 
            else if (targetsFound.target != null)
            {
                ApplyEffect(targetsFound.target, effect, origin);
            }
        }

        public static Task ApplyEffect(Entity entityAffected, Effect effect, Entity origin, List<Entity> affectedEntity = null)
        {   
            if (effect == null || entityAffected.hasWill && ControlEffect.Contains(effect.typeEffect))
            {
                return null;
            }

            if (entityAffected.isLinked && !BuffEffect.Contains(effect.typeEffect))
            {
                if (affectedEntity == null)
                {
                    affectedEntity = new List<Entity>();
                }

                affectedEntity.Add(entityAffected);
                foreach (Entity entityInRange in entityAffected.entityInRange)
                {
                    if (!affectedEntity.Contains(entityInRange) && entityInRange.isLinked)
                    {
                        ApplyEffect(entityInRange, effect, origin, affectedEntity);
                    }
                }
            }

            if (entityAffected.underEffects.ContainsKey(effect.typeEffect))
            {
                Effect effectInList = entityAffected.underEffects[effect.typeEffect];
                effectInList.UpdateEffect(entityAffected, effect);

                return null;
            }

            return StartCoroutineEffect(entityAffected, effect, origin);
        }

        private static Task StartCoroutineEffect(Entity entity, Effect effect, Entity origin)
        {
            Effect cloneEffect = Utils.Clone(effect);
            cloneEffect.launcher = origin;
            entity.underEffects.Add(effect.typeEffect, cloneEffect);

            Task currentTask = PlayEffectOnTime(entity, cloneEffect);

            Console.WriteLine("Effect run");
            return currentTask;
        }

        private static async Task PlayEffectOnTime(Entity entity, Effect effect)
        {
            effect.InitialTrigger(entity);

            await Task.Delay(50);

            if (effect.durationInSeconds == 0)
            {
                StopCurrentEffect(entity, effect);
                return;
            }
            
            while (effect.durationInSeconds > 0)
            {
                await Task.Delay(100);
                if (effect.needEndOfEffect)
                {
                    Console.WriteLine("Receive end of effect instruction");
                    break;
                }

                effect.TriggerEffectAtTime(entity);

                effect.durationInSeconds -= 0.1f;
            }

            StopCurrentEffect(entity, effect);
        }

        public static void StopCurrentEffect(Entity entity, Effect effect)
        {
            if (!effect.needEndOfEffect)
            {
                Console.WriteLine("End of effect");
                effect.needEndOfEffect = true;
            }

            effect.EndEffect(entity);

            entity.underEffects.Remove(effect.typeEffect);
        }
    }
}