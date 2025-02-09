using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CardRow
{
    public List<CardData> cards;

    public CardRow(List<CardData> cards)
    {
        this.cards = cards;
    }
}

[Serializable]
public class GameState
{
    public float timer;
    public int moves;
    public CardData currentWasteCard;
    public List<CardRow> boardStateData; // Заменяем List<List<CardData>> на List<CardRow>
    public List<CardData> stockData;
    public List<CardData> wasteData;
    public List<GameMoveData> moveHistoryData;
}
[Serializable]
public class GameMoveData
{
    public MoveType Type;
    public string MovedCardID;
    public CardState PreviousState;
    public Vector3 PreviousPosition;
    public bool WasFaceUp;
    public string PreviousWasteCardID;
}
[Serializable]
public class CardData
{
    public int value;
    public bool isFaceUp;
    public Vector3 position;
    public CardState state;
    public string uniqueID;

    public CardData(Card card)
    {
        value = card.value;
        isFaceUp = card.isFaceUp;
        position = card.transform.position;
        state = card.state;
        uniqueID = card.UniqueID;
    }
}
public class SaveManager : MonoBehaviour
{
    public const string SAVE_KEY = "TriPeaks_SaveState";

    public static void SaveGame()
    {
        GameState state = new GameState
        {
            timer = TriPeaksGame.instance.GetTimer(),
            moves = TriPeaksGame.instance.GetMoves(),
            boardStateData = DeckManager.instance.boardCards
                .Select(row => new CardRow(row.Select(card => new CardData(card)).ToList()))
                .ToList(),
            stockData = DeckManager.instance.stock.Select(card => new CardData(card)).ToList(),
            wasteData = DeckManager.instance.waste.Select(card => new CardData(card)).ToList(),
            moveHistoryData = new List<GameMoveData>()
        };
        // Сохраняем историю ходов в правильном порядке
        var movesInOrder = new List<GameMove>(TriPeaksGame.instance.moveHistory);
        movesInOrder.Reverse();
        state.moveHistoryData = movesInOrder.Select(move => new GameMoveData
        {
            Type = move.Type,
            MovedCardID = move.MovedCard.UniqueID,
            PreviousState = move.PreviousState,
            PreviousPosition = move.PreviousPosition,
            WasFaceUp = move.WasFaceUp
        }).ToList();

        string jsonData = JsonUtility.ToJson(state);
        PlayerPrefs.SetString(SAVE_KEY, jsonData);
        PlayerPrefs.Save();
    }
    public static GameState LoadGame()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            return JsonUtility.FromJson<GameState>(jsonData);
        }
        return null;
    }

    public static void ClearSave()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
        PlayerPrefs.Save();
    }
}