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
        /*public List<> deck;
        public List<> hand;*/

        public GameGrid GameGrid;
        
        public readonly string headerDataInit = "GameInstance";

        public GameInstance(int idPlayer, int idClasses, int idCategoryWeapon, int idEquipmentdeck, int idMonsterDeck, GameGrid gameGrid)
        {
            this.idPlayer = idPlayer;
            this.GameGrid = gameGrid;
            player = new Player(idClasses, idCategoryWeapon);
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