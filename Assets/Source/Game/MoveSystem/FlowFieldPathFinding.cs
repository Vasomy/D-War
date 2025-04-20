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

    public override bool Equals(object obj)
    {
        return this == (FlowFieldNode)obj;
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
           // Debug.Log("FlowField Size : " + w + " " + h);

            Func<int, int, bool> IsLegal = (x, y) =>
            {
                return x>=0&&x<=w&& y>=0&&y<=h;
            };

            Queue<Vector2Int> s = new Queue<Vector2Int>();
            if(nodes == null)
                nodes = new List<List<FlowFieldNode>> ();
            else
                nodes.Clear();
            //Debug.Log("BFS 0");

            for (int j =0;j<=h;++j)
            {
                nodes.Add(new List<FlowFieldNode>());
                for(int i =0;i<=w;++i)
                {
                    FlowFieldNode node = new FlowFieldNode();
                    node.x = i;
                    node.y = j;
                    Vector2Int key = new Vector2Int(i, j);
                    node.IsLegal = (oG.ContainsKey(key) == false || (oG.ContainsKey(key) == true && (long)oG[key] == 0L ) );
                    nodes[j].Add(node);
                }
            }
            //Debug.Log("BFS 1");
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
            //Debug.Log("BFS 2");



            //Debug.Log("FFPF Count : "+cCount);
        }
    }
}
public class FlowFieldPathFinding
{
    /// <summary>
    /// 该路径结束的原因是什么，
    /// 如 最大时间到了，该路径下所有的单位有了新的寻路路径或该路径下的所有单位因为某些原因被销毁（处于非激活状态）了
    /// 
    /// </summary>
    public string EndReason = "Unknow";

    public Vector2Int target;
    public Vector2    pointTarget;
    public FlowField flowField;
    public int ettCount;
    //  dict replace list
    public Dictionary<long, Entity> entities; // entity.uid is key
    //public List<AControlableActor> entities;
    public float expectMaxTime = 0.0f;// 该流场下的单位到达目的地（附近的最长时间）+ 0.5f, 该时间就是该类是生命周期
    public FTimer timer;
    public FlowFieldPathFinding(List<Entity>selectedEntites,Vector2Int inTarget)
    {
        target = inTarget;
        pointTarget = GridManager.GetPointByIndexedPos(target);
        flowField = new FlowField(target);
        //entities = new List<AControlableActor>();
        entities = new Dictionary<long, Entity>();
        foreach(var ett in selectedEntites)
        {
            
            if(ett.TryGetComponent<ICanMove>(out var icm))
            {
                //Debug.Log(ett.GetType());
                //Debug.Log("To ett a ffpf");
                //var caEtt = FCast.Cast<AControlableActor>(ett);
                //if (caEtt.curFFPF != null)
                //{ 
                //    Debug.Log("Has curFFPF");
                //    caEtt.curFFPF.entities.Remove(caEtt.uid);
                //    caEtt.curFFPF = null;
                //}
                //((AControlableActor)ett).curFFPF = this;
                //entities.Add(ett.uid,(AControlableActor)ett);
                //icm.ChangeToMoveState();

                if(icm.iPathFinding !=null)
                {
                    icm.iPathFinding.entities.Remove(ett.uid);
                    icm.iPathFinding = null;
                }
                icm.iPathFinding = this;
                entities.Add(ett.uid, ett);

            }
        }
        //Debug.Log("Nums of ett is " + entities.Count);
        timer = new FTimer();
        UpdateFlowField();
    }

    public void End()
    {
        
        foreach(var ett in entities)
        {
            if (ett.Value.TryGetComponent<ICanMove>(out var icm))
            {
                icm.iPathFinding = null;
                //if (ett.Value.Tr != null)
                //{
                //    ett.Value.iDirection = Vector2.zero;
                //}
                icm.iDirection = Vector2.zero;
            }
        }
    }

    public void AddEntity(Entity ett)
    {
        ett.TryGetComponent<ICanMove>(out var icm);
        if(icm ==null)
        {
            return;
        }
        entities.Add(ett.uid, ett);
        var node = flowField.GetNode(ett.transform.position);
        float eTime = node.fCost / icm.iSpeed;
        expectMaxTime = Mathf.Max(eTime, expectMaxTime);
    }

    public void RemoveEntity(Entity ett)
    {
        entities.Remove(ett.uid);
    }

    public void UpdateFlowField()
    {
        //Debug.Log("Begin ff update");
        flowField.Update();
        //Debug.Log("Fsh ff update");
        foreach(var ett in entities)
        {
            if (ett.Value == null)
            {
                return;
            }
            if (ett.Value.TryGetComponent<ICanMove>(out var icm))
            {
                // 一个实体所在的位置的节点
                var node = flowField.GetNode(ett.Value.transform.position);
                float eTime = node.fCost / icm.iSpeed;
                expectMaxTime = Mathf.Max(eTime, expectMaxTime);
            }
        }
        timer.SetGap(expectMaxTime);
        //Debug.Log("Expect Time : " + expectMaxTime);
    }

    public bool Update()
    {
        ettCount = entities.Count;
        if(timer.Timer())
        {
            EndReason = "Time up!";
            return true;
        }
        if(entities.Count==0)
        {
            EndReason = "Entities in this ffpf is 0!";
            return true;
        }
        //Debug.Log("Ett Count : "+entities.Count);
        foreach(var ett in entities)
        {
            var node = flowField.GetNode(ett.Value.transform.position);
            var icm = ett.Value.GetComponent<ICanMove>();
            if (node != target)
            {
                Vector2 fDir = node.direction;
                fDir = GridManager.GetPointByIndexedPos(new Vector2Int(node.x, node.y))
                    + fDir * new Vector2(flowField.gridX, flowField.gridY) / 2.0f - (Vector2)ett.Value.transform.position;
                icm.iDirection = fDir.normalized;
            }
            else
            {
                Vector2 fDir = GridManager.GetPointByIndexedPos(target) - (Vector2)ett.Value.transform.position;
                icm.iDirection = fDir.normalized;
            }
        }
        return false;
    }

    public void GizmosDraw()
    {
       flowField.GizmosDrawFlowFieldDirection();
    }
}
