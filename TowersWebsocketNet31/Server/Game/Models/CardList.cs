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

        public void InitDeck(Account.Account account, string json)
        {
            try
            {
                DeckJsonList deckJsonList = JsonSerializer.Deserialize<DeckJsonList>(json);

                if (deckJsonList == null)
                {
                    return;
                }

                List<Deck> decks = new List<Deck>();

                foreach (DeckJsonObject deckJsonObject in deckJsonList.decks)
                {
                    decks.Add(deckJsonObject.ConvertToDeck());
                }

                account.Decks = decks;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Data);
            }
        }

        public void InitCollection(Account.Account account, string json)
        {
            try
            {
                CollectionJsonList collectionList = JsonSerializer.Deserialize<CollectionJsonList>(json);

                if (collectionList == null)
                {
                    return;
                }

                List<Deck> decks = new List<Deck>();

                Dictionary<int, int> cardsInCollection = new Dictionary<int, int>();
                
                foreach (CardInCollection cardInCollection in collectionList.collections)
                {
                    cardsInCollection.Add(Int32.Parse(cardInCollection.cardId), Int32.Parse(cardInCollection.numbers));
                }

                account.CardInCollection = cardsInCollection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Data);
            }
        }
    }
}