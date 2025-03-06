using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class FlowFieldNode
{
    public int x;
    public int y;
    public bool IsLegal = true; // 能通过则为true
    public float cost = 1;// 通过该区域的代价，通常为1若有减速效果则变得更大
    public float fCost = 1;// 到达目前区域的总代价
    public Vector2Int direction = new Vector2Int();// 当前node指向的方向
    
}

public class FlowField
{
    public Vector2Int target;
    public Hashtable occupiedGird;
    public FlowField(Vector2Int inTarget)
    {
        target = inTarget;
    }

   
    // 每次occupiedGrid变化（一般是再建造，拆除，采集完成后发生内容的更改）
    public void UpdateFlowField()
    {
        // occupiedGird
        var oG = GridManager.instance.occupiedGrid;
        if (oG != null)
        {
            int w = GridManager.instance.width;
            int h = GridManager.instance.height;

            Func<int, int, bool> IsLegal = (x, y) =>
            {
                return x>=0&&x<=w&& y>=0&&y<=h;
            };

            Stack<Vector2Int> s = new Stack<Vector2Int>();

            s.Push(target);

        }
    }
}

public class FlowFieldPathFinding
{
    
}
