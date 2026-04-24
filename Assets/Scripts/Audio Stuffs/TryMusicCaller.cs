/***********************************
 * Filename: TryMusicCaller
 * Author: Santiago Caprarulo
 * Description: Contains functions to try and call certain sound effects.
 * This is due to AudioManager only existing on the main menu
 * ********************************/

using UnityEngine;
using static AudioManager;

public class TryCallAudioEfxs : MonoBehaviour
{
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            Debug.LogWarning("No Audiomanager found. If the main menu has not occured, this is okay! No Music will play!");
    }
    
    /// <summary>
    /// Tries to play the main menu music
    /// </summary>
    public void TryCallMainMenu()
    {
        if (audioManager == null) return;
        audioManager.PlayMenuMusic();
    }

    /// <summary>
    /// Tries to play the lose level music
    /// </summary>
    public void TryCallLose()
    {
        if (audioManager == null) return;
        audioManager.PlayLoseLevelMusic();
    }

    /// <summary>
    /// Tries to play the win level music
    /// </summary>
    public void TryCallWin()
    {
        if (audioManager == null) return;
        audioManager.PlayWinLevelMusic();
    }

    /// <summary>
    /// Tries to call the battle music
    /// </summary>
    /// <param name="type">The intensity of the battle</param>
    public void TryCallBattle(int type = 1)
    {
        if (audioManager == null) return;
        audioManager.PlayGameMusic(type);
    }

    /// <summary>
    /// Tries to call the button click sfx
    /// </summary>
    public void TryCallButtonClickSFX()
    {
        if (audioManager == null) return;
        audioManager.PlayClickSFX();
    }
}
