/********************************************
 * Filename: DestroyAudioOnCompletion.cs
 * Author: Santiago Caprarulo
 * Description: Destroys an audio source on it's completion
 * 
 * *****************************************/

using UnityEngine;

public class DestroyAudioOnCompletion : MonoBehaviour
{
    AudioSource refSource;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        refSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if it's done
        if (!refSource.isPlaying)
            // Destroy the game object if so
            Destroy(gameObject);
    }
}
