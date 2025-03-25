using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETree : ECollectableEntity 
{
    public Collider2D cd2d;
    public override void GetResource()
    {
        base.GetResource();
        GameContext.instance.resourceStats.woods++;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, collectRadius);
    }
}
