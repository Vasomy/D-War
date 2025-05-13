using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

// 不再是单例，而是每一个FMap : Mono 都有一个GridManager
public class GridManager : SingletonBase<GridManager>
{
    public float girdPlaneZ = 0.0f;
    
    // override func
    protected override void ConstructFunction() 
    {
        previewGrid = new Hashtable();
        //Debug.Log((int)(-0.5f));
    }
    
    protected override void OnUpdate()
    {
        base.OnUpdate();
        cursorOnGirdX = GetPos(CameraController.instance.GetMousePosByRay(0)).x;
        cursorOnGirdY = GetPos(CameraController.instance.GetMousePosByRay(0)).y;
        //CameraController.instance.text.text = cursorOnGirdX.ToString() +" , "+ cursorOnGirdY.ToString();
    }

    // static function

    /// <summary>
    /// 将目标位置对齐到网格
    /// </summary>
    /// <param name="point"></目标位置>
    /// <returns></返回对齐后的结果>
    public static Vector2 AlignPoint(Vector2 point)
    {
        instance.DebugNullSingletonInstance();
        return instance.GetCenterPoint(instance.GetPos(point));
    }
    public static Vector2 GetPointByIndexedPos(Vector2Int pos)
    {
        instance.DebugNullSingletonInstance();
        return instance.GetCenterPoint(pos);
    }
    // 传入一个坐标，获取他在Grid中对应的Vector2Int索引
    public static Vector2Int GetIndexedPos(Vector2 point)
    {
        instance.DebugNullSingletonInstance();
        return instance.GetPos(point);
    }
    // 传入一个Vector2Int索引，获取该索引对应的entity的id
    public static long GetPosID(Vector2Int pos)
    {
        instance.DebugNullSingletonInstance();
        if (instance.occupiedGrid.ContainsKey(pos))
            return (long)instance.occupiedGrid[pos];
        else
            return 0;
    }
    public static bool CalculateOccupiedArea(
        long objectId,Vector2 point,
        int lWidth,int rWidth,int uHeight,int dHeight, bool isPreview,bool isDelete
        )
    {
        instance.DebugNullSingletonInstance();
        if(isPreview)
        {
            instance.previewGrid.Clear();
        }
        return instance.RegisterOccupiedArea_d(objectId, point, lWidth, rWidth, uHeight, dHeight, isPreview, isDelete); ;
    }
    
    public static void UnRegisterPreviewOccupiedArea()
    {
        instance.DebugNullSingletonInstance();
        instance.previewGrid.Clear();

    }
    //
    public static void LinkAMap(FMap inMap)
    {
        instance.DebugNullSingletonInstance();
        instance.LinkAMap_d(inMap);
    }

    // point -> float // pos(index) -> [int,int]
    //
    public FMap currentMap;
    public Hashtable occupiedGrid=>currentMap.occupiedGrid; // key 为 Vector2Int val 为 目标的Uid
    public Hashtable previewGrid { get; private set; } = new Hashtable();// 用于存储 建造时预览的格子

    // GridManager 所管理的grid的数量 width*height = girdsNum
    // 其数据来源于FMap类
    public int width = 50;
    public int height = 50;
    [SerializeField]
    private int cursorOnGirdX;
    [SerializeField]

    private int cursorOnGirdY;
    public void LinkAMap_d(FMap inMap)
    {
        currentMap = inMap;
    }

    bool RegisterOccupiedArea_d(long objectId,Vector2 point,
        int lWidth,int rWidth,int uHeight,int dHeight,bool isPreview,bool isDelete = false)// rectangle
        //
        //  口口口口口口口口
        //  口口{center}口   // 左边边预览lwidth = 2， rwidth = 1 uheight = 1，dheight = 1
        //  口口口口口口口口
        //
        //  {center}        // lw，rw，uh，dh均为0
        // 若计算位置已经被占用返回 false
    {
        bool isLegal = true;

        Vector2Int centerPos = new Vector2Int();
        centerPos = GetPos(point);
        int bx = centerPos.x;
        int by = centerPos.y;

        // current x&y
        int cx = bx;
        int cy = by;

        Vector2Int cur = new Vector2Int();

        for(int i =bx-lWidth;i<=bx+rWidth;++i)
        {
            for(int j = by-uHeight;j<=by+dHeight;++j)
            {
                cur.x = i;
                cur.y = j;
                if(isPreview)
                {
                    if (occupiedGrid.ContainsKey(cur))
                        isLegal = false;
                    previewGrid[cur] = (int)1;// 1 表明预览已经有了 
                    //Debug.Log(i + " : " + j);
                    continue;
                }
                else
                {
                    if(!isDelete)
                    {
                        occupiedGrid[cur] = objectId;
                        //Debug.Log(i + " : " + j);
                    }
                    else
                    {
                        //occupiedGrid[cur] = 0;
                        occupiedGrid.Remove(cur);
                    }
                }
            }
        }

        return isLegal;
    }

    
    public float XStep = 1.0f;// 对于计算占据网格的精度，例如0.5就是 一个unity标准的格子中划分4个小格子
    public float YStep = 1.0f;
    public Vector2 Anchor = Vector2.zero;// 网格计算的起点，默认为0 0，不建议改动


    //根据实际的X和Y的坐标，获取在Grid的索引
    Vector2Int GetPos(Vector2 point)
    {
        Vector2Int ret = new Vector2Int();

        int x = Mathf.FloorToInt((point.x - (float)Anchor.x) / XStep);
        int y = Mathf.FloorToInt((point.y - (float)Anchor.y) / YStep);

        ret.x = x;
        ret.y = y;

        return ret;
    }
    // 根据 Grid的索引获取世界坐标（无Z）
    Vector2 GetCenterPoint(Vector2Int Pos)
    {
        Vector2 ret = new Vector2Int();

        float x = Pos.x * XStep + Anchor.x;
        float y = Pos.y * YStep + Anchor.y;

        float next_x = x + XStep;
        float next_y = y + YStep;

        float center_x = (x + next_x) / 2.0f;
        float center_y = (y + next_y) / 2.0f;

        ret.x = center_x;
        ret.y = center_y;

        return ret;
    }

    private void OnDrawGizmos()
    {
        if(currentMap == null)
        {
            return;
        }
        var cameraPoint = Camera.main.transform.position;
        var cameraCenterPos = GetPos(cameraPoint);

        int widthNums = 20;// 左右两边各渲染 widthNums个网格
        int heightNums = 10;// 同上（上下两边）

        Color color = Color.white;
        color.a = 0.5f;
        Gizmos.color = color;

        for(int i =-widthNums; i <=widthNums;i++)
        {
            for(int j = -heightNums; j <=heightNums;j++)
            {
                var pos = cameraCenterPos;
                pos.x += i;
                pos.y += j;

                if(pos.x>=0&&pos.x<=currentMap.width
                    &&
                    pos.y>=0&&pos.y<=currentMap.height)
                {

                }
                else
                {
                    continue;
                }

                var point = GetCenterPoint(pos);

                

                if(occupiedGrid.ContainsKey(pos))
                {
                    Gizmos.color = Color.red;
                }
                else if(previewGrid.ContainsKey(pos))
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = color;
                }

                //Gizmos.DrawWireCube(point, Vector3.one * XStep);

                var lt = new Vector2(point.x + XStep / 2.0f, point.y + YStep / 2.0f);
                var rt = new Vector2(point.x - XStep / 2.0f, point.y + YStep / 2.0f);
                var rd = new Vector2(point.x - XStep / 2.0f, point.y - YStep / 2.0f);
                var ld = new Vector2(point.x + XStep / 2.0f, point.y - YStep / 2.0f);

                Gizmos.DrawLine(lt, rt);
                Gizmos.DrawLine(rt, rd);
                Gizmos.DrawLine(rd, ld);
                Gizmos.DrawLine(ld, lt);

            }
        }
    }

    
}
