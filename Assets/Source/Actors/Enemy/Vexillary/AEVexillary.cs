using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;
using System.Net.NetworkInformation;

public class AEVexillary : AEnemyActor , ICanMove
{
    ICanMove icm => GetComponent<ICanMove>();
    Vector2 ICanMove.iDirection { get; set; } = Vector2.zero;
    float ICanMove.iSpeed { get; set; } = 1.0f;
    FlowFieldPathFinding ICanMove.iPathFinding { get; set;} = null;

    public void ChangeToMoveState()
    {
        
    }

    public float skillDuratiuon = 3.0f;

    public float skillCooldown = 3.0f;

    public float skillTimer = 0.0f;

    public float skillRadius = 5.0f;

    public List<GameObject> skillTarget;

    public GameObject moveTarget = null;

   // public BuffVexillary vexillaryBuff;

    // 获得所有的技能目标
    public void GetSkillTarget()
    {
        var allEnemyTarget = GameObject.FindGameObjectsWithTag("friendly");
        foreach (var enemyTarget in allEnemyTarget)
        {
            float dis = CompareFunction.EulerDistance(enemyTarget.transform.position, transform.position);
            if (dis < skillRadius)
            {
                skillTarget.Add(enemyTarget);
            }
        }
    }

    //使用技能
    public void UseSkill()
    {
        skillTarget.Clear();
        GetSkillTarget();    
        
        foreach (var enemy in skillTarget)  //给所有人挂buff
        {
            var buffVexillary = new BuffVexillary();
            buffVexillary.BuffBegin(enemy);
            var enemyActor = enemy.GetComponent<AEnemyActor>();
            enemyActor.buffs.Add(buffVexillary);
        }
    }

    //寻找enemy
    public void FindMoveTarget()
    {
        var allFriendEtt = GameObject.FindGameObjectsWithTag("enemy");
        float minDis = 1e9f;
        foreach(var ett in allFriendEtt)
        {
            float dis  = CompareFunction.EulerDistance(ett.transform.position, transform.position);
            if(dis < minDis)
            {
                moveTarget = ett;
            }
        }
    }

    protected override void OnUpdate()
    {
        skillTimer-=Time.deltaTime;
        if(skillTimer<0.0f)
        {
            skillTimer = skillCooldown;
            UseSkill();
        }

        if(moveTarget == null)  //寻找移动目标
        {
            FindMoveTarget();
        }

        if(skillTarget.Count <= 3)  //为了不立于危险之地，给三个人上buff就够了。
        {
            MMoveSystem.MoveTo(this, moveTarget.transform.position);
            icm.Move(rb2d);
        }
    }
    protected override void Init()
    {
        base.Init();

    }
    
}
