using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TechTreeContentTrigger
{
    public int level = 0;
    public int maxLevel = 1;
    public TechTreeContent enumContent;
    public TechTreeContentTrigger(TechTreeContent enumContent,int level, int maxLevel)
    {
        this.enumContent = enumContent;
        this.level = level;
        this.maxLevel = maxLevel;
        Debug.Log(enumContent);
    }

    public virtual void LevelUp()
    {
        
    }

    public virtual void UnDoTrigger()
    {

    }

}

public class TechTreeAttackUp : TechTreeContentTrigger
{
    /// <summary>
    /// 具体数据数组的长度严格等于最大等级maxLevel+1
    /// </summary>
    public FPercent[] data = { 0,0.1f,0.2f,0.3f};


    public TechTreeAttackUp(TechTreeContent enumContent, int level, int maxLevel)
        :base(enumContent,level,maxLevel)
    {

    }


    public override void LevelUp() 
    { 
        if(level<maxLevel)
        {
            int lastLevel = level;
            level++;
            FPercent diff = data[level] -data[lastLevel];
            FGameStats.instance.techTreeModifier.building_attackForce_Extra_Percent += diff;
            FGameStats.instance.techTreeModifier.attackForce_Extra_Percent += diff;
            Debug.Log("Attack up Level Up!");
        }
    }

    public override void UnDoTrigger()
    {
        int lastLevel = level;
        level = 0;
        FPercent diff = data[level];
        FGameStats.instance.techTreeModifier.building_attackForce_Extra_Percent -= diff;
        FGameStats.instance.techTreeModifier.attackForce_Extra_Percent -= diff;
    }
}