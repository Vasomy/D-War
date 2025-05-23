using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SingletonBase<T> : MonoBehaviour where T:SingletonBase<T> 
{
    static public T instance { get; private set; }
    // 不建议在子类中重写Awake 或者 Start
    private void Awake()
    {
        instance = (T)this;
        //Debug.Log(instance.ToString() + "'s singleton instance was initialized!");
        ConstructFunction();
    }

    private void Start()
    {
    }

    private void Update()
    {
        OnUpdate();
    }
    /// <summary>
    /// 在Awake中调用
    /// </summary>
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
