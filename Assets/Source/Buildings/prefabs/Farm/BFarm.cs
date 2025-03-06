using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFarm : BProducer
{

    protected override void Init()
    {
        base.Init();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void LogicUpdate()
    {
        if(timer.Timer())
        {
            Produce();
        }
    }

}
