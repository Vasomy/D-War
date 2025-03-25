using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// 一切可以响应玩家输入的单位
// 
public enum EControlableProperties : int
{
    None = 0,
    Move = 1<<0,
    Attack = 1<<1,
    Collect = 1<<2,
}


//  可以被框选的实体，其他实体最多被点选
public class AControlableActor : Actor
{
    // path finding properites
    public FlowFieldPathFinding curFFPF = null;
    //

    public Vector2 direction;
    public float speed { private set; get; } = 3.0f;


    public int ControlableProperties = (int)EControlableProperties.None;
    public void AddControlableProperties(EControlableProperties property)
    {
        ControlableProperties |= (int)property;
    }
    public bool HasProperty(EControlableProperties property)
    {
        return (ControlableProperties & (int)property) != 0;
    }

    protected override void Init()
    {
        base.Init();
    }
    public override void SetType()
    {
        ettType = EEntityType.Controlable;
    }
    public void SetVelocityDirection(Vector2 fDir)
    {
        var icm = GetComponent<ICanMove>();
        if (icm == null) return;

        if(fDir == Vector2.zero)
        {
            rb2d.velocity = Vector2.zero;
        }
        rb2d.velocity = fDir.normalized * icm.iSpeed;
        //Debug.Log(fDir * speed);
    }

    public override void OnMouseLeftButtonDown()
    {
        base.OnMouseLeftButtonDown();
        MSelectSystem.SelectEntity(this);// 也许应该放在基类实现？
    }
}
