using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using RestSharp;
using TowersWebsocketNet31.Server.Game.EntityData;
using TowersWebsocketNet31.Server.Game.Models;

namespace TowersWebsocketNet31.Server.Test
{
    public class TestLoadCards
    {
        public static void LoadCards()
        {
            RestClient api = new RestClient("https://www.towers.heolia.eu");
            RestRequest request = new RestRequest("services/game/card/list.php");
            var response = api.Post(request);
            var content = response.Content;
            var cards = JsonSerializer.Deserialize<CardJsonList>(content);
            Console.WriteLine(cards.cards.Count);
            foreach (var card in cards.cards)
            {
                Console.WriteLine(card.id);
            }
        }
    }
}