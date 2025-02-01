using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum CardState
{
    Stock,
    Waste,
    Deck
}

public class Card : MonoBehaviour // Реализуем интерфейс для обработки кликов
{
    public int value; // Значение карты (2-10, J=11, Q=12, K=13, A=1)
    public bool isFaceUp = false;
    public Sprite cardSprite; // Спрайт карты
    public SpriteRenderer spriteRenderer; // Компонент для отображения спрайта карты

    public List<Card> coveringCards;
    public CardState state;

    private TriPeaksGame gameManager;

    private static int orderIndex=0;

    void Awake()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
    }

    public void InitializeCard(int val, Sprite sprite, TriPeaksGame manager)
    {
        value = val;
        cardSprite = sprite;
        spriteRenderer.sprite = cardSprite; // Устанавливаем спрайт в SpriteRenderer
        isFaceUp = false;
        spriteRenderer.color = Color.black; // Изначально карта "закрыта"
        gameManager = manager;
    }

    public void Flip()
    {
        if (!isFaceUp)
        {
            isFaceUp = true;
            spriteRenderer.color = Color.white; // "Открываем" карту
        }
    }

    public void UpdateFace()
    {
        // Проверяем все покрывающие карты
        foreach (Card card in coveringCards)
        {
            if (card.state == CardState.Deck)
            {
                isFaceUp = false;
                spriteRenderer.color = Color.black; // Если карта закрыта, отображаем её как закрытую
                return;
            }
        }

        // Если все покрывающие карты открыты или нет покрывающих карт, открываем текущую карту
        isFaceUp = true;
        spriteRenderer.color = Color.white; // "Открываем" карту
    }

    public void MoveToWaste(Vector3 targetPosition)
    {

        Flip();
        spriteRenderer.sortingOrder=orderIndex;
        orderIndex++;
        transform.DOMove(targetPosition, 0.5f).OnComplete(() =>
        {
            state = CardState.Waste;
        });
    }

    public bool CanBeMoved(int topCardValue)
    {
        if (topCardValue == 0)
        {
            return false;
        }

        return Mathf.Abs(value - topCardValue) == 1 || (value == 1 && topCardValue == 13) || (value == 13 && topCardValue == 1);
    }
    public void MovePosition(Vector3 position)
    {
        transform.DOMove(position, 0.5f);
    }
    // Метод обработки кликов по объекту
    public void OnMouseDown()
    {
        gameManager.OnCardClicked(this);
    }
}
