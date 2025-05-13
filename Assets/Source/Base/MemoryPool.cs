using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EntityMemoryPoolManager
{
    /// <summary>
    /// 字符串对应一个内存池
    /// 字符串的值来自于T.GetType().Name;
    /// </summary>
    static public Dictionary<string, EntityMemoryPool> pools = new Dictionary<string, EntityMemoryPool>();

    static public void Check<T>()where T:MonoBehaviour
    {
        EntityMemoryPoolProxy<T>.Check();
    }
    static public void Check(string tName)
    {
        if(pools.TryGetValue(tName, out EntityMemoryPool pool))
        {

        }
        else
        {
            pools.Add(tName,new EntityMemoryPool(tName));
        }
    }

    static public EntityMemoryPool GetPool<T>()where T : MonoBehaviour
    {
        Check<T>();
        if (pools.TryGetValue(typeof(T).Name, out var pool))
        {
            return pool;
        }
        Debug.LogError("Cant find pool that specify Type named " + typeof(T).Name + " !");
        return null;
    }
    static public EntityMemoryPool GetPool(string tName)
    {
        Check(tName);
        if(pools.TryGetValue(tName,out var pool))
        {
            return pool;
        }
        Debug.LogError("Cant find pool that specify Type named " + tName + " !");
        return null;
    }

    static public void Register<T>(T instance)where T:MonoBehaviour
    {
        EntityMemoryPoolProxy<T>.GetPool().RegisterObject(instance.gameObject);
    }
    static public void Register<T>(GameObject instance)where T:MonoBehaviour
    {
        EntityMemoryPoolProxy<T>.GetPool().RegisterObject(instance);
    }

    static public bool IsInPool<T>(T instance)where T :MonoBehaviour
    {
        return EntityMemoryPoolProxy<T>.GetPool().IsInPool(instance.gameObject);
    }
    static public bool IsInPool<T>(GameObject instance) where T : MonoBehaviour
    {
        return EntityMemoryPoolProxy<T>.GetPool().IsInPool(instance);
    }

}

/// <summary>
/// 某个类型T对应的类型池的代理类
/// </summary>
/// <typeparam name="T"></typeparam>
public class EntityMemoryPoolProxy<T>where T :MonoBehaviour
{
    public EntityMemoryPoolProxy()
    {
        pool = new EntityMemoryPool(typeof(T));
    }

    private EntityMemoryPool pool;

    static public EntityMemoryPool GetPool()
    {
        Check();
        return instance.pool;
    }

    static public EntityMemoryPoolProxy<T> instance = null;
    static public void Free(T target)
    {
        Check();
        instance.pool.Free(target.gameObject);
    }
    static public GameObject Get()
    {
        Check();
        return instance.pool.Get();
    }
    static public void Register(T target)
    {
        Check();
        instance.pool.RegisterObject(target.gameObject);
    }
    static public void Check()
    {
        if (instance == null)
            instance = new EntityMemoryPoolProxy<T>();
    }
}



// 内存池，每次Instantiate Entity
// 应该从该对象的池子里获取
// 如生成一个农民 =>  EntityMemoryPool<AFarmer>.instance().Get(/* Args */);
// 注意每一个不同的单位都应该有他自己的MemoryPool
// 所有的prefab名字应该与类名一致（非常重要！！！！！！！！！！！！！！！！！！！！）
// 所有的prefab名字应该与类名一致（非常重要！！！！！！！！！！！！！！！！！！！！）
// 所有的prefab名字应该与类名一致（非常重要！！！！！！！！！！！！！！！！！！！！）
// 所有的prefab名字应该与类名一致（非常重要！！！！！！！！！！！！！！！！！！！！）
// 所有的prefab名字应该与类名一致（非常重要！！！！！！！！！！！！！！！！！！！！）
// 所有的prefab名字应该与类名一致（非常重要！！！！！！！！！！！！！！！！！！！！）
// 
// 生成object，必须挂载了Entity脚本
public class EntityMemoryPool //<T> where T : MonoBehaviour
{
    //static private EntityMemoryPool<T> instance;
    //static public EntityMemoryPool<T> Instance()
    //{
    //    if(instance == null)
    //    {
    //        instance = new EntityMemoryPool<T>();
    //    }
    //    if (instance == null)
    //        Debug.Log("NULL MEMORY POOL!");
    //    return instance;
    //}

    static public GameObject freeObjectStorge =>GameObject.Find("FreeObjectStorge");
    static public GameObject unitStorge => GameObject.Find("UnitStorge");
    //static public GameObject freeUnitStorge => GameObject.Find("FreeUnitStorge");
    // 在该类型初始化时，创建的内存池的大小
    public const int DefaultSize = 1;
    public GameObject Target;
    public int Count=>freeQueue.Count + busySet.Count;
    public int freeCount=>freeQueue.Count;
    // free object 还没有被Get的对象
    public Queue<GameObject> freeQueue = new Queue<GameObject>();
    // 已经被Get的对象,k
    public HashSet<GameObject> busySet = new HashSet<GameObject>();

    private void FindAndSetPrefab(string pName)
    {

        string prefabName = pName;
        Debug.Log("Try to find prefab named : " + pName);
        if (GameContext.instance.allEntityPrefabsDict.ContainsKey(prefabName)) 
        {
            Target = GameContext.instance.allEntityPrefabsDict[prefabName];
        }
        else
        {
            Debug.LogError("Cant find prefab named : " + prefabName+" , please check your prefab is exist or named wrong!");
        }
    }
    
    public EntityMemoryPool(string typeName,int size = DefaultSize)
    {
        FindAndSetPrefab(typeName);

        for (int i = 0; i < size; i++)
        {
            NewOne();
        }
    }

    public EntityMemoryPool(Type type,int size = DefaultSize)
    {
        FindAndSetPrefab(type.Name);
        
        for(int i = 0;i< size; i++)
        {
            NewOne();
        }

    }
    /// <summary>
    /// 在testing时，如果想要把场景中的实体加入到内存池中管理，在Awake（或者重写Init方法）中
    /// 加RegisterObject（gameobject）
    /// 
    /// 在场景加载时，例如树木等静态资源单位，可以使用该方法，将其加入到内存中管理，
    /// 以便后续有可能生成静态资源单位时节省性能。
    /// 
    /// 在RegisterObject时，被调用的对象一定是active的对象，
    /// 所以应该添加到busySet中，保险起见，应该在添加前检测busySet中是否
    /// 已经含有。
    /// 
    /// </summary>
    /// <param name="obj"></param>
    public void RegisterObject(GameObject obj)
    {
        if(!busySet.Contains(obj))
        {
            busySet.Add(obj);
        }
    }
    /// <summary>
    /// 判断一个obj是否在内存池中管理
    /// </summary>
    /// <param name="obj"></param>
    public bool IsInPool(GameObject obj)
    {
        if(busySet.Contains(obj) || freeQueue.Contains(obj) )
        {
            return true;
        }
        return false;
    }
    public GameObject Get(Transform parent = null)
    {
        //Debug.Log("current fq size : "+freeCount);

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
        peek.GetComponent<Entity>().Enabled();
        return peek;
    }
    // 当一个实体结束时
    public void Free(GameObject obj)
    {
        if(busySet.Contains(obj))
        {
            obj.GetComponent<Entity>().Disabled();
            obj.SetActive(false);
            busySet.Remove(obj);
            freeQueue.Enqueue(obj);
            obj.transform.parent = freeObjectStorge.transform;
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

//
// 生成一般的类
public class ClassMemoryPool<T>
{
    public static ClassMemoryPool<T> instance;
    public const int poolSize = 10;
    public static ClassMemoryPool<T> Instance()
    {
        if (instance == null)
        {
            instance = new ClassMemoryPool<T>();
        }
        return instance;
    }
    private Queue<T> freeQueue;
    private Dictionary<Type, Queue<T>> freeSet;
    private Dictionary<Type,HashSet<T>> busySet;
    ClassMemoryPool()
    {
        freeQueue = new Queue<T>();
        freeSet = new Dictionary<Type, Queue<T>>();
        busySet = new Dictionary<Type, HashSet<T>>();
    }

    public _Ty Get<_Ty>()where _Ty:T,new()
    {
        if (freeSet.ContainsKey(typeof(_Ty)))
        {

        }
        else
        {
            freeSet[typeof(_Ty)] = new Queue<T>();
            
        }
        return default(_Ty);
    }

    private void NewOne<_Ty>()where _Ty : T , new()
    {
        
    }
}

