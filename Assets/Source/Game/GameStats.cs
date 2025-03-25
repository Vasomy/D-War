using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : SingletonBase<GameStats>
{
    public PResourceStats resoureStats;

}
// p->player 玩家相关的数据
// 可以被save
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
    public int woods = 0; //木头
    public int stone = 0; //石头
    public int irons = 0; //铁
}

