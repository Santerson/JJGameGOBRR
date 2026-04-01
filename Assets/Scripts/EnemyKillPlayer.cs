
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
        if (collision.CompareTag("Enemy"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SkillIssue");
            refAudioManager.PlayLoseLevelSFX();
        }
    }
}
