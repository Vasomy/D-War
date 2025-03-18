using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFarmer : AControlableActor , ICanCollect,ICanMove
{
    Vector2 ICanMove.iDirection { get; set; } = Vector2.zero;
    float ICanMove.iSpeed { get; set; } = 1.0f;
    float ICanCollect.iCollectForce { get; set; } = 1.0f;
    float ICanCollect.iCollectDistance { get; set; } = 1.0f;

    FControlableActorStateMachine stateMachine;
    FCAMoveState moveState;

    protected override void OnUpdate()
    {
        base.OnUpdate();
        stateMachine.Update();
    }
    public void ChangeToCollectState(EAlignedEntity ett)
    {
    }

    public void ChangeToMoveState()
    {
        stateMachine.ChangeState(moveState);
    }

    protected override void Init()
    {
        base.Init();
        stateMachine = new FControlableActorStateMachine(this);
        moveState = new FCAMoveState(stateMachine,GetComponent<ICanMove>());
        AddControlableProperties(EControlableProperties.Move);
        AddControlableProperties(EControlableProperties.Collect);
    }
}
