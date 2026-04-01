using UnityEngine;
using UnityEngine.Events;

public class SpriteAnimator : MonoBehaviour
{
    public enum PlayMode { Idle, Looping, Once }

    [Header("Settings")]
    public SpriteRenderer spriteRenderer;
    public Sprite[] animationFrames;
    public float framesPerSecond = 10f;

    [Header("State")]
    public PlayMode currentMode = PlayMode.Idle;

    [Header("Events")]
    public UnityEvent onAnimationComplete;

    private int currentFrame;
    private float timer;

    void Update()
    {
        // Don't run logic if we are Idle or have no frames
        if (currentMode == PlayMode.Idle || animationFrames.Length == 0) return;

        timer += Time.deltaTime;
        float frameTime = 1f / framesPerSecond;

        if (timer >= frameTime)
        {
            timer -= frameTime;
            AdvanceFrame();
        }
    }

    private void AdvanceFrame()
    {
        currentFrame++;

        if (currentFrame >= animationFrames.Length)
        {
            if (currentMode == PlayMode.Looping)
            {
                currentFrame = 0;
            }
            else
            {
                ResetToFirstFrame();
                onAnimationComplete?.Invoke();
                return;
            }
        }

        spriteRenderer.sprite = animationFrames[currentFrame];
    }

    public void PlayOnce()
    {
        if (animationFrames.Length < 2)
        {
            Debug.LogWarning("PlayOnce called but not enough frames to animate.");
            return;
        }

        currentMode = PlayMode.Once;
        timer = 0f;
        currentFrame = 1;
        spriteRenderer.sprite = animationFrames[currentFrame];
    }

    public void PlayLoop()
    {
        currentMode = PlayMode.Looping;
        if (currentFrame == 0) AdvanceFrame();
    }

    public void ResetToFirstFrame()
    {
        currentMode = PlayMode.Idle;
        currentFrame = 0;
        timer = 0;
        if (animationFrames.Length > 0)
            spriteRenderer.sprite = animationFrames[0];
    }
}
