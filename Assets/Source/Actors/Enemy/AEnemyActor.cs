using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;  
/// <summary>
/// �йص��˵�λ�������߼�
/// ����һ��ĵ�λ���з���λ�ǰ��ղ��γ��֣������߼�ӦΪ->
/// 1.Ѱ������Ľ�������������ҿ��Ƶĵ�λ��->2.������·���ߣ��������·����ʱ��һ����Χ��(Detect radius)�����䷢��������
/// ����ɹ���ָ������������Ŀ�꽨����·������->3.Ŀ�꽨����ʧ�ص�1
/// ------------------------------------------------------------------
/// 
/// һЩע�����
/// �ѷ���λ��tag �� friendly �����Ҫ��ȡ���е��ѷ���λ ʹ�� GameObject.FindWithTag("friendly");
/// ���Ͻ���ʹ��ϡ�輯���߹�ϣ�����ڴ� ����tagΪfriendly��objects (δ��ʵ��)
/// 
/// ͬ�����з���λ��tag Ϊ enemy ����ʹ�� .FindWithTag���� ͬ�ϵ����ݽṹ��ѯ
/// ----------------------------------------------------------------
/// ����AEnemyActor��GetTarget���Ż�
/// ����ÿ�λ�ȡtargetʱ��Ҫ����һ�����еľ���friendly Tag��Entity����ʱ�ϳ�
/// �����������Ż�
/// 1.���ڵ�ͼ�����飬�ڵз������ѷ������ƶ�ʱ������������Ϣ�����������ĸ����飩������ѯ��λʱ������в�ѯ��Χ��Detect Radius������
/// ���԰��������ѯ
/// 2.���ڵз���λ�ǰ��ղ��γ��֣�ͨ��������λ�ۼ���һ��
/// ������� FEnemyBound�࣬�����ɸ���λ����һ����λ�����ǹ���һ��target
/// 
/// -----------------------------------------------------------------
/// 
/// ����һЩ�߼��򵥵ĵ�λ���Բ�ʹ�ýӿں�״̬������
/// 
/// </summary>
public class AEnemyActor : EActor
{
    // ���е�enemy actor ����ʼ���� ���ӵ�ϡ�輯�й��������в���
    // ? 
    //public static TSparseSet<AEnemyActor> actors = new TSparseSet<AEnemyActor>();

    public GameObject target; // Ŀ�굥λ
    public bool isInBound = false; // �Ƿ���һ��Bound�У����ڣ���target�ɸ�Bound����

    public List<Buff> buffs;

    // �ж�buff
    public virtual void checkBuff()
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
    //
    /// <GetTarget>
    /// �õ�λ��ȡĿ����߼���Ӧ����Ŀ�걻���ɺ󣬻���Ŀ����ʧ�����
    /// </GetTarget>
    public virtual void GetTarget()
    {

    }

    // -----
    // ���·����̳���Actor | Entity
    // ���Ƽ�����������д
    public override void Enabled()
    {
        base.Enabled();
        if (isInBound == false)
            GetTarget();
    }
    public override void SetType()
    {
        base.SetType();
        ettType = EEntityType.Enemy;
        gameObject.tag = "enemy";
    }

    protected override void Init()
    {
        base.Init();

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (HP <= 0.0f)
        {
            Die();
        }
    }

    //Find
    public FindAttribute EFind;
    // Attack 
    public AttackAttribute EAttack;
    //Skill
    public SkillAttribute ESkill;

    //HP
    public float HP;
    public float maxHP;

    // Find Target
    protected virtual void FindTarget()
    {
        var allFriendEtt = GameObject.FindGameObjectsWithTag("friendly");
        float minDis = 1e9f;

        foreach (var ett in allFriendEtt)
        {
            float dis = CompareFunction.EulerDistance(ett.transform.position, transform.position);
            if (dis < minDis)
            {
                minDis = dis;
                EAttack.attackTarget = ett;
            }
        }
        // reutrn fTarget;
    }

    // Attack Target
    protected virtual void Attack()
    {
        // TODO:
        // Animation

        //Target decrease health
        var attackTargetActor = EAttack.attackTarget.GetComponent<AControlableActor>();
        attackTargetActor.GetDamage(EAttack.attackForce);

    }

    // Die
    protected virtual void Die()
    {

    }

    //Project
    protected virtual void AttackProject()
    {

    }

    protected virtual void FindProject()
    {
        
    }
}

public enum EnemyName
{
    AFarmer,
    AESoldier,
    AEBat,
}


[Serializable]
public class AttackAttribute
{
    public float attackRadius = 2.0f;  //攻击半径
    public float attackForce = 1.0f;  //攻击力
    public float attackCooldown = 1.0f;  //攻击冷却
    public float attackTimer = 0.0f;  //攻击计时器
    public GameObject attackTarget;
}

[Serializable]
public class FindAttribute
{
    public float findCooldown = 3.0f;
    public float findTimer = 0.0f;
}

[Serializable]
public class SkillAttribute
{
    public GameObject skillPrefab;
    public float skillCooldown = 10.0f;
    public float skillTimer = 0;
    public int skillNumber = 3;
}