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
    public List<CardData> stockPileData;
}

[Serializable]
public class CardData
{
    public int value;
    public bool isFaceUp;
    public Vector3 position;
    public CardState state;

    public CardData(Card card)
    {
        value = card.value;
        isFaceUp = card.isFaceUp;
        position = card.transform.position;
        state = card.state;
    }
}
public class SaveManager : MonoBehaviour
{
    public const string SAVE_KEY = "TriPeaks_SaveState";

    public static void SaveGame()
    {
        Debug.Log("SaveGame called. StackTrace:\n" + Environment.StackTrace);
        GameState state = new GameState
        {
            timer = TriPeaksGame.instance.GetTimer(),
            moves = TriPeaksGame.instance.GetMoves(),
            boardStateData = DeckManager.instance.boardCards
                .Select(row => new CardRow(
                    row.Select(card => new CardData(card)).ToList()
                ))
                .ToList(),
            stockPileData = DeckManager.instance.stock
                .Select(card => new CardData(card))
                .ToList()
        };

        // Сохраняем текущую карту из сброса, если она есть
        if (DeckManager.instance.currentWasteCard != null)
        {
            state.currentWasteCard = new CardData(DeckManager.instance.currentWasteCard);
        }

        string jsonData = JsonUtility.ToJson(state);
        //jsonData =
        //    "{\"timer\":27.560985565185548,\"moves\":16,\"currentWasteCard\":{\"value\":9,\"isFaceUp\":true,\"position\":{\"x\":0.17999982833862306,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},\"boardStateData\":[{\"cards\":[{\"value\":13,\"isFaceUp\":false,\"position\":{\"x\":-3.75,\"y\":0.75,\"z\":0.0},\"state\":2},{\"value\":6,\"isFaceUp\":false,\"position\":{\"x\":-2.25,\"y\":0.75,\"z\":0.0},\"state\":2},{\"value\":4,\"isFaceUp\":false,\"position\":{\"x\":-0.75,\"y\":0.75,\"z\":0.0},\"state\":2}]},{\"cards\":[{\"value\":8,\"isFaceUp\":false,\"position\":{\"x\":-4.0,\"y\":0.0,\"z\":0.0},\"state\":2},{\"value\":1,\"isFaceUp\":false,\"position\":{\"x\":-3.5,\"y\":0.0,\"z\":0.0},\"state\":2},{\"value\":8,\"isFaceUp\":true,\"position\":{\"x\":-2.5,\"y\":0.0,\"z\":0.0},\"state\":2},{\"value\":2,\"isFaceUp\":false,\"position\":{\"x\":-2.0,\"y\":0.0,\"z\":0.0},\"state\":2},{\"value\":1,\"isFaceUp\":false,\"position\":{\"x\":-1.0,\"y\":0.0,\"z\":0.0},\"state\":2},{\"value\":10,\"isFaceUp\":false,\"position\":{\"x\":-0.5,\"y\":0.0,\"z\":0.0},\"state\":2}]},{\"cards\":[{\"value\":5,\"isFaceUp\":true,\"position\":{\"x\":-4.25,\"y\":-0.75,\"z\":0.0},\"state\":2},{\"value\":12,\"isFaceUp\":true,\"position\":{\"x\":-3.75,\"y\":-0.75,\"z\":0.0},\"state\":2},{\"value\":8,\"isFaceUp\":true,\"position\":{\"x\":0.18000006675720216,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":9,\"isFaceUp\":true,\"position\":{\"x\":0.18000006675720216,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":5,\"isFaceUp\":true,\"position\":{\"x\":0.18000006675720216,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":13,\"isFaceUp\":false,\"position\":{\"x\":-1.75,\"y\":-0.75,\"z\":0.0},\"state\":2},{\"value\":12,\"isFaceUp\":false,\"position\":{\"x\":-1.25,\"y\":-0.75,\"z\":0.0},\"state\":2},{\"value\":4,\"isFaceUp\":false,\"position\":{\"x\":-0.75,\"y\":-0.75,\"z\":0.0},\"state\":2},{\"value\":3,\"isFaceUp\":false,\"position\":{\"x\":-0.25,\"y\":-0.75,\"z\":0.0},\"state\":2}]},{\"cards\":[{\"value\":1,\"isFaceUp\":true,\"position\":{\"x\":0.17999982833862306,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":9,\"isFaceUp\":true,\"position\":{\"x\":0.17999982833862306,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":6,\"isFaceUp\":true,\"position\":{\"x\":0.18000006675720216,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":6,\"isFaceUp\":true,\"position\":{\"x\":0.18000006675720216,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":3,\"isFaceUp\":true,\"position\":{\"x\":0.18000006675720216,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":5,\"isFaceUp\":true,\"position\":{\"x\":0.18000006675720216,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":3,\"isFaceUp\":true,\"position\":{\"x\":-1.5,\"y\":-1.5,\"z\":0.0},\"state\":2},{\"value\":13,\"isFaceUp\":true,\"position\":{\"x\":0.18000006675720216,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":3,\"isFaceUp\":true,\"position\":{\"x\":-0.5,\"y\":-1.5,\"z\":0.0},\"state\":2},{\"value\":4,\"isFaceUp\":true,\"position\":{\"x\":0.18000000715255738,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1}]}],\"stockPileData\":[{\"value\":6,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":9,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":11,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":11,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":2,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":5,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":12,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":13,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":11,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":10,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":12,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":9,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":1,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":7,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":2,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":10,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":10,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":7,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":4,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0}]}";
        PlayerPrefs.SetString(SAVE_KEY, jsonData);
        PlayerPrefs.Save();
    }

    public static GameState LoadGame()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            jsonData =
                "{\"timer\":25.956680297851564,\"moves\":15,\"currentWasteCard\":{\"value\":12,\"isFaceUp\":true,\"position\":{\"x\":0.18000000715255738,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},\"boardStateData\":[{\"cards\":[{\"value\":2,\"isFaceUp\":false,\"position\":{\"x\":-3.75,\"y\":0.75,\"z\":0.0},\"state\":2},{\"value\":1,\"isFaceUp\":false,\"position\":{\"x\":-2.25,\"y\":0.75,\"z\":0.0},\"state\":2},{\"value\":13,\"isFaceUp\":false,\"position\":{\"x\":-0.75,\"y\":0.75,\"z\":0.0},\"state\":2}]},{\"cards\":[{\"value\":9,\"isFaceUp\":false,\"position\":{\"x\":-4.0,\"y\":0.0,\"z\":0.0},\"state\":2},{\"value\":1,\"isFaceUp\":false,\"position\":{\"x\":-3.5,\"y\":0.0,\"z\":0.0},\"state\":2},{\"value\":13,\"isFaceUp\":true,\"position\":{\"x\":0.18000006675720216,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":3,\"isFaceUp\":false,\"position\":{\"x\":-2.0,\"y\":0.0,\"z\":0.0},\"state\":2},{\"value\":2,\"isFaceUp\":false,\"position\":{\"x\":-1.0,\"y\":0.0,\"z\":0.0},\"state\":2},{\"value\":8,\"isFaceUp\":false,\"position\":{\"x\":-0.5,\"y\":0.0,\"z\":0.0},\"state\":2}]},{\"cards\":[{\"value\":9,\"isFaceUp\":true,\"position\":{\"x\":-4.25,\"y\":-0.75,\"z\":0.0},\"state\":2},{\"value\":6,\"isFaceUp\":true,\"position\":{\"x\":-3.75,\"y\":-0.75,\"z\":0.0},\"state\":2},{\"value\":6,\"isFaceUp\":true,\"position\":{\"x\":-3.25,\"y\":-0.75,\"z\":0.0},\"state\":2},{\"value\":1,\"isFaceUp\":true,\"position\":{\"x\":0.18000006675720216,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":6,\"isFaceUp\":true,\"position\":{\"x\":0.18000006675720216,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":10,\"isFaceUp\":true,\"position\":{\"x\":-1.75,\"y\":-0.75,\"z\":0.0},\"state\":2},{\"value\":8,\"isFaceUp\":true,\"position\":{\"x\":0.18000006675720216,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":5,\"isFaceUp\":true,\"position\":{\"x\":-0.75,\"y\":-0.75,\"z\":0.0},\"state\":2},{\"value\":3,\"isFaceUp\":true,\"position\":{\"x\":-0.25,\"y\":-0.75,\"z\":0.0},\"state\":2}]},{\"cards\":[{\"value\":3,\"isFaceUp\":true,\"position\":{\"x\":0.17999982833862306,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":4,\"isFaceUp\":true,\"position\":{\"x\":0.17999982833862306,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":7,\"isFaceUp\":true,\"position\":{\"x\":0.18000006675720216,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":2,\"isFaceUp\":true,\"position\":{\"x\":0.18000006675720216,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":9,\"isFaceUp\":true,\"position\":{\"x\":0.18000006675720216,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":9,\"isFaceUp\":true,\"position\":{\"x\":0.18000006675720216,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":10,\"isFaceUp\":true,\"position\":{\"x\":0.18000006675720216,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":8,\"isFaceUp\":true,\"position\":{\"x\":0.18000006675720216,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":5,\"isFaceUp\":true,\"position\":{\"x\":0.18000000715255738,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1},{\"value\":12,\"isFaceUp\":true,\"position\":{\"x\":0.18000000715255738,\"y\":-4.03000020980835,\"z\":0.0},\"state\":1}]}],\"stockPileData\":[{\"value\":4,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":12,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":2,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":5,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":8,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":10,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":4,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":11,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":1,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":3,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":10,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":11,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":6,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":4,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":12,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":13,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":7,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":11,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":11,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":12,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":7,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":13,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0},{\"value\":5,\"isFaceUp\":false,\"position\":{\"x\":-1.0199999809265137,\"y\":-4.039999961853027,\"z\":-1.0},\"state\":0}]}";
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