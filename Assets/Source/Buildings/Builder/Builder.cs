using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : SingletonBase<Builder>
{
    // 通过UI当前选中的建筑的名字
    public string selectedBuildingsName = "NULL";
    // 建造的建筑的父object
    public GameObject builds;

    /// <summary>
    /// 预览obj应该有一个spriterenderer
    /// </summary>
    public GameObject previewObj;
    public SpriteRenderer previewSr=>previewObj.GetComponent<SpriteRenderer>();
    protected override void ConstructFunction()
    {
        base.ConstructFunction();
        builds = GameObject.Find("Builds");
    }

    bool dirty = false;
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (selectedBuildingsName != "NULL") 
        {
            if(!dirty)
            {
                dirty = true;
                return;
            }
            var result = GameContext.instance.DictBuildingsPrefab.TryGetValue(selectedBuildingsName, out var targetBd);
            if(result == false)
            {
                Debug.Log("Failed to find building named " +  selectedBuildingsName);
                return;
            }
            var targetPos = GridManager.AlignPoint(CameraController.instance.GetMousePosByRay(GridManager.instance.girdPlaneZ));
            var bd = targetBd.GetComponent<EBuildings>();

            bool isLegal = bd.CalculateBuildingArea(targetPos,true);

            if(!isLegal)
            {
                Debug.Log("This grid already occupied!");
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(targetBd, targetPos, new Quaternion(), builds.transform);
            }
            previewObj.transform.position = new Vector3(targetPos.x, targetPos.y,previewObj.transform.position.z);
            if(Input.GetMouseButtonDown (1))
            {
                selectedBuildingsName = "NULL";
                previewObj.SetActive(false);
                GridManager.UnRegisterPreviewOccupiedArea();
            }
        }
        else
        {
            dirty = false;
        }
    }

}
