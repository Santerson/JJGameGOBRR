/*************************
 * Filename: PlayClickSound.cs
 * Author: Santiago Caprarulo
 * Description: Plays a click ui sound when the 
 * function is called (even if the object was not
 * defined on the scene)
 * *************************/
using UnityEngine;

public class PlayClickSound : MonoBehaviour
{
    AudioManager refAudioManager;
    private void Start()
    {
        refAudioManager = FindFirstObjectByType<AudioManager>();
    }

    /// <summary>
    /// tries to play a click sound
    /// </summary>
    public void PlaySound()
    {
        refAudioManager?.PlayClickSFX();
    }
}
