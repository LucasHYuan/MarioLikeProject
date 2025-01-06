using UnityEngine;
using UnityEngine.Events;

public class LevelPauser : Singleton<LevelPauser>
{
    public bool paused { get; set; }
    public bool canPaused { get; set; }
    public UIAnimator pauseScreen;
    public UnityEvent OnPause;
    public UnityEvent OnUnPause;
    public virtual void Pause(bool value)
    {
        if (paused != value)
        {
            if (!paused)
            {
                Game.LockCursor(false);
                paused = true;
                Time.timeScale = 0;
                pauseScreen.SetActive(true);
                pauseScreen?.Show();
                OnPause?.Invoke();
            }
        }
        else
        {
            Game.LockCursor();
            paused = false;
            Time.timeScale = 1;
            pauseScreen?.Hide();
            OnUnPause?.Invoke();
        }
    }
}