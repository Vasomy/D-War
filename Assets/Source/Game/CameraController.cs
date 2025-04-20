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
    public TextMeshProUGUI text4select;
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
        text4select.text = MSelectSystem.instance.selectedEntity.Count.ToString();
    }
    public Vector2 GetMousePosByRay(float target_plane_z)
    {
        Vector2 pos = new Vector2();

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // {x,y,2} = origin + direction * t;
        // oZ + dZ*t = 2;
        // t = (2-oZ)/dZ;
        float plane_pos_z = target_plane_z;
        var t = (plane_pos_z - ray.origin.z) / ray.direction.z;
        pos.x = ray.origin.x + ray.direction.x * t;
        pos.y = ray.origin.y + ray.direction.y * t;

        return pos;
    }
}
