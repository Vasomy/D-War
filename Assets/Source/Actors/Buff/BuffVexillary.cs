using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffVexillary : Buff
{
    FPercent speedchangeee = 00.01f;
    float speedchange = 0.8f;
    public override void BuffBegin(GameObject obj)
    {

        base.BuffBegin(obj);
        Debug.Log("buff begin");
    }
}
