using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;



public class Buildings : Entity
{
    public string buildingName;
    public SpriteRenderer spriteRenderer;

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
