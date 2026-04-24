
/*******************************************
 * Filename: EnemyKillPlayer.cs
 * Author: Santiago Caprarulo
 * Description: Kills the player (loses the game)
 * if an enemy enters it's collider
 * *****************************************/

using UnityEngine;

public class EnemyKillPlayer : MonoBehaviour
{
    AudioManager refAudioManager;
    private void Start()
    {
        refAudioManager = FindFirstObjectByType<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If an enemy is hitting the collider
        if (collision.CompareTag("Enemy"))
        {
            // Lose the game
            refAudioManager?.PlayMenuMusic();
            refAudioManager?.PlayLoseLevelMusic();
            UnityEngine.SceneManagement.SceneManager.LoadScene("SkillIssue");
        }
    }
}
