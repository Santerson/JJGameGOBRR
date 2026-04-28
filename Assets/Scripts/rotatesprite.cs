/**************************
 * Filename: rotatesprite
 * Author: Micaiah Mariano, Santiago Caprarulo
 * Description: Rotates a sprite by a certain amount each fixed update
 * * ***********************/
using UnityEngine;


public class rotatesprite : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        // Rotates the object around the Z-axis (standard 2D rotation)
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
