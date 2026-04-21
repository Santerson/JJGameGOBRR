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
    
    public void TryCallMainMenu()
    {
        if (audioManager == null) return;
        audioManager.PlayMenuMusic();
    }

    public void TryCallLose()
    {
        if (audioManager == null) return;
        audioManager.PlayLoseLevelMusic();
    }

    public void TryCallWin()
    {
        if (audioManager == null) return;
        audioManager.PlayWinLevelMusic();
    }

    public void TryCallBattle(int type = 1)
    {
        if (audioManager == null) return;
        audioManager.PlayGameMusic(type);
    }

    public void TryCallButtonClickSFX()
    {
        if (audioManager == null) return;
        audioManager.PlayClickSFX();
    }
}
