using UnityEngine;
using System.Collections; /* IEnumerator */


public class TutorialManager : MonoBehaviour
{
    [SerializeField] bool DoTutorial = true;

    [Header("Tutorial Time Delays")]
    [SerializeField] float waitTime1 = 3f;

    [Header("Tutorial Gameobjects")]
    [SerializeField] GameObject tutorial1;
    [SerializeField] GameObject tutorial2;
    [SerializeField] GameObject tutorialEnemy;

    SpawnerEnemy refEnemySpawner;

    public bool IsTutorialing { get; private set; } = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        refEnemySpawner = FindFirstObjectByType<SpawnerEnemy>();
        if (refEnemySpawner == null)
        {
            Debug.LogError("NO enemy spawner found!");
        }
        if (DoTutorial) StartCoroutine(PlayTutorial());
    }

    IEnumerator PlayTutorial()
    {
        // Stop spawns
        refEnemySpawner.EnemiesSpawning = false;
        
        // Enable first text
        tutorial1.SetActive(true);
        yield return new WaitForSeconds(waitTime1);
        // Disable first text
        tutorial1.SetActive(false);

        // Spawn an enemy and enable second text
        refEnemySpawner.SpawnEnemy(tutorialEnemy, 1);
    }
}
