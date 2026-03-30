using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] animationFrames; // Drag all your extracted frames here
    public float framesPerSecond = 10f;
    public bool isActivated = false;

    private int currentFrame;
    private float timer;

    void Update()
    {
        if (isActivated && animationFrames.Length > 0)
        {
            PlayLoop();
        }
        else
        {
            ResetToFirstFrame();
        }
    }

    void PlayLoop()
    {
        timer += Time.deltaTime;

        if (timer >= 1f / framesPerSecond)
        {
            timer -= 1f / framesPerSecond;
            currentFrame = (currentFrame + 1) % animationFrames.Length;
            spriteRenderer.sprite = animationFrames[currentFrame];
        }
    }

    void ResetToFirstFrame()
    {
        if (animationFrames.Length > 0 && spriteRenderer.sprite != animationFrames[0])
        {
            currentFrame = 0;
            timer = 0;
            spriteRenderer.sprite = animationFrames[0];
        }
    }
}
