/********************************
 * Filename: KillField.cs
 * Author: Santiago Caprarulo
 * Description: destroys the object that hit it
 * *****************************/
using UnityEngine;

public class KillField : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }
}
