using Unity.VisualScripting;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    
    [SerializeField] GameObject EnemySmall;
    [SerializeField] GameObject EnemyMedium;
    [SerializeField] GameObject EnemyLarge;
    [SerializeField] float SpawnPositionX = 0;
    [SerializeField] float SpawnPositionY = 0;
    [SerializeField] float SpawnRate = 5;
    private float cooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cooldown = SpawnRate;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 SpawnPosition = new Vector2(transform.position.x + SpawnPositionX, transform.position.y + SpawnPositionY);
        int chances = Random.Range(1, 6);
        GameObject SpawnedEnemy = EnemySmall;
        if (chances <= 3)
        {
            SpawnedEnemy = EnemySmall;
        }
        if (chances <= 5 && chances >= 4)
        {
            SpawnedEnemy = EnemySmall;
        }
        if (chances == 6)
        {
            SpawnedEnemy = EnemySmall;
        }

        cooldown -= Time.fixedDeltaTime;
        if (cooldown <= 0)
        {
            Instantiate(SpawnedEnemy, SpawnPosition, Quaternion.Euler(0f, 0f, 0f));
            cooldown = SpawnRate;
        }
        
    }
}
