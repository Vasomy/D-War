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
    // Attributition 相关的属性无需序列化和反序列化，其属性会在建筑，单位，科技树被加载后初始化时被设置
    [Header("Attributition")]
    [NonSerialized]
    public int BasePopulation;
    // when a game save loaded,
    // first maxPopulation setted as BasePopulation,
    // then it increase by others
    [NonSerialized]
    public int maxPopulation;
}
