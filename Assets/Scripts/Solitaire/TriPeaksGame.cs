using UnityEngine;
using UnityEngine.UI;

public class TriPeaksGame : MonoBehaviour
{
    public DeckManager deckManager;
    public Card currentWasteCard;
    public Transform wastePile;
    public Text movesText, timerText;

    private int moves = 0;
    private float timer = 0f;
    private bool isGameActive = true;

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
                if (currentWasteCard != null&&card.CanBeMoved(currentWasteCard.value))
                {
                    card.MoveToWaste(wastePile.position);
                    currentWasteCard = card;
                    card.state = CardState.Waste;

                    foreach (Card cardInDeck in deckManager.deck)
                    {
                        cardInDeck.UpdateFace();
                    }

                    moves++;
                    movesText.text = "Moves: " + moves;
                    
                    CheckWinCondition();
                }
                break;
        }

    }

    public void DrawNewCard()
    {
        Card newCard = deckManager.DrawFromStock();
        if (newCard != null)
        {
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