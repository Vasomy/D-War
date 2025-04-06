using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 胆小哥布林
public class AGoblin : AControlableActor , ICanMove
{

    // 作为一个哥布林，它只需要找到enemy，然后干掉enemy。

    public Actor Goblin_Enemy;
    public float GoblinEnemy_Dis;
    public float GoblinAttack_Radius = 0.5f;
    public List<float> GoblinPath;
    public bool GoblinCanDo = true; // 检测哥布林是否在执行别的动作

    float ICanMove.iSpeed { get; set; } = 2.0f;
    Vector2 ICanMove.iDirection { get; set; } = Vector2.zero;

    void Update()
    {
        if(Goblin_Enemy ==null)
        {
            FindEnemy();
        }
        else
        {
            if(GoblinEnemy_Dis<=GoblinAttack_Radius)
            {
                AttackEnemy();
            }
            if(true) // 实时检测敌人位置，移动。
            {
                MoveToEnemy(true);
            }
        }

    }
    public Actor FindEnemy()
    {
        // 寻找目标

        // 计算距离

        return null;

    } 
    public void MoveToEnemy(bool EnemyHaveMove)
    {
        // 读取GoblinPath中的数据，并移动

        //如果没有或目标移动
        if(GoblinPath == null || EnemyHaveMove == true)
        {
            CalculatePath();
        }
    }
    public void CalculatePath()
    {

    }

    public void AttackEnemy()
    {

    }

    public void ChangeToMoveState()
    {
        throw new NotImplementedException();
    }
}
