using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriPeaksGame : MonoBehaviour
{
    public static TriPeaksGame instance;

    private Stack<GameMove> moveHistory = new Stack<GameMove>();
    public Transform stockPile, wastePile;
    public DeckManager deckManager;
    public Card currentWasteCard;
    public Text movesText, timerText;

    private int moves = 0;
    private float timer = 0f;
    private bool isGameActive = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        deckManager.SetupGame(this);
    }

    void Update()
    {
        if (isGameActive)
        {
            timer += Time.deltaTime;
            timerText.text = $"Time: {timer:F1}s";
        }
    }

    public void OnCardClicked(Card card)
    {
        if (!card.isFaceUp)
        {
            return;
        }

        switch (card.state)
        {
            case CardState.Deck:
                if (currentWasteCard != null && card.CanBeMoved(currentWasteCard.value))
                {
                    GameMove move = new GameMove(
                        MoveType.CardMove,
                        card,
                        card.state,
                        card.transform.position,
                        card.isFaceUp,
                        currentWasteCard
                    );
                    moveHistory.Push(move);

                    // Execute the move
                    card.MoveToWaste(wastePile.position);
                    currentWasteCard = card;
                    card.state = CardState.Waste;
                    foreach (Card[] row in deckManager.boardCards)
                    {
                        foreach (Card cardInDeck in row)
                        {
                            cardInDeck.UpdateFace();
                        }
                    }
                    moves++;
                    movesText.text = "Moves: " + moves;

                    CheckWinCondition();
                }
                break;
        }
    }
    public void Undo()
    {
        if (moveHistory.Count == 0)
        {
            Debug.Log("No moves to undo!");
            return;
        }

        GameMove lastMove = moveHistory.Pop();
        moves++;
        movesText.text = "Moves: " + moves;
        switch (lastMove.Type)
        {
            case MoveType.CardMove:
                // Восстанавливаем карту на поле
                lastMove.MovedCard.MovePosition(lastMove.PreviousPosition);
                lastMove.MovedCard.state = lastMove.PreviousState;

                if (!lastMove.WasFaceUp)
                {
                    lastMove.MovedCard.spriteRenderer.color = Color.black;
                    lastMove.MovedCard.isFaceUp = false;
                }

                currentWasteCard = lastMove.PreviousWasteCard;

                // Обновляем состояние всех карт
                foreach (Card[] row in deckManager.boardCards)
                {
                    foreach (Card cardInDeck in row)
                    {
                        cardInDeck.UpdateFace();
                    }
                }

                break;

            case MoveType.DrawCard:
                // Возвращаем карту в колоду
                deckManager.ReturnCardToStock(lastMove.MovedCard);
                currentWasteCard = lastMove.PreviousWasteCard;
                
                break;
        }
    }
    public void DrawNewCard()
    {
        Card newCard = deckManager.DrawFromStock();
        if (newCard != null)
        {
            // Сохраняем информацию о ходе перед выполнением
            GameMove move = new GameMove(
                MoveType.DrawCard,
                newCard,
                CardState.Stock,
                stockPile.position,
                newCard.isFaceUp,
                currentWasteCard
            );
            moveHistory.Push(move);

            moves++;
            movesText.text = "Moves: " + moves;

            currentWasteCard = newCard;
            currentWasteCard.MoveToWaste(wastePile.position);
        }
        else
        {
            Debug.Log("No more cards in stock!");
        }
    }

    public void CheckWinCondition()
    {
        /*if (GameObject.FindGameObjectsWithTag("Card").Length == 0)
        {
            isGameActive = false;
            Debug.Log("You win!");
        }*/
    }
}



public enum MoveType
{
    CardMove,
    DrawCard
}

public class GameMove
{
    public MoveType Type { get; set; }
    public Card MovedCard { get; set; }
    public CardState PreviousState { get; set; }
    public Vector3 PreviousPosition { get; set; }
    public bool WasFaceUp { get; set; }
    public Card PreviousWasteCard { get; set; }

    public GameMove(MoveType type, Card movedCard, CardState previousState, Vector3 previousPosition, bool wasFaceUp, Card previousWasteCard)
    {
        Type = type;
        MovedCard = movedCard;
        PreviousState = previousState;
        PreviousPosition = previousPosition;
        WasFaceUp = wasFaceUp;
        PreviousWasteCard = previousWasteCard;
    }
}