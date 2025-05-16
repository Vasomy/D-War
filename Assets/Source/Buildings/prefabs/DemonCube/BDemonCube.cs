using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��������ħ��
public class BDemonCube : EBuildings
{
    public FTimer timer = new FTimer();

    public float gap = 5.0f;
    public float radiuse = 1.0f;
    protected override void Init()
    {
        base.Init();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        var world = FWorld.currentWorld;
        if (timer.Timer())
        {
            //Debug.Log("Begin Eat!");
            foreach (var ett in world.friendlyEntity)
            {
                if (CompareFunction.EulerDistance(ett.transform.position,transform.position)<=radiuse) 
                {
                    var v2i = GridManager.GetIndexedPos(ett.transform.position);
                    //Debug.Log("Get One");
                    ///
                    /// ����п�������Ӧ�ò�����������ɶ���������һ�����������
                    ///

                    ett.Destroy();
                }
            }
            //Debug.Log("End Eat!");

        }
    }

    public override bool CalculateBuildingArea(Vector3 position, bool isPreview = false, bool isDelete = false)
    {
        return GridManager.CalculateOccupiedArea(uid, position,
            lWidth, rWidth, uHeight, dHeight,
            isPreview, isDelete);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radiuse);
    }
}
