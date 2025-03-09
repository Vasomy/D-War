using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serializer<T>
{
    private string toPath;
    private T target;
    public string CastToJson()
    {
        string json = JsonUtility.ToJson(target);
        return json;
    }
    public void SetTarget(T inTarget)
    {
        target = inTarget;
    }
    public void SetToPath(string inPath)
    {
        toPath = inPath;
    }
    // 传入想要保存内容的文件名字
    public void Write(string filename)
    {
        string path = toPath + filename + ".json";
        if(!File.Exists(path))
        {
            File.Create(path).Dispose();
        }
        string json = JsonUtility.ToJson(target);
        File.WriteAllText(path, json);
        Debug.Log(json);
    }
    public T MakeByJson(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }
}
