/********************************************
 * filename: TowerDataStorage.cs
 * Author: Santiago Caprarulo
 * Description: Contains data of different parts that each tower needs,
 * and getter functions for each variable
 * ******************************************/
using UnityEngine;

public class TowerDataStorage : MonoBehaviour
{
    [SerializeField] AudioSource DropSFX;
    [SerializeField] float TowerManaCost = 2f;

    /// <summary>
    /// Gets the tower's dropSFX
    /// </summary>
    /// <returns>Audiosource of the drop sfx</returns>
    public AudioSource GetDropSFX()
    {
        return DropSFX;
    }

    /// <summary>
    /// Gets the tower's mana upkeep cost
    /// </summary>
    /// <returns>float of the mana cost</returns>
    public float GetTowerManaCost()
    {
        return TowerManaCost;
    }
}
