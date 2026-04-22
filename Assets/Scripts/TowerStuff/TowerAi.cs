/********************************************
 * filename: TowerAi.cs
 * Author: Micaiah Mariano
 * Description: Contains the logic for the towers
 * ******************************************/
using UnityEngine;
using UnityEngine.UIElements;

public class TowerAi : MonoBehaviour
{
    // Editable variables

    // Sets game objects
    [SerializeField] GameObject Bullet;
    // Sets size
    [SerializeField] float SizeX = 1;
    [SerializeField] float SizeY = 1;
    // Sets stats
    [SerializeField] int HealthMax = 1;
    [Tooltip("Shots per second. set to -1 to disable")]
    [SerializeField] float RateOfFire = 5;
    [SerializeField] float FirePositionX = 0;
    [SerializeField] float FirePositiony = 0;
    // Timer for animaton to start - fire cooldown
    [SerializeField] public float LengthOfAnimaton = 0;
    [SerializeField] public AudioManager.Towers TowerID;
    // Variables that are changed in the code
    private Animator Animator;
    private int Health = 1;
    [HideInInspector] public float CoolDown;
    [HideInInspector] public bool TowerAttacking = true;
    TowerGrid refGrid;
    LaneCheck refLaneCheck;
    AudioManager refAudioManager; 
    [SerializeField] private GameObject particls;
    [SerializeField] private GameObject particlsRemove;
    /// <summary>
    /// The position of this tower in the grid (index based, where 0,0 is the bottom left)
    /// </summary>
    [HideInInspector] public Vector2Int GridPosition = Vector2Int.zero;

    public bool canBeSold = true;

    // List of animatons
    enum Animatons
    {
        idole,
        attack,
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Sets variables
        Animator = GetComponentInChildren<Animator>();
        Vector3 Size = new Vector3(SizeX, SizeY);
        Health = HealthMax;
        gameObject.transform.localScale = Size;
        CoolDown = CoolDown < LengthOfAnimaton ? LengthOfAnimaton : CoolDown;
        refGrid = FindFirstObjectByType<TowerGrid>();
        refLaneCheck = FindFirstObjectByType<LaneCheck>();
        if (refLaneCheck == null)
        {
            Debug.LogError("No lane check in the scene");
        }
        refAudioManager = FindFirstObjectByType<AudioManager>();
        if (refAudioManager != null)
            refAudioManager.PlayTowerDropSFXs(gameObject, TowerID);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!TowerAttacking)
        {
            return;
        }
        // Destroy the object if it is out of hitpoints and plays sound
        if (Health <= 0)
        {
            refAudioManager?.PlayTowerDieSFX(gameObject, TowerID);
            Instantiate(particls, gameObject.transform.position, Quaternion.identity);
            // Instantiate(DeathSFX, transform.position, Quaternion.identity);
            Die();
            return;
        }
        // Make sure rate of fire causes
        if (RateOfFire == -1)
        {
            return;
        }
        // Cooldown for a shot
        CoolDown -= Time.fixedDeltaTime;
        if ((GridPosition.y == 0 && refLaneCheck.Lane2 != 0) || (GridPosition.y == 1 && refLaneCheck.Lane1 != 0)
            || (GridPosition.y == 2 && refLaneCheck.Lane0 != 0))
        {
            // Make sure animaton is played befor attack
            if (CoolDown <= LengthOfAnimaton)
            {
                Animator.SetInteger("State", (int)Animatons.attack);
            }
            else
            {
                Animator.SetInteger("State", (int)Animatons.idole);
            }
            // Fire a shot if it is cooled down
            if (CoolDown <= 0)
            {
                // Calculate a fireposition
                Vector2 FirePosition = new Vector2(transform.position.x + FirePositionX, transform.position.y + FirePositiony);
                // Shoot
                Instantiate(Bullet, FirePosition, Quaternion.identity);
                // Sfx
                refAudioManager?.PlayTowerShootSFX(gameObject, TowerID);
                // Reset cooldown
                CoolDown = RateOfFire;
            }
        }
        // Make sure attack cooldown cant hit zero tell animaton plays first
        else
        {
            CoolDown = LengthOfAnimaton;
            Animator.SetInteger("State", (int)Animatons.idole);
        }
        
    }

    /// <summary>
    /// Runs logic for the tower to die
    /// </summary>
    void Die()
    {
        // Remove the tower at the position in the grid
        if (refGrid.RemoveTowerAtPosition(GridPosition, false) == null)
        {
            Debug.LogError($"Unable to remove tower at the position {GridPosition.x}, {GridPosition.y}");
        }
        // Death effects (sfx, etc.)

        // Destroy the game object
        Destroy(gameObject);
    }

    /// <summary>
    /// Reduces the health of the tower if it is hit by an enemy
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyDamgeBox"))
        {
            EnemyBoxAi refDamageBox = collision.GetComponent<EnemyBoxAi>();
            if (refDamageBox != null)
            {
                int dmg = (int)refDamageBox.GetDamage();
                Health -= dmg;
                refAudioManager?.PlayTowerHurtSFX(gameObject);
            }
        }
    }

    /// <summary>
    /// Destroys the tower if rightclicked on
    /// </summary>
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && canBeSold)
        {
            Instantiate(particlsRemove, gameObject.transform.position, Quaternion.identity);
            refAudioManager?.PlayTowerSellSFX(gameObject, TowerID);
            Die();
        }
        else if (Input.GetMouseButtonDown(0) && canBeSold && TowerID != AudioManager.Towers.bunny)
        {
            Instantiate(particlsRemove, gameObject.transform.position, Quaternion.identity);
            refAudioManager?.PlayTowerSellSFX(gameObject, TowerID);
            Die();
            UIDraggableTower[] allTowerDrags = FindObjectsByType<UIDraggableTower>(FindObjectsSortMode.None);
            // Make the player now hold a copy of this tower
            foreach (UIDraggableTower towerDrag in allTowerDrags)
            {
                if (towerDrag.TowerPrefab.GetComponent<TowerAi>().TowerID == TowerID)
                {
                    towerDrag.followingMouse = true;
                    towerDrag.SavedLastAtkCD = CoolDown;
                }
            }
        }
    }
}
