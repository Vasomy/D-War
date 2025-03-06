using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : SingletonBase<CameraController>
{
    public Vector2 GetMousePos() 
    {
        Vector3 MousePos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(MousePos);
    }
    public Vector2 GetMouseScreenPos()
    {
        return Input.mousePosition;
    }
}
