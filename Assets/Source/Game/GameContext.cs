using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class at
{
    public int a;
    public float b;
    public string c;
}


public class GameContext : SingletonBase<GameContext>
{
    public Hashtable allBuildings = new Hashtable();
    public Hashtable allFriendlyUnits = new Hashtable();

    public FMap gameMap;
    protected override void ConstructFunction()
    {
        base.ConstructFunction();
        DictBuildingsPrefab = new Dictionary<string, GameObject>();
        foreach(var bd in buildingsPrefab)
        {
            var bds = bd.GetComponent<EBuildings>();
            if (bds != null)
            {
                DictBuildingsPrefab.Add(bds.buildingName, bd);
            }
        }
        
        allEntityPrefabsDict = new Dictionary<string, GameObject>();
        foreach(var prefab in  allEntityPrefabs)
        {
            Debug.Log(prefab.name + " get in dict for find!");
            allEntityPrefabsDict.Add(prefab.name, prefab);
        }
    }
    // player stats info

    // all buildings 
    public List<GameObject> buildingsPrefab;
    public Dictionary<string , GameObject> DictBuildingsPrefab;

    // 非静态实体
    // 所有的prefab的名字应该为其所挂载的主体类，如农民的prefab名字应该为AFarmer 等同于类名 AFarmer
    public List<GameObject> allEntityPrefabs;
    public Dictionary<string, GameObject> allEntityPrefabsDict;

    // static entity
    // 场景物体 可以采集的资源，实体墙壁，地块
    // 可以在地图场景编辑器里面使用
    public List<GameObject> staticEntityPrefab;
    public Dictionary<string,GameObject> staticEntityDictPrefab;
    

    // some global data in a game save
    private long ett_id = 0;
    public long GetId()
    {
        // interlocked increment return the num after increment
        // interlocked increment(ref num(0)) return 1
        return Interlocked.Increment(ref ett_id);
    }

    // smt save data stats ...
    public PResourceStats resourceStats = new PResourceStats();

}
