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
    FlowFieldPathFinding iPathFinding { get; set; }
    void ChangeToMoveState();
    void Move(Rigidbody2D rb2d)
    {
        rb2d.velocity = iSpeed * iDirection;
    }

    void ZeroVeloctiy(Rigidbody2D rb2d)
    {
        rb2d.velocity = Vector2.zero;
    }
}

public interface ICanAttack
{
    public float iAttackForce { get; set; }
    public float iAttackRange { get; set; }
    bool CanAttack(float distance)
    {
        return distance <= iAttackRange;
    }
}

public interface IHealth
{
    float iHealth { get; set; }
    void GetDamaged(float damage)
    {
        iHealth -= damage;
        if(iHealth < 0)
        {
            Death();
        }
        else
        {
            DamageEffect();
        }
    }
    // 生命值小于0时触发
    void Death();
    //每次受到伤害时触发的效果
    void DamageEffect();
}


// 附加到可以采集的单位上
public interface ICanCollect
{
    float iCollectDistance { get;set; }
    float iCollectForce { get; set; }
    public void ChangeToCollectState(ECollectableEntity target);
}
