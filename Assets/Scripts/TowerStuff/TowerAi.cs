using UnityEngine;
using UnityEngine.UIElements;

public class TowerAi : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float SizeX = 1;
    [SerializeField] float SizeY = 1;
    [SerializeField] Color refcolor = Color.white;
    [SerializeField] int HealthMax = 1;
    private int Health = 1;
    [SerializeField] float RateOfFire = 5;
    [SerializeField] float FirePositionX = 0;
    [SerializeField] float FirePositiony = 0;
    private float cooldown;
    private Rigidbody2D RB;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        Vector3 Size = new Vector3(SizeX, SizeY);
        Health = HealthMax;
        gameObject.transform.localScale = Size;
        gameObject.GetComponent<SpriteRenderer>().color = refcolor;
        cooldown = RateOfFire;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
        Vector2 FirePosition = new Vector2(transform.position.x+FirePositionX, transform.position.y + FirePositiony);
        
        cooldown -= Time.fixedDeltaTime;
        if (cooldown <= 0)
        {
            Instantiate(bullet, FirePosition, Quaternion.Euler(0f, 0f, 0f));
            cooldown = RateOfFire;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemySmallDamageBox"))
        {
            Health -= 1;
        }
    }

}
