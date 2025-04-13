using System.Collections.Generic;
using UnityEngine;

// ����������λ�Ľ���
public class BProducer : EBuildings
{
    static public GameObject unitsStorge => GameObject.FindWithTag("UnitStorge");// ������λ�ĸ�transform

    public int maxProduceNums = 3; // �ý��������ĵ�λ�����Դ��ڵ�����
    public int curProduceNums = 0; // �ý��������ĵ�λ��ǰ��������
    public GameObject target = null; // �ý���������Ŀ��
    // message ��ɶ���أ��ڴ�أ�
    // �������ɵ�λ�İ󶨶������Ƕ����ʵ�壬���Ƕ���ʵ���Ӧ���ڴ��
    

    public float produceGap = 1.0f;

    public FTimer timer;

    protected override void Init()
    {
        base.Init();
        timer = new FTimer();
        timer.SetGap(produceGap);
        
    }
    // ��Ӧ������ͼ����า��
    private void Update()
    {
        LogicUpdate();
    }
    // �ҵ���Χһ��Ϸ��Ŀյأ�������λ�����´��ƶ����ô�������
    public void Produce()
    {
        if(curProduceNums >=maxProduceNums)
        {
            return;
        }
        List<Vector2Int> cells = new List<Vector2Int>();
        var gridPos = GridManager.GetIndexedPos(transform.position);
        for(int i = -1 - lWidth;i<=1+rWidth;i++)
        {
            for(int j = -1 - dHeight;j<=1+uHeight;j++)
            { 
                if (i <= rWidth && i >= -lWidth && j <= uHeight && j >= -dHeight)
                {
                    continue;
                }
                Vector2Int cPos = new Vector2Int(i+gridPos.x,j+gridPos.y);
                var id = GridManager.GetPosID(cPos);
                if(id == 0)
                {
                    cells.Add(cPos);
                }
            }
        }
        if(cells.Count!=0)
        {
            
            var rd = Random.Range(0, cells.Count);
            var gPos = GridManager.GetPointByIndexedPos(cells[rd]);
            //gPos = gPos - (Vector2)transform.position;
            //Instantiate(target,gPos,new Quaternion(),unitsStorge.transform);
            // now we get memorypool so use it replace Instantiate
            // OvO
            Generate();

            curProduceNums++;
        }

    }
    // ��������д��
    // belike��BFarm�� ��
    //
    // {
    //      MemoryPool<what you want>.Instance().Get(/* Args */);
    //      ....you can gen some else ett if you want
    //      MemoryPool<what you want 2>.Instance().Get(/* Args */);
    //      ....
    // }
    //
    // ����ڷ�װһ�㣬������Ҫ���������ͣ�������ֱ�ӵ���MemoryPool����
    //
    //

    public virtual void Generate()
    {

    }
}
