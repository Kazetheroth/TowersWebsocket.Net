using System;
using System.Numerics;
using TowersWebsocketNet31.Server.Game.EntityData;
using TowersWebsocketNet31.Server.Game.Mechanics;

namespace TowersWebsocketNet31.Server.Game.SpellData
{
    public enum MovementSpellType
    {
        Dash,
        Charge,
        Tp,
        TpWithTarget
    }

    [Serializable]
    public class MovementSpell : SpellComponent
    {
        public MovementSpell()
        {
            typeSpell = TypeSpell.Movement;
        }

        public bool isFollowingMouse { get; set; }
        public MovementSpellType movementSpellType { get; set; }

        private float distanceTravelled;
    }
}