using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff 
{
    public float timer;
    public virtual void BuffBegin(GameObject obj)
    {

    }
    public virtual void Buffing(GameObject obj)
    {

    }
    public virtual void BuffEnd(GameObject obj)
    {

    }

    public virtual void BuffBegin(GameObject obj, float num, bool percent)
    {
        
    }
}
