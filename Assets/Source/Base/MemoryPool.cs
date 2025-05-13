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
    /// �ַ�����Ӧһ���ڴ��
    /// �ַ�����ֵ������T.GetType().Name;
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
/// ĳ������T��Ӧ�����ͳصĴ�����
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



// �ڴ�أ�ÿ��Instantiate Entity
// Ӧ�ôӸö���ĳ������ȡ
// ������һ��ũ�� =>  EntityMemoryPool<AFarmer>.instance().Get(/* Args */);
// ע��ÿһ����ͬ�ĵ�λ��Ӧ�������Լ���MemoryPool
// ���е�prefab����Ӧ��������һ�£��ǳ���Ҫ������������������������������������������
// ���е�prefab����Ӧ��������һ�£��ǳ���Ҫ������������������������������������������
// ���е�prefab����Ӧ��������һ�£��ǳ���Ҫ������������������������������������������
// ���е�prefab����Ӧ��������һ�£��ǳ���Ҫ������������������������������������������
// ���е�prefab����Ӧ��������һ�£��ǳ���Ҫ������������������������������������������
// ���е�prefab����Ӧ��������һ�£��ǳ���Ҫ������������������������������������������
// 
// ����object�����������Entity�ű�
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
    // �ڸ����ͳ�ʼ��ʱ���������ڴ�صĴ�С
    public const int DefaultSize = 1;
    public GameObject Target;
    public int Count=>freeQueue.Count + busySet.Count;
    public int freeCount=>freeQueue.Count;
    // free object ��û�б�Get�Ķ���
    public Queue<GameObject> freeQueue = new Queue<GameObject>();
    // �Ѿ���Get�Ķ���,k
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
    /// ��testingʱ�������Ҫ�ѳ����е�ʵ����뵽�ڴ���й�����Awake��������дInit��������
    /// ��RegisterObject��gameobject��
    /// 
    /// �ڳ�������ʱ��������ľ�Ⱦ�̬��Դ��λ������ʹ�ø÷�����������뵽�ڴ��й���
    /// �Ա�����п������ɾ�̬��Դ��λʱ��ʡ���ܡ�
    /// 
    /// ��RegisterObjectʱ�������õĶ���һ����active�Ķ���
    /// ����Ӧ����ӵ�busySet�У����������Ӧ�������ǰ���busySet���Ƿ�
    /// �Ѿ����С�
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
    /// �ж�һ��obj�Ƿ����ڴ���й���
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
    // ��һ��ʵ�����ʱ
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
// ����һ�����
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

