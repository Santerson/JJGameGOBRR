using UnityEngine;
/************************************
 * Filename: TutorialButten.cs
 * Author: Micaiah Mariano
 * Description: destroys TutorialButten if Tutorial is already ready
 * *********************************/

public class TutorialButten : MonoBehaviour
{
    private void Awake()
    {
        if (TutorialManager.tutorialOccured == false)
            Destroy(gameObject);
    }
    // turns on Tutorial
    public void scoobydoo()
    {
        TutorialManager.tutorialOccured = false;
    }
}
