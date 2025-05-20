using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABrightWAzard :AControlableActor ,ICanMove
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

    public void Restore()
    {
        var aConActor = CSkill.skillTarget.GetComponent<AControlableActor>();
        aConActor.HP += CSkill.skillDamage;
        if (aConActor.HP >= aConActor.maxHP)
        {
            aConActor.HP = aConActor.maxHP;
        }
    }

    public void RestoreProject()
    {
        MMoveSystem.MoveTo(this, CSkill.skillTarget.transform.position);
        float disTarget = CompareFunction.EulerDistance(CSkill.skillTarget.transform.position, transform.position);
        if (disTarget <= CSkill.skillRadius && CSkill.skillTimer < 0.0f)
        {
            Restore();
            CSkill.skillTimer = CSkill.skillCooldown;
        }
        else if (disTarget > CSkill.skillRadius)
        {
            icm.Move(rb2d);
        }

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        CSkill.skillTimer -= Time.deltaTime;

        if (CAttack.attackTarget != null)
        {
            RestoreProject();
        }

    }


    
    protected override void Init()
    {
        base.Init();
    }


}
