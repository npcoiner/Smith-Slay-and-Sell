using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField]
    private Image timerImage;

    private float totalGameTime;

    void Start()
    {
        totalGameTime = GameStateManager.Instance.GetDefaultGameTime();
    }

    void Update()
    {
        timerImage.fillAmount = 0f;
        // Example: Update the timer based on game logic
        // float currentTime = Time.time;
        float currentTime = GameStateManager.Instance.GetGameTimeRemaining();
        timerImage.fillAmount = currentTime / totalGameTime;
    }
}
