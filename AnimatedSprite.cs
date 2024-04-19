using System;
using GXPEngine;

public class AnimatedSprite : AnimationSprite
{
    public bool IsPlaying { get; private set; }
    private int frameDelay;
    private int frameTimer;
    
    public AnimatedSprite(string filename, int cols, int rows, int frameDelay = 5) : base(filename, cols, rows)
    {
        this.frameDelay = frameDelay;
        frameTimer = 0;
    }

    public void PlayOnce()
    {
        foreach (GameObject child in parent.GetChildren())
        {
            AnimationSprite sprite = child as AnimationSprite;
            if (sprite != null)
            {
                Console.WriteLine(sprite.name);
                sprite.visible = false; // Replace with your desired action
            }
        }

        visible = true; // Make this animation visible
        IsPlaying = true;
        currentFrame = 0; // Start from the first frame
        frameTimer = 0;
    }

    void Update()
    {
        if (IsPlaying)
        {
            frameTimer++;
            if (frameTimer >= frameDelay) // Check if it's time to switch the frame
            {
                if (currentFrame < frameCount - 1)
                {
                    NextFrame(); // Move to the next frame
                    frameTimer = 0; // Reset timer
                }
                else
                {
                    IsPlaying = false; // Stop playing after the last frame
                    visible = false;
                }
            }
        }
    }

    public bool ReadyForAction()
    {
        return (currentFrame < frameCount - 2);
    }
    
    public void SetFrameDelay(int newDelay)
    {
        frameDelay = newDelay;
    }
}