using UnityEngine;

public class BulletAi : MonoBehaviour
{
    [SerializeField] float SizeX = 1;
    [SerializeField] float SizeY = 1;
    [SerializeField] Color refcolor = Color.white;
    [SerializeField] float Speed = 10;
    private Rigidbody2D RB;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        Vector3 Size = new Vector3(SizeX, SizeY);
        gameObject.transform.localScale = Size;
        gameObject.GetComponent<SpriteRenderer>().color = refcolor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RB.linearVelocityX = Speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            Destroy(gameObject);
        }
            
    }
}
