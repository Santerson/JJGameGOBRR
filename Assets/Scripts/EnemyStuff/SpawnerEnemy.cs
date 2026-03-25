using Unity.VisualScripting;
using System.Collections.Generic; /* List<T> */
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{

    [SerializeField] GameObject EnemySmall;
    [SerializeField] GameObject EnemyMedium;
    [SerializeField] GameObject EnemyLarge;
    [SerializeField] List<Vector2> SpawnPositions;
    [SerializeField] int Min;
    [SerializeField] int Max;
    [Tooltip("Roll below this is small")]
    [SerializeField] int SamllLess;
    [Tooltip("Roll below this is medium")]
    [SerializeField] int MediumLess;
    [Tooltip("Roll above this is large")]
    [SerializeField] int LargeGreater;

    [Header("SpawnRate")]
    [SerializeField] float SpawnRate = 5;
    [SerializeField] float SpawnRateReductionPerEnemy = 0.1f;
    [SerializeField] float MinimumSpawnRate = 2f;
    [SerializeField] float SpawnBurstChance = .1f;
    [Tooltip("The spawn range of a burst. With x being low (inclusive) and y being high (not inclusive)")]
    [SerializeField] Vector2Int SpawnBurstRange = new Vector2Int(2, 5);
    [SerializeField] float CooldownAfterBurst = 10f;

    private float cooldown;
    float currentSpawnRate = 5;

    private void OnDrawGizmosSelected()
    {
        for(int i = 0; i < SpawnPositions.Count; i++)
        {
            Vector2 spawnPos = SpawnPositions[i];
            Debug.DrawLine(new Vector2(spawnPos.x - 0.1f, spawnPos.y), new Vector2(spawnPos.x + 0.1f, spawnPos.y), Color.red);
            Debug.DrawLine(new Vector2(spawnPos.x, spawnPos.y - 0.1f), new Vector2(spawnPos.x, spawnPos.y + 0.1f), Color.red);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cooldown = SpawnRate;
        currentSpawnRate = SpawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        // Reduce cooldown
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            HandleEnemySpawnLogic();
        }
    }

    /// <summary>
    /// Handles all logic for spawning enemies
    /// </summary>
    void HandleEnemySpawnLogic()
    {
        // Check for a burst
        int quantity = 1;
        if (Random.Range(0, 1) > SpawnBurstChance)
        {
            // Increase the spawn quantity if it is a burst
            quantity = Random.Range(SpawnBurstRange.x, SpawnBurstRange.y);
        }
        // Spawn the amount of enemies
        for (int i = 0; i < quantity; i++)
        {
            SpawnEnemy();
        }
        // Decrease the spawn rate
        currentSpawnRate = currentSpawnRate - SpawnRateReductionPerEnemy < MinimumSpawnRate ? MinimumSpawnRate : currentSpawnRate - SpawnRateReductionPerEnemy;
        // Increase the cooldown
        cooldown = quantity > 1 ? CooldownAfterBurst : currentSpawnRate;
    }

    /// <summary>
    /// Spawns an enemy at one of the positions given by the SpawnPositions array
    /// </summary>
    /// <param name="SpawnedEnemy">The enemy to be spawned</param>
    void SpawnEnemy()
    {
        int chances = Random.Range(Min, Max);
        GameObject SpawnedEnemy = EnemySmall;
        if (chances <= SamllLess)
        {
            SpawnedEnemy = EnemySmall;
        }
        else if (chances <= MediumLess)
        {
            SpawnedEnemy = EnemyMedium;
        }
        else if (chances >= LargeGreater)
        {
            SpawnedEnemy = EnemyLarge;
        }

        // Get a random spawn position
        int selectedSpawnLocation = Random.Range(0, SpawnPositions.Count);
        Vector2 spawnPos = SpawnPositions[selectedSpawnLocation];
        // Instantiate an enemy at that position
        Instantiate(SpawnedEnemy, spawnPos, Quaternion.identity);
    }
}
