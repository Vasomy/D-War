using System.Collections.Generic;
using UnityEngine;

// 可以生产单位的建筑
public class BProducer : EBuildings
{
    static public GameObject unitsStorge => GameObject.FindWithTag("UnitStorge");// 生产单位的父transform

    public int maxProduceNums = 3; // 该建筑生产的单位最多可以存在的数量
    public int curProduceNums = 0; // 该建筑生产的单位当前存活的数量
    public GameObject target = null; // 该建筑生产的目标
    // message 完成对象池（内存池）
    // 现在生成单位的绑定对象不再是对象的实体，而是对象实体对应的内存池
    

    public float produceGap = 1.0f;

    public FTimer timer;

    protected override void Init()
    {
        base.Init();
        timer = new FTimer();
        timer.SetGap(produceGap);
        
    }
    // 不应该在最低级子类覆盖
    private void Update()
    {
        LogicUpdate();
    }
    // 找到周围一块合法的空地，生产单位，并下达移动到该处的命令
    public void Produce()
    {
        if(curProduceNums >=maxProduceNums)
        {
            return;
        }
        List<Vector2Int> cells = new List<Vector2Int>();
        var gridPos = GridManager.GetIndexedPos(transform.position);
        for(int i = -1 - lWidth;i<=1+rWidth;i++)
        {
            for(int j = -1 - dHeight;j<=1+uHeight;j++)
            { 
                if (i <= rWidth && i >= -lWidth && j <= uHeight && j >= -dHeight)
                {
                    continue;
                }
                Vector2Int cPos = new Vector2Int(i+gridPos.x,j+gridPos.y);
                var id = GridManager.GetPosID(cPos);
                if(id == 0)
                {
                    cells.Add(cPos);
                }
            }
        }
        if(cells.Count!=0)
        {
            
            var rd = Random.Range(0, cells.Count);
            var gPos = GridManager.GetPointByIndexedPos(cells[rd]);
            //gPos = gPos - (Vector2)transform.position;
            //Instantiate(target,gPos,new Quaternion(),unitsStorge.transform);
            // now we get memorypool so use it replace Instantiate
            // OvO
            Generate();

            curProduceNums++;
        }

    }
    // 在子类重写，
    // belike（BFarm） ：
    //
    // {
    //      MemoryPool<what you want>.Instance().Get(/* Args */);
    //      ....you can gen some else ett if you want
    //      MemoryPool<what you want 2>.Instance().Get(/* Args */);
    //      ....
    // }
    //
    // 最好在封装一层，传递想要产生的类型，而不是直接调用MemoryPool方法
    //
    //

    public virtual void Generate()
    {

    }
}
