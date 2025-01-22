// Card.cs - Represents a card in the game.

using TMPro;
using UnityEngine;

public class Card:MonoBehaviour
{
    public CardValue cardValue;
    public bool isFaceUp; // Whether the card is face up
    public CardSuit cardSuit;
    public Card CoveringCard1; // First card covering this card (if any)
    public Card CoveringCard2; // Second card covering this card (if any)


    public TextMeshProUGUI cardText;    // Ссылка на UI Text компонент
    public void FlipCard()
    {
        isFaceUp = true;
        // Update card visuals here
    }

    public bool CanBeMoved(int topWasteValue)
    {
        return isFaceUp && (Mathf.Abs((int)cardValue - topWasteValue) == 1 || ((int)cardValue == 1 && topWasteValue == 13) || ((int)cardValue == 13 && topWasteValue == 1));
    }


    private void OnValidate()
    {
        // Вызываем DisplayCard при изменении значений через инспектор
        DisplayCard();
    }
    public void DisplayCard()
    {
        // Собираем текстовое описание карты
        string cardDescription = $"{cardSuit}\n{cardValue}";

        // Отображаем этот текст на UI
        cardText.text = cardDescription;

    }
}
