using UnityEngine;
/********************************************
 * filename: particleDeath.cs
 * Author: Micaiah Mariano
 * Description: Contains the logic for the particles
 * ******************************************/
public class particleDeath : MonoBehaviour
{
    // Is the length of time for the particle 
    [SerializeField] private float LeangthOfParticle;

    // Sets it so it can be changed
    private float timerForParticls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Sets variables
        timerForParticls = LeangthOfParticle;
    }

    // Destroys the particle once it finishes
    private void FixedUpdate()
    {
        timerForParticls -= Time.fixedDeltaTime;
        if(timerForParticls <= 0)
        {
            Destroy(gameObject);
        }
    }
}
