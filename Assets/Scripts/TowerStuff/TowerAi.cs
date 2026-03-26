/********************************************
 * filename: BulletAi.cs
 * Author: Micaiah Mariano
 * Description: Contains the logic for the bullets form towers
 * ******************************************/
using UnityEngine;
using UnityEngine.UIElements;

public class TowerAi : MonoBehaviour
{
    //editable variables

    // sets gameobject
    [SerializeField] GameObject bullet;
    // sets size
    [SerializeField] float SizeX = 1;
    [SerializeField] float SizeY = 1;
    // sets stats
    [SerializeField] int HealthMax = 1;
    [Tooltip("Shots per second. set to -1 to disable")]
    [SerializeField] float RateOfFire = 5;
    [SerializeField] float FirePositionX = 0;
    [SerializeField] float FirePositiony = 0;
    // timer for animaton to start - fire cooldown
    [SerializeField] float LengthOfAnimaton = 0;
    // sounds
    [SerializeField] AudioSource DeathSFX;
    [SerializeField] AudioSource SellSFX;
    [SerializeField] AudioSource ShootSFX;
    [Header("Audio")]
    // variables that are changed in the code
    private Animator animator;
    private int Health = 1;
    private float cooldown;
    TowerGrid refGrid;
    LaneCheck refLaneCheck;

    /// <summary>
    /// The position of this tower in the grid (index based, where 0,0 is the bottom left)
    /// </summary>
    [HideInInspector] public Vector2Int GridPosition = Vector2Int.zero;
    //list of animatons
    enum Animatons
    {
        idole,
        attack,
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // sets variables
        animator = GetComponentInChildren<Animator>();
        Vector3 Size = new Vector3(SizeX, SizeY);
        Health = HealthMax;
        gameObject.transform.localScale = Size;
        cooldown = RateOfFire;
        refGrid = FindFirstObjectByType<TowerGrid>();
        refLaneCheck = FindFirstObjectByType<LaneCheck>();
        if (refLaneCheck == null)
        {
            Debug.LogError("No lane check in the scene");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Destroy the object if it is out of hitpoints and plays sound
        if (Health <= 0)
        {
            Instantiate(DeathSFX, transform.position, Quaternion.identity);
            Die();
            return;
        }
        // make sure rate of fire causes
        if (RateOfFire == -1)
        {
            return;
        }
        // Cooldown for a shot
        cooldown -= Time.fixedDeltaTime;
        if ((GridPosition.y == 0 && refLaneCheck.Lane2 != 0) || (GridPosition.y == 1 && refLaneCheck.Lane1 != 0)
            || (GridPosition.y == 2 && refLaneCheck.Lane0 != 0))
        {
            if (cooldown <= LengthOfAnimaton)
            {
                animator.SetInteger("State", (int)Animatons.attack);
            }
            else
            {
                animator.SetInteger("State", (int)Animatons.idole);
            }
            // Fire a shot if it is cooled down
            if (cooldown <= 0)
            {
                // Calculate a fireposition
                Vector2 FirePosition = new Vector2(transform.position.x + FirePositionX, transform.position.y + FirePositiony);
                // Shoot
                Instantiate(bullet, FirePosition, Quaternion.identity);
                // Sfx
                ShootSFX.Play();
                // Reset cooldown
                cooldown = RateOfFire;
            }
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

            }
        }
    }

    /// <summary>
    /// Destroys the tower if rightclicked on
    /// </summary>
    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Instantiate(SellSFX, transform.position, Quaternion.identity);
            Die();
        }
    }
}
