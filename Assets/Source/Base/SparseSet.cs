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
    public int idx = 0;

    public const int x = 512;
    public const int y = 512;
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
        return indexed[index % y,index/y];
    }

    public void Add(long index)
    {
        long ix = index % Page.y;
        long iy = index / Page.y;
        indexed[ix, iy] = index;
    }

    public bool IsFull()
    {
        return Count == x * y;
    }
}
public class TSparseSet<T> : MonoBehaviour
{
    public List<T> packed { get; private set; }
    [SerializeField]
    private List<PackedInfo> packedInfo;
    private Page page;

    private void Start()
    {
        packed = new List<T>();
        packedInfo = new List<PackedInfo>();
        page = new Page();
    }
    public void Add(T t, long id)
    {
        packed.Add(t);
        int backIdx = packed.Count - 1;
        page.Add(id);

    }

    public T Find(long id)
    {
        long index = page.Find(id);
        return packed[(int)index];
    }
}
