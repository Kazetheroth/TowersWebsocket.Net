using System;

namespace TowersWebsocketNet31.Server.Game.EquipmentData
{
    [Serializable]
    public class Spear : Weapon
    {
        public Spear()
        {
            animationToPlay = "doingSpearAttack";
            //pattern = //pattern[2];
            //pattern[0] = //pattern(PA_INST.FRONT, 1, 0.2f, 0.01f);
            //pattern[1] = //pattern(PA_INST.BACK, 1, 0.2f, 0.01f);
        }
    }
}