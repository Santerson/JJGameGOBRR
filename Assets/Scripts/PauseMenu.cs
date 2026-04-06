/***************************
 * Filename: pausemenu.cs
 * Author: Santiago Caprarulo
 * Description: A small script that will pause the game
 * if the escape key is pressed
 * ************************/
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] AudioSource pauseSound;
    public static bool paused { get; private set; } = false;
    // Update is called once per frame
    void Update()
    {
        // Check if the pause key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangePauseStatus();
        }
    }

    public void ChangePauseStatus()
    {
        // Pause or unpause the game
        paused = !paused;
        Time.timeScale = (paused ? 0.0f : 1.0f);
        // Enable or disable the pause menu
        pauseMenu.SetActive(paused);
        pauseSound.Play();
    }
}
