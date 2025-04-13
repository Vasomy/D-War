using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETree : ECollectableEntity 
{
    public Collider2D cd2d;
    protected override void Init()
    {
        base.Init();
        var nam = typeof(ETree).Name;
        Debug.Log(nam);
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
}
