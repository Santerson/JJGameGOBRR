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
    [SerializeField] float SpawnRate = 5;
    [SerializeField] float rateOfIncresseExponetel = 0;
    [SerializeField] float rateOfIncresselinerer = 0;
    [SerializeField] bool TrueExponetelFalseLiner;

    private float cooldown;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TrueExponetelFalseLiner)
        {
            cooldown /= rateOfIncresseExponetel;
        }
        else
        {
            cooldown -= rateOfIncresselinerer;
        }
        
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

        cooldown -= Time.fixedDeltaTime;
        if (cooldown <= 0)
        {
            SpawnEnemy(SpawnedEnemy);
            cooldown = SpawnRate;
        }
        
    }

    /// <summary>
    /// Spawns an enemy at one of the positions given by the SpawnPositions array
    /// </summary>
    /// <param name="SpawnedEnemy">The enemy to be spawned</param>
    void SpawnEnemy(GameObject SpawnedEnemy)
    {
        // Get a random spawn position
        int selectedSpawnLocation = Random.Range(0, SpawnPositions.Count);
        Vector2 spawnPos = SpawnPositions[selectedSpawnLocation];
        // Instantiate an enemy at that position
        Instantiate(SpawnedEnemy, spawnPos, Quaternion.identity);
    }
}
