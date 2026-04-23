using UnityEngine;

public class TutorialButten : MonoBehaviour
{
    private void Awake()
    {
        if (TutorialManager.tutorialOccured == false)
            Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void scoobydoo()
    {
        TutorialManager.tutorialOccured = false;
    }
}
