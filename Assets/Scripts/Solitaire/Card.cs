using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;



public enum CardState
{
    Stock,
    Waste,
    Deck
}
public class Card : MonoBehaviour, IPointerClickHandler  // Реализуем интерфейс для обработки кликов
{
    public int value;  // Значение карты (2-10, J=11, Q=12, K=13, A=1)
    public bool isFaceUp = false;
    public Image cardImage;

    public List<Card> coveringCards;
    public CardState state;

    private TriPeaksGame gameManager;

    public void InitializeCard(int val, Sprite sprite, TriPeaksGame manager)
    {
        value = val;
        cardImage.sprite = sprite;
        isFaceUp = false;
        gameManager = manager;
    }

    public void Flip()
    {
        if (!isFaceUp)
        {
            isFaceUp = true;
            transform.DOScale(1.1f, 0.2f).OnComplete(() => transform.DOScale(1f, 0.2f));
        }
    }

    public void UpdateFace()
    {
        foreach (Card card in coveringCards)
        {
            if (card.state==CardState.Deck)
            {
                isFaceUp=false;
            }
        }
        isFaceUp=true;
    }

    public void MoveToWaste(Vector3 targetPosition)
    {
        transform.DOMove(targetPosition, 0.5f).OnComplete(() =>
        {
            //gameObject.SetActive(false);
            state=CardState.Waste;
        });
    }

    public bool CanBeMoved(int topCardValue)
    {
        return Mathf.Abs(value - topCardValue) == 1 || (value == 1 && topCardValue == 13) || (value == 13 && topCardValue == 1);
    }

    // Метод обработки кликов по объекту
    public void OnPointerClick(PointerEventData eventData)
    {
        gameManager.OnCardClicked(this);
    }
}