using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject CreditsOverlay;

    public void ClickResume()
    {
        GameManager.Instance.TogglePause();
    }

    public void ClickQuit()
    {
        GameManager.Instance.CloseGame();
    }    
}
