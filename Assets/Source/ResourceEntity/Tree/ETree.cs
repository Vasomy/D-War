using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETree : ECollectableEntity 
{
    public Collider2D cd2d;

    
    public override void OnMouseRightButtonDown()
    {
        var selectedEntities = MSelectSystem.instance.selectedEntity;
        if(selectedEntities != null )
        {
            foreach(var ett in selectedEntities )
            {
                if (ett.ettType != EEntityType.Collectable) return;
                if (((AControlableActor)ett).HasProperty(EControlableProperties.Collect) == false)
                {
                    return;
                }
                var icc = ett.GetComponent<ICanCollect>();
                if(icc!=null)
                {
                    icc.ChangeToCollectState();
                }
            }
        }
    }

    public override void OnMouseLeftButtonDown()
    {
        Debug.Log("Left TREE!!!!!!!!!!!!!!!");
    }
}
