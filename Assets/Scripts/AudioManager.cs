/********************************************
 * Filename: AudioManager.cs
 * Authors: Santiago Caprarulo, Henry Falaand
 * Description: Handles logic for playing and manipulating
 * sounds with Wwise.
 * *****************************************/

using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("Towers")]
    [SerializeField] List<AK.Wwise.Event> TowerDropSFXs;
    [SerializeField] List<AK.Wwise.Event> TowerShootSFXs;
    [SerializeField] List<AK.Wwise.Event> TowerDieSFXs;

    [Header("Enemies")]
    [SerializeField] List<AK.Wwise.Event> EnemyWalkSFXs;
    [SerializeField] List<AK.Wwise.Event> EnemySpawnSFXs;
    [SerializeField] List<AK.Wwise.Event> EnemyAttackSFXs;
    [SerializeField] List<AK.Wwise.Event> EnemyHurtSFXs;
    [SerializeField] List<AK.Wwise.Event> EnemyDieSFXs;
    [SerializeField] List<AK.Wwise.Event> EnemyVoiceSFXs;

    [Header("MSC")]
    [SerializeField] AK.Wwise.Event LoseLevelSFX;
    [SerializeField] AK.Wwise.Event WinLevelSFX;
    [SerializeField] AK.Wwise.Event ManaGainSFX;
    [SerializeField] AK.Wwise.Event ManaLoseSFX;
    [SerializeField] AK.Wwise.Event UIClickSFX;

    /// <summary>
    /// Handles don't destroy on load functionality
    /// </summary>
    private void Start()
    {
        // Check if other objects exist
        DontDestroyOnLoad[] objs = FindObjectsByType<DontDestroyOnLoad>(FindObjectsSortMode.None);
        if (objs.Length > 1)
        {
            // Destroy itself if so
            Destroy(gameObject);
            return;
        }
        // Otherwise, never destroy it
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// A reference to the id of every tower
    /// </summary>
    public enum Towers
    {
        bunny = 0,
        mushman = 1,
        fairy = 2,
        golem = 3
    }

    /// <summary>
    /// A reference to the id of every enemy
    /// </summary>
    public enum Enemies
    {
        person = 0,
        mech = 1
    }

    /// <summary>
    /// Plays a drop sfx for a given tower
    /// </summary>
    /// <param name="tower">The ID of the tower</param>
    public void PlayTowerDropSFXs(Towers tower)
    {
        AK.Wwise.Event refSound = TowerDropSFXs[(int)tower];
        AkUnitySoundEngine.PostEvent(refSound.Id, gameObject);
    }

    /// <summary>
    /// Plays a shoot sfx for a given tower
    /// </summary>
    /// <param name="tower">The ID of the tower</param>
    public void PlayTowerShootSFX(Towers tower)
    {
        AK.Wwise.Event refSound = TowerShootSFXs[(int)tower];
        AkUnitySoundEngine.PostEvent(refSound.Id, gameObject);
    }

    /// <summary>
    /// Plays a death sfx for a given tower
    /// </summary>
    /// <param name="tower">The ID of the tower</param>
    public void PlayTowerDieSFX(Towers tower)
    {
        AK.Wwise.Event refSound = TowerDieSFXs[(int)tower];
        AkUnitySoundEngine.PostEvent(refSound.Id, gameObject);
    }

    /// <summary>
    /// Plays the attack sfx for a given enemy
    /// </summary>
    /// <param name="enemy">The ID of the enemy</param>
    public void PlayEnemyAttackSFX(Enemies enemy)
    {
        AK.Wwise.Event refSound = EnemyAttackSFXs[(int)enemy];
        AkUnitySoundEngine.PostEvent(refSound.Id, gameObject);
    }
    
    /// <summary>
    /// Plays the hurt sfx for a given enemy
    /// </summary>
    /// <param name="enemy">The ID of the enemy</param>
    public void PlayEnemyHurtSFX(Enemies enemy)
    {
        AK.Wwise.Event refSound = EnemyHurtSFXs[(int)enemy];
        AkUnitySoundEngine.PostEvent(refSound.Id, gameObject);
    }

    /// <summary>
    /// Plays the die sfx for a given enemy
    /// </summary>
    /// <param name="enemy">The ID of the enemy</param>
    public void PlayEnemyDieSFX(Enemies enemy)
    {
        AK.Wwise.Event refSound = EnemyDieSFXs[(int)enemy];
        AkUnitySoundEngine.PostEvent(refSound.Id, gameObject);
    }

    /// <summary>
    /// Plays the walk sfx for a given enemy
    /// </summary>
    /// <param name="enemy">The ID of the enemy</param>
    public void PlayEnemyWalkSFX(Enemies enemy)
    {
        AK.Wwise.Event refSound = EnemyWalkSFXs[(int)enemy];
        AkUnitySoundEngine.PostEvent(refSound.Id, gameObject);
    }

    /// <summary>
    /// Plays the voice sfx for a given enemy
    /// </summary>
    /// <param name="enemy">The ID of the enemy</param>
    public void PlayEnemyVoiceSFX(Enemies enemy)
    {
        AK.Wwise.Event refSound = EnemyVoiceSFXs[(int)enemy];
        AkUnitySoundEngine.PostEvent(refSound.Id, gameObject);
    }

    /// <summary>
    /// Plays the spawn sfx for a given enemy
    /// </summary>
    /// <param name="enemy">The ID of the enemy</param>
    public void PlayEnemySpawnSFX(Enemies enemy)
    {
        AK.Wwise.Event refSound = EnemySpawnSFXs[(int)enemy];
        AkUnitySoundEngine.PostEvent(refSound.Id, gameObject);
    }

    /// <summary>
    /// Plays the sfx for losing the level
    /// </summary>
    public void PlayLoseLevelSFX()
    {
        AkUnitySoundEngine.PostEvent(LoseLevelSFX.Id, gameObject);
    }

    /// <summary>
    /// Plays the sfx for wining the level
    /// </summary>
    public void PlayWinLevelSFX()
    {
        AkUnitySoundEngine.PostEvent(LoseLevelSFX.Id, gameObject);
    }

    #pragma warning disable IDE0060 // Remove unused parameter warnings
    public void PlayManaGainOrLossSFX(float manaGain, float manaLose)
    {
        // Placeholder, do whatever you need here henry (and remove the pragmas)
        if (manaGain > 0)
        {
            AkUnitySoundEngine.PostEvent(ManaGainSFX.Id, gameObject);
        }
        else if (manaGain < 0)
        {
            AkUnitySoundEngine.PostEvent(ManaLoseSFX.Id, gameObject);
        }
    }
    #pragma warning restore IDE0060 // Re-enable unused parameter warnings

    public void PlayClickSFX()
    {
        AkUnitySoundEngine.PostEvent(UIClickSFX.Id, gameObject);
    }
}
