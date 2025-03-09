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
    public FMap gameMap;
    protected override void ConstructFunction()
    {
        base.ConstructFunction();
        DictBuildingsPrefab = new Dictionary<string, GameObject>();
        foreach(var bd in buildingsPrefab)
        {
            var bds = bd.GetComponent<Buildings>();
            if (bds != null)
            {
                DictBuildingsPrefab.Add(bds.buildingName, bd);
            }
        }
        
        
        
    }
    // player stats info

    // player unlocked buildings 
    public List<GameObject> buildingsPrefab;
    public Dictionary<string , GameObject> DictBuildingsPrefab; 

    // some global data in a game save
    private long ett_id = 0;
    public long GetId()
    {
        // interlocked increment return the num after increment
        // interlocked increment(ref num(0)) return 1
        return Interlocked.Increment(ref ett_id);
    }

    //
}
