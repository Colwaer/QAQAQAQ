using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
using Battle;

public class GameManager : MonoBehaviour
{
    public void GameStart()
    {
        PathSearch.Instance.Search();
        EnemyManager.Instance.GetMap();
        EnemyManager.Instance.SpawnEnemy();
    }
}
