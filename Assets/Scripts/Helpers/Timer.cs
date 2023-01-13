using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public float amount;
    private float timeStamp = -1;
    private float watchTimeStamp = -1;

    //Elapsed number of seconds on the StopWatch
    public float elapsedTime
    {
        get { return Time.time - watchTimeStamp; }
    }

    //How many seconds left until timer is done
    public float timeLeft
    {
        get
        {
            if (timeStamp < Time.time)
                return 0;
            return timeStamp - Time.time;
        }
    }

    //Not implemented
    public void StartWatch()
    {
        watchTimeStamp = Time.time;
    }

    //Returns number of seconds since starting watch
    public float StopWatch()
    {
        //elapsedTime = Time.time - timeStamp;
        return elapsedTime;
    }

    //Start a timer for a given number of seconds, use CheckTime() to see if timer has elapsed
    public void StartTimerForSeconds(float seconds)
    {
        timeStamp = Time.time + seconds;
        //Debug.Log("Starting a timer for " + seconds + " seconds");
    }

    // Add seconds onto an active timer
    public void AddSecondsToTimer(float seconds)
    {
        if (timeStamp < 0)
            return;

        timeStamp += seconds;
    }

    // Resets the timestamp
    public void CancelTimer()
    {
        timeStamp = -1;
    }

    public bool checkTime()
    {
        //if(timeStamp == -1)
        //    return false;

        if (timeStamp > Time.time)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
