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

    AudioManager RefAudioManager;
    public static bool paused { get; private set; } = false;

    private void Start()
    {
        RefAudioManager = FindFirstObjectByType<AudioManager>();
        paused = false;
        if (RefAudioManager == null)
            Debug.LogWarning("No audio manager found! If this is started from not the main menu, this is okay! Some audio bugs might occur");
    }

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
        RefAudioManager.MuffleorUnmuffleMusic(paused);
    }
}
