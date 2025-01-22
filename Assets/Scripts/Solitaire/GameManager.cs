using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public DeckManager DeckManager;
    public List<Card> WastePile = new List<Card>();
    public Transform WasteTransform;
    public int Moves = 0;

    void Start()
    {
        DeckManager.CreateDeck();
        DeckManager.DealCards();
    }

    public void MoveCardToWaste(Card card)
    {
        if (WastePile.Count == 0 || card.CanBeMoved((int)WastePile[^1].cardValue))
        {
            card.transform.SetParent(WasteTransform);
            WastePile.Add(card);
            Moves++;
            CheckGameOver();
        }
    }

    private void CheckGameOver()
    {
        if (DeckManager.Deck.Count == 0 && WastePile.Count == 0)
        {
            Debug.Log("Game Over! You Win!");
        }
    }
}