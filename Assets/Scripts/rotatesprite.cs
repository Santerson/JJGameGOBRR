/**************************
 * Filename: rotatesprite
 * Author: Micaiah Mariano
 * Description: Rotates a sprite by a certain amount each fixed update
 * * ***********************/
using UnityEngine;


public class rotatesprite : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
