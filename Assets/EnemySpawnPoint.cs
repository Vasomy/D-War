using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlan
{
    public float acculatedTime;
    public float generateGap;
    public int targetNums;
    public int curNums = 0;
    public EntityMemoryPool genTargetPool;
}
public class EnemySpawnPoint : EStaticAlignedEntity
{
    protected override void Init()
    {
        base.Init();
        
        GridManager.CalculateOccupiedArea(uid,transform.position,2,2,2,2,false,false);
    }

    public void MakeSpawnPlan(List<SpawnBatch> batch)
    {
        plans.Enqueue(new List<SpawnPlan>());
        var front = plans.Peek();
        foreach(var sb in batch)
        {
            SpawnPlan plan = new SpawnPlan();
            plan.acculatedTime = 0.0f;
            plan.generateGap = 1.0f / sb.spawnNumsPerSecond;
            plan.targetNums = sb.count;
            plan.curNums = 0;
            plan.genTargetPool = EntityMemoryPoolManager.GetPool(sb.enemyName.ToString());
            front.Add(plan);
            //Debug.Log("Prepare to gen : " + sb.enemyName.ToString());
        }
    }

    public Queue<List<SpawnPlan>> plans = new Queue<List<SpawnPlan>>();
    private void Update()
    {
        if(plans.Count > 0)
        {
            var front = plans.Peek();
            bool isDone = true;
            foreach (var sp in front) {
                if (sp.curNums == sp.targetNums)
                {
                    continue;
                }
                sp.acculatedTime += Time.deltaTime;
                if (sp.acculatedTime > sp.generateGap)
                {
                    int gNum = (int)(sp.acculatedTime / sp.generateGap);
                    sp.curNums += gNum;
                    sp.acculatedTime -= gNum * sp.generateGap;
                    //
                    // spawn gNum targets
                    for (int i = 0; i < gNum; ++i)
                    {
                        var obj = sp.genTargetPool.Get();
                        obj.transform.position = transform.position;   
                    }
                }
                isDone = false;
            }
            if(isDone)
            {
                plans.Dequeue();
            }
        }
    }
}
