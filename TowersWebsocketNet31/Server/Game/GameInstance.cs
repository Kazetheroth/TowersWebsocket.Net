using System;
using System.Collections.Generic;
using System.Text.Json;
using TowersWebsocketNet31.Server.Game.Controller;
using TowersWebsocketNet31.Server.Game.EntityData;
using TowersWebsocketNet31.Server.Game.EquipmentData;
using TowersWebsocketNet31.Server.Game.Mechanics;

namespace TowersWebsocketNet31.Server.Game
{
    [Serializable]
    public class GameInstance
    {
        public int idPlayer;
        public Player player;

        public List<Entity> otherPlayers;
        public List<Entity> monsters;

        public Grid grid;
        
        public readonly string headerDataInit = "GameInstance";

        public GameInstance(int idPlayer, Classes classes, CategoryWeapon categoryWeapon, int idEquipmentdeck, int idMonsterDeck, Grid grid)
        {
            this.idPlayer = idPlayer;
            this.grid = grid;
            player = new Player(classes, categoryWeapon);
        }

        public void SendGameData(TowersWebsocket session)
        {
            string gameInstance = JsonSerializer.Serialize(this);
            session.SendToTarget("{\"callbackMessages\":{\"" + headerDataInit + "\":" + gameInstance + "}}", session.ID);
        }

        public void StartGame()
        {
            
        }
    }
}