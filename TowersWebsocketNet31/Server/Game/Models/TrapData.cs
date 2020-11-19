using System;
using System.Collections.Generic;

namespace TowersWebsocketNet31.Server.Game.Models
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
    
    [Serializable]
    public class TrapData
    {
        public TrapType mainType { get; set; }
        public List<AdditionalEffects> trapEffects { get; set; }
    }
}