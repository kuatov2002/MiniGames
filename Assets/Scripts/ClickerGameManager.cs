// ClickerGameManager.cs - Manages the clicker game mechanics.
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickerGameManager : MonoBehaviour
{
    public int clickCount = 0;
    public UnityEngine.UI.Text clickCounterText;

    void Start()
    {
        LoadClickCount();
        UpdateClickCounterUI();
    }

    public void OnClick()
    {
        clickCount++;
        UpdateClickCounterUI();
        SaveClickCount();
    }

    void UpdateClickCounterUI()
    {
        clickCounterText.text = "Clicks: " + clickCount;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void SaveClickCount()
    {
        PlayerPrefs.SetInt("ClickCount", clickCount);
    }

    private void LoadClickCount()
    {
        clickCount = PlayerPrefs.GetInt("ClickCount", 0);
    }
}