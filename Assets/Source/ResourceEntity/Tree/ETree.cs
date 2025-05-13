using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETree : ECollectableEntity
{
    public Collider2D cd2d;
    protected override void Init()
    {
        base.Init();
        //Debug.Log(nam);
        EntityMemoryPoolProxy<ETree>.Register(this);
    }
    public override void GetResource()
    {
        base.GetResource();
        FGameStats.instance.resoureStats.woods++;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, collectRadius);
    }

    public override void Destory()
    {
        EntityMemoryPoolProxy<ETree>.Free(this);
        GridManager.CalculateOccupiedArea(uid, transform.position, lw, rw, th, dh, false, true);
    }
}
