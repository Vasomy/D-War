using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : SingletonBase<GameStats>
{
    public PResourceStats resoureStats;

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

