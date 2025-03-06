﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 一切可以响应玩家输入的单位
// 
public enum EControlableProperties : int
{
    None = 0,
    Move = 1<<0,
    Attack = 1<<1,
}
public class AControlableActor : Actor
{
    public int ControlableProperties = (int)EControlableProperties.None;
    public void AddControlableProperties(EControlableProperties property)
    {
        ControlableProperties &= (int)property;
    }
    public bool HasProperty(EControlableProperties property)
    {
        return (int)(ControlableProperties & (int)property) != 0;
    }

    protected override void Init()
    {
        base.Init();
    }
}
