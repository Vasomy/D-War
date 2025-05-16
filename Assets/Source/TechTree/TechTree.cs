using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ���еĿƼ����Ľڵ������ʹ��ö�ٴ���
/// �ڿƼ����У��Ƽ����ڵ�����ֱ���Ϊcontent����TechTreeNode�д����־����Ϊȫ�ֲ��ҵ������Ͷ�һid
/// ��node��contentö����ֻ�ǣ������˿Ƽ�����Ҫ����Ķ�������֣�id�������弤�������ú�������TechTreeNodeContentTrigger���������ʵ��
/// </summary>
public enum TechTreeContent
{
    eNone,// Ĭ�ϵ�һ���տƼ����ն������
    eAttackUp_3,// _num1 ��ʾ�ÿƼ����ȼ���0Ĭ��Ϊû�м���ÿƼ�

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
    /// ���ڴ浵��ȡʱ���ؿƼ���
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
