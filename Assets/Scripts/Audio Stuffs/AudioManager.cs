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
    [SerializeField] bool PlayMainMenu = false;
    [Header("Music")]
    [SerializeField] AK.Wwise.Event MenuMusic;
    [SerializeField] AK.Wwise.Event GameMusic1;
    [SerializeField] AK.Wwise.Event GameMusic2;

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

    static bool HasStartedMenuMusic = false;

    static AudioManager Instance;
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
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = GetComponent<AudioManager>();
        DontDestroyOnLoad(gameObject);
        // Tell WWise MusicStateGroup should exist because it is needed for music to play
        AkUnitySoundEngine.PostEvent("Play_MusicPlaylist", gameObject);
        AkUnitySoundEngine.PostEvent("Play_MMusicPlaylistPlaying", gameObject);
        AkUnitySoundEngine.PostEvent("Play_MMusicPlaylistPaused", gameObject);
        // Plays the main menu music if it is not playing
        if (PlayMainMenu && !HasStartedMenuMusic)
        {
            PlayMenuMusic();
            HasStartedMenuMusic = true;
        }
    }

    /// <summary>
    /// Plays a drop sfx for a given tower
    /// </summary>
    /// <param name="tower">The ID of the tower</param>
    public void PlayTowerDropSFXs(GameObject spawnLocation, Towers tower)
    {
        AK.Wwise.Event refSound = TowerDropSFXs[(int)tower];
        AkUnitySoundEngine.PostEvent(refSound.Id, spawnLocation);
        AkUnitySoundEngine.PostEvent(TowerGenericDrop.Id, spawnLocation);
    }

    /// <summary>
    /// Plays a shoot sfx for a given tower
    /// </summary>
    /// <param name="tower">The ID of the tower</param>
    public void PlayTowerShootSFX(GameObject spawnLocation, Towers tower)
    {
        AK.Wwise.Event refSound = TowerShootSFXs[(int)tower];
        AkUnitySoundEngine.PostEvent(refSound.Id, spawnLocation);
    }

    /// <summary>
    /// Plays a death sfx for a given tower
    /// </summary>
    /// <param name="tower">The ID of the tower</param>
    public void PlayTowerDieSFX(GameObject spawnLocation, Towers tower)
    {
        AK.Wwise.Event refSound = TowerDieSFXs[(int)tower]; 
        AkUnitySoundEngine.PostEvent(refSound.Id, spawnLocation);
        AkUnitySoundEngine.PostEvent(TowerGenericDeath.Id, spawnLocation);
    }

    public void PlayTowerSellSFX(GameObject spawnLocation, Towers tower)
    {
        AK.Wwise.Event refSound = TowerRemoveSFXs[(int)tower];
        AkUnitySoundEngine.PostEvent(refSound.Id, spawnLocation);
        AkUnitySoundEngine.PostEvent(TowerGenericRemove.Id, spawnLocation);
    }

    /// <summary>
    /// Plays a hurt sfx for a tower
    /// </summary>
    public void PlayTowerHurtSFX(GameObject spawnLocation)
    {
        AkUnitySoundEngine.PostEvent(TowerGenericHurt.Id, spawnLocation);
    }

    /// <summary>
    /// Plays a click sfx for a tower
    /// </summary>
    public void PlayTowercClickSFX(GameObject spawnLocation)
    {
        AkUnitySoundEngine.PostEvent(TowerGenericClick.Id, spawnLocation);
    }

    /// <summary>
    /// Plays the attack sfx for a given enemy
    /// </summary>
    /// <param name="enemy">The ID of the enemy</param>
    public void PlayEnemyAttackSFX(GameObject spawnLocation, Enemies enemy)
    {
        AK.Wwise.Event refSound = EnemyAttackSFXs[(int)enemy];
        AkUnitySoundEngine.PostEvent(refSound.Id, spawnLocation);
    }
    
    /// <summary>
    /// Plays the hurt sfx for a given enemy
    /// </summary>
    /// <param name="enemy">The ID of the enemy</param>
    public void PlayEnemyHurtSFX(GameObject spawnLocation, Enemies enemy)
    {
        AK.Wwise.Event refSound = EnemyHurtSFXs[(int)enemy];
        AkUnitySoundEngine.PostEvent(refSound.Id, spawnLocation);
    }

    /// <summary>
    /// Plays the die sfx for a given enemy
    /// </summary>
    /// <param name="enemy">The ID of the enemy</param>
    public void PlayEnemyDieSFX(GameObject spawnLocation, Enemies enemy)
    {
        AK.Wwise.Event refSound = EnemyDieSFXs[(int)enemy];
        AkUnitySoundEngine.PostEvent(refSound.Id, spawnLocation);
    }

    /// <summary>
    /// Plays the walk sfx for a given enemy
    /// </summary>
    /// <param name="enemy">The ID of the enemy</param>
    public void PlayEnemyWalkSFX(GameObject spawnLocation, Enemies enemy)
    {
        AK.Wwise.Event refSound = EnemyWalkSFXs[(int)enemy];
        AkUnitySoundEngine.PostEvent(refSound.Id, spawnLocation);
    }

    /// <summary>
    /// Plays the voice sfx for a given enemy
    /// </summary>
    /// <param name="enemy">The ID of the enemy</param>
    public void PlayEnemyVoiceSFX(GameObject spawnLocation, Enemies enemy)
    {
        AK.Wwise.Event refSound = EnemyVoiceSFXs[(int)enemy];
        AkUnitySoundEngine.PostEvent(refSound.Id, spawnLocation);
    }

    /// <summary>
    /// Plays the spawn sfx for a given enemy
    /// </summary>
    /// <param name="enemy">The ID of the enemy</param>
    public void PlayEnemySpawnSFX(GameObject spawnLocation, Enemies enemy)
    {
        AK.Wwise.Event refSound = EnemySpawnSFXs[(int)enemy];
        AkUnitySoundEngine.PostEvent(refSound.Id, spawnLocation);
    }

    /// <summary>
    /// Plays the sfx for losing the level
    /// </summary>
    public void PlayLoseLevelMusic()
    {
        //AkUnitySoundEngine.PostEvent(LoseLevelSFX.Id, gameObject);
        AkUnitySoundEngine.SetState("MusicStateGroup", "Lose");
    }

    /// <summary>
    /// Plays the sfx for wining the level
    /// </summary>
    public void PlayWinLevelMusic()
    {
        AkUnitySoundEngine.SetState("MusicStateGroup", "Win");
    }

#pragma warning disable IDE0060 // Remove unused parameter warnings
    /// <summary>
    /// Plays a mana gain or loss sfx
    /// </summary>
    /// <param name="manaGain">The current mana gain of the manabar</param>
    /// <param name="maxMana">The maximum amount of mana</param>
    public void PlayManaGainOrLossSFX(GameObject spawnLocation, float manaGain, float maxMana)
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
        AkUnitySoundEngine.SetState("MusicStateGroup", "Title");
    }

    /// <summary>
    /// Stops all music and plays game music track 1
    /// (called in the unity inspector i believe)
    /// <param name="waveIntensity"/>int of the wave from 1 to 3</param>
    /// </summary>
    public void PlayGameMusic(int waveIntensity)
    {
        AkUnitySoundEngine.SetState("MusicStateGroup", $"Battle{waveIntensity}");
    }

    /// <summary>
    /// Plays or pauses the sound muffling when the game is paused
    /// </summary>
    /// <param name="paused">True if the game is NOT paused, false if it is being paused</param>
    public void PlayPauseMusic(bool paused)
    {
        if (paused)
            // Muffle music
            AkUnitySoundEngine.SetState("PauseOrPlay", "Pause");
        else
            // Unmuffle music
            AkUnitySoundEngine.SetState("PauseOrPlay", "Play");

    }
}
