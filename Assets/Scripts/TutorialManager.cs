using UnityEngine;
using System.Collections; /* IEnumerator */


public class TutorialManager : MonoBehaviour
{
    [SerializeField] bool DoTutorial = true;

    [Header("Tutorial Time Delays")]
    [SerializeField] float waitTime1 = 3f;
    [SerializeField] float waitTime2 = 2f;

    [Header("Tutorial Gameobjects")]
    [SerializeField] GameObject tutorial1;
    [SerializeField] GameObject tutorial2;
    [SerializeField] GameObject tutorialEnemy;
    [SerializeField] GameObject tutorial3;
    [SerializeField] GameObject tutorial4;

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
        GameObject refenemy = refEnemySpawner.SpawnEnemy(tutorialEnemy, 1);
        tutorial2.SetActive(true);
        yield return new WaitForSeconds(waitTime2);

        // Disable the second text
        tutorial2.SetActive(false);
        // Stop the enemy
        Rigidbody2D enemyRB = refenemy.GetComponent<Rigidbody2D>();
        float enemySpeed = enemyRB.linearVelocityX;
        enemyRB.linearVelocityX = 0;
        // Show the third text box
    }
}
