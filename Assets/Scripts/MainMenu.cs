using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject CreditsOverlay;

    private void Start()
    {
        CreditsOverlay.SetActive(false);
    }

    public void ClickStart()
    {
        GameManager.Instance.StartGame();
    }

    public void ClickCredits()
    {
        CreditsOverlay.SetActive(!CreditsOverlay.activeSelf);
    }

    public void ClickQuit()
    {
        GameManager.Instance.CloseGame();
    }    
}
