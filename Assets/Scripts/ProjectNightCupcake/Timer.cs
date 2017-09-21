using UnityEngine;

public class Timer : MonoBehaviour {

    public delegate void OnTimerComplete(Timer t);
    private OnTimerComplete OnComplete { get; set; }

    private float ElapsedTime { get; set; }
    private float TimerLength { get; set; }
    private bool Active { get; set; }
    private int Loops { get; set; }
    private int CurrentLoop { get; set; }

    public Timer(float timerLength)
    {
        OnComplete = null;
        TimerLength = timerLength;
        Loops = 1;
        Active = false;
    }

    public Timer Start()
    {
        Active = true;
        CurrentLoop = 0;
        ElapsedTime = 0f;
        return this;
    }

    public Timer Stop()
    {
        Active = false;
        return this;
    }

    void Update()
    {
        if (Active)
        {
            ElapsedTime += Time.deltaTime;
            if (ElapsedTime >= TimerLength)
            {
                CurrentLoop++;
                if (OnComplete != null)
                {
                    OnComplete(this);
                }
                if (CurrentLoop >= Loops)
                {
                    Active = false;
                }
                else
                {
                    ElapsedTime -= ElapsedTime;
                }
            }
        }
    }

}
