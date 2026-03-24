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
    public void PlaySound()
    {
        GameObject refObj = GameObject.Find("UIButtonClickSFX");
        if (refObj != null)
        {
            refObj.GetComponent<AudioSource>().Play();
        }
        else
        {
            Debug.LogWarning("Attempted to play a ui click sound but could not find a click sound. If this has not run from the starting scene, this is okay!");
        }
    }
}
