using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerNode : MonoBehaviour
{
    [SerializeField] bool autostart;
    [SerializeField] bool oneShot;
    [SerializeField] bool paused = true;
    float time_left;
    [SerializeField] float wait_time;

    public UnityEngine.Events.UnityEvent TimerStarted;
    public UnityEngine.Events.UnityEvent Timeout;

    // Start is called before the first frame update
    void Start()
    {
        if (autostart)
        {
            StartTimer();
        }
    }

    public void StartTimer()
    {
        StartTimer(false);
    }

    public void StartTimer(float _time)
    {
        if (paused)
        {
            wait_time = _time;
            StartTimer(false);
        }
        else
        {
            time_left += _time;
        }
    }

    public void SetOneShot(bool b)
    {
        oneShot = b;
    }

    public void ResetTImer()
    {
        time_left = 0;
    }

    void StartTimer(bool b)
    {       
        time_left = wait_time;
        paused = b;
        TimerStarted.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (paused == false)
        {
            if (time_left > 0)
            {
                time_left = time_left - Time.deltaTime;
            }
            else
            {
                Timeout.Invoke();
                if (oneShot == false)
                {
                    StartTimer();
                } else
                {
                    time_left = 0;
                    paused = true;
                }
            }
        }
    }
}
