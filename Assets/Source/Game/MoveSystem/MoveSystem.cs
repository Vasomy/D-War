using System;
using System.Collections.Generic;

using UnityEngine;
using System.Runtime.CompilerServices;
using System.Collections;
using Unity.VisualScripting;
using System.Security.Cryptography;
using UnityEngine.EventSystems;

[assembly: InternalsVisibleTo("SelectSystem")]
public class MMoveSystem : SingletonBase<MMoveSystem>
{
    public List<FlowFieldPathFinding> ffpf = new List<FlowFieldPathFinding>();
    public Dictionary<Vector2Int,FlowFieldPathFinding> ffpfTable = new Dictionary<Vector2Int, FlowFieldPathFinding>();
    public List<FlowFieldPathFinding> deleteQueue = new List<FlowFieldPathFinding>();

    //test
    public int ffpfCount = 0;
    public int deleteCoznt;
    public int ffpfTableCount;
    //
    private bool DeleteFF(FlowFieldPathFinding ff)
    {
        ffpf.Remove(ff);
        return true;
    }
    private void Update()
    {
        ProccessMoveCommand();
        ffpfCount = ffpf.Count;
        deleteCoznt = deleteQueue.Count;
        ffpfTableCount = ffpfTable.Count;
    }

    public void AddFlowFieldPathFinding(Vector2Int target, List<Entity> entities)
    {
        if (ffpfTable.ContainsKey(target))
        {
           
            foreach (var ett in entities)
            {
                Debug.Log(ett.ettType);

                if(ett.TryGetComponent<ICanMove>(out var icm))
                {
                    if(icm.iPathFinding == ffpfTable[target])
                    {
                        return;
                    }

                    if (ffpfTable[target].entities.ContainsKey(ett.uid))
                    {
                        ffpfTable[target].entities.Add(ett.uid,ett);
                    }

                    if(icm.iPathFinding != null)
                    {
                        icm.iPathFinding.entities.Remove(ett.uid);
                        icm.iPathFinding = null;
                    }
                    icm.iPathFinding = ffpfTable[target];

                }

                //if (ett.ettType == EEntityType.Controlable)
                //{
                //    var caEtt = FCast.Cast<AControlableActor>(ett);
                //    var icm = ett.GetComponent<ICanMove>();
                //    if(caEtt.curFFPF == ffpfTable[target])
                //    {
                //        return;
                //    }

                //    if (!ffpfTable[target].entities.ContainsKey(ett.uid))
                //    {
                //        ffpfTable[target].entities.Add(ett.uid, (AControlableActor)ett);
                //    }
                //    if(caEtt.curFFPF != null)
                //    {
                //        caEtt.curFFPF.entities.Remove(caEtt.uid);
                //        caEtt.curFFPF = null;
                //    }
                //    ((AControlableActor)ett).curFFPF = ffpfTable[target];
                //    //icm.ChangeToMoveState();
                //}
            }
        }
        else
        {
            var FFPF = new FlowFieldPathFinding(entities, target);
            ffpf.Add(FFPF);
            ffpfTable.Add(target, FFPF);
        }
    }

    public static void MoveTo(Entity ett,Vector2 target)
    {
        MoveTo(ett, GridManager.GetIndexedPos(target));
    }

    public static void MoveTo(Entity ett,Vector2Int target)
    {
        instance.ffpfTable.TryGetValue(target,out var FFPF);

        if (FFPF != null)
        {
            if(!FFPF.entities.ContainsKey(ett.uid))
                FFPF.entities.Add(ett.uid,ett);
            //ett.GetComponent<ICanMove>().ChangeToMoveState();
        }
        else
        {
            instance.AddFlowFieldPathFinding(target, new List<Entity>{ett});

        }
    }

    public static void MoveTo(List<Entity>etts,Vector2Int target)
    {
        instance.ffpfTable.TryGetValue(target, out var FFPF);
        if(FFPF!=null)
        {
            foreach(var ett in etts)
            {
                FFPF.entities.Add(ett.uid, (AControlableActor)ett);
            }
        }
        else
        {
            instance.AddFlowFieldPathFinding(target, etts);
        }
    }

    public static void CancelMoveCommand(Entity cActor)
    {
        if (cActor.TryGetComponent<ICanMove>(out var icm))
        {
            var cFFPF = icm.iPathFinding;
            cFFPF.RemoveEntity(cActor);
            cFFPF = null;// 置空
        }
    }

    public void DeleteFlowFiledPathFinding(Vector2Int target)
    {
        var result = ffpfTable.TryGetValue(target, out var FFPF);
        if (!result) return;
        ffpfTable.Remove(target);
        ffpf.Remove(FFPF);
    }
    
    private void ProccessMoveCommand()
    {
        if(MSelectSystem.instance.selectedEntity.Count != 0)
        {
            if(Input.GetMouseButtonDown(1))
            {
                var cd = Physics2D.OverlapPoint(CameraController.instance.GetMousePos());
                if (cd != null)
                {
                    return;
                }
                //Debug.Log("S");
                Vector2 pos = new Vector2();
                
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // {x,y,2} = origin + direction * t;
                // oZ + dZ*t = 2;
                // t = (2-oZ)/dZ;
                float plane_pos_z = 0f;
                var t = (plane_pos_z - ray.origin.z) / ray.direction.z;
                pos.x = ray.origin.x + ray.direction.x * t;
                pos.y = ray.origin.y + ray.direction.y * t;

                CameraController.instance.text.text = pos.ToString();

                var indexedPos = GridManager.GetIndexedPos(pos);
                if(indexedPos.x < 0||indexedPos.y<0)
                {
                    return;
                }

                AddFlowFieldPathFinding(GridManager.GetIndexedPos(pos),
                    MSelectSystem.instance.selectedEntity)
                    ;
                foreach(var ett in MSelectSystem.instance.selectedEntity)
                {
                    if(ett.TryGetComponent<ICanMove>(out var icm))
                    {
                        icm.ChangeToMoveState();
                    }
                }
                //ffpf.Add(new FlowFieldPathFinding(MSelectSystem.instance.selectedEntity,
                //    GridManager.GetIndexedPos(CameraController.instance.GetMousePos())
                //    ));
            }
        }

        foreach(var ff in ffpf)
        {
            if(ff.Update())
            {
                deleteQueue.Add(ff);
            }
        }
        foreach(var ff in deleteQueue)
        {
            ff.End();
            DeleteFlowFiledPathFinding(ff.target);
        }
        if (deleteQueue.Count>=0)
            deleteQueue.Clear();
    }

    private void Removeff(FlowFieldPathFinding ff)
    {
        ffpf.Remove(ff);
        ffpfTable.Remove(ff.target);
    }

    private void OnDrawGizmos()
    {
        foreach(var ff in ffpf)
        {
            ff.GizmosDraw();
        }
    }
}
