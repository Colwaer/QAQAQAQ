using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
using Battle;

public class EnemyManager : Singleton<EnemyManager>
{
    MapEntity mapEntity;

    [System.Serializable]
    public struct EnemyPrefab
    {
        public EnemyType type;
        public GameObject prefab;
    }
    public EnemyPrefab[] enemyPrefabs;
    Dictionary<EnemyType, GameObject> enemyPrefabDic;

    //********************需要改成结构体数组
    List<object> enemySpawnList;
    //****************
    KeyCode getMap = KeyCode.G;

    private void Start()
    {
        //enermy = Resources.Load("Prefabs/Enermy/Dog") as GameObject;
        
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(getMap))
        {
            GetMap();
            SpawnEnemy();
        }
    }
    private void Init()
    {
        enemyPrefabDic = new Dictionary<EnemyType, GameObject>();
        foreach(EnemyPrefab item in enemyPrefabs)
        {
            enemyPrefabDic.Add(item.type, item.prefab);
        }

        enemySpawnList = new List<object>();
        
        enemySpawnList.Add(0.5f);
        enemySpawnList.Add(enemyPrefabDic[EnemyType.Dog]);
        enemySpawnList.Add(5f);
        enemySpawnList.Add(enemyPrefabDic[EnemyType.Dog]);
        enemySpawnList.Add(5f);
        enemySpawnList.Add(enemyPrefabDic[EnemyType.Dog]);
    }
    public void GetMap()
    {
        mapEntity = GameObject.FindGameObjectWithTag("MapCreator").GetComponent<MapCreate>().mapEntity;
    }
    public void SpawnEnemy()
    {
        StartCoroutine(IESpawnEnermy());
    }
    IEnumerator IESpawnEnermy()
    {
        for (int i = 0; i < enemySpawnList.Count; i += 2)
        {
            //float t = (float)enemySpawnList[i];
            yield return new WaitForSeconds((float)enemySpawnList[i]);
            Instantiate(enemySpawnList[i + 1] as GameObject, mapEntity.StartPos, Quaternion.identity);
        }      
    }
    
    

}
