/********************************************
 * filename: BulletAi.cs
 * Author: Micaiah Mariano
 * Description: Contains the logic for the bullets form towers
 * ******************************************/
using UnityEngine;

public class BulletAi : MonoBehaviour
{
    // size and color
    [SerializeField] float SizeX = 1;
    [SerializeField] float SizeY = 1;
    [SerializeField] Color refcolor = Color.white;
    //stats
    [SerializeField] float Speed = 10;
    [SerializeField] uint Damage = 2;

    private Rigidbody2D RB;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // initializes variables such as size and color
        RB = GetComponent<Rigidbody2D>();
        Vector3 Size = new Vector3(SizeX, SizeY);
        gameObject.transform.localScale = Size;
        gameObject.GetComponentInChildren<SpriteRenderer>().color = refcolor;
    }

    void FixedUpdate()
    {
        // moves the bullet
        RB.linearVelocityX = Speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checks for collision with enemy then dies
        if (collision.CompareTag("enemy"))
        {
            Destroy(gameObject);
        }
            
    }
    
    /// <summary>
    /// Gets this bullet's damage
    /// </summary>
    /// <returns>uint of the bullet's damage</returns>
    public uint GetDamage()
    {
        return Damage;
    }
}
