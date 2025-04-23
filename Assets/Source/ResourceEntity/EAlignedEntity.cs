using System;
using System.Runtime.CompilerServices;
using UnityEngine;
// 静态实体
public class EAlignedEntity : Entity
{
    static bool NeedAlign = true;
    protected override void Init()
    {
        base.Init();
        AlignToGrid();
    }

    void AlignToGrid()
    {
        float z = transform.position.z;
        var afterPos2 = GridManager.AlignPoint(transform.position);
        transform.position = new UnityEngine.Vector3(afterPos2.x, afterPos2.y, z);
    }
}

public class EStaticAlignedEntity : EAlignedEntity
{
    // 左宽，右宽，上高，下高
    public int lw, rw, th, dh;
    protected override void Init()
    {
        base.Init();
    }
    public override void SetType()
    {
        gameObject.tag = "static";
    }
}

public class ECollectableEntity : EStaticAlignedEntity
{
    public float collectRadius = 0.5f;
    public float collectGap = 3.0f;
    public float health = 15.0f;// 耐久
    FTimer timer = new FTimer();
    public override void SetType()
    {
        ettType = EEntityType.Collectable;
        
    }
    protected override void Init()
    {
        base.Init();
        timer.SetGap(collectGap);

        //  目前所有的可采集资源均为1*1大小
        GridManager.CalculateOccupiedArea(uid, transform.position, lw, rw, th, dh, false, false);

    }

    public override void OnMouseRightButtonDown()
    {
        var selectedEntities = MSelectSystem.instance.selectedEntity;
        if (selectedEntities != null)
        {
            foreach (var ett in selectedEntities)
            {
                if (ett.ettType != EEntityType.Controlable) return;
                if (((AControlableActor)ett).HasProperty(EControlableProperties.Collect) == false)
                {
                    return;
                }

                var icc = ett.GetComponent<ICanCollect>();
                if (icc != null)
                {
                    
                    icc.ChangeToCollectState(this);
                   
                }
            }
        }
    }

    public override void OnMouseLeftButtonDown()
    {
        
    }

    public virtual void GetResource()
    {
        
    }
    
    public bool DoCollect(float collectForce,float toCollectGap = 1.0f)
        // 如果完成采集 则返回 true
    {
        timer.SetGap(toCollectGap);
        if(timer.Timer())
        {
            health -= collectForce;
            // Resource Stats -> smt += health;
            GetResource();
            
        }
        
        if(health>0)
        {
            return false;
        }
        return true;
    }
}

