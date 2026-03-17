using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] GameObject Hurtfield;
    [SerializeField] float SizeX = 1;
    [SerializeField] float SizeY = 1;
    [SerializeField] Color refcolor = Color.white;
    [SerializeField] float Speed = -1;
    [SerializeField] int HealthMax = 1;
    [SerializeField] float AttackCooldown = 0;
    [SerializeField] float AttackSpeed = 0;
    [SerializeField] float AttackHitBoxX = 0;
    [SerializeField] float AttackHitBoxY = 0;
    private int Health;

    private Rigidbody2D RB;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        RB = GetComponent<Rigidbody2D>();
        Vector3 Size = new Vector3(SizeX, SizeY);
        gameObject.transform.localScale = Size;
        gameObject.GetComponent<SpriteRenderer>().color = refcolor;
        Health = HealthMax;
         
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
        RB.linearVelocityX = Speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 FirePosition = new Vector2(gameObject.GetComponent<Transform>().position.x + AttackHitBoxX, gameObject.GetComponent<Transform>().position.y + AttackHitBoxY);
        if (collision.CompareTag("BulletFromSmallTower"))
        {
            Health -= 1;
        }
        if (collision.CompareTag("Tower"))
        {
            Speed = 0;
            Instantiate(Hurtfield, FirePosition, Quaternion.Euler(0f, 0f, 0f));
        }
    }
}
