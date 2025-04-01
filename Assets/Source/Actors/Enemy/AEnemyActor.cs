using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEnemyActor : Actor
{
    // 所有的enemy actor 被初始化后 添加到稀疏集中供其他类中查找
    public static TSparseSet<AEnemyActor> actors;

    public override void SetType()
    {
        base.SetType();
        ettType = EEntityType.Enemy;
        gameObject.tag = "Enemy";
    }

    protected override void Init()
    {
        base.Init();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }



}
