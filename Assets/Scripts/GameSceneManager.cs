/********************************************
 * filename: GameSceneManager.cs
 * Author: Santiago Caprarulo
 * Description: Contains functions that can be called from gameobjects
 * to change scenes in runtime
 * ******************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    /// <summary>
    /// Loads the game scene (or scene named "GameScene")
    /// </summary>
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// Loads the main menu (or scene named "MainMenu")
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
