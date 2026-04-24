/**************************
 * Filename: rotatesprite
 * Author: Micaiah Mariano, Santiago Caprarulo
 * Description: Rotates a sprite by a certain amount each fixed update
 * * ***********************/
using UnityEngine;

 
public class rotatesprite : MonoBehaviour
{
    [SerializeField] float RotateAmount;

    private void Start()
    {
        GetComponent<Rigidbody2D>().angularVelocity = RotateAmount;
    }
}
