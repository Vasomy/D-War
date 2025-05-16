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
public class AControlableActor : EActor 
{
    
    //
    public FControlableActorStateMachine stateMachine;
    // stop actor move & reset\clear other state's data
    public FCAIdleState idleState;

    public float currentHealth = 10.0f;
    public float maxHealth = 10.0f;

    public virtual void GetDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth < 0)
        {
            // free this
        }
    }

    public void ChangeToIdleState()
    {
        stateMachine.ChangeState(idleState);
    }


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
        rb2d.freezeRotation = true;

        var world = FWorld.currentWorld;
        world.RegisterFriendlyEntity(this);
    }
    public override void SetType()
    {
        ettType = EEntityType.Controlable;
        gameObject.tag = "friendly";
    }
    public void SetVelocityDirection(Vector2 fDir)
    {
        var icm = GetComponent<ICanMove>();
        if (icm == null) return;

        if(fDir == Vector2.zero)
        {
            rb2d.velocity = Vector2.zero;
        }
    }

    public override void OnMouseLeftButtonDown()
    {
        base.OnMouseLeftButtonDown();
        MSelectSystem.SelectEntity(this);// 也许应该放在基类实现？
    }


    //attack相关参数
    public GameObject attackTarget;

    public float attackForce = 3.0f;
    public float attackCooldown = 3.0f;
    public float attackRadius = 3.0f;
    public float attackTimer = 3.0f;

    //攻击，启动！
    public virtual void AttackStart(GameObject target)
    {
        Debug.Log(gameObject +" 开始攻击"+ target);
        attackTarget = target;
    }

    //攻击！！！
    public virtual void Attack()
    {
        Debug.Log(attackTarget +"-"+attackForce+"血量");
        attackTimer = attackCooldown;
    }

}
