using DG.Tweening.Core.Easing;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriPeaksGame : MonoBehaviour
{
    public static TriPeaksGame instance;

    private Stack<GameMove> moveHistory = new Stack<GameMove>();
    public Transform stockPile, wastePile;
    
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
        //DeckManager.instance.SetupGame();
        LoadGameState();
    }
    private void OnEnable()
    {
        
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
        switch (card.state)
        {
            case CardState.Deck: 
                if (!card.isFaceUp)
                {
                    return;
                }
                if (DeckManager.instance.currentWasteCard != null && card.CanBeMoved(DeckManager.instance.currentWasteCard.value))
                {
                    GameMove move = new GameMove(
                        MoveType.CardMove,
                        card,
                        card.state,
                        card.transform.position,
                        card.isFaceUp,
                        DeckManager.instance.currentWasteCard
                    );
                    moveHistory.Push(move);

                    // Execute the move
                    card.MoveToWaste(wastePile.position);
                    DeckManager.instance.currentWasteCard = card;
                    card.state = CardState.Waste;
                    UpdateAllCards();

                    moves++;
                    movesText.text = "Moves: " + moves;

                    CheckWinCondition();
                }

                break;
            case CardState.Stock:
                DrawNewCard();
                break;
        }
    }

    public void UpdateAllCards()
    {
        foreach (Card[] row in DeckManager.instance.boardCards)
        {
            foreach (Card cardInDeck in row)
            {
                cardInDeck.UpdateFace();
            }
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

                DeckManager.instance.currentWasteCard = lastMove.PreviousWasteCard;

                // Обновляем состояние всех карт
                foreach (Card[] row in DeckManager.instance.boardCards)
                {
                    foreach (Card cardInDeck in row)
                    {
                        cardInDeck.UpdateFace();
                    }
                }

                break;

            case MoveType.DrawCard:
                // Возвращаем карту в колоду
                DeckManager.instance.ReturnCardToStock(lastMove.MovedCard);
                DeckManager.instance.currentWasteCard = lastMove.PreviousWasteCard;

                break;
        }
    }

    public void DrawNewCard()
    {
        Card newCard = DeckManager.instance.DrawFromStock();
        if (newCard != null)
        {
            // Сохраняем информацию о ходе перед выполнением
            GameMove move = new GameMove(
                MoveType.DrawCard,
                newCard,
                CardState.Stock,
                stockPile.position,
                newCard.isFaceUp,
                DeckManager.instance.currentWasteCard
            );
            moveHistory.Push(move);

            moves++;
            movesText.text = "Moves: " + moves;

            DeckManager.instance.currentWasteCard = newCard;
            DeckManager.instance.currentWasteCard.MoveToWaste(wastePile.position);
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


    private void OnApplicationQuit()
    {
        SaveGameState();
    }

    public float GetTimer()
    {
        return timer;
    }

    public int GetMoves()
    {
        return moves;
    }

    private void SaveGameState()
    {
        SaveManager.SaveGame();
    }

    private void LoadGameState()
    {
        GameState savedState = SaveManager.LoadGame(); 
        if (savedState != null)
        {
            timer = savedState.timer;
            moves = savedState.moves;
            movesText.text = "Moves: " + moves;
            timerText.text = $"Time: {timer:F1}s";

            // Let DeckManager handle the card restoration
            DeckManager.instance.RestoreGameState(savedState);
        }
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

    public GameMove(MoveType type, Card movedCard, CardState previousState, Vector3 previousPosition, bool wasFaceUp,
        Card previousWasteCard)
    {
        Type = type;
        MovedCard = movedCard;
        PreviousState = previousState;
        PreviousPosition = previousPosition;
        WasFaceUp = wasFaceUp;
        PreviousWasteCard = previousWasteCard;
    }
}