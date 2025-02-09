using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum CardState
{
    Stock,
    Waste,
    Deck
}

public class Card : MonoBehaviour // ��������� ��������� ��� ��������� ������
{
    public int value; // �������� ����� (2-10, J=11, Q=12, K=13, A=1)
    public bool isFaceUp = false;
    public Sprite cardSprite; // ������ �����
    public SpriteRenderer spriteRenderer; // ��������� ��� ����������� ������� �����

    public List<Card> coveringCards;
    public CardState state;

    public static int orderIndex=0;

    public string UniqueID;
    void Awake()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
    }

    public void InitializeCard(int val, Sprite sprite)
    {
        value = val;
        cardSprite = sprite;
        spriteRenderer.sprite = cardSprite;
        isFaceUp = false;
        spriteRenderer.color = Color.black;
        UniqueID = System.Guid.NewGuid().ToString(); // ���������� ���������� ID
    }

    public void Flip()
    {
        if (!isFaceUp)
        {
            isFaceUp = true;
            spriteRenderer.color = Color.white; // "���������" �����
        }
    }

    public void UpdateFace()
    {
        // ��������� ��� ����������� �����
        foreach (Card card in coveringCards)
        {
            if (card.state == CardState.Deck)
            {
                isFaceUp = false;
                spriteRenderer.color = Color.black; // ���� ����� �������, ���������� � ��� ��������
                return;
            }
        }

        // ���� ��� ����������� ����� ������� ��� ��� ����������� ����, ��������� ������� �����
        isFaceUp = true;
        spriteRenderer.color = Color.white; // "���������" �����
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
    // ����� ��������� ������ �� �������
    public void OnMouseDown()
    {
        TriPeaksGame.instance.OnCardClicked(this);
    }
}
