using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField]
    Button mainMenuButton;
    Button closeButton; // TODO implement

    [SerializeField]
    GameObject pauseMenu;

    private void Start()
    {
        GameStateManager.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameStateManager.Instance.OnUnPauseAction += GameInput_OnUnPauseAction;
        mainMenuButton.onClick.AddListener(OnMainMenuClicked);
    }

    private void OnMainMenuClicked()
    {
        // TODO: Add logic to handle main menu click, e.g., quitting the game or changing scene
        Debug.Log("Main Menu Button Clicked!");
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnPauseAction -= GameInput_OnPauseAction;
        GameStateManager.Instance.OnUnPauseAction -= GameInput_OnUnPauseAction;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        Show();
    }

    private void GameInput_OnUnPauseAction(object sender, EventArgs e)
    {
        Hide();
    }

    void Awake()
    {
        if (!pauseMenu)
        {
            Debug.LogError("Pause Menu not set or found!");
        }
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.LoadScene(Loader.Scene.MainMenuScene);
            Time.timeScale = 1f;
        });
        ;
        Hide();
    }

    void Hide()
    {
        pauseMenu.SetActive(false);
    }

    void Show()
    {
        pauseMenu.SetActive(true);
    }
}
