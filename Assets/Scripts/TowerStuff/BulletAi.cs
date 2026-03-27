/********************************************
 * filename: BulletAi.cs
 * Author: Micaiah Mariano
 * Description: Contains the logic for the bullets from the towers
 * ******************************************/
using UnityEngine;

public class BulletAi : MonoBehaviour
{
    // Size and color
    [SerializeField] float SizeX = 1;
    [SerializeField] float SizeY = 1;
    [SerializeField] Color refcolor = Color.white;
    // Stats
    [SerializeField] float Speed = 10;
    [SerializeField] uint Damage = 2;
    
    //private Animator animator;
    private Rigidbody2D RB;

    enum animatons
    {
        idale,
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initializes variables such as size and color
        RB = GetComponent<Rigidbody2D>();
        Vector3 Size = new Vector3(SizeX, SizeY);
        gameObject.transform.localScale = Size;
        gameObject.GetComponentInChildren<SpriteRenderer>().color = refcolor;
    }

    void FixedUpdate()
    {
        // Animates the bullet
        //animator.SetInteger("State", (int)animatons.idale);
        // Moves the bullet
        RB.linearVelocityX = Speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Checks for collision with enemy then dies
        if (collision.CompareTag("Enemy"))
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
