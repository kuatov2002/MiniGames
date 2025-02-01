using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform cardContainer;
    public Sprite[] cardSprites;

    

    public Card[][] boardCards = new Card[][]
    {
        new Card[3],
        new Card[6],
        new Card[9],
        new Card[10]
    };
    private Stack<Card> stock = new Stack<Card>();
    private TriPeaksGame gameManager;

    public void SetupGame(TriPeaksGame manager)
    {
        gameManager = manager;
        GenerateDeck();
        DealCards();
    }

    void GenerateDeck()
    {
        List<Card> deck = new List<Card>();
        for (int i = 1; i <= 13; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                GameObject newCard = Instantiate(cardPrefab,TriPeaksGame.instance.stockPile.position,Quaternion.identity, cardContainer);
                Card cardComponent = newCard.GetComponent<Card>();
                cardComponent.InitializeCard(i, cardSprites[i - 1], gameManager);
                deck.Add(cardComponent);
                newCard.name = cardComponent.value.ToString();
            }
        }

        for (int i = 0; i < 5; i++)
        {
            ShuffleDeck(deck);
        }
        

        int index = 0;

        for (int row = 0; row < boardCards.Length; row++)
        {
            for (int col = 0; col < boardCards[row].Length; col++)
            {
                boardCards[row][col] = deck[index];
                boardCards[row][col].spriteRenderer.color = Color.black;
                index++;
            }
        }
        // Удаляем первые 28 карт, которые уже на boardCards, из основной колоды
        deck.RemoveRange(0, 28);
            
        // Оставшиеся карты добавляем в стопку
        foreach (var card in deck)
        {
            stock.Push(card);
        }
    }

    void DealCards()
    {
        // Define the positions for each row of cards
        Vector3[] rowPositions = new Vector3[]
        {
            new Vector3(-3.75f, 0.75f, 0), // Top row (1 card)
            new Vector3(-4f, 0, 0),  // Second row (3 cards)
            new Vector3(-4.25f, -0.75f, 0), // Third row (6 cards)
            new Vector3(-4.5f, -1.5f, 0)   // Bottom row (10 cards)
        };


        for (int col = 0; col < boardCards[0].Length; col++)
        {
            Card card = boardCards[0][col];
            card.transform.position = rowPositions[0] + new Vector3(col * 1.5f, 0, 0);
            card.state = CardState.Deck;

        }
        for (int col = 0; col < boardCards[1].Length; col++)
        {
            Card card = boardCards[1][col];
            card.transform.position = rowPositions[1] + new Vector3((col + (col / 2)) * 0.5f, 0, 0);
            card.state = CardState.Deck;
        }
        for (int row = 2; row < boardCards.Length; row++)
        {
            for (int col = 0; col < boardCards[row].Length; col++)
            {
                Card card = boardCards[row][col];
                card.transform.position = rowPositions[row] + new Vector3(col * 0.5f, 0, 0);
                card.state = CardState.Deck;

                if (row == 3) // Bottom row
                {
                    card.Flip(); // Bottom row cards are face-up
                }
            }
        }

        for (int i = 0; i < boardCards[0].Length; i++)
        {
            boardCards[0][i].coveringCards.Add(boardCards[1][2 * i]);
            boardCards[0][i].coveringCards.Add(boardCards[1][2 * i + 1]);
        }
        for (int i = 0; i < boardCards[1].Length; i++)
        {
            boardCards[1][i].coveringCards.Add(boardCards[2][i + (i / 2)]);
            boardCards[1][i].coveringCards.Add(boardCards[2][i + (i / 2) + 1]);
        }
        for (int i = 0; i < boardCards[2].Length; i++)
        {
            boardCards[2][i].coveringCards.Add(boardCards[3][i]);
            boardCards[2][i].coveringCards.Add(boardCards[3][i + 1]);
        }
    }




    public Card DrawFromStock()
    {
        if (stock.Count > 0)
        {
            Card drawnCard = stock.Pop();
            drawnCard.transform.position = TriPeaksGame.instance.stockPile.position;
            return drawnCard;
        }
        return null;
    }

    public void ReturnCardToStock(Card card)
    {
        stock.Push(card);
        card.MovePosition(TriPeaksGame.instance.stockPile.position);
        card.state = CardState.Stock;
        card.spriteRenderer.color = Color.black;
        card.isFaceUp = false;
    }

    void ShuffleDeck(List<Card> deck)
    {
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (deck[i], deck[randomIndex]) = (deck[randomIndex], deck[i]); // Обмен местами
        }
    }

}