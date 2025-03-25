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
    public AControlableActor actor => stateMachine.actor;
}

public class FCAIdleState : FControlableActorState
{
    public FCAIdleState(FControlableActorStateMachine inStateMachine)
        :base(inStateMachine)
    {

    }
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
        icm.Move(stateMachine.actor.rb2d);
    }

    public override void End()
    {
        base.End();
    }
}

public class FCACollectState : FControlableActorState
{
    public ICanMove icm;
    public ICanCollect icc;
    public ECollectableEntity target;
    bool goCollect = false;
    public FCACollectState(FControlableActorStateMachine stateMachine,ICanCollect icc)
        :base(stateMachine)
    {
        this.icc = icc;
    }

    public void SetTarget(ECollectableEntity inTarget)
    {
        target = inTarget;
    }

    public override void Begin()
    {
        base.Begin();
        Debug.Log("wt go to move");
        
        MMoveSystem.MoveTo(stateMachine.actor, target.indexedPos);
        Debug.Log("finish wt go to move");
        icm = stateMachine.actor.GetComponent<ICanMove>();
    }

    public override void End()
    {
        base.End();
        goCollect = false;
        target = null;
    }

    public override void Update()
    {
        base.Update();
        float tDis = target.collectRadius;
        var pPos = stateMachine.actor.transform.position;
        var tPos = target.transform.position;

        if(goCollect)
        {
            // do collect
            // set animation
            target.DoCollect(icc.iCollectForce);
            return;
        }
        else
        {
            icm.Move(actor.rb2d);
        }

        if((pPos - tPos).magnitude<=tDis+icc.iCollectDistance)
        {
            goCollect = true;
        }
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