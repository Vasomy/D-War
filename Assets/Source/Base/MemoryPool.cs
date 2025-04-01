using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

// 内存池，每次Instantiate Entity
// 应该从该对象的池子里获取
// 如生成一个农民 =>  MemoryPool<AFarmer>.instance().Get(/* Args */);
// 注意每一个不同的单位都应该有他自己的MemoryPool
// 所有的prefab名字应该与类名一致（非常重要！！！！！！！！！！！！！！！！！！！！）
// 所有的prefab名字应该与类名一致（非常重要！！！！！！！！！！！！！！！！！！！！）
// 所有的prefab名字应该与类名一致（非常重要！！！！！！！！！！！！！！！！！！！！）
// 所有的prefab名字应该与类名一致（非常重要！！！！！！！！！！！！！！！！！！！！）
// 所有的prefab名字应该与类名一致（非常重要！！！！！！！！！！！！！！！！！！！！）
// 所有的prefab名字应该与类名一致（非常重要！！！！！！！！！！！！！！！！！！！！）
// 

public class MemoryPool<T>
{
    static public MemoryPool<T> instance;
    static public MemoryPool<T> Instance()
    {
        if(instance == null)
        {
            instance = new MemoryPool<T>();
        }
        return instance;
    }

    public GameObject freeObjectStorge =>GameObject.Find("FreeObjectStorge");
    static public GameObject unitStorge => GameObject.Find("UnitStorge");
    // 在该类型初始化时，创建的内存池的大小
    public const int DefaultSize = 1;
    public GameObject Target;
    public int Count=>freeQueue.Count + busySet.Count;
    public int freeCount=>freeQueue.Count;
    // free object 还没有被Get的对象
    public Queue<GameObject> freeQueue = new Queue<GameObject>();
    // 已经被Get的对象,k
    public HashSet<GameObject> busySet = new HashSet<GameObject>();
    public MemoryPool(int size = DefaultSize)
    {
        string prefabName = typeof(T).Name;
        var paths = Directory.GetFiles("Assets", prefabName + ".prefab", SearchOption.AllDirectories);
        Debug.Log(paths.Count());
        if(paths.Count() > 0)
        {
            Target = AssetDatabase.LoadAssetAtPath<GameObject>(paths[0]);
        }
        else
        {
            Debug.LogError("Cant find prefab named : " + prefabName+" , please check your prefab is exist or named wrong!");
        }

        if(Target == null)
        {
            Debug.Log("Target is null? WTF!");
        }
        
        for(int i = 0;i< size; i++)
        {
            NewOne();
        }
    }

    public GameObject Get(Transform parent = null)
    {
        Debug.Log("current fq size : "+freeCount);

        if (parent == null)
        {
            parent = unitStorge.transform;
        }
        if(freeCount==0)
        {
            LargeStorge();
        }
        var peek = freeQueue.Peek();
        peek.transform.parent = parent;
        busySet.Add(peek);
        freeQueue.Dequeue();
        peek.SetActive(true);
        return peek;
    }
    // 当一个实体结束时
    public void Free(GameObject obj)
    {
        if(busySet.Contains(obj))
        {
            obj.SetActive(false);
            busySet.Remove(obj);
            freeQueue.Enqueue(obj);
        }
    }
    public void LargeStorge()
    {
        int targetLargeNum = (int)(Mathf.Ceil(Count * 0.5f));
        
        for(int i =0;i<targetLargeNum;i++)
        {
            NewOne();
        }
    }

    private void NewOne()
    {
        var go = GameObject.Instantiate(Target, freeObjectStorge.transform);
        freeQueue.Enqueue(go);
        go.SetActive(false);
    }
}


