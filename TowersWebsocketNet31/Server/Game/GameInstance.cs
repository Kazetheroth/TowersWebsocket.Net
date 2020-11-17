using System;
using System.Collections.Generic;
using System.Text.Json;
using TowersWebsocketNet31.Server.Game.Controller;
using TowersWebsocketNet31.Server.Game.EntityData;

namespace TowersWebsocketNet31.Server.Game
{
    [Serializable]
    public class GameInstance
    {
        public string mapJson;
        public List<Entity> players;
        public List<Entity> monsters;

        public readonly string headerDataInit = "GameInstance";

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