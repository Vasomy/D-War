using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTSlow : BuffTemporary
{
    public float unitSpeed = 0.0f;
    public float buffNum =  0.0f;
    public bool buffPercent = false;
    public float buffDuration = 5.0f;

    public void BuffBegin(GameObject obj, float num, bool percent)
    {
        base.BuffBegin(obj, num, percent);
        // timer = buffDuration;
        // if (percent)
        // {
        //     buffNum = num;
        //     obj.iSpeed = obj.iSpeed * num;
        //     buffPercent = percent;
        // }
    }

    public void BuffEnd(GameObject obj)
    {
        base.BuffEnd(obj);
        // if(buffPercent)
        // {
        //     obj.iSpeed = obj.iSpeed / num;
        // }
    }
}
