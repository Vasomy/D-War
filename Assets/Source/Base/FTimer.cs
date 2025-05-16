using System;
using UnityEngine;

public class FTimer
{
    private float sumTime = 0.0f;
    /// <summary>
    /// set gap <=0 ,if you dont want reset sumTime
    /// </summary>
    private float gap = 1.0f;
    public void SetGap(float inGap)
    {
        gap = inGap;
    }
    public float GetTime()
    {
        return sumTime;
    }
    /// <summary>
    /// 如果已经到达时间间隔，返回true并把累计时间置0
    /// return value 可以被忽略
    /// </summary>
    /// <returns></returns>
    public bool Timer()
    {
        sumTime += Time.deltaTime;
        if(gap > 0.0f && sumTime >= gap)
        {
            sumTime = 0.0f;
            return true;
        }
        return false;
    }
}
