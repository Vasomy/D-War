using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

// EA->EnemyActor

/// <summary>
/// 该类是一个良好的例子对于一般敌方单位的寻敌逻辑
/// </summary>
/// 

public class EASkeleton : AEnemyActor , ICanMove,ICanAttack
{
    float detectRadius = 4.0f;
    float maxTraceDistance = 5.0f;// 对于一个玩家单位，最远的追踪距离，即当两者距离超出
    
    Vector2 ICanMove.iDirection { get; set; } = Vector2.zero;
    float ICanMove.iSpeed { get; set; } = 1.0f;

    float ICanAttack.iAttackForce { get; set; } = 3.0f;
    float ICanAttack.iAttackRange { get; set; } = 0.5f;


    public GameObject attackTarget;
    

    void ICanMove.ChangeToMoveState()
    {
        // dsmt

    }

    protected override void OnUpdate()
    {
        if(attackTarget == null)
        {
            var allFriendEtt = GameObject.FindGameObjectsWithTag("friendly");
            float maxDis = Mathf.Infinity;
            attackTarget = null;
            foreach(var ett in allFriendEtt)
            {
                float dis = CompareFunction.EulerDistance(ett.transform.position, transform.position);

                var type = ett.GetComponent<Entity>().ettType;

                if(type == EEntityType.Building)
                {

                }
                if(type == EEntityType.Controlable)
                {

                }

                if (dis <= detectRadius)
                {
                    if (dis < maxDis)
                    {
                        maxDis = dis;
                        attackTarget = ett;
                    }
                }
            }

            if(attackTarget != null)
            {
                MMoveSystem.MoveTo(this, 
                    attackTarget.transform.position);
            }
            else
            {
                attackTarget = target;
                MMoveSystem.MoveTo(this,
                    target.transform.position);
            }

        }
        else
        {
            float dis2CurrentTarget = CompareFunction.EulerDistance(attackTarget.transform.position,transform.position);
            if(dis2CurrentTarget <= iAttackRange)
            {

            }
            else
            {
                // move
                //GetComponent<ICanMove>().Move(rb2d);
            }
        }
    }

    public override void GetTarget()
    {
        var allFriendEtt = GameObject.FindGameObjectsWithTag("friendly");
        if(allFriendEtt != null && allFriendEtt.Count()!=0)
        {
            float maxDis = Mathf.Infinity;

            foreach (var item in allFriendEtt)
            {
                float dis = CompareFunction.EulerDistance(item.transform.position, transform.position);
               
                //if(dis<=detectRadius)
                {
                    if(dis<maxDis)
                    {
                        maxDis = dis;
                        target = item;
                    }
                }
               
            }
        }
    }

    protected override void Init()
    {
        base.Init();
        
    }
}
