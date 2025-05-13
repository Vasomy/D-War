using System.Collections.Generic;
using UnityEngine;

// 每一个不同的游戏关卡应该有一个自己的FWorld用于管理实体
public class FWorld : MonoBehaviour
{
    public FTechTreeNode node;

    static public FWorld currentWorld;
    private void Awake()
    {
        currentWorld = this;

        friendlyEntity = new TSparseSet<Entity>();
        enemyEntity = new TSparseSet<Entity>();
        staticEntity = new TSparseSet<Entity>();
    }

    public TSparseSet<Entity> friendlyEntity;
    public TSparseSet<Entity> enemyEntity;
    public TSparseSet<Entity> staticEntity;// 如树木矿石等资源实体

    public void RegisterFriendlyEntity(Entity ett)
    {
        friendlyEntity.Add(ett, ett.uid);
    }
    public void RegisterEnemyEntity(Entity ett)
    {
        enemyEntity.Add(ett,ett.uid);
    }
    public void RegisterStaticEntity(Entity ett)
    {
        staticEntity.Add(ett,ett.uid);
    }

    public List<Entity> GetAllFriendlyEntity()
    {
        return friendlyEntity.packed;
    }
    public List<Entity> GetAllEnemyEntity()
    {
        return enemyEntity.packed;
    }
    public List<Entity> GetAllStaticEntity()
    {
        return staticEntity.packed;
    }
}
