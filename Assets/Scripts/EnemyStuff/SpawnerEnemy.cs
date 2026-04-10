/**********************************************
 * Filename: SpawnerEnemy
 * Author: Micaiah Mariano, Santiago Caprarulo
 * Description: Handles all enemy spawn logic
 * *******************************************/

using System.Collections.Generic; /* List<T> */
using TMPro;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] TextMeshProUGUI stageText;

    [Header("Spawn Ranges")]
    [SerializeField] List<Vector2> SpawnPositions;

    [Header("Enemy Spawns")]
    [Tooltip("Nodes consisting of enemy wave data. Refer to the EnemyNode struct for more info")]
    [SerializeField] List<EnemyNode> EnemyNodes;

    // Whether or not enemies are currently spawning in
    [HideInInspector] public bool EnemiesSpawning = true;
    /// <summary>
    /// The amount of time until the next enemy spawns in
    /// </summary>
    private float cooldownToNextEnemy;
    LaneCheck refLaneCheck = null;
    // The current stage of the player
    uint stage = 0;

    // If all the monsters in the current wave have been spawned
    bool allMobsInWaveSpawned = false;
    bool EndOfGameCheckedThisWave = false;

    /// <summary>
    /// A struct containing data for spawning enemies
    /// </summary>
    [System.Serializable]
    struct EnemyNode
    {
        [Tooltip("The enemies spawning in the current wave")]
        [SerializeField] public List<GameObject> Enemies;
        [Tooltip("The time in-between each enemy in the wave")]
        [SerializeField] public float TimeInbetweenSpawns;
        [Tooltip("The time after all enemies have been DEFEATED before the next wave starts")]
        [SerializeField] public float WaitTimeAfterLastSpawn;
        [Tooltip("Whether or not this wave is considered a hard wave (WIP)")]
        [SerializeField] public bool IsHardWave;
    }


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
        refLaneCheck = FindAnyObjectByType<LaneCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        // Do nothing if enemies are not spawning (IE tutorial)
        if (!EnemiesSpawning) return;
        // Check if all enemies have not been spawned
        if (!allMobsInWaveSpawned)
        {
            // Otherwise, check reduce cooldown to next enemy and spawn one if it should be spawned
            cooldownToNextEnemy -= Time.deltaTime;
            if (cooldownToNextEnemy <= 0)
            {
                // Check if the player has reached the end of the game
                if (!EndOfGameCheckedThisWave)
                {
                    // Increase the wave count
                    stage++;
                    stageText.text = $"{stage}";
                    if (!CheckIfGameWin()) // CheckIfGameWin will change scenes if the game is won
                        EndOfGameCheckedThisWave = true;
                    else
                        return;
                }
                SpawnEnemy();
            }
        }
        // Otherwise, check if all lanes are empty
        else
        {
            if (refLaneCheck.Lane0 == 0 && refLaneCheck.Lane1 == 0 && refLaneCheck.Lane2 == 0)
            {
                // Restart the cooldown
                allMobsInWaveSpawned = false;
                EndOfGameCheckedThisWave = false;
            }
        }
        // Update stage text
        stageText.text = $"{stage : 0}";
    }

    /// <summary>
    /// Spawns an enemy at one of the positions given by the SpawnPositions array
    /// </summary>
    void SpawnEnemy()
    {
        // Get a random spawn position
        int selectedSpawnLocation = Random.Range(0, SpawnPositions.Count);

        // TODO: create a safety so not >2 enemies spawn on the same lane

        // Get the next enemy in the spawnqueue
        EnemyNode currNode = EnemyNodes[0];
        GameObject SpawnedEnemy = currNode.Enemies[0];
        // Spawn that enemy
        SpawnEnemy(SpawnedEnemy, selectedSpawnLocation);
        // Remove that enemy from the spawn queue
        currNode.Enemies.RemoveAt(0);
        // Remove the node if the node is clear
        if (currNode.Enemies.Count == 0)
        {
            EnemyNodes.RemoveAt(0);
            allMobsInWaveSpawned = true;
            cooldownToNextEnemy = currNode.WaitTimeAfterLastSpawn;
        }
        // Otherwise, wait a bit and spawn another
        else
        {
            cooldownToNextEnemy = currNode.TimeInbetweenSpawns;
        }
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
        refLaneCheck.Laneincress(selectedSpawnLocation);
        return spawndenemy;
    }

    /// <summary>
    /// Runs logic to check if the game has been won
    /// <returns>True if the game is won, false otherwise</returns>
    /// </summary>
    bool CheckIfGameWin()
    {
        if (EnemyNodes.Count == 0)
        {
            // Win game epic dub yippe
            UnityEngine.SceneManagement.SceneManager.LoadScene("EpicDub");
            FindFirstObjectByType<AudioManager>().PlayMenuMusic();
            return true;
        }
        return false;
    }
}
