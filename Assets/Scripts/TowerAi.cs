using UnityEngine;

public class TowerAi : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float SizeX = 1;
    [SerializeField] float SizeY = 1;
    [SerializeField] Color refcolor = Color.white;
    [SerializeField] int HealthMax = 1;
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

        gameObject.transform.localScale = Size;
        gameObject.GetComponent<SpriteRenderer>().color = refcolor;
        cooldown = RateOfFire;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 FirePosition = new Vector2(gameObject.GetComponent<Transform>().position.x+FirePositionX, gameObject.GetComponent<Transform>().position.y + FirePositiony);
        
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            Instantiate(bullet, FirePosition, Quaternion.Euler(0f, 0f, 0f));
            cooldown = RateOfFire;
        }
        
    }
}
