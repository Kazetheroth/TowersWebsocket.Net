using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using TowersWebsocketNet31.Server.Game.EntityData;
using TowersWebsocketNet31.Server.Game.Mechanics;
using TowersWebsocketNet31.Server.Game.SpellData;
using TowersWebsocketNet31.Server.Game.SpellData.SpellParam;

namespace TowersWebsocketNet31.Server.Game.Controller
{
    public class SpellController
    {
        public static bool CastSpell(Entity entity, Spell spell, int spellSlotIfPlayer = 0)
        {
            if (spell == null && spell.cost <= entity.ressource1 - spell.cost)
            {
                return false;
            }

            Task currentTask;

            if ((!spell.isOnCooldown && spell.nbUse != 0) || (spell.activeSpellComponent != null && entity.weapons.Count > 0))
            {
                if (spell.nbUse > 0)
                {
                    spell.nbUse--;
                }

                spell.isOnCooldown = true;
                currentTask = PlayCastTime(entity, spell, spellSlotIfPlayer);
            }
            else if (spell.canCastDuringCast)
            {
                spell.wantToCastDuringCast = true;
            }
            else if (spell.canRecast && !spell.alreadyRecast && entity.canRecast)
            {
                spell.alreadyRecast = true;
                currentTask = PlayCastTime(entity, spell);
            }
            else
            {
                return false;
            }

            return true;
        }

        public static TargetsFound GetTargetGetWithStartForm(Entity caster, StartFrom startFromNewSpellComponent,
            SpellComponent lastSpellComponent = null)
        {
            TargetsFound targetFound = new TargetsFound();
            Random rand = new Random();

            switch (startFromNewSpellComponent)
            {
                /* Can be use by Spell and ActionTriggered */
                case StartFrom.Caster:
                    targetFound.position = caster.position;
                    targetFound.target = caster;
                    break;
                /* Can be use by Spell and ActionTriggered */
                case StartFrom.TargetEntity:
                    if (lastSpellComponent == null)
                    {
                        targetFound.position = caster.target.position;
                        targetFound.target = caster.target;
                    }
                    else
                    {
                        targetFound.position = lastSpellComponent.targetAtCast.position;
                        targetFound.target = lastSpellComponent.targetAtCast;
                    }

                    if (targetFound.target != null && targetFound.target.hp <= 0)
                    {
                        targetFound.target = null;
                        targetFound.position = Vector3.Zero;
                    }
                    
                    break;
                /* Can be use by ActionTriggered after area */
                case StartFrom.RandomPositionInArea:
                    if (lastSpellComponent == null)
                    {
                        break;
                    }

                    SpellToInstantiate spellToInstantiate = lastSpellComponent.spellToInstantiate;
                    Vector3 currentPosition = lastSpellComponent.position;

                    if (lastSpellComponent.spellToInstantiate.geometry == Geometry.Sphere)
                    {
                        float t = (float)(2 * Math.PI * rand.NextDouble() );
                        float rx = (float)rand.NextDouble() * spellToInstantiate.scale.X / 2;
                        float rz = (float)rand.NextDouble() * spellToInstantiate.scale.Z / 2;
                        targetFound.position = new Vector3
                        {
                            X = (float)(currentPosition.X + rx * Math.Cos(t)), 
                            Y = (float)(currentPosition.Y), 
                            Z = (float)(currentPosition.Z + rz * Math.Sin(t))
                        };
                    }
                    else
                    {
                        targetFound.position = new Vector3
                        {
                            X = (float)(currentPosition.X + rand.NextDouble() * spellToInstantiate.scale.X - spellToInstantiate.scale.X / 2),
                            Y = (float)(currentPosition.Y), 
                            Z = (float)(currentPosition.Z + rand.NextDouble() * spellToInstantiate.scale.Z - spellToInstantiate.scale.Z / 2)
                        };
                    }

                    break;
                /* Can be use by ActionTriggered after area */
                case StartFrom.RandomEnemyInArea:
                    if (lastSpellComponent == null || lastSpellComponent.enemiesTouchedBySpell.Count == 0)
                    {
                        break;
                    }

                    List<Entity> enemiesAlive =
                        lastSpellComponent.enemiesTouchedBySpell.FindAll(enemy => enemy.hp > 0);

                    if (enemiesAlive.Count == 0)
                    {
                        break;
                    }

                    int randEnemy = rand.Next(0, enemiesAlive.Count);
                    targetFound.position = enemiesAlive[randEnemy].position;
                    targetFound.target = enemiesAlive[randEnemy];
                    break;
                /* Can be use by ActionTriggered */
                case StartFrom.LastSpellComponent:
                    if (lastSpellComponent == null)
                    {
                        break;
                    }

                    targetFound.position = lastSpellComponent.position;
                    break;
                /* Can be use by Spell and ActionTriggered */
                case StartFrom.ClosestEnemyFromCaster:
                    if (caster.isPlayer)
                    {
                        // TODO : get closestMonster
//                        Monster closestMonster = DataObject.monsterInScene.FindAll(monster => monster.hp > 0).OrderByDescending(monster =>
//                            caster.entityPrefab.transform.position -
//                            monster.entityPrefab.transform.position).First();
//
//                        targetFound.position = closestMonster.entityPrefab.transform.position;
//                        targetFound.target = closestMonster;
                    }
                    break;
                /* Can be use by Spell */
                case StartFrom.CursorTarget:
                    targetFound.position = caster.positionPointed;
                    break;
                /* Can be use by ActionTriggered after area */
                case StartFrom.AllAlliesInArea:
                    if (lastSpellComponent == null)
                    {
                        break;
                    }
                    
                    targetFound.targets = lastSpellComponent.alliesTouchedBySpell;
                    break;
                /* Can be use by ActionTriggered after area */
                case StartFrom.AllEnemiesInArea:
                    if (lastSpellComponent == null)
                    {
                        break;
                    }

                    targetFound.targets = lastSpellComponent.enemiesTouchedBySpell.FindAll(enemy => enemy.hp > 0);
                    break;
            }

            return targetFound;
        }

        public static void CastSpellComponentFromTargetsFound(Entity caster, SpellComponent spellComponent, TargetsFound targetsFound, SpellComponent lastSpellComponent = null)
        {
            if (targetsFound.targets.Count > 0)
            {
                targetsFound.targets.ForEach(target => CastSpellComponent(caster, spellComponent, target, target.position, lastSpellComponent));
            }
            else if (targetsFound.target != null)
            {
                CastSpellComponent(caster, spellComponent, targetsFound.target, targetsFound.position, lastSpellComponent);
            }
            else if (targetsFound.position != Vector3.Zero)
            {
                CastSpellComponent(caster, spellComponent, caster.target, targetsFound.position, lastSpellComponent);
            }
        }

        private static SpellComponent CloneCorrectSpellComponent(SpellComponent spellComponent)
        {
            switch (spellComponent.typeSpell)
            {
                case TypeSpell.Movement:
                    return Utils.Clone(spellComponent as MovementSpell);
                case TypeSpell.Transformation:
                    return Utils.Clone(spellComponent as TransformationSpell);
                case TypeSpell.Passive:
                    return Utils.Clone(spellComponent as PassiveSpell);
                case TypeSpell.BasicAttack:
                    return Utils.Clone(spellComponent as BasicAttackSpell);
                case TypeSpell.Summon:
                    return Utils.Clone(spellComponent as SummonSpell);
                default:
                    return Utils.Clone(spellComponent);
            }
        } 

        public static SpellComponent CastSpellComponent(Entity caster, SpellComponent spellComponent, Entity target, Vector3 startPosition, SpellComponent lastSpellComponent = null)
        {
            SpellComponent cloneSpellComponent = CloneCorrectSpellComponent(spellComponent);
            caster.activeSpellComponents.Add(cloneSpellComponent);
            
            cloneSpellComponent.caster = caster;
            cloneSpellComponent.targetAtCast = target;

            if (cloneSpellComponent.trajectory != null)
            {
                // TODO : Gestion côté client
//                if (cloneSpellComponent.trajectory.followCategory == FollowCategory.FOLLOW_TARGET)
//                {
//                    cloneSpellComponent.trajectory.objectToFollow = target.entityPrefab.transform;
//                } 
//                else if (cloneSpellComponent.trajectory.followCategory == FollowCategory.FOLLOW_LAST_SPELL && lastSpellComponent != null && lastSpellComponent.spellPrefabController != null)
//                {
//                    cloneSpellComponent.trajectory.objectToFollow = lastSpellComponent.spellPrefabController.transform;
//                }
            }

            SpellInterpreter.instance.StartSpellTreatment(cloneSpellComponent, startPosition);
            return cloneSpellComponent;
        }

        public static async Task StartCooldown(Entity caster, Spell spell)
        {
            float duration = spell.cooldown;
            while (duration > 0)
            {
                await Task.Delay(1000);
                duration -= 1;
            }

            spell.isOnCooldown = false;
            spell.alreadyRecast = false;
        }

        public static async Task PlayCastTime(Entity caster, Spell spell, int spellSlotIfPlayer = 0)
        {
            TargetsFound targetsFound = GetTargetGetWithStartForm(caster, spell.startFrom);
            caster.ressource1 -= spell.cost;
            
            float duration = spell.castTime;

            if (spell.duringCastSpellComponent != null)
            {
                spell.canCastDuringCast = true;
            }

            while (duration > 0)
            {
                await Task.Delay(100);
                duration -= 0.1f;

                if (spell.wantToCastDuringCast)
                {
                    spell.wantToCastDuringCast = false;
                    spell.canCastDuringCast = false;

                    CastSpellComponentFromTargetsFound(caster, spell.duringCastSpellComponent, targetsFound);
                    if (spell.interruptCurrentCast)
                    {
                        break;
                    }
                }
            }

            if (spell.activeSpellComponent == null)
            {
                return;
            }

            StartCooldown(caster, spell);
            CastSpellComponentFromTargetsFound(caster, spell.activeSpellComponent, targetsFound);
        }

        public static void CastPassiveSpell(Entity entity)
        {
            if (entity.spells == null)
            {
                return;
            }

            foreach (Spell spell in entity.spells)
            {
                if (spell.passiveSpellComponent != null)
                {
                    // TODO : Reimplement Passive spell
//                    CastSpellComponent(entity, spell.passiveSpellComponent, entity.entityPrefab.target);
                }
            }
        }

        public static Spell LoadSpellByName(string nameSpell)
        {
            Console.WriteLine("Gestion des spells ?");
            return null;
        }
    }
}