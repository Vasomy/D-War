using System;
using System.Collections.Generic;

using UnityEngine;
using System.Runtime.CompilerServices;
using System.Collections;
using Unity.VisualScripting;

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
            Debug.Log("Has this ffpf!");
            foreach (var ett in entities)
            {
                if (ett.ettType == EEntityType.Controlable)
                {
                    if (!ffpfTable[target].entities.ContainsKey(ett.uid))
                    {
                        ffpfTable[target].entities.Add(ett.uid, (AControlableActor)ett);
                    }
                    var caEtt = FCast.Cast<AControlableActor>(ett);
                    if(caEtt.curFFPF != null)
                    {
                        caEtt.curFFPF.entities.Remove(caEtt.uid);
                        caEtt.curFFPF = null;
                        Debug.Log("Move e to !");
                    }
                    ((AControlableActor)ett).curFFPF = ffpfTable[target];
                }
            }
        }
        else
        {
            var FFPF = new FlowFieldPathFinding(entities, target);
            ffpf.Add(FFPF);
            ffpfTable.Add(target, FFPF);
        }
    }

    public static void MoveTo(Entity ett,Vector2Int target)
    {
        instance.ffpfTable.TryGetValue(target,out var FFPF);
        if (FFPF != null)
        {
            FFPF.entities.Add(ett.uid, (AControlableActor)ett);
        }
        else
        {
            instance.AddFlowFieldPathFinding(target, new List<Entity>{ett});
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
                AddFlowFieldPathFinding(GridManager.GetIndexedPos(CameraController.instance.GetMousePos()),
                    MSelectSystem.instance.selectedEntity);
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
