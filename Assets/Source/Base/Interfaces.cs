using System;
using UnityEngine;
public class FCast
{
    public static T Cast<T>(object type)
    {
        return (T)type;
    } 
}

// 附加到可以移动的单位上
public interface ICanMove
{
    float iSpeed { get; set; }
    Vector2 iDirection { get; set; } // 速度方向
    void ChangeToMoveState();
}


// 附加到可以采集的单位上
public interface ICanCollect
{
    float iCollectDistance { get;set; }
    float iCollectForce { get; set; }
    public void ChangeToCollectState(EAlignedEntity target);
}
