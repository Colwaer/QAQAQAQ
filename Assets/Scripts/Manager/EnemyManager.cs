using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;


public class EnemyManager : Singleton<EnemyManager>
{
    MapEntity mapEntity;

    public GameObject enemy;

    List<GameObject> enemies = new List<GameObject>();

    KeyCode getMap = KeyCode.G;

    private void Start()
    {
        //enermy = Resources.Load("Prefabs/Enermy/Dog") as GameObject;
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(getMap))
        {
            GetMap();
            SpawnEnermy();
        }
    }

    void GetMap()
    {
        mapEntity = GameObject.FindGameObjectWithTag("MapCreator").GetComponent<MapCreate>().mapEntity;
    }
    void SpawnEnermy()
    {
        GameObject t = Instantiate(enemy);
        t.transform.position = mapEntity.StartPos;
        enemies.Add(t);
        //StartCoroutine(spawnEnermy());
    }
    IEnumerator spawnEnermy()
    {
        yield return new WaitForSeconds(0.2f);
        GameObject t = Instantiate(enemy);
        t.transform.position = mapEntity.StartPos;
        enemies.Add(t);
        yield return new WaitForSeconds(2.0f);
        t = Instantiate(enemy);
        t.transform.position = mapEntity.StartPos;
        enemies.Add(t);
        yield return new WaitForSeconds(2.0f);
        t = Instantiate(enemy);
        t.transform.position = mapEntity.StartPos;
        enemies.Add(t);
        yield return new WaitForSeconds(2.0f);
        t = Instantiate(enemy);
        t.transform.position = mapEntity.StartPos;
        enemies.Add(t);

        yield return new WaitForSeconds(2.0f);
    }

}
