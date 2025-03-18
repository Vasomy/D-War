using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class FControlableActorState : FState
{
    protected FControlableActorState(FControlableActorStateMachine inStateMachine) 
    { 
        stateMachine = inStateMachine;
    }
    //public EControlableProperties actor=>stateMachine.actor;
    public FControlableActorStateMachine stateMachine;
}

public class FCAMoveState : FControlableActorState
{
    public ICanMove icm;
    public FCAMoveState(FControlableActorStateMachine inStateMachine,ICanMove inIcm)
        :base(inStateMachine)
    {
        icm = inIcm;
    }
    public override void Begin()
    {
        base.Begin();
    }

    public override void Update()
    {
        base.Update();
        if(icm.iDirection == Vector2.zero)
        {
            // change to idle
        }
        Vector2 v = icm.iDirection * icm.iSpeed;
        stateMachine.actor.rb2d.velocity = v;
    }

    public override void End()
    {
        base.End();
    }
}

public class FCACollectState : FControlableActorState
{
    public ICanCollect icc;
    public EAlignedEntity target;
    
    public FCACollectState(FControlableActorStateMachine stateMachine,ICanCollect icc)
        :base(stateMachine)
    {
        this.icc = icc;
    }

    public void SetTarget(EAlignedEntity inTarget)
    {
        target = inTarget;
    }

    public override void Begin()
    {
        base.Begin();
    }

    public override void End()
    {
        base.End();
    }

    public override void Update()
    {
        base.Update();
        if()
    }
}




//具体到某一类型
public class FControlableActorStateMachine
{
    public FState currentState;
    public AControlableActor actor;
    public FControlableActorStateMachine(AControlableActor inActor)
    {
        actor = inActor;
    }
    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    public void ChangeState(FState state)
    {
        if(state == null)
        {
            Debug.LogError("Pass a null state to state machine!");
            return;
        }
        if(currentState!=null)
            currentState.End();
        currentState = state;
        currentState.Begin();
    }
}