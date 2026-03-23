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
    [SerializeField] float Speed = -1;
    [SerializeField] int HealthMax = 1;
    [SerializeField] float AttackCooldown = 3;
    [SerializeField] float AttackHitBoxX = 0;
    [SerializeField] float AttackHitBoxY = 0;

    // variables that are changed in the code
    Color refcolor = Color.white;
    private int Health;

    private float coolDownAttack = 0;
    private float MaxSpeed;
    private Rigidbody2D RB;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
    }
    void FixedUpdate()
    {
        // - timers
        coolDownAttack -= Time.fixedDeltaTime;
        // checks if dead
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
        // moves enemy
        RB.linearVelocityX = MaxSpeed;
        MaxSpeed = Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // lowers health if hit by bullet
        if (collision.CompareTag("Bullet"))
        {
            BulletAi refBullet = collision.GetComponent<BulletAi>();
            if (refBullet != null)
            {
                int dmg = (int)refBullet.GetDamage();
                Health -= dmg;

            }
            else
            {
                Debug.LogError("No bullet script attached! attach one.");
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
            if (coolDownAttack <= 0)
            {
                coolDownAttack = AttackCooldown;
                Instantiate(Hurtfield, FirePosition, Quaternion.identity);
                MaxSpeed = 0;
            }
            else
            {
                MaxSpeed = 0;
            }
        }

        
    }
}
