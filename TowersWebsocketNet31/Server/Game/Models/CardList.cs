using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace TowersWebsocketNet31.Server.Game.Models
{   
    [Serializable]
    public class CardList
    {
        public List<Card> cards;

        public CardList(string json)
        {
            cards = new List<Card>();
            
            InitCards(json);
        }

        public Card GetCardById(int id)
        {
            return Utils.Clone(cards.First(card => card.id == id));
        }

        private void InitCards(string json)
        {
            try
            {
                CardJsonList cardJsonList = JsonSerializer.Deserialize<CardJsonList>(json);

                if (cardJsonList == null)
                {
                    return;
                }

                foreach (CardJsonObject card in cardJsonList.cards)
                {
                    cards.Add(card.ConvertToCard());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Data);
            }
        }
    }
}