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
        if (Time.timeScale == 0)
            return;
        if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.C))
        {
            Time.timeScale = TimeWarpedGameSpeed;
        }
        else
        {
            if (Time.timeScale != baseGameSpeed && Time.timeScale != TimeWarpedGameSpeed)
                baseGameSpeed = Time.timeScale;
            Time.timeScale = baseGameSpeed;
        }
    }
}
