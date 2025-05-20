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


    static public int deathNumber = 0; //胆小哥布林的死亡数量

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        Debug.Log("goblin get" + damage);
        JudgeFlee();
    }

    // 胆小哥布林的特性
    public bool isFleeing = false;

    public GameObject fleeDirection;
    /// <JudgeFlee>
    /// 这里的逻辑应该是，受到伤害后，生命值小于一半，
    /// 然后有50%几率逃跑
    /// </JudgeFlee>
    public void JudgeFlee()
    {
        if(currentHealth <= maxHealth/2)
        {
            Flee();        
        }
    }

    public void Flee()
    {
        //

        // give a random direction
        // play a Flee animation
        // after goblin flee
        // put goblin back to memory pool

        EntityMemoryPoolProxy<AGoblinTimid>.Free(this);

        //
        return;
        isFleeing = true;
        float fleeX = Random.Range(1,10);
        float fleeY = Random.Range(1,10);
        MMoveSystem.MoveTo(this, fleeDirection.transform.position);

        Debug.Log("FLee" + fleeX + "," + fleeY);
    }

    public override void Die()
    {
        base.Die();
        deathNumber++;
    }

    public void AttackProject()
    {
        MMoveSystem.MoveTo(this, CAttack.attackTarget.transform.position);
        float disTarget = CompareFunction.EulerDistance(CAttack.attackTarget.transform.position, transform.position);
        if (disTarget <= CAttack.attackRadius && CAttack.attackTimer < 0.0f)
        {
            Attack();
        }
        else if (disTarget > CAttack.attackRadius)
        {
            icm.Move(rb2d);
        }
        
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        CAttack.attackTimer -= Time.deltaTime;

        if (CAttack.attackTarget != null)
        {
            AttackProject();
        }

    }
}
