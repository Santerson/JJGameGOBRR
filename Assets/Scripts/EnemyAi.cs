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
    [SerializeField] Color refcolor = Color.white;
    // changes stats
    [SerializeField] float Speed = -1;
    [SerializeField] int HealthMax = 1;
    [SerializeField] float AttackCooldown = 3;
    [SerializeField] float AttackHitBoxX = 0;
    [SerializeField] float AttackHitBoxY = 0;
    [SerializeField] float WalkBackAfterAttackTime = 2;
    // variabls that are changed in the code
    private int Health;
    private float TimeUntilWalkBackAgain;
    private float coolDownAttack = 0;
    private float MaxSpeed;
    private Rigidbody2D RB;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //sets riged body
        RB = GetComponent<Rigidbody2D>();
        //sets size
        Vector3 Size = new Vector3(SizeX, SizeY);
        gameObject.transform.localScale = Size;
        //sets color
        gameObject.GetComponent<SpriteRenderer>().color = refcolor;
        //sets states
        Health = HealthMax;
        coolDownAttack = AttackCooldown;
        MaxSpeed = Speed;   
    }

    private void Update()
    {
        // Check if the enemy is currently walking backwards
        if (TimeUntilWalkBackAgain > 0)
        {
            // Reduce the amoutn of tiem they are walking back for left by deltatime
            TimeUntilWalkBackAgain -= Time.deltaTime;
            // If they should start walking back forwards again
            if (TimeUntilWalkBackAgain <= 0)
            {
                // Make them walk forward again
                MaxSpeed *= -1;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // - timers
        
        coolDownAttack -= Time.deltaTime;
        // checks if dead
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
        // moves enemy
        RB.linearVelocityX = MaxSpeed;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //sets positon of were attack box will spawn
        Vector2 FirePosition = new Vector2(gameObject.GetComponent<Transform>().position.x + AttackHitBoxX, gameObject.GetComponent<Transform>().position.y + AttackHitBoxY);
        // lowers health if hit by bullet
        if (collision.CompareTag("BulletFromSmallTower"))
        {
            Health -= 1;
        }
        // souposed to spawn attack box when ready then move back a little so it can attack agin
        if (collision.CompareTag("Tower"))
        {
            if (coolDownAttack <= 0)
            {
                coolDownAttack = AttackCooldown;
                Instantiate(Hurtfield, FirePosition, Quaternion.Euler(0f, 0f, 0f));
                TimeUntilWalkBackAgain = WalkBackAfterAttackTime;
                MaxSpeed *= -1;
            }
        }
    }
}
