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
    [Header("Music")]
    [SerializeField] AK.Wwise.Event MenuMusic;
    [SerializeField] AK.Wwise.Event GameMusic1;
    [SerializeField] AK.Wwise.Event GameMusic2;
    [SerializeField] bool PlayMainMenuMusicOnStart = false;

    [Header("Towers")]
    [SerializeField] List<AK.Wwise.Event> TowerDropSFXs;
    [SerializeField] List<AK.Wwise.Event> TowerShootSFXs;
    [SerializeField] List<AK.Wwise.Event> TowerDieSFXs;
    [SerializeField] List<AK.Wwise.Event> TowerRemoveSFXs;
    [SerializeField] AK.Wwise.Event TowerGenericDrop;
    [SerializeField] AK.Wwise.Event TowerGenericDeath;
    [SerializeField] AK.Wwise.Event TowerGenericClick;
    [SerializeField] AK.Wwise.Event TowerGenericHurt;
    [SerializeField] AK.Wwise.Event TowerGenericRemove;

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
    [SerializeField] AK.Wwise.Event PauseSFX;

    // Sound IDS
    private static uint MainMenuID = 0;
    private static uint GameMusic1ID = 0;
    private static uint GameMusic2ID = 0;

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
        guy = 0,
        mech = 1
    }

    private void Start()
    {
        // Plays the main menu music if it is not playing
        if (MainMenuID == 0 && PlayMainMenuMusicOnStart )
            MainMenuID = AkUnitySoundEngine.PostEvent(MenuMusic.Id, gameObject);
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// Plays a drop sfx for a given tower
    /// </summary>
    /// <param name="tower">The ID of the tower</param>
    public void PlayTowerDropSFXs(Towers tower)
    {
        AK.Wwise.Event refSound = TowerDropSFXs[(int)tower];
        AkUnitySoundEngine.PostEvent(refSound.Id, gameObject);
        AkUnitySoundEngine.PostEvent(TowerGenericDrop.Id, gameObject);
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
        AkUnitySoundEngine.PostEvent(TowerGenericDeath.Id, gameObject);
    }

    public void PlayTowerSellSFX(Towers tower)
    {
        AK.Wwise.Event refSound = TowerRemoveSFXs[(int)tower];
        AkUnitySoundEngine.PostEvent(refSound.Id, gameObject);
        AkUnitySoundEngine.PostEvent(TowerGenericRemove.Id, gameObject);
    }

    /// <summary>
    /// Plays a hurt sfx for a tower
    /// </summary>
    public void PlayTowerHurtSFX()
    {
        AkUnitySoundEngine.PostEvent(TowerGenericHurt.Id, gameObject);
    }

    /// <summary>
    /// Plays a click sfx for a tower
    /// </summary>
    public void PlayTowercClickSFX()
    {
        AkUnitySoundEngine.PostEvent(TowerGenericClick.Id, gameObject);
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
    /// <summary>
    /// Plays a mana gain or loss sfx
    /// </summary>
    /// <param name="manaGain">The current mana gain of the manabar</param>
    /// <param name="maxMana">The maximum amount of mana</param>
    public void PlayManaGainOrLossSFX(float manaGain, float maxMana)
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

    /// <summary>
    /// Plays a click sfx
    /// </summary>
    public void PlayClickSFX()
    {
        AkUnitySoundEngine.PostEvent(UIClickSFX.Id, gameObject);
    }

    /// <summary>
    /// Stops all music and plays menu music
    /// </summary>
    public void PlayMenuMusic()
    {
        if (GameMusic1ID != 0)
        {
            AkUnitySoundEngine.StopPlayingID(GameMusic1ID);
            GameMusic1ID = 0;
        }
        if (GameMusic2ID != 0)
        {
            AkUnitySoundEngine.StopPlayingID(GameMusic2ID);
            GameMusic2ID = 0;
        }
        AkUnitySoundEngine.StopAll();
        MainMenuID = AkUnitySoundEngine.PostEvent(MenuMusic.Id, gameObject);
    }

    /// <summary>
    /// Stops all music and plays game music track 1
    /// (called in the unity inspector i believe)
    /// </summary>
    public void PlayGameMusic1()
    {
        if (MainMenuID != 0)
        {
            AkUnitySoundEngine.StopPlayingID(MainMenuID);
            MainMenuID = 0;
        }
        if (GameMusic2ID != 0)
        {
            AkUnitySoundEngine.StopPlayingID(GameMusic2ID);
            GameMusic2ID = 0;
        }
        AkUnitySoundEngine.StopAll();
        GameMusic1ID = AkUnitySoundEngine.PostEvent(GameMusic1.Id, gameObject);

    }

    /// <summary>
    /// Stops all music and plays game music track 2
    /// </summary>
    public void PlayGameMusic2()
    {
        if (MainMenuID != 0)
        {
            AkUnitySoundEngine.StopPlayingID(MainMenuID);
            MainMenuID = 0;
        }
        if (GameMusic1ID != 0)
        {
            AkUnitySoundEngine.StopPlayingID(GameMusic1ID);
            GameMusic1ID = 0;
        }
        AkUnitySoundEngine.StopAll();
        GameMusic2ID = AkUnitySoundEngine.PostEvent(GameMusic2.Id, gameObject);
    }

    public void PlayPauseSFX()
    {
        AkUnitySoundEngine.PostEvent(PauseSFX.Id, gameObject);
    }
}
