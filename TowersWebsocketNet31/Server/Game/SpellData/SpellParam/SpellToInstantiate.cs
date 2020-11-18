using System.Numerics;
using TowersWebsocketNet31.Server.Game.Mechanics;

namespace TowersWebsocketNet31.Server.Game.SpellData.SpellParam
{
    // Class used for instantiation of spell
    public class SpellToInstantiate
    {
        public Geometry geometry { get; set; }
        public Vector3 scale { get; set; }
        public float height { get; set; }

        // If not null, set objetToPool at children of SpellPrefabController
        public int idPoolObject { get; set; } = -1;
        public Vector3 incrementAmplitudeByTime { get; set; }

        public bool passingThroughEntity { get; set; } = true;
    }
}