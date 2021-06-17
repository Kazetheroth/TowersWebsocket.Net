using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using TowersWebsocketNet31.Server.Game.Controller;
using TowersWebsocketNet31.Server.Game.EntityData;

namespace TowersWebsocketNet31.Server.Game.Mechanics
{
    public enum TypeEffect
    {
        Pierce,
        PierceOnBack,
        Burn,
        Poison,
        Freezing,
        Bleed,
        Weak,
        Stun,
        Sleep,
        Immobilization,
        Thorn,
        Mirror,
        Slow,
        Expulsion,
        DefenseUp,
        Intangible,
        AntiSpell,
        DivineShield,
        PhysicalDefUp,
        MagicalDefUp,
        Resurrection,
        Purification,
        Silence,
        BrokenDef,
        Confusion,
        Will,
        Fear,
        Charm,
        Regen,
        Blind,
        AttackUp,
        SpeedUp,
        AttackSpeedUp,
        DotDamageIncrease,
        Untargetable,
        Heal,
        ResourceFill,
        DisableBasicAttack,
        LifeSteal,
        Taunt,
        NoAggro,
        UnkillableByBleeding,
        Invisibility,
        Link,
        LifeLink,
        RefreshCd1,
        RefreshCd2,
        RefreshCd3,
        ReduceCd1,
        ReduceCd2,
        ReduceCd3,
        CanRecast,
        DeactivatePassive,
        Redirection
    }
    
    public enum OriginExpulsion
    {
        Entity,
        SrcDamage,
        ForwardOfPositionSrcDamage
    }

    public enum DirectionExpulsion
    {
        Out,
        In
    }

    [Serializable]
    public class Effect
    {
        public string nameEffect { get; set; }

        public TypeEffect typeEffect { get; set; }
        public int level { get; set; }
        public float durationInSeconds { get; set; }

        public Entity launcher;

        public bool needEndOfEffect { get; set; }
        // direction use for effect expulsion
//        public Vector3 direction;
        public DirectionExpulsion directionExpul { get; set; }
        public OriginExpulsion originExpulsion { get; set; }
        public Vector3 positionSrcDamage { get; set; }

        public void InitialTrigger(Entity entity)
        {
            switch (typeEffect)
            {
                case TypeEffect.Pierce:
                    entity.canPierce = true;
                    break;
                case TypeEffect.PierceOnBack:
                    entity.canPierceOnBack = true;
                    break;
                case TypeEffect.Stun:
                    // TODO : Pourquoi cette variable n'est pas dans entity ?
//                    entity.entityPrefab.canDoSomething = false;
//                    AddEffectMaterials(entity,StaticMaterials.electricityMaterial);
                    break;
                case TypeEffect.Sleep:
                    entity.isSleep = true;
                    // TODO : Pourquoi cette variable n'est pas dans entity ?
//                    entity.entityPrefab.canDoSomething = false;
                    break;
                case TypeEffect.Invisibility:
                    entity.isInvisible = true;
                    // TODO : Gestion côté client
//                    entity.entityPrefab.SetInvisibility();
                    break;
                case TypeEffect.Immobilization:
                    // TODO : Pourquoi cette variable n'est pas dans entity ?
//                    entity.entityPrefab.canMove = false;
                    break;
                case TypeEffect.Thorn:
                    // TODO : Gestion côté client
//                    entity.entityPrefab.thornSphere.SetActive(true);
                    entity.hasThorn = true;
                    break;
                case TypeEffect.Mirror:
                    entity.hasMirror = true;
                    break;
                case TypeEffect.Expulsion:
                    // TODO : Gestion côté client pour l'instant
//                    Vector3 direction = CreateDirection(entity);
//                    entity.entityPrefab.WantToApplyForce(direction, level);
                    break;
                case TypeEffect.Intangible:
                    entity.isIntangible = true;
                    break;
                case TypeEffect.AntiSpell:
                    // TODO : Gestion côté client pour l'instant
//                    entity.entityPrefab.distortionSphere.SetActive(true);
                    entity.hasAntiSpell = true;
                    break;
                case TypeEffect.DivineShield:
                    entity.hasDivineShield = true;
                    break;
                case TypeEffect.Resurrection:
                    entity.shooldResurrect = true;
                    break;
                case TypeEffect.Weak:
                    entity.isWeak = true;
                    break;
                case TypeEffect.Heal:
                    entity.hp += level;
                    break;
                case TypeEffect.Purification:
                    List<Effect> effects = entity.GetUnderEffects();

                    foreach (Effect effect in effects)
                    {
                        EffectController.StopCurrentEffect(entity, effect);
                    }

                    break;
                case TypeEffect.Silence:
                    entity.isSilence = true;
                    break;
                case TypeEffect.BrokenDef:
                    entity.def = entity.def - entity.initialDef >= 0 ? entity.def - entity.initialDef : 0;
                    entity.physicalDef = entity.physicalDef - entity.initialPhysicalDef >= 0
                        ? entity.physicalDef - entity.initialPhysicalDef
                        : 0;
                    entity.magicalDef = entity.magicalDef - entity.initialMagicalDef >= 0
                        ? entity.magicalDef - entity.initialMagicalDef
                        : 0;
                    break;
                case TypeEffect.Confusion:
                    entity.isConfuse = true;
                    break;
                case TypeEffect.Will:
                    entity.hasWill = true;

                    foreach (TypeEffect type in EffectController.ControlEffect)
                    {
                        if (entity.EntityIsUnderEffect(type))
                        {
                            EffectController.StopCurrentEffect(entity, entity.TryGetEffectInUnderEffect(type));
                        }
                    }

                    break;
                case TypeEffect.Fear:
                    entity.isFeared = true;
                    break;
                case TypeEffect.Charm:
                    entity.isCharmed = true;
                    break;
                case TypeEffect.Blind:
                    entity.isBlind = true;
                    break;
                case TypeEffect.Untargetable:
                    entity.isUntargeatable = true;
                    break;
                case TypeEffect.DisableBasicAttack:
                    entity.canBasicAttack = false;
                    break;
                case TypeEffect.LifeSteal:
                    entity.hasLifeSteal = true;
                    break;
                case TypeEffect.Taunt:
                    entity.hasTaunt = true;
                    break;
                case TypeEffect.NoAggro:
                    entity.hasNoAggro = true;
                    break;
                case TypeEffect.ResourceFill:
                    entity.ressource1 += level;
                    break;
                case TypeEffect.UnkillableByBleeding:
                    entity.isUnkillableByBleeding = true;
                    break;
                case TypeEffect.Link:
                    entity.isLinked = true;
                    break;
                case TypeEffect.Redirection:
                    entity.hasRedirection = true;
                    break;
                case TypeEffect.DeactivatePassive:
                    entity.hasPassiveDeactivate = true;
                    break;
                case TypeEffect.CanRecast:
                    entity.canRecast = true;
                    break;
                case TypeEffect.LifeLink:
                    entity.hasLifeLink = true;
                    break;
                case TypeEffect.RefreshCd1:
                    if (entity.spells.Count > 0)
                    {
                        entity.spells[0].isOnCooldown = false;
                    }

                    break;
                case TypeEffect.RefreshCd2:
                    if (entity.spells.Count > 1)
                    {
                        entity.spells[1].isOnCooldown = false;
                    }

                    break;
                case TypeEffect.RefreshCd3:
                    if (entity.spells.Count > 2)
                    {
                        entity.spells[2].isOnCooldown = false;
                    }

                    break;
                case TypeEffect.ReduceCd1:
                    if (entity.spells.Count > 0)
                    {
                        entity.spells[0].initialCooldown = entity.spells[0].cooldown;
                        entity.spells[0].cooldown = level;
                    }

                    break;
                case TypeEffect.ReduceCd2:
                    if (entity.spells.Count > 1)
                    {
                        entity.spells[1].initialCooldown = entity.spells[1].cooldown;
                        entity.spells[1].cooldown = level;
                    }

                    break;
                case TypeEffect.ReduceCd3:
                    if (entity.spells.Count > 2)
                    {
                        entity.spells[2].initialCooldown = entity.spells[2].cooldown;
                        entity.spells[2].cooldown = level;
                    }

                    break;
                case TypeEffect.Burn:
                    // TODO : côté client seulement
//                    AddEffectMaterials(entity,StaticMaterials.burningMaterial);
                    break;
                case TypeEffect.Freezing:
//                    AddEffectMaterials(entity,StaticMaterials.freezingMaterial);
                    break;
                case TypeEffect.Poison:
//                    AddEffectMaterials(entity,StaticMaterials.poisonMaterial);
                    break;
            }
        }

        public void TriggerEffectAtTime(Entity entity)
        {
            float extraDamage = launcher.EntityIsUnderEffect(TypeEffect.DotDamageIncrease) ? 0.2f : 0;
            Vector3 dir = Vector3.Zero;

            switch (typeEffect)
            {
                case TypeEffect.SpeedUp:
                    entity.speed = entity.initialSpeed + (1 * level);
                    break;
                case TypeEffect.Slow:
                    entity.speed = entity.EntityIsUnderEffect(TypeEffect.SpeedUp)
                        ? entity.speed / 2
                        : entity.initialSpeed / 2;
                    break;
                case TypeEffect.Burn:
                    if (entity.EntityIsUnderEffect(TypeEffect.Sleep))
                    {
                        Effect sleep = entity.TryGetEffectInUnderEffect(TypeEffect.Sleep);
                        EffectController.StopCurrentEffect(entity, sleep);
                    }

                    entity.ApplyDamage(0.2f + extraDamage);
                    break;
                case TypeEffect.Freezing:
                    if (entity.EntityIsUnderEffect(TypeEffect.Sleep))
                    {
                        Effect sleep = entity.TryGetEffectInUnderEffect(TypeEffect.Sleep);
                        EffectController.StopCurrentEffect(entity, sleep);
                    }

                    entity.speed = entity.initialSpeed - level;
                    entity.ApplyDamage(0.1f + extraDamage);
                    break;
                case TypeEffect.Bleed:
                    if (entity.EntityIsUnderEffect(TypeEffect.Sleep))
                    {
                        Effect sleep = entity.TryGetEffectInUnderEffect(TypeEffect.Sleep);
                        EffectController.StopCurrentEffect(entity, sleep);
                    }

                    if ((entity.hp - 0.1f * level) < 0 && entity.isUnkillableByBleeding)
                    {
                        entity.hp = 1;
                    }
                    else
                    {
                        entity.ApplyDamage(0.1f * level);
                    }

                    break;
                case TypeEffect.Poison:
                    entity.ApplyDamage(0.1f + extraDamage);
                    break;
                case TypeEffect.DefenseUp:
                    entity.def = entity.EntityIsUnderEffect(TypeEffect.BrokenDef)
                        ? (1 * level)
                        : entity.initialDef + (1 * level);
                    break;
                case TypeEffect.PhysicalDefUp:
                    entity.physicalDef = entity.EntityIsUnderEffect(TypeEffect.BrokenDef)
                        ? (1 * level)
                        : entity.initialPhysicalDef + (1 * level);
                    break;
                case TypeEffect.MagicalDefUp:
                    entity.magicalDef = entity.EntityIsUnderEffect(TypeEffect.BrokenDef)
                        ? (1 * level)
                        : entity.initialMagicalDef + (1 * level);
                    break;
                case TypeEffect.AttackUp:
                    entity.att = entity.initialAtt + (1 * level);
                    break;
                case TypeEffect.AttackSpeedUp:
                    entity.attSpeed = entity.initialAttSpeed + (0.5f * level);
                    break;
                case TypeEffect.Regen:
                    entity.hp += 0.2f * level;
                    break;
                case TypeEffect.Fear:
                    originExpulsion = OriginExpulsion.Entity;
                    directionExpul = DirectionExpulsion.Out;
                    // TODO : Gestion côté client pour l'instant
//                    dir = CreateDirection(entity);

//                    entity.entityPrefab.forcedDirection = dir;
                    break;
                case TypeEffect.Charm:
                    originExpulsion = OriginExpulsion.Entity;
                    directionExpul = DirectionExpulsion.In;
                    // TODO : Gestion côté client pour l'instant
//                    dir = CreateDirection(entity);
//
//                    entity.entityPrefab.forcedDirection = dir;
                    break;
            }
        }

        public void EndEffect(Entity entity)
        {
            switch (typeEffect)
            {
                case TypeEffect.Pierce:
                    entity.canPierce = false;
                    break;
                case TypeEffect.PierceOnBack:
                    entity.canPierceOnBack = false;
                    break;
                case TypeEffect.Stun:
                    if (!entity.EntityIsUnderEffect(TypeEffect.Sleep))
                    {
                        // TODO : Pourquoi cette variable n'est pas dans entity ?
//                        entity.entityPrefab.canDoSomething = true;
                    }
                    // TODO : Gestion côté client pour l'instant
//                    RemoveEffectMaterials(entity,StaticMaterials.electricityMaterial);

                    break;
                case TypeEffect.Sleep:
                    entity.isSleep = false;
                    if (!entity.EntityIsUnderEffect(TypeEffect.Stun))
                    {
                        // TODO : Pourquoi cette variable n'est pas dans entity ?
//                        entity.entityPrefab.canDoSomething = true;
                    }

                    break;
                case TypeEffect.SpeedUp:
                    entity.speed = entity.initialSpeed;
                    break;
                case TypeEffect.Slow:
                case TypeEffect.Freezing:
                    entity.speed = entity.initialSpeed;
                    // TODO : Gestion côté client pour l'instant
//                    RemoveEffectMaterials(entity,StaticMaterials.freezingMaterial);
                    break;
                case TypeEffect.DefenseUp:
                    entity.def = entity.initialDef;
                    break;
                case TypeEffect.Invisibility:
                    entity.isInvisible = false;
                    // TODO : Gestion côté client pour l'instant
//                    entity.entityPrefab.SetInvisibility();
                    break;
                case TypeEffect.AttackSpeedUp:
                    entity.attSpeed = entity.initialAttSpeed;
                    break;
                case TypeEffect.AttackUp:
                    entity.att = entity.initialAtt;
                    break;
                case TypeEffect.Immobilization:
                    // TODO : Pourquoi cette variable n'est pas dans entity ?
//                    entity.entityPrefab.canMove = true;
                    break;
                case TypeEffect.Thorn:
                    // TODO : Gestion côté client pour l'instant
//                    entity.entityPrefab.thornSphere.SetActive(false);
                    entity.hasThorn = false;
                    break;
                case TypeEffect.Mirror:
                    entity.hasMirror = false;
                    break;
                case TypeEffect.Intangible:
                    entity.isIntangible = false;
                    break;
                case TypeEffect.AntiSpell:
                    // TODO : Gestion côté client pour l'instant
//                    entity.entityPrefab.distortionSphere.SetActive(false);
                    entity.hasAntiSpell = false;
                    break;
                case TypeEffect.DivineShield:
                    entity.hasDivineShield = false;
                    break;
                case TypeEffect.PhysicalDefUp:
                    entity.physicalDef = entity.initialPhysicalDef;
                    break;
                case TypeEffect.MagicalDefUp:
                    entity.magicalDef = entity.initialMagicalDef;
                    break;
                case TypeEffect.Resurrection:
                    entity.shooldResurrect = false;
                    break;
                case TypeEffect.Silence:
                    entity.isSilence = false;
                    break;
                case TypeEffect.BrokenDef:
                    entity.def = entity.EntityIsUnderEffect(TypeEffect.DefenseUp)
                        ? entity.def + entity.initialDef
                        : entity.initialDef;
                    entity.magicalDef = entity.EntityIsUnderEffect(TypeEffect.MagicalDefUp)
                        ? entity.magicalDef + entity.initialMagicalDef
                        : entity.initialMagicalDef;
                    entity.physicalDef = entity.EntityIsUnderEffect(TypeEffect.PhysicalDefUp)
                        ? entity.physicalDef + entity.initialPhysicalDef
                        : entity.initialPhysicalDef;
                    break;
                case TypeEffect.Confusion:
                    entity.isConfuse = false;
                    break;
                case TypeEffect.Will:
                    entity.hasWill = false;
                    break;
                case TypeEffect.Fear:
                    entity.isFeared = false;
                    break;
                case TypeEffect.Charm:
                    entity.isCharmed = false;
                    break;
                case TypeEffect.Blind:
                    entity.isBlind = false;
                    break;
                case TypeEffect.Weak:
                    entity.isWeak = false;
                    break;
                case TypeEffect.Untargetable:
                    entity.isUntargeatable = false;
                    break;
                case TypeEffect.DisableBasicAttack:
                    entity.canBasicAttack = true;
                    break;
                case TypeEffect.LifeSteal:
                    entity.hasLifeSteal = false;
                    break;
                case TypeEffect.Taunt:
                    entity.hasTaunt = false;
                    break;
                case TypeEffect.NoAggro:
                    entity.hasNoAggro = false;
                    break;
                case TypeEffect.UnkillableByBleeding:
                    entity.isUnkillableByBleeding = false;
                    break;
                case TypeEffect.Link:
                    entity.isLinked = false;
                    break;
                case TypeEffect.Redirection:
                    entity.hasRedirection = false;
                    break;
                case TypeEffect.DeactivatePassive:
                    entity.hasPassiveDeactivate = false;
                    break;
                case TypeEffect.CanRecast:
                    entity.canRecast = false;
                    break;
                case TypeEffect.LifeLink:
                    entity.hasLifeLink = false;
                    break;
                case TypeEffect.ReduceCd1:
                    if (entity.spells.Count > 0)
                    {
                        entity.spells[0].cooldown = entity.spells[0].initialCooldown;
                    }

                    break;
                case TypeEffect.ReduceCd2:
                    if (entity.spells.Count > 1)
                    {
                        entity.spells[1].cooldown = entity.spells[1].initialCooldown;
                    }

                    break;
                case TypeEffect.ReduceCd3:
                    if (entity.spells.Count > 2)
                    {
                        entity.spells[2].cooldown = entity.spells[2].initialCooldown;
                    }
                    break;
                case TypeEffect.Burn:
                    // TODO : Gestion côté client pour l'instant
//                    RemoveEffectMaterials(entity,StaticMaterials.burningMaterial);
                    break;
                case TypeEffect.Poison:
                    // TODO : Gestion côté client pour l'instant
//                    RemoveEffectMaterials(entity,StaticMaterials.poisonMaterial);
                    break;
            }
        }

        public void UpdateEffect(Entity entity, Effect newEffect)
        {
            switch (typeEffect)
            {
                case TypeEffect.Poison:
                case TypeEffect.Burn:
                    durationInSeconds += newEffect.durationInSeconds;

                    if (durationInSeconds > 20)
                    {
                        durationInSeconds = 20;
                    }

                    break;
                case TypeEffect.Freezing:
                    if (durationInSeconds < newEffect.durationInSeconds)
                    {
                        durationInSeconds = newEffect.durationInSeconds;
                    }

                    if (level < 4)
                    {
                        level += newEffect.level;

                        if (level == 4)
                        {
                            Effect stunEffect = new Effect
                                {typeEffect = TypeEffect.Stun, level = 1, durationInSeconds = 1, launcher = entity};
                            EffectController.ApplyEffect(entity, stunEffect, entity);
                            EffectController.StopCurrentEffect(entity, this);
                        }
                    }

                    break;
                case TypeEffect.AttackUp:
                case TypeEffect.SpeedUp:
                case TypeEffect.AttackSpeedUp:
                case TypeEffect.Bleed:
                    if (durationInSeconds < newEffect.durationInSeconds)
                    {
                        durationInSeconds = newEffect.durationInSeconds;
                    }

                    if (level < 5)
                    {
                        level += newEffect.level;
                    }

                    break;
                case TypeEffect.Weak:
                case TypeEffect.Blind:
                case TypeEffect.Untargetable:
                case TypeEffect.DisableBasicAttack:
                case TypeEffect.Immobilization:
                case TypeEffect.Thorn:
                case TypeEffect.Mirror:
                case TypeEffect.Slow:
                case TypeEffect.Intangible:
                case TypeEffect.AntiSpell:
                case TypeEffect.DivineShield:
                case TypeEffect.Resurrection:
                case TypeEffect.Silence:
                case TypeEffect.BrokenDef:
                case TypeEffect.Confusion:
                case TypeEffect.Will:
                case TypeEffect.DotDamageIncrease:
                case TypeEffect.Taunt:
                case TypeEffect.NoAggro:
                case TypeEffect.UnkillableByBleeding:
                case TypeEffect.Invisibility:
                case TypeEffect.Link:
                case TypeEffect.Redirection:
                case TypeEffect.DeactivatePassive:
                case TypeEffect.CanRecast:
                case TypeEffect.ReduceCd1:
                case TypeEffect.ReduceCd2:
                case TypeEffect.ReduceCd3:
                    if (durationInSeconds < newEffect.durationInSeconds)
                    {
                        durationInSeconds = newEffect.durationInSeconds;
                    }

                    break;
                case TypeEffect.MagicalDefUp:
                case TypeEffect.PhysicalDefUp:
                case TypeEffect.DefenseUp:
                case TypeEffect.Regen:
                case TypeEffect.LifeSteal:
                    durationInSeconds = newEffect.durationInSeconds;
                    level = newEffect.level;
                    break;
            }
        }
/*
 // TODO : Garder la gestion côté client pour l'instant
        private Vector3 CreateDirection(Entity entity)
        {
            Vector3 originExpulsionPosition = entity.entityPrefab.transform.position;
            Vector3 entityPosition = entity.entityPrefab.transform.position;

            if (originExpulsion == OriginExpulsion.Entity)
            {
                originExpulsionPosition = launcher.entityPrefab.transform.position;
            }
            else if (originExpulsion == OriginExpulsion.SrcDamage)
            {
                originExpulsionPosition = positionSrcDamage;
            }

            Vector3 dir = Vector3.Zero;
            if (directionExpul == DirectionExpulsion.Out)
            {
                if (originExpulsion == OriginExpulsion.ForwardOfPositionSrcDamage)
                {
                    dir = positionSrcDamage;
                }
                else
                {
                    dir = (entityPosition - originExpulsionPosition).;
                }
            }
            else if (directionExpul == DirectionExpulsion.In)
            {
                if (originExpulsion == OriginExpulsion.ForwardOfPositionSrcDamage)
                {
                    dir = -positionSrcDamage;
                }
                else
                {
                    dir = (originExpulsionPosition - entityPosition).normalized;
                }
            }

            return dir;
        }
        */
    }
}