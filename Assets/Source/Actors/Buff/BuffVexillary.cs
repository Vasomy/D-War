using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffVexillary : BuffTemporary
{
    FPercent speedchangeee = 00.01f;
    float attackForceChange = 2.0f;
    AEnemyActor aEnemyActor = null;
    public override void BuffBegin(GameObject obj)
    {

        base.BuffBegin(obj);
        Debug.Log("buff begin");
        aEnemyActor = obj.GetComponent<AEnemyActor>();
        aEnemyActor.EAttack.attackForce *= attackForceChange;
    }
    public override void Buffing(GameObject obj)
    {
        
    }
    public override void BuffEnd(GameObject obj)
    {
        aEnemyActor.EAttack.attackForce /= attackForceChange;
    }
}
