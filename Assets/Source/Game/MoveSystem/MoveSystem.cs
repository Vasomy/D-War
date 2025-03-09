using System;
using System.Collections.Generic;

using UnityEngine;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SelectSystem")]
public class MMoveSystem : SingletonBase<MMoveSystem>
{
    public List<FlowFieldPathFinding> ffpf = new List<FlowFieldPathFinding>();

    private void Update()
    {
        ProccessMoveCommand();
    }
    private void ProccessMoveCommand()
    {
        if(MSelectSystem.instance.selectedEntity.Count != 0)
        {
            if(Input.GetMouseButtonDown(1))
            {
                ffpf.Add(new FlowFieldPathFinding(MSelectSystem.instance.selectedEntity,
                    GridManager.GetIndexedPos(CameraController.instance.GetMousePos())));
            }
        }
        foreach(var ff in ffpf)
        {
            ff.Update();
        }
    }

    private void OnDrawGizmos()
    {
        foreach(var ff in ffpf)
        {
            ff.GizmosDraw();
        }
    }
}
