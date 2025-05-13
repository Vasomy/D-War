using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 科技树节点，挂载在ui的科技节点上
/// </summary>
public class FTechTreeNode : MonoBehaviour
{
    public string description;
    public string techName;

    public int level = 0;
    public int maxLevel = 1;

    public TechTreeContent content;
    public List<TechTreeContent> parent;

    public Sprite techSprite;
    public Image techImage;

    public TextMeshProUGUI levelText;

    private void Start()
    {
        techImage = GetComponent<Image>();  
        techImage.sprite = techSprite;
        levelText.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        TechTree.Instance().previewImage.sprite = techSprite;
        TechTree.Instance().activeNode = this;
        
    }

}

