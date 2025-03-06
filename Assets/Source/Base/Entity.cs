using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum EEntityType
{
    Entity,
    Actor,
    Controlable,
    Building
}

public class Entity : MonoBehaviour
{
    public long uid;
    private bool isSelected = false;
    public EEntityType ettType = EEntityType.Entity;
    // 通知实体被选中
    public void InfoSelect()
    {
        isSelected = true;
    }
    public void ReleaseSelected()
    {
        isSelected = false;
    }
    public bool IsSelected()
    {
        return isSelected;
    }

    // 不能重写Awake
    
    private void Awake()
    {
        uid = GameContext.instance.GetId();
    }
    // 在最低子类中不应该重写Start
    private void Start()
    {
        gameObject.tag = "Entity";
        Init();
    }

    public virtual void SetType()
    {
        ettType = EEntityType.Entity;
    }

    /// <summary>
    /// 实体的初始化函数，应该在每个子类中具体实现
    /// 在父类中的Start调用
    /// </summary>
    protected virtual void Init()
    {
        SetType();
    }
  
}
