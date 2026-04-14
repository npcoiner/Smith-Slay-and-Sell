using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button quitButton;

    private void Awake()
    {
        //Lambda expression
        playButton.onClick.AddListener(() =>
        {
            //Click
            Loader.LoadScene(Loader.Scene.Level_One);
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
