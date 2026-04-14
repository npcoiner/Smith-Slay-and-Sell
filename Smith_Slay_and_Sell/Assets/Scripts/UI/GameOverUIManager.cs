using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    void Awake()
    {
        gameOverUI.SetActive(false);
    }

    void Start()
    {
        GameStateManager.Instance.OnStateChange += GameManager_OnStateChange;
    }

    private void GameManager_OnStateChange(object idk, EventArgs e)
    {
        if (GameStateManager.Instance.CurrentState == GameStateManager.GameState.End)
        {
            gameOverUI.SetActive(true);
            int score = ScoreHandler.Instance.GetScore();
            scoreText.text = "Final Score: " + score;
        }
        else
        {
            gameOverUI.SetActive(false);
        }
    }
}
