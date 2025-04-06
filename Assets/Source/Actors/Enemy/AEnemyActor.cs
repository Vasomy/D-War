using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 有关敌人单位的索敌逻辑
/// 对于一般的单位，敌方单位是按照波次出现，索敌逻辑应为->
/// 1.寻找最近的建筑（或者是玩家控制的单位）->2.按照线路行走，如果按线路行走时在一定范围内(Detect radius)便想其发动攻击，
/// 在完成攻击指令后继续按朝向目标建筑的路线行走->3.目标建筑消失回到1
/// ------------------------------------------------------------------
/// 
/// 一些注意事项：
/// 友方单位的tag 是 friendly 如果想要获取所有的友方单位 使用 GameObject.FindWithTag("friendly");
/// 以上建议使用稀疏集或者哈希表用于存 所有tag为friendly的objects (未来实现)
/// 
/// 同理，敌方单位的tag 为 enemy 可以使用 .FindWithTag或者 同上的数据结构查询
/// ----------------------------------------------------------------
/// 对于AEnemyActor中GetTarget的优化
/// 由于每次获取target时需要遍历一遍所有的具有friendly Tag的Entity，耗时较长
/// 可以做以下优化
/// 1.对于地图分区块，在敌方或者友方物体移动时更新其区块信息（包括所在哪个区块），当查询单位时，如果有查询范围（Detect Radius变量）
/// 可以按照区块查询
/// 2.由于敌方单位是按照波次出现，通常大量单位聚集在一起，
/// 考虑设计 FEnemyBound类，将若干个单位看作一个单位，他们共享一个target
/// 
/// -----------------------------------------------------------------
/// 
/// 对于一些逻辑简单的单位可以不使用接口和状态机控制
/// 
/// </summary>
public class AEnemyActor : Actor
{
    // 所有的enemy actor 被初始化后 添加到稀疏集中供其他类中查找
    // ? 
    //public static TSparseSet<AEnemyActor> actors = new TSparseSet<AEnemyActor>();

    public GameObject target; // 目标单位
    public bool isInBound = false; // 是否在一个Bound中，若在，则target由该Bound设置
    
    //
    /// <GetTarget>
    /// 该单位获取目标的逻辑，应该在目标被生成后，或者目标消失后调用
    /// </GetTarget>
    public virtual void GetTarget()
    {

    }

    // -----
    // 以下方法继承于Actor | Entity
    // 不推荐在子类中重写
    public override void Enabled()
    {
        base.Enabled();
        if(isInBound == false)
            GetTarget();
    }
    public override void SetType()
    {
        base.SetType();
        ettType = EEntityType.Enemy;
        gameObject.tag = "enemy";
    }

    protected override void Init()
    {
        base.Init();
        
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }



}
