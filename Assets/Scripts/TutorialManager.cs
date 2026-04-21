/************************************
 * Filename: TutorialManager.cs
 * Author: Santiago Caprarulo
 * Description: Performs a tutorial at the start
 * of the game
 * 
 * Note: This script sucked to make and sucks
 *       to look at :/
 * ********************************/

using UnityEngine;
using System.Collections; /* IEnumerator */


public class TutorialManager : MonoBehaviour
{
    [SerializeField] bool DoTutorial = true;

    [Header("I'm sorry in advance")]

    [SerializeField] KeyCode ContinueButton = KeyCode.Mouse0;
    [SerializeField] KeyCode SkipButton = KeyCode.Mouse1;

    [Header("Tutorial Time Delays")]
    [SerializeField] float waitTime2 = 2f;
    [SerializeField] float waitTime5 = 4f; 
    [SerializeField] float waitTime7 = 2f;
    [SerializeField] float waitTime8 = 4f;
    [SerializeField] float waitTimeAfter9 = 2f;

    [Header("Tutorial Textbox Gameobjects")]
    [SerializeField] GameObject tutorial1;
    [SerializeField] GameObject tutorial2;
    [SerializeField] GameObject tutorialEnemy;
    [SerializeField] GameObject tutorial3;
    [SerializeField] GameObject tutorial4;
    [SerializeField] GameObject tutorial5;
    [SerializeField] GameObject tutorial6;
    [SerializeField] GameObject tutorial7;
    [SerializeField] GameObject tutorial8;
    [SerializeField] GameObject tutorial9;
    [SerializeField] GameObject tutorial10;
    [SerializeField] GameObject tutorial11;
    [SerializeField] GameObject tutorial12;
    [SerializeField] GameObject tutorial13;
    [SerializeField] GameObject tutorial14;
    [SerializeField] GameObject tutorial15;
    [SerializeField] GameObject tutorial16;
    [SerializeField] GameObject tutorial17;
    [SerializeField] GameObject tutorial13_2;

    SpawnerEnemy refEnemySpawner;
    static bool tutorialOccured = false;

    public bool IsTutorialing { get; private set; } = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        refEnemySpawner = FindFirstObjectByType<SpawnerEnemy>();
        if (refEnemySpawner == null)
        {
            Debug.LogError("NO enemy spawner found!");
        }
        if (DoTutorial && !tutorialOccured) StartCoroutine(PlayTutorial());
    }

    /// <summary>
    /// Runs tutorial logic, alongside waiting for players to perform inputs
    /// </summary>
    IEnumerator PlayTutorial()
    {
        tutorialOccured = true;
        // Stop spawns
        refEnemySpawner.EnemiesSpawning = false;
        // Disable towerdragging
        UIDraggableTower[] towerProfiles = FindObjectsByType<UIDraggableTower>(FindObjectsSortMode.None);
        foreach (UIDraggableTower towerProfile in towerProfiles)
        {
            towerProfile.CanDrag = false;
        }

        // Enable first text
        tutorial1.SetActive(true);
        // Wait
        while (!Input.GetKey(ContinueButton))
        {
            if (Input.GetKey(SkipButton))
            {
                // Restart the game and keep it going
                foreach (UIDraggableTower towerProfile in towerProfiles)
                {
                    towerProfile.CanDrag = true;
                }
                refEnemySpawner.EnemiesSpawning = true;
                tutorial1.SetActive(false);
                tutorialOccured = true;
                IsTutorialing = false;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
        while (Input.GetKey(ContinueButton))
        {
            if (Input.GetKey(SkipButton))
            {
                // Restart the game and keep it going
                foreach (UIDraggableTower towerProfile in towerProfiles)
                {
                    towerProfile.CanDrag = true;
                }
                refEnemySpawner.EnemiesSpawning = true;
                tutorial1.SetActive(false);
                tutorialOccured = true;
                IsTutorialing = false;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
        // Disable first text
        tutorial1.SetActive(false);

        // Spawn an enemy and enable second text
        GameObject refenemy = refEnemySpawner.SpawnEnemy(tutorialEnemy, 1);
        tutorial2.SetActive(true);
        yield return new WaitForSeconds(waitTime2);

        // Disable the second text
        tutorial2.SetActive(false);
        // Stop the enemy
        EnemyAi refEnemy = refenemy.GetComponent<EnemyAi>();
        refEnemy.StoppedEnemy = true;
        // Show the third text box
        tutorial3.SetActive(true);
        UIDraggableTower mushManPortrait = null;

        // Enable dragging for the mushman
        foreach (UIDraggableTower towerProfile in towerProfiles)
        {
            // Check if it would instantiate a musman
            if (towerProfile.TowerPrefab.GetComponent<TowerAi>().TowerID == AudioManager.Towers.mushman)
            {
                // Allow dragging for it
                towerProfile.CanDrag = true;
                mushManPortrait = towerProfile;
                break;
            }
        }
        // Force the next tower to be the mushman at 1, 1
        mushManPortrait.ForceNextPositionTower(AudioManager.Towers.mushman, new Vector2Int(1, 1));
        // Wait for the player to start dragging the tower
        while (!mushManPortrait.followingMouse)
        {
            yield return new WaitForEndOfFrame();
        }
        // Go to the next tutorial object
        tutorial3.SetActive(false);
        
        tutorial4.SetActive(true);
        // Wait for the player to put down the mushman there
        while (UIDraggableTower.forcedNextTowerPosition != new Vector2Int(-1, -1))
        {
            yield return new WaitForEndOfFrame();
        }
        // Continue time
        tutorial4.SetActive(false);
        refEnemy.StoppedEnemy = false;
        mushManPortrait.CanDrag = false;
        tutorial5.SetActive(true);
        TowerAi[] towers = FindObjectsByType<TowerAi>(FindObjectsSortMode.None);
        foreach (TowerAi tower in towers)
        {
            tower.canBeSold = false;
        }
        // Wait for a bit
        yield return new WaitForSeconds(waitTime5);
        // Stop time a bit
        tutorial5.SetActive(false);
        refEnemy.StoppedEnemy = true;
        TowerAi PlacedMushMan1 = FindFirstObjectByType<TowerAi>();
        PlacedMushMan1.TowerAttacking = false;
        mushManPortrait.CanDrag = true;
        // Make the player place another mushman down
        tutorial6.SetActive(true);
        mushManPortrait.ForceNextPositionTower(AudioManager.Towers.mushman, new Vector2Int(0, 1));
        while (UIDraggableTower.forcedNextTowerPosition != new Vector2Int(-1, -1))
        {
            yield return new WaitForEndOfFrame();
        }
        
        tutorial6.SetActive(false);
        refEnemy.StoppedEnemy = false;
        PlacedMushMan1.TowerAttacking = true;
        mushManPortrait.CanDrag = false;
        towers = FindObjectsByType<TowerAi>(FindObjectsSortMode.None);
        foreach (TowerAi tower in towers)
        {
            tower.canBeSold = false;
        }
        // Wait for the enemies to destroy the enemy
        tutorial7.SetActive(true);
        yield return new WaitForSeconds(waitTime7);
        // Spawn the next enemy
        tutorial7.SetActive(false);
        tutorial8.SetActive(true);
        refEnemySpawner.SpawnEnemy(tutorialEnemy, 0, false);
        yield return new WaitForSeconds(waitTime8);
        refEnemy = FindFirstObjectByType<EnemyAi>();
        refEnemy.StoppedEnemy = true;
        tutorial8.SetActive(false);
        // Make the player drop the fairy
        tutorial9.SetActive(true);
        // Enable dragging for the fairy
        UIDraggableTower fairyPortrait = null;
        foreach (UIDraggableTower towerProfile in towerProfiles)
        {
            // Check if it would instantiate a musman
            if (towerProfile.TowerPrefab.GetComponent<TowerAi>().TowerID == AudioManager.Towers.fairy)
            {
                // Allow dragging for it
                towerProfile.CanDrag = true;
                fairyPortrait = towerProfile;
                break;
            }
        }
        // Force the next position
        fairyPortrait.ForceNextPositionTower(AudioManager.Towers.fairy, new Vector2Int(2, 2));
        // wait
        while (UIDraggableTower.forcedNextTowerPosition != new Vector2Int(-1, -1))
        {
            yield return new WaitForEndOfFrame();
        }
        // Continue Time
        tutorial9.SetActive(false);
        fairyPortrait.CanDrag = false;
        refEnemy.StoppedEnemy = false;
        towers = FindObjectsByType<TowerAi>(FindObjectsSortMode.None);
        foreach (TowerAi tower in towers)
        {
            tower.canBeSold = false;
        }
        // Stop for a bit
        yield return new WaitForSeconds(waitTimeAfter9);
        // Show the next text box
        tutorial10.SetActive(true);
        Mana manaBar = FindFirstObjectByType<Mana>();
        while (!Input.GetKey(ContinueButton))
        {
            if (manaBar.CurrentMana <= 3)
                manaBar.ManaDraining = false;
            yield return new WaitForEndOfFrame();
        }
        while (Input.GetKey(ContinueButton))
        {
            if (manaBar.CurrentMana <= 3)
                manaBar.ManaDraining = false;
            yield return new WaitForEndOfFrame();
        }
        // Freeze the whole game
        manaBar.ManaDraining = false;
        tutorial10.SetActive(false);
        tutorial11.SetActive(true);
        while (!Input.GetKey(ContinueButton))
            yield return new WaitForEndOfFrame();
        while (Input.GetKey(ContinueButton))
            yield return new WaitForEndOfFrame();
        tutorial11.SetActive(false);
        // Do stuff to make it so you can only sell the mushman towers
        towers = FindObjectsByType<TowerAi>(FindObjectsSortMode.None);
        foreach (TowerAi tower in towers)
        {
            if (tower.TowerID == AudioManager.Towers.mushman)
            {
                tower.canBeSold = true;
            }
        }
        tutorial12.SetActive(true);
        // freeze time until the player does the thing again
        while (towers.Length != 1)
        {
            towers = FindObjectsByType<TowerAi>(FindObjectsSortMode.None);
            yield return new WaitForEndOfFrame();
        }
        tutorial12.SetActive(false);
        manaBar.ManaDraining = true;
        tutorial13.SetActive(true);
        while (!Input.GetKey(ContinueButton))
            yield return new WaitForEndOfFrame();
        while (Input.GetKey(ContinueButton))
            yield return new WaitForEndOfFrame();
        tutorial13.SetActive(false);
        // New Tutorial about moving towers
        tutorial13_2.SetActive(true);
        while (!Input.GetKey(ContinueButton))
            yield return new WaitForEndOfFrame();
        while (Input.GetKey(ContinueButton))
            yield return new WaitForEndOfFrame();
        tutorial13_2.SetActive(false);
        // Force the next tower to be the bunny
        UIDraggableTower bunnyPortrait = null;
        foreach (UIDraggableTower towerProfile in towerProfiles)
        {
            // Check if it would instantiate a musman
            if (towerProfile.TowerPrefab.GetComponent<TowerAi>().TowerID == AudioManager.Towers.bunny)
            {
                // Allow dragging for it
                towerProfile.CanDrag = true;
                bunnyPortrait = towerProfile;
                break;
            }
        }
        bunnyPortrait.ForceNextPositionTower(AudioManager.Towers.bunny, new Vector2Int(0, 2));
        tutorial14.SetActive(true);
        // Wait until the next tower is the bunny
        while (UIDraggableTower.forcedNextTowerPosition != new Vector2Int(-1, -1))
        {
            yield return new WaitForEndOfFrame();
        }
        // Disable placement for the bunny
        foreach (UIDraggableTower towerProfile in towerProfiles)
        {
            // Check if it would instantiate a bunny
            if (towerProfile.TowerPrefab.GetComponent<TowerAi>().TowerID == AudioManager.Towers.bunny)
            {
                // Allow dragging for it
                towerProfile.CanDrag = false;
                bunnyPortrait = towerProfile;
                break;
            }
        }
        tutorial14.SetActive(false);
        tutorial15.SetActive(true);
        while (!Input.GetKey(ContinueButton))
            yield return new WaitForEndOfFrame();
        while (Input.GetKey(ContinueButton))
            yield return new WaitForEndOfFrame();
        tutorial15.SetActive(false);
        tutorial16.SetActive(true);
        while (!Input.GetKey(ContinueButton))
            yield return new WaitForEndOfFrame();
        while (Input.GetKey(ContinueButton))
            yield return new WaitForEndOfFrame();
        tutorial16.SetActive(false);
        tutorial17.SetActive(true);
        while (!Input.GetKey(ContinueButton))
            yield return new WaitForEndOfFrame();
        while (Input.GetKey(ContinueButton))
            yield return new WaitForEndOfFrame();
        tutorial17.SetActive(false);

        // Restart the game and keep it going
        foreach (UIDraggableTower towerProfile in towerProfiles)
        {
            towerProfile.CanDrag = true;
        }
        refEnemySpawner.EnemiesSpawning = true;
        // make every tower avaliable to be sold
        foreach (TowerAi tower in towers)
        {
            tower.canBeSold = true;
        }
    }
}
