/********************************************
 * filename: TutorialPopup.cs
 * Author: Santiago Caprarulo
 * Description: pops a tutorial up
 * ******************************************/
using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    [SerializeField] bool StopTime = true;

    private void Start()
    {
        if (StopTime) Time.timeScale = 0.0f;
    }

    public void ContinueTime()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);
    }
}
