using UnityEngine;
using UnityEngine.UIElements;

public class TowerAi : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float SizeX = 1;
    [SerializeField] float SizeY = 1;
    [SerializeField] int HealthMax = 1;
    [SerializeField] float RateOfFire = 5;
    [SerializeField] float FirePositionX = 0;
    [SerializeField] float FirePositiony = 0;

    [Header("Audio")]
    [SerializeField] AudioSource DeathSFX;

    private int Health = 1;
    private float cooldown;
    private Rigidbody2D RB;
    Color refcolor = Color.white;
    TowerGrid refGrid;

    /// <summary>
    /// The position of this tower in the grid (index based, where 0,0 is the bottom left)
    /// </summary>
    [HideInInspector] public Vector2Int GridPosition = Vector2Int.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        Vector3 Size = new Vector3(SizeX, SizeY);
        Health = HealthMax;
        gameObject.transform.localScale = Size;
        refcolor = gameObject.GetComponentInChildren<SpriteRenderer>().color;
        cooldown = RateOfFire;
        refGrid = FindFirstObjectByType<TowerGrid>();
        if (refGrid == null)
        {
            Debug.LogError("No tower grid found on the scene!");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Destroy the object if it is out of hitpoints
        if (Health <= 0)
        {
            Die();
            return;
        }
        
        // Cooldown for a shot
        cooldown -= Time.fixedDeltaTime;
        // Fire a shot if it is cooled down
        if (cooldown <= 0)
        {
            // Calculate a fireposition
            Vector2 FirePosition = new Vector2(transform.position.x + FirePositionX, transform.position.y + FirePositiony);
            // Shoot
            Instantiate(bullet, FirePosition, Quaternion.identity);
            // Reset cooldown
            cooldown = RateOfFire;
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
        Instantiate(DeathSFX, transform.position, Quaternion.identity);

        // Destroy the game object
        Destroy(gameObject);
    }

    /// <summary>
    /// Reduces the health of the tower if it is hit by an enemy
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemySmallDamageBox"))
        {
            Health -= 1;
        }
    }

}
