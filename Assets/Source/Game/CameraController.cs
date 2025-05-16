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
    public float moveSpeed;
    [SerializeField]
    private float edgeLengthScale = 0.05f;

    /// <summary>
    /// 获取鼠标位置在游戏内的坐标
    /// </summary>
    /// <returns></returns>
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

        if(!FCommandManager.Instance().isOnUI)
        {
            var mousePos = Input.mousePosition;
            var screenW = mousePos.x/Camera.main.pixelWidth;
            var screenH = mousePos.y/Camera.main.pixelHeight;
            if (screenW < 0 || screenW > 1.0f || screenH < 0 || screenH > 1.0f) return;

            if(screenW<=edgeLengthScale || screenW>=1.0f-edgeLengthScale) 
            {
                // move horizontally
                float dir = screenW<=edgeLengthScale ? -1.0f : 1.0f;
                Vector3 position = Camera.main.gameObject.transform.position;
                position.x += dir * moveSpeed * Time.deltaTime;
                Camera.main.gameObject.transform.position = position;
            }
            if(screenH<=edgeLengthScale || screenH>=1.0f-edgeLengthScale)
            {
                float dir = screenH <= edgeLengthScale ? -1.0f : 1.0f;
                Vector3 position = Camera.main.gameObject.transform.position;
                position.y += dir * moveSpeed * Time.deltaTime;
                Camera.main.gameObject.transform.position = position;
            }

        }

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
