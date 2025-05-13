using System;
using UnityEngine;

public class FTimer
{
    private float sumTime = 0.0f;
    private float gap = 1.0f;
    public void SetGap(float inGap)
    {
        gap = inGap;
    }
    /// <summary>
    /// 如果已经到达时间间隔，返回true并把累计时间置0
    /// </summary>
    /// <returns></returns>
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
