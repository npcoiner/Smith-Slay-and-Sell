using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public enum GameState
    {
        Idle,
        Starting,
        Active,
        End,
        Paused,
    }

    public static GameStateManager Instance { get; private set; }
    public GameState CurrentState { get; private set; } = GameState.Active;

    public event EventHandler OnPauseAction;
    public event EventHandler OnStateChange;
    public event EventHandler OnUnPauseAction;

    private float DEFAULT_GAME_TIME = 10f;
    private float countDownTimer = 3f;
    private float gameTimer;

    [SerializeField]
    TextMeshProUGUI countDownTimerText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        BeginGame();
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case GameState.Idle:
                // No update logic needed for Idle state
                break;

            case GameState.Starting:
                countDownTimerText.text = Mathf.Ceil(countDownTimer).ToString("0");
                countDownTimer -= Time.deltaTime;
                if (countDownTimer <= 0)
                {
                    countDownTimerText.text = "";
                    StartGame();
                }
                break;

            case GameState.Active:
                gameTimer -= Time.deltaTime;
                if (gameTimer <= 0f)
                {
                    ChangeState(GameState.End);
                }
                break;

            case GameState.End:
                // Handle end-game sequence
                break;

            case GameState.Paused:
                // Do nothing, time scale is 0
                break;
        }
    }

    public void PauseGame()
    {
        if (CurrentState == GameState.Active)
        {
            Time.timeScale = 0;
            //Trigger Event OnPauseAction
            OnPauseAction?.Invoke(this, EventArgs.Empty);
            Debug.Log("RAH");

            ChangeState(GameState.Paused);
        }
    }

    public void UnPauseGame()
    {
        if (CurrentState == GameState.Paused)
        {
            Time.timeScale = 1;
            OnUnPauseAction?.Invoke(this, EventArgs.Empty);
            ChangeState(GameState.Active);
        }
    }

    public void BeginGame()
    {
        countDownTimer = 3f;
        gameTimer = DEFAULT_GAME_TIME;
        ChangeState(GameState.Starting);
    }

    public void StartGame()
    {
        if (CurrentState == GameState.Starting)
        {
            Time.timeScale = 1;
            ChangeState(GameState.Active);
        }
    }

    public void EndGame()
    {
        if (CurrentState != GameState.End)
        {
            ChangeState(GameState.End);
        }
    }

    public void RestartGame()
    {
        if (CurrentState != GameState.Idle)
        {
            Time.timeScale = 1;
            ChangeState(GameState.Idle);
        }
    }

    private void ChangeState(GameState newState)
    {
        CurrentState = newState;
        OnStateChange?.Invoke(this, EventArgs.Empty);
        Debug.Log($"Changing GameState to {newState}");
    }

    public bool IsPaused()
    {
        if (CurrentState == GameState.Paused)
        {
            return true;
        }
        return false;
    }

    public float GetGameTimeRemaining()
    {
        return gameTimer;
    }

    public float GetDefaultGameTime()
    {
        return DEFAULT_GAME_TIME;
    }
}
