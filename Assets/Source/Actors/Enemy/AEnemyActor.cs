using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEnemyActor : Actor
{
    // ���е�enemy actor ����ʼ���� ��ӵ�ϡ�輯�й��������в���
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
