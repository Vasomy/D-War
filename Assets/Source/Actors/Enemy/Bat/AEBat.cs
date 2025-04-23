using UnityEngine;

using UnityEngine;


public class AEBat : AEnemyActor , ICanMove
{
    ICanMove icm => GetComponent<ICanMove>();
    Vector2 ICanMove.iDirection { get; set; } = Vector2.zero;
    float ICanMove.iSpeed { get; set; } = 1.0f;
    FlowFieldPathFinding ICanMove.iPathFinding { get; set; } = null;

    public void ChangeToMoveState()
    {

    }

    public GameObject currentTarget = null;

    public float attackRadius = 1.0f;

    public float attackForce = 1.0f;

    public float attackCooldown = 1.0f;

    public float attackTimer = 0.0f;

    public float disTarget = 1e9f;
        
    private GameObject FindTarget()
    {
        var allFriendEtt = GameObject.FindGameObjectsWithTag("friendly");
        float minDis = 1e9f;
        GameObject _target = null;
        foreach(var ett in allFriendEtt)
        {
            float dis  = CompareFunction.EulerDistance(ett.transform.position, transform.position);
            if(dis < minDis)
            {
                _target = ett;
            }
        }
        return _target;
    }

    private void AttackTarget()
    {
        // TODO:
            // Animation

            //Target decrease health
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        attackTimer -= Time.deltaTime;

        if (currentTarget == null)
        {
            currentTarget = FindTarget();
        }
        disTarget = CompareFunction.EulerDistance(currentTarget.transform.position, transform.position);
        if(disTarget < attackRadius)
        {
            if(attackTimer < 0.0)
            {
                attackTimer  = attackCooldown;
                AttackTarget();
            }
        }
        else
        {
            MMoveSystem.MoveTo(this, currentTarget.transform.position);
            icm.Move(rb2d);
        }
    }

    protected override void Init()
    {
        base.Init();

    }
}
