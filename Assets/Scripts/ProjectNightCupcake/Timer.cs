using UnityEngine;

public class Timer : MonoBehaviour {

    public delegate void OnTimerComplete(Timer t);

    private OnTimerComplete _onComplete = null;
    private OnTimerComplete OnComplete { get { return _onComplete; } set { _onComplete = value; } }

    private float ElapsedTime { get; set; }
    private float TimerLength { get; set; }

    [SerializeField]
    private bool _active = false;
    public  bool Active { get { return _active; } private set { _active = value; } }

    [SerializeField]
    private int _loops = 1;
    public int Loops { get { return _loops; } private set { _loops = value; } }

    private int CurrentLoop { get; set; }

    public Timer StartTimer(float timerLength, int loops, OnTimerComplete onComplete=null)
    {
        TimerLength = timerLength;
        Loops = loops;
        OnComplete = onComplete;
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
                    Debug.Log("Timer complete calling " + OnComplete.GetType().Name);
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
