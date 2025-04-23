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

    public GameObject currentTarget = null;

    public float attackRadius = 2.0f;

    public float attackForce = 1.0f;

    public float attackCooldown = 1.0f;

    public float attackTimer = 0.0f;

    public float disTarget = 1e9f;

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
                currentTarget = ett;
            }
        }
        // reutrn fTarget;
    }

    private void AttackTarget()
    {
        // TODO:
            // Animation

            //Target decrease health

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        attackTimer -= Time.deltaTime;

        if (currentTarget == null)
        {
            FindTarget();
            if(currentTarget == null) return;
            MMoveSystem.MoveTo(this, currentTarget.transform.position);
        }
        disTarget = CompareFunction.EulerDistance(currentTarget.transform.position, transform.position);
        Debug.Log(disTarget);
        if(disTarget < attackRadius)
        {
            Debug.Log("in soldier radius");
            if(attackTimer < 0.0)
            {
                attackTimer  = attackCooldown;
                AttackTarget();
            }
        }
        else
        {
            
            MMoveSystem.MoveTo(this, currentTarget.transform.position);
            icm.Move(rb2d);
            Debug.Log("Move");
        }
    }

    protected override void Init()
    {
        base.Init();

    }
}
