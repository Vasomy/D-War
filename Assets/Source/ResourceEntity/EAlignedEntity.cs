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
    public override void SetType()
    {
        ettType = EEntityType.Collectable;
    }
}

