using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFarmer : AControlableActor
{
    protected override void Init()
    {
        base.Init();
        AddControlableProperties(EControlableProperties.Move);
    }
}
