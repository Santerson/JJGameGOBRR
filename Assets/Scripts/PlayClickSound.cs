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

    public void PlaySound()
    {
        refAudioManager.PlayClickSFX();
    }
}
