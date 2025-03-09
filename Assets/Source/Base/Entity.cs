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
    public Rigidbody2D rb2d;
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

    public virtual void SetType()
    {
        ettType = EEntityType.Entity;
    }

    /// <summary>
    /// ʵ��ĳ�ʼ��������Ӧ����ÿ�������о���ʵ��
    /// �ڸ����е�Start����
    /// </summary>
    protected virtual void Init()
    {
        gameObject.tag = "Entity";
        uid = GameContext.instance.GetId();
        rb2d = GetComponent<Rigidbody2D>();
        SetType();
    }
  
}
