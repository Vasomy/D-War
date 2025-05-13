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
        uiTechTree.SetActive(true);
    }

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        InitAllTech();
    }

    TechTreeAttackUp attackUp;
    private Dictionary<TechTreeContent, TechTreeContentTrigger> dictContents;
    void InitAllTech()
    {
        dictContents = new Dictionary<TechTreeContent, TechTreeContentTrigger> ();
        attackUp = new TechTreeAttackUp(TechTreeContent.eAttackUp_3, 0, 3);
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
            if(dictContents.TryGetValue(activeNode.content,out var val))
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
