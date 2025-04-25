using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGoblinTimid : AControlableActor, ICanMove
{
    Vector2 ICanMove.iDirection { get; set; } = Vector2.zero;
    FlowFieldPathFinding ICanMove.iPathFinding { get; set; } = null;
    float ICanMove.iSpeed { get; set; } = 1.0f;

    public void ChangeToMoveState()
    {
        // stateMachine.ChangeState(moveState);
    }
    public ICanMove icm =>GetComponent<ICanMove>();

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        Debug.Log("goblin get" + damage);
        JudgeFlee();
    }

    // 胆小哥布林的特性
    public bool isFleeing = false;

    public GameObject fleeDirection;
    public void JudgeFlee()
    {
        if(currentHealth <= maxHealth/2)
        {
            Flee();        
        }
    }

    public void Flee()
    {
        isFleeing = true;
        float fleeX = Random.Range(1,10);
        float fleeY = Random.Range(1,10);
        MMoveSystem.MoveTo(this, fleeDirection.transform.position);

        Debug.Log("FLee" + fleeX + "," + fleeY);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if(attackTarget!=null&&isFleeing == false)
        {
            attackTimer -= Time.deltaTime;

            MMoveSystem.MoveTo(this, attackTarget.transform.position);
            float disTarget = CompareFunction.EulerDistance(attackTarget.transform.position, transform.position);
            if(disTarget <= attackRadius && attackTimer<0.0f)
            {
                Attack();
                // MMoveSystem.MoveTo(this, transform.position);
            }
            else if(disTarget > attackRadius)
            {
                icm.Move(rb2d);
            }
        }
        else
        {
            icm.Move(rb2d);
        }
    }
}
