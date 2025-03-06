using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SingletonBase<T> : MonoBehaviour where T:SingletonBase<T> 
{
    static public T instance { get; private set; }
    // ����������������дAwake ���� Start
    private void Awake()
    {
        instance = (T)this;
        Debug.Log(instance.ToString() + "'s singleton instance was initialized!");
    }

    private void Start()
    {
        ConstructFunction();
    }

    private void Update()
    {
        OnUpdate();
    }

    protected virtual void ConstructFunction()
    {

    }

    protected virtual void OnUpdate()
    {

    }

    protected void DebugNullSingletonInstance()
    {
        if(instance == null)
            Debug.LogError(instance.ToString() + "'s singleton instance wasn't initialized!");
    }
}
