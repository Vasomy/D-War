using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 所有的科技树的节点的名称使用枚举储存
/// 在科技树中，科技树节点的名字被称为content，在TechTreeNode中储存标志，作为全局查找的索引和独一id
/// 在node和content枚举中只是，定义了科技树想要激活的对象的名字（id），具体激活后的作用和属性在TechTreeNodeContentTrigger类的子类中实现
/// </summary>
public enum TechTreeContent
{
    eNone,// 默认的一个空科技，空对象设计
    eAttackUp_3,// _num1 表示该科技最大等级，0默认为没有激活该科技

}

public class TechTree : MonoBehaviour
{
    private static TechTree instance;
    public static TechTree Instance()
    {
        if(instance == null)
        {
            return null;
        }
        return instance;
    }
    public FTechTreeNode activeNode = null;
    public Image previewImage;
    public Button upgradeButton;

    public GameObject uiTechTree;
    /// <summary>
    /// call in ui button
    /// </summary>
    public void OpenTechTree()
    {
        if (!FCommandManager.Instance().isOnUI)
        {
            FCommandManager.Instance().isOnUI = true;
            uiTechTree.SetActive(true);
        }
    }

    public void CloseTechTree()
    {
        FCommandManager.Instance().isOnUI = false;
        uiTechTree.SetActive(false);
    }

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        InitAllTech();
    }

    private Dictionary<TechTreeContent, TechTreeContentTrigger> dictContents;
    public TechTreeContentTrigger GetContentTrigger(TechTreeContent content)
    {
        return dictContents[content];
    }
    /// <summary>
    /// all  tech contents
    /// </summary>
    [Header("All tech contents")]
    public TechTreeAttackUp attackUp;


    void InitAllTech()
    {
        dictContents = new Dictionary<TechTreeContent, TechTreeContentTrigger> ();
        RegisterContent(attackUp);
    }

    private void RegisterContent(TechTreeContentTrigger content)
    {
        // content.enumContent as key
        dictContents.Add(content.enumContent, content); 
    }
    /// <summary>
    /// 
    /// 用于存档读取时加载科技树
    /// </summary>
    public void LoadFromSave()
    {
        ///
        attackUp = JsonUtility.FromJson<TechTreeAttackUp>("");
    }
    
    public void UpgradeTechTree()
    {
        if(activeNode != null) 
        {
            foreach (TechTreeContent content in activeNode.parent)
            {
                var ct = TechTree.Instance().GetContentTrigger(content);
                if(ct.level == 0)
                {
                    Debug.Log("The parent tech node hasnt unlocked!");
                    return;
                }
            }
            if (dictContents.TryGetValue(activeNode.content,out var val))
            {
                val.LevelUp();
            }
            else
            {
                Debug.LogError("Cant find tech tree content named : "+activeNode.content);
            }
        }
    }
}
