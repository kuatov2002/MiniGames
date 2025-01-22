using UnityEngine;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    public List<Card> Deck = new List<Card>();
    public Transform StockTransform;
    public Transform PeaksParent;
    public GameObject CardPrefab;


    void Start()
    {
        CreateDeck();
    }
    public void CreateDeck()
    {
        Deck.Clear();
        for (int value = 1; value <= 13; value++) // 1-13 for Ace to King
        {
            for (int suit = 0; suit < 4; suit++) // Four suits
            {
                Card card = Instantiate(CardPrefab, StockTransform).GetComponent<Card>();
                card.cardValue = (CardValue)value;
                card.cardSuit = (CardSuit)suit;
                card.isFaceUp = false;
                Deck.Add(card);
            }
        }
        ShuffleDeck();
    }

    private void ShuffleDeck()
    {
        for (int index = 0; index < Deck.Count; index++)
        {
            Card temp = Deck[index];
            int randomIndex = Random.Range(0, Deck.Count);
            Deck[index] = Deck[randomIndex];
            Deck[randomIndex] = temp;
        }
    }

    public void DealCards()
    {
        int index = 0;
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col <= row; col++)
            {
                if (index >= Deck.Count) return;

                Card card = Deck[index];
                card.transform.SetParent(PeaksParent);
                card.transform.localPosition = new Vector3(col - row / 2f, -row * 1.5f, 0);
                card.isFaceUp = row == 3;
                card.DisplayCard();
                index++;
            }
        }
    }
}