using UnityEngine;

public class EnemyBoxAi : MonoBehaviour
{
    [SerializeField] float SizeX = 1;
    [SerializeField] float SizeY = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 Size = new Vector3(SizeX, SizeY);
        gameObject.transform.localScale = Size;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tower"))
        {
            Destroy(gameObject);
        }

    }
}
