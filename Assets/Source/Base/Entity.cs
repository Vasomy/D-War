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

    /// <summary>
    /// ����EEntityType���Ժ�object�ı�ǩ
    /// </summary>
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

        ///
        ///
        switch (gameObject.tag)
        
        {
            case "friendly":
                FWorld.currentWorld.RegisterFriendlyEntity(this);
                break;
            case "enemy":
                FWorld.currentWorld.RegisterEnemyEntity(this);
                break;
            case "static":
                FWorld.currentWorld.RegisterStaticEntity(this);
                break;
            default:
                break;
        }

    }


    private void OnMouseOver()
    {
        int key_code = Input.GetMouseButtonDown(0) == true?0:-1;
        key_code = Input.GetMouseButtonDown(1) == true?1:key_code;
        switch (key_code)
        {
        // left mouse button
        case 0:
            {
                OnMouseLeftButtonDown();
                    break;
            }
        case 1:
                //Debug.Log("SSSS");
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
    ///
    /// ����ʵ�屻���٣��Ƴ����������߽�Ҫ�ص��ڴ����ʱ����
    ///
    public virtual void Disabled()
    {

    }
}
