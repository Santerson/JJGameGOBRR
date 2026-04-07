/********************************************
 * filename: EnemyAi.cs
 * Author: Micaiah Mariano, Santiago Caprarulo
 * Description: Contains all AI for the enemy basics allowing it to be edited outside of the scripts
 * ******************************************/
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
    [SerializeField] AudioManager.Enemies EnemyID;
    [Header("SFX")]
    [Tooltip("1 in this chance to play the sound each fixed update (50 times per second)")]
    [SerializeField] float RandomVoiceChance = 500f;
    [Tooltip("The amount of time in seconds inbetween each walk sound")]
    [SerializeField] float TimeInbetweenWalkSounds = 1f;
    [SerializeField] float DeathAnimationtime;
    [SerializeField] float DeathAnimation;
    [SerializeField] private GameObject particls;    
    
    // variables that are changed in the code
    public int lane;
    Color refcolor = Color.white;
    private int Health;
    private Animator animator;
    private float coolDownAttack = 0;
    private float timeToNextEnemyWalkSound = 0;
    private float MaxSpeed;
    private Rigidbody2D RB;
    private float deathAnimationtimprivete;

    [HideInInspector] public bool StoppedEnemy = false;

    AudioManager refAudioManager;
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
        deathAnimationtimprivete = DeathAnimationtime;
        // Play Spawn Sound
        refAudioManager = FindFirstObjectByType<AudioManager>();
        refAudioManager.PlayEnemySpawnSFX(EnemyID);
    }
    void FixedUpdate()
    {
        // - timers
        coolDownAttack -= Time.fixedDeltaTime;
        // Play the audio for the enemy
        if (!StoppedEnemy)
        {
            if (Random.Range(0, RandomVoiceChance) == 0) refAudioManager.PlayEnemyVoiceSFX(EnemyID);
            timeToNextEnemyWalkSound -= Time.fixedDeltaTime;
            if (timeToNextEnemyWalkSound <= 0)
            {
                refAudioManager.PlayEnemyWalkSFX(EnemyID);
                timeToNextEnemyWalkSound = TimeInbetweenWalkSounds;
            }
        }
        // Stop the enemy if they should be stopped, otherwise move it more
        if (StoppedEnemy)
        {
            RB.linearVelocityX = 0;
        }
        else
        {
            // moves enemy
            RB.linearVelocityX = MaxSpeed;
            MaxSpeed = Speed;
        }
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
                refAudioManager.PlayEnemyHurtSFX(EnemyID);
                int dmg = (int)refBullet.GetDamage();
                Health -= dmg;
                // checks if dead
                if (Health <= 0)
                {
                    Instantiate(particls, gameObject.transform.position, Quaternion.identity);
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
                refAudioManager.PlayEnemyAttackSFX(EnemyID);
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
        if (deathAnimationtimprivete >= DeathAnimation)
        {
            // animator.SetInteger("State", (int)animatons.die);
        }
        refAudioManager.PlayEnemyDieSFX(EnemyID);
        FindFirstObjectByType<LaneCheck>().Lanedecreesss(lane);
        Destroy(gameObject);
    }
}
