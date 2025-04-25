using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AWolf : AControlableActor ,ICanMove
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
    
    

 

     protected override void OnUpdate()
    {
        base.OnUpdate();
        // stateMachine.Update();
        if(attackTarget!=null)
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
                Debug.Log("Wolf Move 1");
            }
        }
        else
        {
            icm.Move(rb2d);
            Debug.Log("Wolf Move 2");
        }
        
    }


    
    protected override void Init()
    {
        base.Init();
        // stateMachine = new FControlableActorStateMachine(this);
        
        // moveState = new FCAMoveState(stateMachine,GetComponent<ICanMove>());
        // idleState = new FCAIdleState(stateMachine);

        // stateMachine.ChangeState(idleState);

        // AddControlableProperties(EControlableProperties.Move);
        // AddControlableProperties(EControlableProperties.Collect);
    }


}
