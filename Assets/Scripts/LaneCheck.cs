/********************************************
 * filename: LaneCheck.cs
 * Author: Micaiah Mariano
 * Description: Contains the logic for enemys so they know what lane there on
 * ******************************************/
using UnityEngine;

public class LaneCheck : MonoBehaviour
{
    /// <summary>
    /// Enemies in lane 0
    /// </summary>
    public int Lane0 { get; private set; }
    /// <summary>
    /// Enemies in lane 1
    /// </summary>
    public int Lane1 { get; private set; }
    /// <summary>
    /// Enemies in lane 2
    /// </summary>
    public int Lane2 { get; private set; }
    
    /// <summary>
    /// Increase the amount of enemies in a lane
    /// </summary>
    /// <param name="lane">The lane number</param>
    public void Laneincress(int lane)
    {
        if (lane == 0)
        {
            Lane0++;
        }
        if (lane == 1)
        {
            Lane1++;
        }
        if (lane == 2)
        {
            Lane2++;
        }
    }

    /// <summary>
    /// Decreases the amount of enemies in a lane
    /// </summary>
    /// <param name="lane">the lane number</param>
    public void Lanedecreesss(int lane)
    {
        if (lane == 0)
        {
            Lane0--;
        }
        if (lane == 1)
        {
            Lane1--;
        }
        if (lane == 2)
        {
            Lane2--;
        }
    }
}
