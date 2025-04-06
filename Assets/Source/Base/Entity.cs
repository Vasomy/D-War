using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum EEntityType
{
    Entity,
    Actor,
    Controlable,
    Building,
    Collectable,
    Enemy,
}

public class Entity : MonoBehaviour , IColliderable
{
    public long uid;
    private bool isSelected = false;
    public EEntityType ettType = EEntityType.Entity;
    public Rigidbody2D rb2d;
    public Vector2Int indexedPos => GridManager.GetIndexedPos(transform.position);
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
        
    }
    // 在最低子类中不应该重写Start
    private void Start()
    {
        Init();
    }

    // 不应该重写Update
    private void Update()
    {
        OnUpdate();
    }

    public virtual void SetType()
    {
        ettType = EEntityType.Entity;
    }

    /// <summary>
    /// 实体的初始化函数，应该在每个子类中具体实现
    /// 在父类中的Start调用
    /// 每一个子类重写Init时必须在内部调用 base.Init();
    /// </summary>
    protected virtual void Init()
    {
        gameObject.tag = "Entity";
        uid = GameContext.instance.GetId();
        rb2d = GetComponent<Rigidbody2D>();
        SetType();
    }

   
    private void OnMouseOver()
    {
        int key_code = Input.GetMouseButtonDown(0) == true?0:-1;
        key_code = Input.GetMouseButtonDown(1) == true?1:key_code;
        Debug.Log(key_code);
        switch (key_code)
        {
        // left mouse button
        case 0:
            {
                OnMouseLeftButtonDown();
                    break;
            }
        case 1:
                OnMouseRightButtonDown();
            break;
        }
    }

    public virtual void OnMouseLeftButtonDown()
    {
    }    
    public virtual void OnMouseRightButtonDown()
    {
    }

    protected virtual void OnUpdate()
    {

    }

    /// <summary>
    /// 当该实体被SetActive(true)后调用
    /// 当该实体被通过MemoryPool<T>.Instance().Get()后调用
    /// </summary>
    public virtual void Enabled()
    {

    }
}
