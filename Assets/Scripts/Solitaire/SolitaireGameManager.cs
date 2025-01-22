using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SolitaireGameManager : MonoBehaviour
{
    public int moveCount = 0;
    public UnityEngine.UI.Text moveCounterText;
    public UnityEngine.UI.Text timerText;

    private float timer = 0f;
    private bool isGameRunning = true;

    void Start()
    {
        InvokeRepeating(nameof(UpdateTimer), 1f, 1f);
        UpdateMoveCounterUI();
    }

    void UpdateTimer()
    {
        if (isGameRunning)
        {
            timer++;
            timerText.text = "Time: " + timer.ToString("F0") + "s";
        }
    }

    public void MakeMove()
    {
        moveCount++;
        UpdateMoveCounterUI();
    }

    void UpdateMoveCounterUI()
    {
        moveCounterText.text = "Moves: " + moveCount;
    }

    public void LoadMainMenu()
    {
        isGameRunning = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadClickerGame()
    {
        isGameRunning = false;
        SceneManager.LoadScene("ClickerGame");
    }
}