using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class AEBat : AEnemyActor , ICanMove
{
    Vector2 ICanMove.iDirection { get; set; } = Vector2.zero;
    float ICanMove.iSpeed { get; set; } = 1.0f;

    public GameObject currentTarget = null;

    public float attackRadius = 1.0;

    public float attackForce = 1;

    public float attackCooldown = 1.0;

    public float attackTimer = 0.0;

    public float disTarget = 1e9;
        
    private GameObject FindTarget()
    {
        var allFriendEtt = GameObject.FindGameObjectsWithTag("friendly");
        float minDis = 1e9;
        GameObject _target;
        foreach(var ett in allFriendEtt)
        {
            float dis  = CompareFunction.EulerDistance(ett.transform.position, transform.position);
            if(dis < minDis)
            {
                _target = ett;
            }
        }
        reutrn _target;
    }

    private void AttackTarget()
    {
        // TODO:
            // Animation

            //Target decrease health
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        attackTimer -= Time.deltaTime;

        if (currentTarget == null)
        {
            currentTarget = FindTarget();
        }
        disTarget = CompareFunction.EulerDistance(ett.transform.position, transform.position);
        if(disTarget < attackRadius)
        {
            if(attackTimer < 0.0)
            {
                attackTimer  = attackCooldown;
                AttackTarget();
            }
        }
        else
        {
            MMoveSystem.MoveTo(this, currentTarget.transform.position);
        }
    }

    public override void Init()
    {
        base.Init();

    }
}
