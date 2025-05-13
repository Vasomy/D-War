using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFarmer : AControlableActor, ICanCollect, ICanMove
{
    Vector2 ICanMove.iDirection { get; set; } = Vector2.zero;
    FlowFieldPathFinding ICanMove.iPathFinding { get; set; } = null;
    float ICanMove.iSpeed { get; set; } = 1.0f;
    float ICanCollect.iCollectForce { get; set; } = 1.0f;
    float ICanCollect.iCollectDistance { get; set; } = 0.5f;


    public ICanCollect icc => GetComponent<ICanCollect>();
    public ICanMove icm => GetComponent<ICanMove>();

    public FCAMoveState moveState;
    public FCACollectState collectState;

    protected override void OnUpdate()
    {
        base.OnUpdate();
        stateMachine.Update();
        //Debug.Log(stateMachine.currentState.ToString());
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

        // add 'this' into specify pool
        if (!EntityMemoryPoolManager.IsInPool(this))
            EntityMemoryPoolManager.Register(this);

        stateMachine = new FControlableActorStateMachine(this);

        moveState = new FCAMoveState(stateMachine, GetComponent<ICanMove>());
        collectState = new FCACollectState(stateMachine, GetComponent<ICanCollect>());
        idleState = new FCAIdleState(stateMachine);

        stateMachine.ChangeState(idleState);

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

    public override void Destroy()
    {
        EntityMemoryPoolProxy<AFarmer>.Free(this);
    }
}
