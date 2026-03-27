/********************************************
 * filename: LaneCheck.cs
 * Author: Micaiah Mariano
 * Description: Contains the logic for enemys so they know what lane there on
 * ******************************************/
using UnityEngine;

public class LaneCheck : MonoBehaviour
{
    public int Lane0 { get; private set; }
    public int Lane1 { get; private set; }
    public int Lane2 { get; private set; }
    
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
