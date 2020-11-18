using System.Collections.Generic;

namespace TowersWebsocketNet31.Server.Game.Models
{
    public class TrapBehavior
    {
        public enum TrapType
        {
            Arrows,
            Spikes,
            EjectionPlate,
            RammingRail,
            BearTrap,
            Fan,
            SpikyPole,
            Mine
        }

        public enum AdditionalEffects
        {
            Poison,
            Burn,
            Freeze,
            Weakness,
            Stun
        }

        public TrapType mainType;
        public List<AdditionalEffects> trapEffects;

        public int rotation = 0;

        public void CopyBehavior(TrapBehavior newTrapBehavior)
        {
            mainType = newTrapBehavior.mainType;
            trapEffects = newTrapBehavior.trapEffects;
        }
    }
}
