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


    public TextMeshProUGUI cardText;    // ������ �� UI Text ���������
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
        // �������� DisplayCard ��� ��������� �������� ����� ���������
        DisplayCard();
    }
    public void DisplayCard()
    {
        // �������� ��������� �������� �����
        string cardDescription = $"{cardSuit}\n{cardValue}";

        // ���������� ���� ����� �� UI
        cardText.text = cardDescription;

    }
}
