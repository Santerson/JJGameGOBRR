using UnityEngine;

public class ColliderForLane : MonoBehaviour
{
    public bool EnemiesPresent { get; private set; } = false;
    bool EnemyFoundLastFrame = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!EnemyFoundLastFrame)
            EnemiesPresent = false;
        else
            EnemyFoundLastFrame = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemiesPresent = true;
            EnemyFoundLastFrame = true;
        }
    }
}
