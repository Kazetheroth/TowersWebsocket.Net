using System;
using TowersWebsocketNet31.Server.Game.Controller;
using TowersWebsocketNet31.Server.Game.Mechanics;

namespace TowersWebsocketNet31.Server.Game.SpellData
{
    [Serializable]
    public class PassiveSpell : SpellComponent
    {
        public PassiveSpell()
        {
            typeSpell = TypeSpell.Passive;
        }

        public SpellComponent permanentSpellComponent { get; set; }
        
        // use in game
        private SpellComponent permanentSpellComponentInstantiate;

        public override void DuringInterval()
        {
            spellDuration = 99999;

            if (caster.hasPassiveDeactivate && permanentSpellComponentInstantiate != null)
            {
                SpellInterpreter.EndSpellComponent(permanentSpellComponentInstantiate);
                permanentSpellComponentInstantiate = null;
            }
            else if (permanentSpellComponentInstantiate == null)
            {
                permanentSpellComponentInstantiate = SpellController.CastSpellComponent(caster, permanentSpellComponent, caster, caster.position, this);
            }
        }
    }
}