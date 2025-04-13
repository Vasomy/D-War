
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class FPercent
{
    public FPercent(FPercent rhs)
    {
        percent = rhs.percent;
    }
    public FPercent(float percent)
    {
        this.percent = percent;
    }


    static public float operator *(float f, FPercent p)
    {
        return f * p.percent;
    }
    static public float operator *(FPercent f, FPercent p)
    {
        return f.percent * p.percent;
    }
    static public float operator+(float f,FPercent p)
    {
        return f+p.percent;
    }
    static public float operator +(FPercent f, FPercent p)
    {
        return f.percent + p.percent;
    }
    static public float operator-(float f,FPercent p)
    {
        return f-p.percent;
    }
    static public float operator -(FPercent f, FPercent p)
    {
        return f.percent + p.percent;
    }

    public static implicit operator FPercent(float v)
    {
        return new FPercent(v);
    }

    float percent;
}

public enum BuildingSpriteUsage
{

}
[Serializable]
public class BuildingSprite
{
    public int level = 0;
}

[Serializable]
[CreateAssetMenu(fileName = "New Building Data", menuName = "Data/New Building Data")]
public class BuildingData
{
    [Header("Global Data")]
    public string buildingName;
    // this building unique id , not the object id
    public long buildingUId;
    
    [Space]
    //attack parser
    [Header("Attack Data")]
    public float attackForce;
    public float attackRange;
    //attackGap => attackBaseGap - attackBaseGap * attackSpeed;
    public float attackGap;
    public float attackBaseGap;
    public FPercent attackSpeed = 0.1f;
    
    [Space]
    [Header("Defence Data")]
    public float health;
    // get damage = damage - armor
    public float armor;
    // get mana damage = manaDamage*(1.0f - manaResistance)
    public FPercent manaResistance;
    [Space]
    [Header("Produce Data")]
    // 生产队列的最大值，目前采用公共生产队列，姑注释
    // public int maxInQueue; 
    // produceGap = produceBaseGap;
    public float produceGap;
    public float produceBaseGap;
    public FPercent produceSpeed;
    [Space]
    // some attributition data that can give player
    [Header("Attributition Data")]
    public int providePopulation = 0; 
}




public class EBuildings : EAlignedEntity
{
    public string buildingName;
    public SpriteRenderer spriteRenderer;
    // all building data adjust through inspector
    public BuildingData buildingData;

    protected int lWidth = 0;
    protected int rWidth = 0;
    protected int uHeight = 0;
    protected int dHeight = 0;

    // 原则上不允许在子类覆盖Start
    private void Start()
    {
        Init();
        RegisterBuildingArea();
    }
    public override void SetType()
    {
        ettType = EEntityType.Building;
    }

    public void RegisterBuildingArea()
    {
        CalculateBuildingArea(transform.position,false,false);
    }
    public void UnRegisterBuildingArea()
    {
        CalculateBuildingArea(transform.position,false,true);
    }
    public virtual bool CalculateBuildingArea(Vector3 position, bool isPreview = false,bool isDelete = false)
    {
        return false;
    }

    public virtual void LogicUpdate()
        // do attack
        // generate
        // ...
    { 
        
    }

}
