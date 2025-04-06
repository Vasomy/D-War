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
    public const int x = 256;
    public const int y = 256;
    public static int PageSize()
    {
        return x * y;
    }

    //List<List<int>> indexed;
    long[,] indexed;
    public int Count { get; private set; } = 0;
    public Page()
    {
        indexed = new long[x, y];    
    }
    public long Find(long index)
    {
        return indexed[index / y,index%y];
    }

    public void Add(long index)
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
    public void Add(T t, long id)
    {
        packed.Add(t);
        long pageNum = id % (Page.x * Page.y);
    }
}
