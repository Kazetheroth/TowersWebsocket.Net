using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Games.Global.Armors;
using TowersWebsocketNet31.Server.Game.Controller;
using TowersWebsocketNet31.Server.Game.EquipmentData;
using TowersWebsocketNet31.Server.Game.Mechanics;

namespace TowersWebsocketNet31.Server.Game.EntityData
{
    public enum TypeEntity
    {
        ALLIES,
        MOB
    }
    
    public enum BehaviorType
    {
        Player,
        MoveOnTargetAndDie,
        Distance,
        Melee,
        WithoutTarget
    }
    
    public enum AttackBehaviorType {
        Random,
        AllSpellsIFirst
    }

    // Class for mobs and players
    public class Entity : SynchronizablePosition
    {
        public int IdEntity { get; set; }

        public bool isPlayer { get; set; } = false;
        public bool isSummon { get; set; } = false;

        private const float DEFAULT_HP  = 100;
        private const int DEFAULT_DEF = 10;
        private const float DEFAULT_ATT = 10;
        private const float DEFAULT_SPEED = 10;
        private const float DEFAULT_ATT_SPEED = 0;
        private const float DEFAULT_RESSOURCE = 50;
        
        private const int DEFAULT_NB_WEAPONS = 1;

        public float initialHp { get; set; } = DEFAULT_HP;
        public int initialDef { get; set; } = DEFAULT_DEF;
        public int initialMagicalDef { get; set; } = 0;
        public int initialPhysicalDef { get; set; } = 0;
        public float initialAtt { get; set; } = DEFAULT_ATT;
        public float initialSpeed { get; set; } = DEFAULT_SPEED;
        public float initialAttSpeed { get; set; } = DEFAULT_ATT_SPEED;
        public float initialRessource1 { get; set; } = DEFAULT_RESSOURCE;

        public float hp { get; set; } = DEFAULT_HP;
        public int def { get; set; } = DEFAULT_DEF;
        public float att { get; set; } = DEFAULT_ATT;
        public float speed { get; set; } = DEFAULT_SPEED;
        public float attSpeed { get; set; } = DEFAULT_ATT_SPEED;
        public int magicalDef { get; set; } = 0;
        public int physicalDef { get; set; } = 0;

        public float ressource1 { get; set; } = DEFAULT_RESSOURCE;

        public List<int> playerInBack { get; set; }
        
        // If needed, create WeaponExposer to get all scripts of a weapon
        public List<Weapon> weapons { get; set; }
        public List<Armor> armors { get; set; }

        public TypeEntity typeEntity { get; set; }

        // Suffered effect 
        public Dictionary<TypeEffect, Effect> underEffects { get; set; }

        // Effect add to damage deal
        public List<Effect> damageDealExtraEffect { get; set; }

        // Effect add to damage receive
        public List<Effect> damageReceiveExtraEffect { get; set; }

        public List<SpellComponent> activeSpellComponents;
        public List<Entity> entityInRange;

        // TODO : a supprimer - dépendence circulaire côté client, horrible
        //public EntityPrefab entityPrefab;

        public Spell basicAttack { get; set; }
        public Spell basicDefense { get; set; }
        public List<Spell> spells { get; set; }

        public BehaviorType BehaviorType { get; set; }
        public AttackBehaviorType AttackBehaviorType { get; set; }

        public bool doingSkill { get; set; } = false;
        public int nbCharges;

        // Bool set by effect
        public bool isWeak = false;
        public bool canPierce = false;
        public bool isInvisible = false;
        public bool isUntargeatable = false;
        public bool isSleep = false;
        public bool canPierceOnBack = false;
        public bool hasThorn = false;
        public bool hasMirror = false;
        public bool isIntangible = false;
        public bool hasAntiSpell = false;
        public bool hasDivineShield = false;
        public bool shooldResurrect = false;
        public bool isSilence = false;
        public bool isConfuse = false;
        public bool hasWill = false;
        public bool isFeared = false;
        public bool isCharmed = false;
        public bool isBlind = false;
        public bool canBasicAttack = true;
        public bool hasLifeSteal = false;
        public bool hasTaunt = false;
        public bool hasNoAggro = false;
        public bool isUnkillableByBleeding = false;
        public bool isLinked = false;
        public bool hasRedirection = false;
        public bool hasPassiveDeactivate = false;
        public bool canRecast = false;
        public bool hasLifeLink = false;

        public Entity target;
        public Vector3 positionPointed;

        public Entity()
        {
            weapons = new List<Weapon>();
            armors = new List<Armor>();
            underEffects = new Dictionary<TypeEffect, Effect>();
            damageDealExtraEffect = new List<Effect>();
            damageReceiveExtraEffect = new List<Effect>();
            entityInRange = new List<Entity>();
            activeSpellComponents = new List<SpellComponent>();
            spells = new List<Spell>();

            InitStatBasedOnOriginal();
        }

        public void InitStatBasedOnOriginal()
        {
            hp = initialHp;
            def = initialDef;
            magicalDef = initialMagicalDef;
            physicalDef = initialPhysicalDef;
            att = initialAtt;
            speed = initialSpeed;
            attSpeed = initialAttSpeed;
        }

        // Take true damage is usefull with effect pierce
        public virtual void TakeDamage(float initialDamage, Entity originDamage, DamageType damageType, SpellComponent originSpellComponent = null)
        {
            float damageReceived = (initialDamage - def) > 0 ? (initialDamage - def) : 0;
            bool takeTrueDamage = originDamage.canPierce;

            bool isMagic = damageType == DamageType.Magical;
            bool isPhysic = damageType == DamageType.Physical;

            if (hasDivineShield || (isIntangible && isPhysic) || (hasAntiSpell && isMagic) || originDamage.isBlind)
            {
                initialDamage = 0;
                damageReceived = 0;
            }
            
            if (isMagic)
            {
                damageReceived = (damageReceived - magicalDef) > 0 ? (damageReceived - magicalDef) : 0;
            } 
            else if (isPhysic)
            {
                damageReceived = (damageReceived - physicalDef) > 0 ? (damageReceived - physicalDef) : 0;
            }

            if (takeTrueDamage ||
                (originDamage.canPierceOnBack && 
                 playerInBack.Contains(originDamage.IdEntity)
                ))
            {
                damageReceived = initialDamage;
            }

            /*
             // TODO : Cas particuler pour les summons avec un effect
            if (originDamage.hasLifeLink)
            {
                if (originDamage.isSummon)
                {
                    GenericSummonSpell genericSummonSpell = (GenericSummonSpell) originDamage.entityPrefab;
                    genericSummonSpell.summoner.hp =
                        genericSummonSpell.summoner.hp + (damageReceived * 0.25f) <=
                        genericSummonSpell.summoner.initialHp
                            ? genericSummonSpell.summoner.hp + (damageReceived * 0.25f)
                            : genericSummonSpell.summoner.initialHp;
                }
            }
            */

            if (originDamage.hasLifeSteal)
            {
                originDamage.hp += damageReceived * originDamage.underEffects[TypeEffect.LifeSteal].level;
                if (originDamage.hp > originDamage.initialHp)
                {
                    originDamage.hp = originDamage.initialHp;
                }
            }

            // TODO invocationInScene set in GameInstance
//            if (hasRedirection && DataObject.invocationsInScene.Count > 0)
//            {
//                DataObject.invocationsInScene[0].ApplyDamage(damageReceived * 0.75f, originSpellComponent);
//                ApplyDamage(damageReceived * 0.25f, originSpellComponent);
//            }
//            else
//            {
                ApplyDamage(damageReceived, originSpellComponent);
//            }

            List<Effect> effects = 
                damageReceiveExtraEffect
                    .GroupBy(currentEffect => currentEffect.typeEffect)
                    .Select(g => g.First())
                    .ToList();
            foreach (Effect effect in effects)
            {
                EffectController.ApplyEffect(this, effect, originDamage);
            }

            if (isSleep)
            {
                EffectController.StopCurrentEffect(this, underEffects[TypeEffect.Sleep]);
            }

            // TODO : Wait for spellInterpreter
            //SpellInterpreter.TriggerWhenEntityReceivedDamage(activeSpellComponents);
        }

        public virtual void ApplyDamage(float directDamage, SpellComponent originSpellComponent = null)
        {
            //Console.WriteLine(modelName + " - Damage applied = " + directDamage);
            hp -= directDamage;
            
            if (hp <= 0)
            {
                if (originSpellComponent != null)
                {
                    // TODO : Wait for spellInterpreter
                    //SpellInterpreter.TriggerWhenEntityDie(originSpellComponent);
                }
                
                if (shooldResurrect)
                {
                    hp = initialHp / 2;
                    EffectController.StopCurrentEffect(this, underEffects[TypeEffect.Resurrection]);

                    return;
                }

                /*
                 // TODO Old system to player dead
                 
                 // TODO : gérer côté client
                entityPrefab.EntityDie();

                if (isPlayer)
                {
                    TowersWebSocket.TowerSender("OTHERS", NetworkingController.CurrentRoomToken, "Player", "SendDeath", null);
                    TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken, "Player", "HasWon", null);
                    Console.WriteLine("Vous êtes mort");
                    Cursor.lockState = CursorLockMode.None;
                }
                */
            }
        }
    }
}