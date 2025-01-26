using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform stockPile, wastePile, cardContainer;
    public Sprite[] cardSprites;

    public List<Card> deck = new List<Card>();
    private Stack<Card> stock = new Stack<Card>();
    private TriPeaksGame gameManager;

    public void SetupGame(TriPeaksGame manager)
    {
        gameManager = manager;
        GenerateDeck();
        ShuffleDeck();
        DealCards();
    }

    void GenerateDeck()
    {
        for (int i = 1; i <= 13; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                GameObject newCard = Instantiate(cardPrefab,stockPile.position,Quaternion.identity, cardContainer);
                Card cardComponent = newCard.GetComponent<Card>();
                cardComponent.InitializeCard(i, cardSprites[i - 1], gameManager);
                deck.Add(cardComponent);
            }
        }
    }

    void ShuffleDeck()
    {
        deck.Sort((a, b) => Random.Range(-1, 2));
    }

    void DealCards()
    {
        int[][] cardOffsets = new int[][]
        {
            new int[] { 0 },                  // Верхний ряд (1 карта)
            new int[] { -1, 1 },               // Второй ряд (2 карты)
            new int[] { -2, 0, 2 },             // Третий ряд (3 карты)
            new int[] { -4, -3, -2, -1, 0, 1, 2, 3, 4 }  // Нижний ряд (9 карт)
        };

        float yOffset = 1.2f;
        int index = 0;

        for (int row = 0; row < cardOffsets.Length; row++)
        {
            for (int col = 0; col < cardOffsets[row].Length; col++)
            {
                Card card = deck[index++];
                card.transform.position = new Vector3(cardOffsets[row][col] * 1.1f, -row * yOffset + 3, 0);
                card.state = CardState.Deck;

                if (row == 3)  // Нижний ряд карт открытый
                {
                    card.Flip();
                }
                else
                {
                    card.gameObject.SetActive(true);
                }
            }
        }

        // Оставшиеся карты в запас
        while (index < deck.Count)
        {
            stock.Push(deck[index++]);
        }
    }



    public Card DrawFromStock()
    {
        if (stock.Count > 0)
        {
            Card drawnCard = stock.Pop();
            drawnCard.transform.position = stockPile.position;
            return drawnCard;
        }
        return null;
    }
}