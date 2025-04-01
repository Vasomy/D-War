using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EMouseEnum : int
{
    Left = 0,
    Right = 1,
    Middle
}
public class CameraController : SingletonBase<CameraController>
{
    public Camera mainCamera => Camera.main;
    public TextMeshProUGUI text;
    public Vector2 mousePositionOnScreen;
    public Vector2 GetMousePos() 
    {
        Vector3 MousePos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(MousePos);
    }
    public Vector2 GetMouseScreenPos()
    {
        return Input.mousePosition;
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        mousePositionOnScreen = Input.mousePosition;
        text.text = mousePositionOnScreen.ToString();
    }

}
