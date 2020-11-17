using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TowersWebsocketNet31.Server.Game.Controller;
using TowersWebsocketNet31.Server.Game.EntityData;
using TowersWebsocketNet31.Server.Game.Mechanics;

namespace TowersWebsocketNet31.Server.Test
{
    public class TestMain
    {
        public static async Task RunTestAsyncOneEffect()
        {
            Console.WriteLine("Enter Main !");
            Effect effect = new Effect {level = 2, typeEffect = TypeEffect.Bleed, durationInSeconds = 5};
            Effect effect2 = new Effect {level = 2, typeEffect = TypeEffect.AttackUp, durationInSeconds = 8};

            Entity origin = new Entity();
            Entity target = new Entity();
            
            Console.WriteLine("Target hp " + target.hp);
            
            Task effectTask = EffectController.ApplyEffect(target, effect, origin);
            Task effectTask2 = EffectController.ApplyEffect(origin, effect2, origin);

            await Task.Delay(1000);
            Console.WriteLine("Current origin att : " + origin.att);

            await Task.Delay(2000);
            EffectController.StopCurrentEffect(origin, origin.underEffects[TypeEffect.AttackUp]);
            
            await effectTask;
            await effectTask2;

            Console.WriteLine("Target hp " + target.hp);
            Console.WriteLine("Current origin att : " + origin.att);
            Console.WriteLine("Task is finished");
        }
    }
}