using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : AEnemyActor, ICanMove
{
    ICanMove icm => GetComponent<ICanMove>();
    Vector2 ICanMove.iDirection { get; set; } = Vector2.zero;
    float ICanMove.iSpeed { get; set; } = 1.0f;
    FlowFieldPathFinding ICanMove.iPathFinding { get; set; } = null;


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
        float disTarget = CompareFunction.EulerDistance(attackTarget.transform.position, transform.position);
        
        if (disTarget < attackRadius && attackTimer < 0.0f)
        {
            Attack();
            attackTimer = attackCooldown;

        }
        else
        {
            MMoveSystem.MoveTo(this, attackTarget.transform.position);
            icm.Move(rb2d);
            Debug.Log("Move" + attackTarget);
        }
    }

    protected override void FindProject()
    {
        FindTarget();
        findTimer = 3.0f;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        attackTimer -= Time.deltaTime;
        findTimer -= Time.deltaTime;

        if (findTimer <= 0.0)
        {
            FindProject();
        }
        
        if (attackTarget != null)
        {
            AttackProject();
        }


    }


        protected override void Init()
    {
        base.Init();

    }

}
