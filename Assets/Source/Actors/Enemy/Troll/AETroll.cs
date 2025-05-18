using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AETroll : AEnemyActor , ICanMove
{
    ICanMove icm => GetComponent<ICanMove>();
    Vector2 ICanMove.iDirection { get; set; } = Vector2.zero;
    float ICanMove.iSpeed { get; set; } = 1.0f;
    FlowFieldPathFinding ICanMove.iPathFinding { get; set;} = null;

    public void ChangeToMoveState()
    {
    }
    
    protected override void FindTarget()
    {
        base.FindTarget();
    }

    protected override void Attack()
    {
        base.Attack();
    }

    protected override void AttackProject()
    {
        float disTarget = CompareFunction.EulerDistance(EAttack.attackTarget.transform.position, transform.position);
        if (disTarget < EAttack.attackRadius)
        {
            if (EAttack.attackTimer < 0.0)
            {
                EAttack.attackTimer = EAttack.attackCooldown;
                Attack();
            }
        }
        else
        {
            MMoveSystem.MoveTo(this, EAttack.attackTarget.transform.position);
            icm.Move(rb2d);
            Debug.Log("Move" + EAttack.attackTarget);
        }
    }

    protected override void FindProject()
    {
        FindTarget();
        EFind.findTimer = 3.0f;
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();

        EAttack.attackTimer -= Time.deltaTime;
        EFind.findTimer -= Time.deltaTime;

        if (EFind.findTimer <= 0.0)
        {
            FindProject();
        }
        if (EAttack.attackTarget != null)
        {
            AttackProject();
        }
    }

    protected override void Init()
    {
        base.Init();
    }
}
