using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        if(isInBound == false)
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
    }



}
