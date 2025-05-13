using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
[Serializable]
public class SpawnBatch
{
    public int count;
    public EnemyName enemyName;
    public float spawnNumsPerSecond = 2.0f;
}
[Serializable]
public class SpawnBound
{
    public List<SpawnBatch> spawnBatches = new List<SpawnBatch>();
}

public class SpawnInfo : SingletonBase<SpawnInfo>
{
    public List<SpawnBound> spawnBounds;
}
