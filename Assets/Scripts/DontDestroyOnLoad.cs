/********************************
 * Filename: DontDestroyOnLoad.cs
 * Author: Santiago Caprarulo
 * Description: Allows a gameobject to persist through
 * scene transitions, including it's children. Only
 * One of these objects may exist EVER.
 * *****************************/

using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Check if other objects exist
        DontDestroyOnLoad[] objs = FindObjectsByType<DontDestroyOnLoad>(FindObjectsSortMode.None);
        if (objs.Length > 1)
        {   
            // Destroy itself if so
            Destroy(gameObject);
            return;
        }
        // Otherwise, never destroy it
        DontDestroyOnLoad(gameObject);
    }
}
