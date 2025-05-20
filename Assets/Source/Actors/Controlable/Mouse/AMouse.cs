using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMouse :AControlableActor ,ICanMove
{
    Vector2 ICanMove.iDirection { get; set; } = Vector2.zero;
    FlowFieldPathFinding ICanMove.iPathFinding { get; set; } = null;
    float ICanMove.iSpeed { get; set; } = 1.0f;

    public void ChangeToMoveState()
    {
        // stateMachine.ChangeState(moveState);
    }


    public ICanMove icm =>GetComponent<ICanMove>();

    public FCAMoveState moveState;
    
    

 

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


    
    protected override void Init()
    {
        base.Init();
    }


}
