/********************************************
 * filename: EnemyAi.cs
 * Author: Micaiah Mariano
 * Description: Contains all AI for the enemy basics allowing it to be edited outside of the scripts
 * ******************************************/
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    // editable variables

    // the attack hit box for the enemy
    [SerializeField] GameObject Hurtfield;
    // size and color for the enemy
    [SerializeField] float SizeX = 1;
    [SerializeField] float SizeY = 1;

    // changes stats
    [Header("Stats")]
    [SerializeField] float Speed = -1;
    [SerializeField] int HealthMax = 1;
    [SerializeField] float AttackCooldown = 3;
    [SerializeField] float AttackHitBoxX = 0;
    [SerializeField] float AttackHitBoxY = 0;
    [SerializeField] float LengthOfAnimaton = 0;
    [Header("SFX")]
    [SerializeField] AudioSource SpawnSFX;
    [SerializeField] AudioSource TakeDMGSFX;
    [Tooltip("1 in this chance to play the sound each fixed update (50 times per second)")]
    [SerializeField] float randomVoiceChance = 500f;
    [SerializeField] AudioSource RandomVoiceSFX;
    [Tooltip("The amount of time in seconds inbetween each walk sound")]
    [SerializeField] float timeInbetweenWalkSounds = 1f;
    [SerializeField] AudioSource WalkSFX;
    [SerializeField] AudioSource AttackSFX;
    [SerializeField] GameObject DeathSFX;
    [SerializeField] float deathAnimationtime;
    [SerializeField] float deathAnimation;

    // variables that are changed in the code
    Color refcolor = Color.white;
    private int Health;
    private Animator animator;
    private float coolDownAttack = 0;
    private float timeToNextEnemyWalkSound = 0;
    private float MaxSpeed;
    private Rigidbody2D RB;
    private float deathAnimationtimprivete;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    enum animatons
    {
        walk,
        attack,
        die,
        idale
    }
    void Start()
    {
        //initializes the animations
        animator = GetComponentInChildren<Animator>();
        //sets rigid body
        RB = GetComponent<Rigidbody2D>();
        //sets size
        Vector3 Size = new Vector3(SizeX, SizeY);
        gameObject.transform.localScale = Size;
        //sets color
        refcolor = gameObject.GetComponentInChildren<SpriteRenderer>().color;
        //sets states
        Health = HealthMax;
        coolDownAttack = AttackCooldown;
        MaxSpeed = Speed;
        deathAnimationtimprivete = deathAnimationtime;
        // Play Spawn Sound
        SpawnSFX.Play();
    }
    void FixedUpdate()
    {
        // - timers
        coolDownAttack -= Time.fixedDeltaTime;
        if (Random.Range(0, randomVoiceChance) == 0) RandomVoiceSFX.Play();
        timeToNextEnemyWalkSound -= Time.fixedDeltaTime;
        if (timeToNextEnemyWalkSound <= 0)
        {
            WalkSFX.Play();
            timeToNextEnemyWalkSound = timeInbetweenWalkSounds;
        }
        // moves enemy
        RB.linearVelocityX = MaxSpeed;
        MaxSpeed = Speed;
        // animator.SetInteger("State", (int)animatons.walk);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // lowers health if hit by bullet
        if (collision.CompareTag("Bullet"))
        {
            BulletAi refBullet = collision.GetComponent<BulletAi>();
            if (refBullet != null)
            {
                TakeDMGSFX.Play();
                int dmg = (int)refBullet.GetDamage();
                Health -= dmg;
                // checks if dead
                if (Health <= 0)
                {
                    Die();
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // souposed to spawn attack box when ready then move back a little so it can attack agin
        if (collision.CompareTag("Tower"))
        {
            //sets positon of were attack box will spawn
            Vector2 FirePosition = new Vector2(transform.position.x + AttackHitBoxX, transform.position.y + AttackHitBoxY);
            // starts to play animaton befor the tower shoots
            if (coolDownAttack <= LengthOfAnimaton)
            {
                // animator.SetInteger("State", (int)animatons.attack);
            }
            // shoots a bullet when cooldown is ready
            if (coolDownAttack <= 0)
            {
                AttackSFX.Play();
                coolDownAttack = AttackCooldown;
                Instantiate(Hurtfield, FirePosition, Quaternion.identity);
                MaxSpeed = 0;
            }
            // plays idale 
            else
            {
                // animator.SetInteger("State", (int)animatons.idale);
                MaxSpeed = 0;
            }
        }
    }

    /// <summary>
    /// Kills the enemy and does cool effects
    /// </summary>
    void Die()
    {
        // plays death animatons
        if (deathAnimationtimprivete >= deathAnimation)
        {
            // animator.SetInteger("State", (int)animatons.die);
        }
        Instantiate(DeathSFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
