/***********************************
 * Filename: TimeWarp
 * Author: Santiago Caprarulo
 * Description: Contains a cheat to speed up gametime by holding down x, z, and c
 * ********************************/
using UnityEngine;

public class TimeWarp : MonoBehaviour
{
    [SerializeField] float TimeWarpedGameSpeed = 10f;
    float baseGameSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseGameSpeed = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Do nothing if game is paused
        if (Time.timeScale == 0)
            return;
        // Check for input and speed up time if so
        if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.C))
        {
            Time.timeScale = TimeWarpedGameSpeed;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
