// MenuManager.cs - Handles the main menu functionality.
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadClickerGame()
    {
        SceneManager.LoadScene("ClickerGame");
    }

    public void LoadSolitaireGame()
    {
        SceneManager.LoadScene("SolitaireGame");
    }
}