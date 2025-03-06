using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PackedInfo
{
    int page;
    int index;
}
public class Page
{
    public static int x = 100;
    public static int y = 100;
    List<List<int>> indexed;
    public int Count { get; private set; } = 0;
    public Page()
    {
        indexed = new List<List<int>>(y);
        for(int i =  0; i < indexed.Count; i++)
        {
            indexed[i] = new List<int>(x);
        }
    }
    public int Find(int index)
    {
        return indexed[index / y][index % y];
    }

    public void Add(int index)
    {

    }

    public bool IsFull()
    {
        return Count == x * y;
    }
}
public class TSparseSet<T> : MonoBehaviour
{
    private List<T> packed;
    [SerializeField]
    private List<PackedInfo> packedInfo;
    private List<Page> page;

    private void Start()
    {
        packed = new List<T>();
        packedInfo = new List<PackedInfo>();
        page = new List<Page>();
        page.Add(new Page());
    }
    public void Add(T t, int id)
    {
        packed.Add(t);
        int pageNum = id % (Page.x * Page.y);
    }
}
