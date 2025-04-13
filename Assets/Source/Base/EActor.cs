using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EActor : Entity
{
    public SpriteRenderer spriteRenderer;
    private void Start()
    {
        Init();
    }
    protected override void Init()
    {
        base.Init();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void MoveTo(Vector2 point)
    {

    }
}
