using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 科技树所提供的数值会在这里储存
[Serializable] // 使其可序列化是为了可以在inspector里面看到
public class TechTreeModifier
{
    // 
    [Header("Demon Cube")]
    public FPercent demonCubeEngul_Speed = 0;// 加速
    public FPercent demonCubeEngul_Gain = 0;// 提升获取资源（金币）的数量
    public bool enable_DemonCube_EngulEarlyWarning = false;// 吞噬预警是否开启 
    [Space]
    [Header("Monster Cave")]
    public int monsterCave_Produce_Extra_Num = 0;// 魔窟每次生产额外生产的数量
    public FPercent monsterCave_Produce_Minus_Consumption = 0;// 魔窟每次生产减少的资源消耗
    // monsterCave 生产哥布林的概率 +=monsterCave_Produce_Probility_Adjust & 生产骷髅的概率-=monsterCave_Produce_Probility_Adjust;
    public FPercent monsterCave_Produce_Probility_Adjust = 0;
    [Space]
    [Header("Friendly Controlable Actor")] // 友方单位的 属性，！ 不包括建筑
    public FPercent attackForce_Extra_Percent = 0;// 友方单位（非建筑）的攻击力百分比提升
    public float attackForce_Extra = 0;// 具体提升数值
    //[Space]
}


public class FGameStats : SingletonBase<FGameStats>
{
    [Header("Resource")]
    public PResourceStats resoureStats;
    [Space]
    // Attributition 相关的属性无需序列化和反序列化，其属性会在建筑，单位，科技树被加载后初始化时被设置
    [Header("Attributition")]
    public int BasePopulation;
    // when a game save loaded,
    // first maxPopulation setted as BasePopulation,
    // then it increase by others
    public int maxPopulation;
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

