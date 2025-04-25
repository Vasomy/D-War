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

    public override void Generate()
    {
        EntityMemoryPool<AFarmer>.Instance().Get();
    }


    public override bool CalculateBuildingArea(Vector3 position, bool isPreview = false, bool isDelete = false)
    {
        return GridManager.CalculateOccupiedArea(uid, position,
            lWidth, rWidth, uHeight, dHeight,
            isPreview, isDelete);
    }

}
