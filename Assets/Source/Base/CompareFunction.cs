using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CompareFunction
{
    const float m_eps = (float)1e-2;
    static public bool is_same_vec2f(Vector2 v1, Vector2 v2, float eps = m_eps)
    {
        return (Mathf.Abs(v1.x - v2.x) <= eps) && (Mathf.Abs(v1.y - v2.y) <= eps);
    }
    static public float ManhattanDistance(Vector2 v1, Vector2 v2)
    {
        return Mathf.Abs(v1.x - v2.x) + Mathf.Abs(v1.y - v2.y);
    }
    static public float ManhattanDistance(Vector3 v1, Vector3 v2)
    {
        return Mathf.Abs(v1.x - v2.x) + Mathf.Abs(v1.y - v2.y) + Mathf.Abs(v1.z - v2.z);
    }

    static public float EulerDistance(Vector2 p1, Vector2 p2)
    {
        return Mathf.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
    }

    static public float EulerDistance(Vector3 p1, Vector3 p2)
    {
        return Mathf.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y) + (p1.z - p2.z) * (p1.z - p2.z));
    }
}



