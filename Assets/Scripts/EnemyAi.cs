using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] float SizeX = 1;
    [SerializeField] float SizeY = 1;
    Vector3 Size;
    [SerializeField] float Red = 1;
    [SerializeField] float Green = 1;
    [SerializeField] float Blue = 1;
    [SerializeField] Color refcolor;
    Color color;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 Size = new Vector3(SizeX, SizeY);
        Color color = new Color(Red, Green, Blue);
        gameObject.transform.localScale = Size;
        gameObject.GetComponent<SpriteRenderer>().color = refcolor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
