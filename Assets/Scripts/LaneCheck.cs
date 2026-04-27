/********************************************
 * filename: LaneCheck.cs
 * Author: Micaiah Mariano
 * Description: Contains the logic for enemys so they know what lane there on
 * ******************************************/
using UnityEngine;

public class LaneCheck : MonoBehaviour
{
    [SerializeField] ColliderForLane Lane0Collider;
    [SerializeField] ColliderForLane Lane1Collider;
    [SerializeField] ColliderForLane Lane2Collider;


    /// <summary>
    /// Increase the amount of enemies in a lane
    /// </summary>
    /// <param name="lane">The lane number</param>
    public bool EnemiesInLane(int lane)
    {
        switch (lane)
        {
            case 0:
                return Lane0Collider.EnemiesPresent;
            case 1:
                return Lane1Collider.EnemiesPresent;
            case 2:
                return Lane2Collider.EnemiesPresent;
            default:
                Debug.LogWarning("Bad lane!");
                return false;
        }
    }
}
