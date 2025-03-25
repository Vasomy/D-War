using System;
using UnityEngine;
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

public class ECollectableEntity : EAlignedEntity
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
        
    }

    public override void OnMouseRightButtonDown()
    {
        var selectedEntities = MSelectSystem.instance.selectedEntity;
        if (selectedEntities != null)
        {
            Debug.Log("Begin to collect");
            Debug.Log(selectedEntities.Count);
            foreach (var ett in selectedEntities)
            {
                Debug.Log("Step : " + 1+" " + ett.ettType);
                if (ett.ettType != EEntityType.Controlable) return;
                Debug.Log("Step : " + 2);
                if (((AControlableActor)ett).HasProperty(EControlableProperties.Collect) == false)
                {
                    return;
                }
                Debug.Log("Step : " + 3);

                var icc = ett.GetComponent<ICanCollect>();
                if (icc != null)
                {
                    Debug.Log("Step : " + 4);

                    icc.ChangeToCollectState(this);
                }
            }
        }
    }

    public override void OnMouseLeftButtonDown()
    {
        Debug.Log("Left TREE!!!!!!!!!!!!!!!");
    }

    public virtual void GetResource()
    {
        
    }

    public void DoCollect(float collectForce,float toCollectGap = 0)
    {
        if(timer.Timer())
        {
            health -= collectForce;
            // Resource Stats -> smt += health;
            GetResource();
        }
        else
        {
            if(toCollectGap == 0)
                timer.SetGap(collectGap);
            else
            {
                timer.SetGap(toCollectGap);
            }
        }
    }
}

