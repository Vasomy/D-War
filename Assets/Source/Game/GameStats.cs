using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class FGameStats : SingletonBase<FGameStats>
{
    [Header("Modifier")]
    public TechTreeModifier techTreeModifier;


    [Header("Resource")]
    public PResourceStats resoureStats;
    [Space]
    // Attributition ��ص������������л��ͷ����л��������Ի��ڽ�������λ���Ƽ��������غ��ʼ��ʱ������
    [Header("Attributition")]
    [NonSerialized]
    public int BasePopulation;
    // when a game save loaded,
    // first maxPopulation setted as BasePopulation,
    // then it increase by others
    [NonSerialized]
    public int maxPopulation;
}
