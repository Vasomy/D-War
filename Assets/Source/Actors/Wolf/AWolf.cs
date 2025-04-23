using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AWolf : AControlableActor ,ICanMove
{
    Vector2 ICanMove.iDirection { get; set; } = Vector2.zero;
    FlowFieldPathFinding ICanMove.iPathFinding { get; set; } = null;
    float ICanMove.iSpeed { get; set; } = 1.0f;


    public ICanMove icm =>GetComponent<ICanMove>();

    public FCAMoveState moveState;
    

    public Vector2 moveTarget;
    public override void OnMouseRightButtonDown()
    {
        Debug.Log("????");

    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        // stateMachine.Update();
        Debug.Log("Wolf" + isSelected);

        if(Input.GetMouseButtonDown(1))
        {
            
        }
        
        icm.Move(rb2d);
    }


    public void ChangeToMoveState()
    {
        // stateMachine.ChangeState(moveState);
    }
    protected override void Init()
    {
        base.Init();
        moveTarget = gameObject.transform.position;
        // stateMachine = new FControlableActorStateMachine(this);
        
        // moveState = new FCAMoveState(stateMachine,GetComponent<ICanMove>());
        // idleState = new FCAIdleState(stateMachine);

        // stateMachine.ChangeState(idleState);

        // AddControlableProperties(EControlableProperties.Move);
        // AddControlableProperties(EControlableProperties.Collect);
    }


}
