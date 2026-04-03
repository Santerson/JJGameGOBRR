using System.Collections.Generic; /* List<T> */
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] GameObject GuyWeak;
    [SerializeField] GameObject GuyNormal;
    [SerializeField] GameObject GuyBuff;
    [SerializeField] GameObject MechWeak;
    [SerializeField] GameObject MechNormal;
    [SerializeField] GameObject MechBuff;
    [SerializeField] TextMeshProUGUI stageText;

    [Header("Spawn Ranges")]
    [SerializeField] List<Vector2> SpawnPositions;
    [SerializeField] int Min;
    [SerializeField] int Max;


    [Header("SpawnRate")]
    [SerializeField] float SpawnRate = 5;
    [SerializeField] float SpawnRateReductionPerEnemy = 0.1f;
    [SerializeField] float MinimumSpawnRate = 2f;
    [SerializeField] float SpawnBurstChance = .1f;
    [Tooltip("The spawn range of a burst. With x being low (inclusive) and y being high (not inclusive)")]
    [SerializeField] Vector2Int SpawnBurstRange = new Vector2Int(2, 5);
    [SerializeField] float CooldownAfterBurst = 10f;

    [Header("Spawn chances, check guy weak for tooltip")]
    [Tooltip("Where the index in the list is the stage and a random number inbetween 0 and 1 must be rolled. " +
        "A number below the given number will have that enemy spawned. It will check from weakguy to buff mech in the order." +
        " If one criteria is met, none other will be checked.")]
    [SerializeField] float[] GuyWeakSpawnChances;
    [SerializeField] float[] GuyNormalSpawnChances;
    [SerializeField] float[] GuyBuffSpawnChances;
    [SerializeField] float[] MechWeakSpawnChances;
    [SerializeField] float[] MechNormalSpawnChances;
    [SerializeField] float[] MechBuffSpawnChances;

    [Tooltip("Note: Every spawn range should have the same amount of quantity")]
    [SerializeField] uint[] StageEnemyCount = new uint[] { 10, 100 };

    private float cooldown;
    AudioManager refAudioManager;
    float currentSpawnRate = 5;
    uint currentSpawns = 0;
    uint spawnsInStage = 0;
    uint stage = 0;
    public bool EnemiesSpawning = true;

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
        refAudioManager = FindFirstObjectByType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!EnemiesSpawning) return;
        // Reduce cooldown
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            HandleEnemySpawnLogic();
        }
        stageText.text = $"{stage : 0}";
    }

    /// <summary>
    /// Handles all logic for spawning enemies
    /// </summary>
    void HandleEnemySpawnLogic()
    {
        // Check for a burst
        int quantity = 1;
        if (Random.Range(0f, 1f) < SpawnBurstChance)
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
        // Get the range for the current stage
        GameObject SpawnedEnemy = GuyWeak;
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber < GuyWeakSpawnChances[stage])
            SpawnedEnemy = GuyWeak;
        else
        {
            randomNumber -= GuyWeakSpawnChances[stage];
            if (randomNumber < GuyNormalSpawnChances[stage])
                SpawnedEnemy = GuyNormal;
            else
            {
                randomNumber -= GuyNormalSpawnChances[stage];
                if (randomNumber < GuyBuffSpawnChances[stage])
                    SpawnedEnemy = GuyBuff;
                else
                {
                    randomNumber -= GuyBuffSpawnChances[stage];
                    if (randomNumber < MechWeakSpawnChances[stage])
                        SpawnedEnemy = MechWeak;
                    else
                    {
                        randomNumber -= MechWeakSpawnChances[stage];
                        if (randomNumber < MechNormalSpawnChances[stage])
                            SpawnedEnemy = MechNormal;
                        else
                        {
                            SpawnedEnemy = MechBuff;
                        }
                    }
                }
            }
        } 

        // Get a random spawn position
        int selectedSpawnLocation = Random.Range(0, SpawnPositions.Count);
        SpawnEnemy(SpawnedEnemy, selectedSpawnLocation);
    }

    /// <summary>
    /// Spawns an enemy in a given lane
    /// </summary>
    /// <param name="spawnedEnemy">A gameobject for the enemy</param>
    /// <param name="selectedSpawnLocation">The lane the enemy should spawn in</param>
    /// <param name="countAsSpawn">Whether or not this should count to make the game more difficult</param>
    public GameObject SpawnEnemy(GameObject spawnedEnemy, int selectedSpawnLocation, bool countAsSpawn = true)
    {
        // Instantiate an enemy at that position
        Vector2 spawnPos = SpawnPositions[selectedSpawnLocation];
        GameObject spawndenemy = Instantiate(spawnedEnemy, spawnPos, Quaternion.identity);
        spawndenemy.GetComponent<EnemyAi>().lane = selectedSpawnLocation;
        FindFirstObjectByType<LaneCheck>().Laneincress(selectedSpawnLocation);
        if (countAsSpawn)
        {
            currentSpawns++;
            spawnsInStage++;
            if (spawnsInStage > StageEnemyCount[stage])
            {
                IncreaseStage();
                spawnsInStage = 0;
            }
        }
        return spawndenemy;
    }

    void IncreaseStage()
    {
        stage++;
        if (stage >= StageEnemyCount.Length)
        {
            refAudioManager.PlayWinLevelSFX();
            UnityEngine.SceneManagement.SceneManager.LoadScene("EpicDub");
        }
    }
}
