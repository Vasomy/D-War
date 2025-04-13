using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Ƽ������ṩ����ֵ�������ﴢ��
[Serializable] // ʹ������л���Ϊ�˿�����inspector���濴��
public class TechTreeModifier
{
    // 
    [Header("Demon Cube")]
    public FPercent demonCubeEngul_Speed = 0;// ����
    public FPercent demonCubeEngul_Gain = 0;// ������ȡ��Դ����ң�������
    public bool enable_DemonCube_EngulEarlyWarning = false;// ����Ԥ���Ƿ��� 
    [Space]
    [Header("Monster Cave")]
    public int monsterCave_Produce_Extra_Num = 0;// ħ��ÿ��������������������
    public FPercent monsterCave_Produce_Minus_Consumption = 0;// ħ��ÿ���������ٵ���Դ����
    // monsterCave �����粼�ֵĸ��� +=monsterCave_Produce_Probility_Adjust & �������õĸ���-=monsterCave_Produce_Probility_Adjust;
    public FPercent monsterCave_Produce_Probility_Adjust = 0;
    [Space]
    [Header("Friendly Controlable Actor")] // �ѷ���λ�� ���ԣ��� ����������
    public FPercent attackForce_Extra_Percent = 0;// �ѷ���λ���ǽ������Ĺ������ٷֱ�����
    public float attackForce_Extra = 0;// ����������ֵ
    //[Space]
}


public class FGameStats : SingletonBase<FGameStats>
{
    [Header("Resource")]
    public PResourceStats resoureStats;
    [Space]
    // Attributition ��ص������������л��ͷ����л��������Ի��ڽ�������λ���Ƽ��������غ��ʼ��ʱ������
    [Header("Attributition")]
    public int BasePopulation;
    // when a game save loaded,
    // first maxPopulation setted as BasePopulation,
    // then it increase by others
    public int maxPopulation;
}


// p->player �����ص�����
// ���Ա�save
[Serializable]
public class PStats<T>  where T : PStats<T>
{
    public Serializer<T> serializer = new Serializer<T>();
    public void Save()
    {

    }
}
[Serializable]
public class PResourceStats : PStats<PResourceStats>
{
    public int woods = 0; //ľͷ
    public int stone = 0; //ʯͷ
    public int irons = 0; //��
}

