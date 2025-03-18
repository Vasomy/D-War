using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsUISlot : MonoBehaviour
{
    public string buildingsName = "NULL";
    public Sprite buildingsSprite = null;
    private void Start()
    {
        var image = GetComponent<Image>();
        if (image != null)
            image.sprite = buildingsSprite;
    }
    public void OnClick()
    {
        Builder.instance.previewObj.SetActive(true);
        Builder.instance.selectedBuildingsName = buildingsName;
        Builder.instance.previewSr.sprite = buildingsSprite;
    }
}
