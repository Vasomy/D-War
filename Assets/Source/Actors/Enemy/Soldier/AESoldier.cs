using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class AESoldier : AEnemyActor , ICanMove
{
    ICanMove icm => GetComponent<ICanMove>();
    Vector2 ICanMove.iDirection { get; set; } = Vector2.zero;
    float ICanMove.iSpeed { get; set; } = 1.0f;
    FlowFieldPathFinding ICanMove.iPathFinding { get; set;} = null;


    public void ChangeToMoveState()
    {

    }


    //Attack 数据
    public GameObject attackTarget = null;

    public float attackRadius = 2.0f;

    public float attackForce = 1.0f;

    public float attackCooldown = 1.0f;

    public float attackTimer = 0.0f;

    public float disTarget = 1e9f;

    public float findTimer = 0.0f;

    // 检查自身buff
    public override void checkBuff()
    {
        foreach(var buff in buffs)
        {
            buff.timer -= Time.deltaTime;
            if(buff.timer < 0.0f)     //buff时间到了，删除。
            {
                buff.BuffEnd(gameObject);
                buffs.Remove(buff);
            }
            else
            {
                buff.Buffing(gameObject);
            }
        }
    }
        
    //寻找目标，谁都打
    private void FindTarget()
    {
        var allFriendEtt = GameObject.FindGameObjectsWithTag("friendly");
        float minDis = 1e9f;
        // GameObject fTarget = null;
        foreach(var ett in allFriendEtt)
        {
            float dis  = CompareFunction.EulerDistance(ett.transform.position, transform.position);
            if(dis < minDis)
            {
                minDis = dis;
                attackTarget = ett;
            }
        }
        // reutrn fTarget;
    }

    private void Attack()
    {
        // TODO:
            // Animation

            //Target decrease health
        var attackTargetActor = attackTarget.GetComponent<AControlableActor>();
        attackTargetActor.GetDamage(1.0f); 

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        attackTimer -= Time.deltaTime;
        findTimer -= Time.deltaTime;


        if (findTimer <=0.0)
        {
            FindTarget();
            findTimer = 3.0f;
            if(attackTarget == null) return;
            MMoveSystem.MoveTo(this, attackTarget.transform.position);
        }
        disTarget = CompareFunction.EulerDistance(attackTarget.transform.position, transform.position);
        // Debug.Log(disTarget);
        if(disTarget < attackRadius)
        {
            Debug.Log("in soldier radius");
            if(attackTimer < 0.0)
            {
                attackTimer  = attackCooldown;
                Attack();
            }
        }
        else
        {
            
            MMoveSystem.MoveTo(this, attackTarget.transform.position);
            icm.Move(rb2d);
            Debug.Log("Move"+attackTarget);
        }


    }

    protected override void Init()
    {
        base.Init();

    }


    public override void OnMouseRightButtonDown()
    {
        foreach (var item in MSelectSystem.instance.selectedEntity)
        {
            AControlableActor ent = item.GetComponent<AControlableActor>();
            ent.AttackStart(gameObject);
        }   
    }
}
