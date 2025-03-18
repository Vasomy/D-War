using System;
using UnityEngine;

public class FTimer
{
    private float sumTime = 0.0f;
    private float gap = 1.0f;
    public void SetGap(float inGap)
    {
        gap = inGap;
        sumTime = 0.0f;
    }
    public bool Timer()

    {
        sumTime += Time.deltaTime;
        if(sumTime >= gap)
        {
            sumTime = 0.0f;
            return true;
        }
        return false;
    }
}
