using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class Mana : MonoBehaviour
{
    [Tooltip("The player's maximum mana")]
        [SerializeField] float MaxMana = 30f;
    [Tooltip("The player's base mana regeneration")]
        [SerializeField] float BaseManaRegen = 5f;
    [Tooltip("A linerenderer containing 2 positions, where position 1 is the end and position 0 is the start")]
        [SerializeField] LineRenderer ManaBar;
    [Tooltip("Textbox for the mana regeneration")]
        [SerializeField] TextMeshProUGUI manaGainText;

    [Header("ScreenTint")]
    [SerializeField] Volume volume;
    [Tooltip("Enables the vignette")]
        [SerializeField] bool enableVignette;
    [Tooltip("The maximum vignette intensity (0 to 1)")]
        [SerializeField] float maxVignetteEffect = 0.5f;

    private Vignette screenTint;

    float CurrentMana;
    Vector2 ManaBarInitialPosition;

    /// <summary>
    /// Initialize variables
    /// </summary>
    private void Start()
    {
        // Max mana
        CurrentMana = MaxMana;
        ManaBarInitialPosition = ManaBar.GetPosition(1);
        // Vignette
        if (volume.profile.TryGet<Vignette>(out screenTint))
        {
            screenTint.intensity.value = 0;
        }
        else
        {
            Debug.LogError("No vignette / volume found for the screen vignette!");
        }
    }

    /// <summary>
    /// Handles mana regeneration and mana vignette
    /// </summary>
    void Update()
    {
        HandleManaRegeneration();
    }

    /// <summary>
    /// Increases or decreases the mana of the player depending on their regeneration rate
    /// Also updates the linerenderer and effects
    /// </summary>
    void HandleManaRegeneration()
    {
        // Get the mana regen
        float manaRegen = CalculateCurrentManaRegen();
        // Add that to the current mana
        CurrentMana += manaRegen * Time.deltaTime;
        // Make sure that the mana does not exceed max mana
        CurrentMana = CurrentMana > MaxMana ? MaxMana : CurrentMana;
        // Update the manaLine
        float manaBarPosition = Mathf.Lerp(0, ManaBarInitialPosition.x, CurrentMana / MaxMana);
        ManaBar.SetPosition(1, new Vector3(manaBarPosition, 0, 0));
        // Update the display
        if (manaRegen >= 0)
        {
            // This means gaining mana
            manaGainText.text = $"+{manaRegen : 0}";
        }
        else
        {
            // This means losing mana
            manaGainText.text = $"{manaRegen: 0}";
        }
        // Update the vignette
        if (enableVignette) screenTint.intensity.value = Mathf.Lerp(maxVignetteEffect, 0, CurrentMana / MaxMana);
        // Check if the player died
        if (CurrentMana <= 0)
        {
            // Make the player lose :/
            Skissue();
        }
    }

    /// <summary>
    /// Calculates the mana regen (can be negative) and returns it
    /// <return>float containing the mana gained/lost</return>
    /// </summary>
    float CalculateCurrentManaRegen()
    {
        // Create the base regen
        float regen = BaseManaRegen;

        // Get every tower on field
        TowerDataStorage[] towers = FindObjectsByType<TowerDataStorage>(FindObjectsSortMode.None);
        // Get their mana cost and deduct it from regen
        foreach (TowerDataStorage currTower in towers)
        {
            regen -= currTower.GetTowerManaCost();
        }

        // TODO: Get every mana booster on the field and increase regen by that amount

        // Return the mana
        return regen;
    }

    /// <summary>
    /// Code that runs when the player loses
    /// </summary>
    void Skissue()
    {
        // TODO: do something cool here eventually
        Debug.Log("Player loses lmao");

        // Set lose screen
        SceneManager.LoadScene("SkillIssue");
    }
}
