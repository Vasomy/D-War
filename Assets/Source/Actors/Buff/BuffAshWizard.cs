using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAshWizard : BuffTemporary
{

    public float damage = 0.2f;
    public float damageTimer;
    public float damageCooldown = 0.2f;
    public float duration;
    AEnemyActor aEnemyActor = null;

    public override void BuffBegin(GameObject obj)
    {
        timer = duration;
        aEnemyActor = obj.GetComponent<AEnemyActor>();
    }
    public override void Buffing(GameObject obj)
    {
        damageTimer -= Time.deltaTime;
        if (damageTimer < 0)
        {
            aEnemyActor.GetDamage(damage);
        }
    }
    public override void BuffEnd(GameObject obj)
    {

    }
}
