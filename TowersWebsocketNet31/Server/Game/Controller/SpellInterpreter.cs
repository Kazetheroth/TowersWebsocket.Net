using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using TowersWebsocketNet31.Server.Game.EntityData;
using TowersWebsocketNet31.Server.Game.Mechanics;
using TowersWebsocketNet31.Server.Game.SpellData;

namespace TowersWebsocketNet31.Server.Game.Controller
{
    public class SpellInterpreter
    {
        public static SpellInterpreter instance
        {
            set { instance = value; }
            get
            {
                if (instance == null)
                {
                    instance = new SpellInterpreter();
                }

                return instance;
            }
        }

        public void StartSpellTreatment(SpellComponent spellComponent, Vector3 startPosition)
        {
            spellComponent.startAtPosition = startPosition;
            // TODO : Instantiation côté client - send callback
            //InstantiateSpell.InstantiateNewSpell(spellComponent, startPosition);
            
            IntervallSpell(spellComponent);
        }

        private void AtTheStartSpellBehavior(SpellComponent spellComponent)
        {
            spellComponent.AtTheStart();
            PlaySpellActions(spellComponent, Trigger.START);
        }

        private static void AtTheEndSpellBehavior(SpellComponent spellComponent)
        {
            spellComponent.AtTheEnd();
            PlaySpellActions(spellComponent, Trigger.END);
        }

        private void DuringIntervalSpellBehavior(SpellComponent spellComponent)
        {
            spellComponent.DuringInterval();
            PlaySpellActions(spellComponent, Trigger.INTERVAL);
        }

        public static void TriggerWhenEntityAttack(List<SpellComponent> spellComponents)
        {
            spellComponents.ForEach(spellComponent =>
            {
                PlaySpellActions(spellComponent, Trigger.ON_ATTACK);
                spellComponent.OnAttack();

                if (spellComponent.conditionReduceCharge == ConditionReduceCharge.OnAttack)
                {
                    spellComponent.spellCharges--;
                }
            });
        }

        public static void TriggerWhenEntityReceivedDamage(List<SpellComponent> spellComponents)
        {
            spellComponents.ForEach(spellComponent =>
            {
                PlaySpellActions(spellComponent, Trigger.ON_DAMAGE_RECEIVED);
                spellComponent.OnDamageReceive();

                if (spellComponent.stopSpellComponentAtDamageReceived)
                {
                    EndSpellComponent(spellComponent);
                }

                if (spellComponent.conditionReduceCharge == ConditionReduceCharge.OnDamageReceived)
                {
                    spellComponent.spellCharges--;
                }
            });
        }

        public static void TriggerWhenEntityDie(SpellComponent spellComponent)
        {
            PlaySpellActions(spellComponent, Trigger.ON_ENTITY_DIE);
        }

        private static bool CheckActionConditionsAndValidTarget(Entity caster, ActionTriggered action, TargetsFound targetsFound)
        {
            Random rand = new Random();
            
            if (action.percentageToTrigger != 100)
            {
                int triggerAction = rand.Next(0, 100);
                if (triggerAction < action.percentageToTrigger)
                {
                    return false;
                }
            }

            if (action.conditionToTrigger != null)
            {
                if (targetsFound.target != null)
                {
                    return action.conditionToTrigger.TestCondition(caster, targetsFound.target);
                }

                if (targetsFound.targets != null)
                {
                    List<Entity> entities = targetsFound.targets;
                    entities.ForEach(target =>
                    {
                        if (!action.conditionToTrigger.TestCondition(caster, target))
                        {
                            targetsFound.targets.Remove(target);
                        }
                    });

                    if (targetsFound.targets.Count == 0)
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (targetsFound.target == null && (targetsFound.targets == null || targetsFound.targets.Count == 0) &&
                    targetsFound.position == Vector3.Zero)
                {
                    return false;
                }
            }
            
            return true;
        }
        
        public static bool PlaySpellActions(SpellComponent spellComponent, Trigger trigger)
        {
            bool findAction = false;
            
            if (!spellComponent.actions.ContainsKey(trigger))
            {
                return findAction;
            }

            List<ActionTriggered> actionsToPlay = spellComponent.actions[trigger];
            
            foreach (ActionTriggered action in actionsToPlay)
            {   
                TargetsFound targetsFound =
                    SpellController.GetTargetGetWithStartForm(spellComponent.caster, action.startFrom,
                        spellComponent);

                if (!CheckActionConditionsAndValidTarget(spellComponent.caster, action, targetsFound))
                {
                    continue;
                }

                findAction = true;

                if (action.spellComponent != null)
                {
                    SpellController.CastSpellComponentFromTargetsFound(spellComponent.caster, action.spellComponent, targetsFound, spellComponent);
                }

                if (action.effect != null)
                {
                    if (action.actionOnEffectType == ActionOnEffectType.ADD)
                    {
                        EffectController.ApplyEffectFromTargetsFound(spellComponent.caster, action.effect, targetsFound);
                    } else if (action.actionOnEffectType == ActionOnEffectType.DELETE)
                    {
                        EffectController.DeleteEffectFromTargetsFound(action.effect, targetsFound);
                    }
                }

                if (action.damageDeal > 0)
                {
                    int damageDeal = action.damageDeal;

                    // TODO : need travel distance
//                    if (spellComponent.damageMultiplierOnDistance != 0)
//                    {
//                        damageDeal += (int)(spellComponent.spellPrefabController.distanceTravelled *
//                                            spellComponent.damageMultiplierOnDistance);
//                    }
                    
                    if (targetsFound.targets.Count > 0)
                    {
                        targetsFound.targets.ForEach(entity => entity.TakeDamage(damageDeal, spellComponent.caster, spellComponent.damageType));
                    }
                    else if (targetsFound.target != null)
                    {
                        targetsFound.target.TakeDamage(damageDeal, spellComponent.caster, spellComponent.damageType);
                    }

                    if (spellComponent.appliesPlayerOnHitEffect)
                    {
                        List<Effect> effects = spellComponent.caster.damageDealExtraEffect
                            .GroupBy(currentEffect => currentEffect.typeEffect)
                            .Select(g => g.First())
                            .ToList();
                        foreach (Effect effect in effects)
                        {
                            EffectController.ApplyEffectFromTargetsFound(spellComponent.caster, effect, targetsFound);
                        }
                    }
                }
            }

            return findAction;
        }

        private async Task IntervallSpell(SpellComponent spellComponent)
        {
            // Wait for instantiation if needed
            await Task.Delay(50);

            AtTheStartSpellBehavior(spellComponent);

            float spellDuration = spellComponent.spellDuration;
            float spellInterval = spellComponent.spellInterval <= 0.00001 ? 1 : spellComponent.spellInterval;
            int originalSpellCharges = spellComponent.spellCharges;
            
            while (
                spellDuration > 0 || 
                (originalSpellCharges != 0 && spellComponent.spellCharges > 0) || 
                (spellComponent.trajectory != null && spellComponent.trajectory.disapearAtTheEndOfTrajectory) ||
                (spellComponent.originSpell != null && spellComponent.originSpell.isHolding))
            {
                DuringIntervalSpellBehavior(spellComponent);

                await Task.Delay((int)(spellInterval * 1000));
                spellDuration -= spellInterval;
            }

            EndSpellComponent(spellComponent);
        }

        public static void EndSpellComponent(SpellComponent spellComponent)
        {
            AtTheEndSpellBehavior(spellComponent);

            spellComponent.caster.activeSpellComponents.Remove(spellComponent);
            
            // TODO : côté client
//            InstantiateSpell.DeactivateSpell(spellComponent);
        }
    }
}