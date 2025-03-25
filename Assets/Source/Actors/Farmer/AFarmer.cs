using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFarmer : AControlableActor , ICanCollect,ICanMove
{
    Vector2 ICanMove.iDirection { get; set; } = Vector2.zero;
    float ICanMove.iSpeed { get; set; } = 1.0f;
    float ICanCollect.iCollectForce { get; set; } = 1.0f;
    float ICanCollect.iCollectDistance { get; set; } = 0.5f;

    public ICanCollect icc =>GetComponent<ICanCollect>();
    public ICanMove icm =>GetComponent<ICanMove>();

    FControlableActorStateMachine stateMachine;
    FCAMoveState moveState;
    FCACollectState collectState;

    protected override void OnUpdate()
    {
        base.OnUpdate();
        stateMachine.Update();
        if (stateMachine.currentState != null)
            Debug.Log(stateMachine.currentState.ToString());
        else
            Debug.Log("Null state");
    }
    public void ChangeToCollectState(ECollectableEntity ett)
    {
        collectState.target = ett;
        stateMachine.ChangeState(collectState);
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
        collectState = new FCACollectState(stateMachine,GetComponent<ICanCollect>());
        AddControlableProperties(EControlableProperties.Move);
        AddControlableProperties(EControlableProperties.Collect);
    }

    private void OnDrawGizmos()
    {
        if (icc != null)
        {
            Gizmos.DrawWireSphere(transform.position, icc.iCollectDistance);
        }
    }
}
