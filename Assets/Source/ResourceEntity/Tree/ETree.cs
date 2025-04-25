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
        EntityMemoryPool<ETree>.Instance().RegisterObject(gameObject);
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
        EntityMemoryPool<ETree>.Instance().Free(gameObject);
        GridManager.CalculateOccupiedArea(uid, transform.position, lw, rw, th, dh, false, true);
    }
}
