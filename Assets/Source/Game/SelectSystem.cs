using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class MSelectSystem : SingletonBase<MSelectSystem>
{
    protected override void ConstructFunction()
    {
        base.ConstructFunction();
    }

    public List<Entity> selectedEntity;

    [SerializeField]
    bool onDrawingRect = false;
    [SerializeField]
    Vector3 RectBeginPos = Vector3.zero;
    [SerializeField]
    Vector3 RectEndPos = Vector3.zero;
    [Space]
    [SerializeField]
    Vector3 RectLeftBotPos = Vector3.zero;
    [SerializeField]
    Vector3 RectRightTopPos = Vector3.zero;
    protected override void OnUpdate()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            onDrawingRect = true;
            RectBeginPos = CameraController.instance.GetMouseScreenPos();

        }
        if (onDrawingRect && Input.GetMouseButtonUp(0))
        {
            onDrawingRect = false;
            if (CompareFunction.ManhattanDistance(RectBeginPos, RectEndPos) > 5.0f)
                CheckSelectEntity(RectBeginPos, RectEndPos);
        }
        if (onDrawingRect)
        {
            RectEndPos = CameraController.instance.GetMouseScreenPos();
        }
        // do someting on selected entity
        // try to move
        var mousePos2 = CameraController.instance.GetMouseScreenPos();
        if (Input.GetMouseButtonDown(1))
        {
            
        }



    }

    void CheckSelectEntity(Vector3 rectBeginPos, Vector3 rectEndPos)
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Entity");

        //Vector2 leftBotPos = new Vector2(Mathf.Min(rectBeginPos.x,rectEndPos.x),Mathf.Min(rectBeginPos.y,rectEndPos.y));
        //Vector2 rightTopPos = new Vector2(Mathf.Max(rectBeginPos.x,rectEndPos.x),Mathf.Max(rectBeginPos.y,rectEndPos.y));
        RectLeftBotPos.Set(Mathf.Min(rectBeginPos.x, rectEndPos.x), Mathf.Min(rectBeginPos.y, rectEndPos.y), 0);
        RectRightTopPos.Set(Mathf.Max(rectBeginPos.x, rectEndPos.x), Mathf.Max(rectBeginPos.y, rectEndPos.y), 0);
        foreach (var unit in units)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
            if (
                screenPos.x >= RectLeftBotPos.x && screenPos.x <= RectRightTopPos.x &&
                screenPos.y >= RectLeftBotPos.y && screenPos.y <= RectRightTopPos.y
                )
            {
                SelectEntityOneByOne(unit.GetComponent<Entity>());
            }
        }
    }

    public static void ReleaseCapture()
    {
        foreach (var ett in instance.selectedEntity)
        {
            (ett).ReleaseSelected();
        }

        instance.selectedEntity.Clear();
    }

    void SelectEntityOneByOne(Entity entity)
    {
        entity.InfoSelect();
        instance.selectedEntity.Add(entity);
    }
    public GUIStyle rectStyle;
    
    public Material GLRectMat;//绘图的材质，在Inspector中设置
    public Color GLRectColor;//矩形的内部颜色，在Inspector中设置
    public Color GLRectEdgeColor;//矩形的边框颜色，在Inspector中设置


    void OnPostRender()
    {
        if (onDrawingRect)
        {
            //准备工作:获取确定矩形框各角坐标所需的各个数值
            float Xmin = Mathf.Min(RectBeginPos.x, RectEndPos.x);
            float Xmax = Mathf.Max(RectBeginPos.x, RectEndPos.x);
            float Ymin = Mathf.Min(RectBeginPos.y, RectEndPos.y);
            float Ymax = Mathf.Max(RectBeginPos.y, RectEndPos.y);

            if (!GLRectMat)
                return;
            
            GL.PushMatrix();//GL入栈
                            //在这里，只需要知道GL.PushMatrix()和GL.PopMatrix()分别是画图的开始和结束信号,画图指令写在它们中间


            GLRectMat.SetPass(0);//启用线框材质rectMat

            GL.LoadPixelMatrix();//设置用屏幕坐标绘图


            /*------第一步，绘制矩形------*/
            GL.Begin(GL.QUADS);//开始绘制矩形,这一段画的是框中间的半透明部分，不包括边界线

            GL.Color(GLRectColor);//设置矩形的颜色，注意GLRectColor务必设置为半透明

            //陈述矩形的四个顶点
            GL.Vertex3(Xmin, Ymin, 0);//陈述第一个点，即框的左下角点，记为点1
            GL.Vertex3(Xmin, Ymax, 0);//陈述第二个点，即框的左上角点，记为点2
            GL.Vertex3(Xmax, Ymax, 0);//陈述第三个点，即框的右上角点，记为点3
            GL.Vertex3(Xmax, Ymin, 0);//陈述第四个点，即框的右下角点，记为点4

            GL.End();//告一段落，此时画好了一个无边框的矩形


            /*------第二步，绘制矩形的边框------*/
            GL.Begin(GL.LINES);//开始绘制线，用来描出矩形的边框

            GL.Color(GLRectEdgeColor);//设置方框的边框颜色，建议设置为不透明的

            //描第一条边
            GL.Vertex3(Xmin, Ymin, 0);//起始于点1
            GL.Vertex3(Xmin, Ymax, 0);//终止于点2

            //描第二条边
            GL.Vertex3(Xmin, Ymax, 0);//起始于点2
            GL.Vertex3(Xmax, Ymax, 0);//终止于点3

            //描第三条边
            GL.Vertex3(Xmax, Ymax, 0);//起始于点3
            GL.Vertex3(Xmax, Ymin, 0);//终止于点4

            //描第四条边
            GL.Vertex3(Xmax, Ymin, 0);//起始于点4
            GL.Vertex3(Xmin, Ymin, 0);//返回到点1

            GL.End();//画好啦！

            GL.PopMatrix();//GL出栈
        }
    }
}
