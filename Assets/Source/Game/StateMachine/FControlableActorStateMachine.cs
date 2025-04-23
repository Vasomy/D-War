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

    public override void Begin()
    {
        base.Begin();
        actor.GetComponent<ICanMove>().ZeroVeloctiy(actor.rb2d);
    }

    public override void End()
    {
        base.End();
    }

    public override void Update()
    {
        base.Update();
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

        MMoveSystem.MoveTo(stateMachine.actor, target.indexedPos);
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
            var isdone = target.DoCollect(icc.iCollectForce);

            //if((pPos - tPos).magnitude > tDis + icc.iCollectDistance)
            {
                // stop collect
            }
            Debug.Log("collecting!");
            if(isdone)
            {
                // change to idle
                actor.stateMachine.ChangeState(actor.idleState);
            }

            return;
        }
        else
        {
            icm.Move(actor.rb2d);
        }

        if((pPos - tPos).magnitude<=tDis+icc.iCollectDistance)
        {
            goCollect = true;
            icm.ZeroVeloctiy(actor.rb2d);
            MMoveSystem.CancelMoveCommand(actor);
            Debug.Log("In Radius!");
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