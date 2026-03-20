/********************************************
 * filename: EnemyBoxAi.cs
 * Author: Micaiah Mariano
 * Description: Contains the logic for the attack box of enemys
 * ******************************************/
using UnityEngine;


public class EnemyBoxAi : MonoBehaviour
{
    // determines size
    [SerializeField] float SizeX = 1;
    [SerializeField] float SizeY = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //sets size
        Vector3 Size = new Vector3(SizeX, SizeY);
        gameObject.transform.localScale = Size;
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checks for collision with tower that destroy itself if it did
        if (collision.CompareTag("Tower"))
        {
            Destroy(gameObject);
        }

    }
}
