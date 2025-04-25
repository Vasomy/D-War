using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

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
public class EntityMemoryPool<T> where T : MonoBehaviour
{
    static private EntityMemoryPool<T> instance;
    static public EntityMemoryPool<T> Instance()
    {
        if(instance == null)
        {
            instance = new EntityMemoryPool<T>();
        }
        if (instance == null)
            Debug.Log("NULL MEMORY POOL!");
        return instance;
    }

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
    public EntityMemoryPool(int size = DefaultSize)
    {
        string prefabName = typeof(T).Name;
        //Debug.Log(paths.Count());
        if (GameContext.instance.allEntityPrefabsDict.ContainsKey(prefabName)) 
        {
            Target = GameContext.instance.allEntityPrefabsDict[prefabName];
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

