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
    // ֪ͨʵ�屻ѡ��
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

    // ������дAwake
    
    private void Awake()
    {
        
    }
    // ����������в�Ӧ����дStart
    private void Start()
    {
        Init();
    }

    // ��Ӧ����дUpdate
    private void Update()
    {
        OnUpdate();
    }

    public virtual void SetType()
    {
        ettType = EEntityType.Entity;
    }

    /// <summary>
    /// ʵ��ĳ�ʼ��������Ӧ����ÿ�������о���ʵ��
    /// �ڸ����е�Start����
    /// ÿһ��������дInitʱ�������ڲ����� base.Init();
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
    /// ����ʵ�屻SetActive(true)�����
    /// ����ʵ�屻ͨ��MemoryPool<T>.Instance().Get()�����
    /// </summary>
    public virtual void Enabled()
    {

    }
}
