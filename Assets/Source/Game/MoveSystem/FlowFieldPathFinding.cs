using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class FlowFieldNode
{
    public int x;
    public int y;
    public bool used = false;
    public bool IsLegal = true; // 能通过则为true
    public float cost = 1;// 通过该区域的代价，通常为1若有减速效果则变得更大
    public float fCost = int.MaxValue;// 到达目前区域的总代价
    public Vector2Int direction = new Vector2Int();// 当前node指向的方向
    public static bool operator ==(FlowFieldNode node,Vector2Int pos)
    {
        return node.x==pos.x && node.y==pos.y;
    }
    public static bool operator !=(FlowFieldNode node, Vector2Int pos)
    {
        return node.x != pos.x || node.y != pos.y;
    }
}

public class FlowField
{
    public Vector2Int target;
    public List<List<FlowFieldNode>> nodes;
    public float gridX;
    public float gridY;
    public FlowField(Vector2Int inTarget)
    {

        target = inTarget;
    }

    public FlowFieldNode GetNode(Vector2 point)
    {
        Vector2Int pos = GridManager.GetIndexedPos(point);
        return GetNode(pos.x,pos.y);
    }

    public FlowFieldNode GetNode(Vector2Int pos)
    {
        return GetNode(pos.x, pos.y);
    }

    public FlowFieldNode GetNode(int x,int y)
    {
        return nodes[y][x];
    }


    public void GizmosDrawFlowFieldDirection()
    {
        int widthNums = 20;// 左右两边各渲染 widthNums个网格
        int heightNums = 10;// 同上（上下两边）

        var cameraPoint = Camera.main.transform.position;
        var gm = GridManager.instance;
        var cameraCenterPos = GridManager.GetIndexedPos(cameraPoint);

        var currentMap = gm.currentMap;
        gridX = gm.XStep;
        gridY = gm.YStep;

        for (int i = -widthNums; i <= widthNums; i++)
        {
            for (int j = -heightNums; j <= heightNums; j++)
            {
                var pos = cameraCenterPos;
                pos.x += i;
                pos.y += j;
                if (pos.x >= 0 && pos.x <= currentMap.width
                    &&
                    pos.y >= 0 && pos.y <= currentMap.height)
                {

                }
                else
                {
                    continue;
                }

                var currentNode = GetNode(pos);

                Vector3 from = GridManager.GetPointByIndexedPos(pos) - (Vector2)currentNode.direction*(gridX/2.0f*0.8f);
                
                Vector3 to = GridManager.GetPointByIndexedPos(pos) + (Vector2)currentNode.direction * (gridX / 2.0f * 0.8f);

                Gizmos.DrawLine(from,to);
                Gizmos.DrawWireCube(to, Vector3.one*0.1f);
                
            }
        }
    }
    // 每次occupiedGrid变化（一般是再建造，拆除，采集完成后发生内容的更改）
    public void Update()
    {
        // occupiedGird
        var oG = GridManager.instance.occupiedGrid;
        if (oG != null)
        {
            int w = GridManager.instance.width;
            int h = GridManager.instance.height;
            Debug.Log("FlowField Size : " + w + " " + h);

            Func<int, int, bool> IsLegal = (x, y) =>
            {
                return x>=0&&x<=w&& y>=0&&y<=h;
            };

            Queue<Vector2Int> s = new Queue<Vector2Int>();
            if(nodes == null)
                nodes = new List<List<FlowFieldNode>> ();
            else
                nodes.Clear();
            for(int j =0;j<=h;++j)
            {
                nodes.Add(new List<FlowFieldNode>());
                for(int i =0;i<=w;++i)
                {
                    FlowFieldNode node = new FlowFieldNode();
                    node.x = i;
                    node.y = j;
                    Vector2Int key = new Vector2Int(i, j);
                    node.IsLegal = (oG.ContainsKey(key) == false || (oG.ContainsKey(key) == true && (int)oG[key] == 0 ) );
                    nodes[j].Add(node);
                }
            }
            
            nodes[target.y][target.x].fCost = 0;

            var targetNode = nodes[target.y][target.x];
            s.Enqueue(target);
            
            int cCount = 0;
            while(s.Count>0)
            {
                var top = s.Dequeue();
                var topNode = nodes[top.y][top.x];
                topNode.used = true;
                int x = top.x;
                int y = top.y;


                for(int i = -1;i<=1;++i)
                {
                    for(int j =-1;j<=1;++j)
                    {
                        if (i == 0 && j == 0) continue;
                        if (!IsLegal(x + i, y + j)) continue;
                        int nX = x + i;
                        int nY = y + j;
                        var nearNode = nodes[nY][nX];
                        
                        float cDis = Mathf.Sqrt(i*i+ j*j) + topNode.fCost;
                        if (nearNode.used) continue;
                        if (cDis < nearNode.fCost)
                        {
                            nearNode.direction = -new Vector2Int(i, j);
                            nearNode.fCost = cDis;
                            
                            s.Enqueue(new Vector2Int(nearNode.x, nearNode.y));
                        }
                        
                        cCount++;
                    }
                }
            }

            //Debug.Log("FFPF Count : "+cCount);
        }
    }
}

public class FlowFieldPathFinding
{
    public Vector2Int target;
    public Vector2    pointTarget;
    public FlowField flowField;
    public List<AControlableActor> entities;
    public FlowFieldPathFinding(List<Entity>selectedEntites,Vector2Int inTarget)
    {
        target = inTarget;
        pointTarget = GridManager.GetPointByIndexedPos(target);
        flowField = new FlowField(target);
        UpdateFlowField();
        entities = new List<AControlableActor>();
        foreach(var ett in selectedEntites)
        {
            if(ett.ettType == EEntityType.Controlable)
            {
                entities.Add((AControlableActor)ett);
            }
        }
    }

    public void UpdateFlowField()
    {
        flowField.Update();
    }

    public void Update()
    {
        //Debug.Log("Ett Count : "+entities.Count);
        foreach(var ett in entities)
        {
            var node = flowField.GetNode(ett.transform.position);
            if (node != target)
            {
                Vector2 fDir = node.direction;
                fDir = GridManager.GetPointByIndexedPos(new Vector2Int(node.x, node.y))
                    + fDir * new Vector2(flowField.gridX, flowField.gridY) / 2.0f - (Vector2)ett.transform.position;
                ett.SetVelocityDirection(fDir);
            }
            else
            {
                Vector2 fDir = GridManager.GetPointByIndexedPos(target) - (Vector2)ett.transform.position;
                ett.SetVelocityDirection(fDir);
            }
        }
    }

    public void GizmosDraw()
    {
       flowField.GizmosDrawFlowFieldDirection();
    }
}
