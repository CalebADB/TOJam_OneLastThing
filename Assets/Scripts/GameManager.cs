using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    public bool SkipMenu = true;
    public static bool IsPlaying = false;
    public GameObject MainMenuGO;
    public GameObject PauseMenuGO;

    void Start()
    {
        IsPlaying = false;
        MainMenuGO.SetActive(true);
        PauseMenuGO.SetActive(false);

#if UNITY_EDITOR
        if (SkipMenu) StartGame();
#endif
    }

    public void StartGame()
    {
        IsPlaying = true;
        MainMenuGO.SetActive(false);
    }

    public void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && !MainMenuGO.activeSelf)
        {
            TogglePause();
        }          
    }

    public void TogglePause()
    {
        PauseMenuGO.SetActive(!PauseMenuGO.activeSelf);
        IsPlaying = !PauseMenuGO.activeSelf;
    }

    public void CloseGame()
    {
#if UNITY_EDITOR
        Debug.Break();
#else
        Application.Quit();
#endif
    }
}
