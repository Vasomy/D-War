using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTower : Buildings
{
    protected override void Init()
    {
        base.Init();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override bool CalculateBuildingArea(Vector3 position,bool isPreview = false, bool isDelete = false)
    {
        return GridManager.CalculateOccupiedArea(uid, position,
            lWidth,rWidth,uHeight,dHeight,
            isPreview,isDelete);
    }
}
