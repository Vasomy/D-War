using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour
{
    public static Layer currentLayer = null;

    public List<TSparseSet<Entity>> friendlyEntityInLayer;
    public List<TSparseSet<Entity>> enemyEntityInLayer;
    public List<TSparseSet<Entity>> buildingEntityInLayer;
    
    private void Awake()
    {
        currentLayer = this;
    }

    public List<EnemySpawnPoint> spawnPoints;
    
}
