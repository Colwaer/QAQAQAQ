using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
using Battle;
using System;

public class GameManager : MonoBehaviour
{

    bool isGameStarted = false;

    private int enemyEscapeNum = 0;
    private int maxEnemyEscapeNum = 1;

    Vector2 endPos;

    public Action gameOver;
    public int MaxEnemyEscapeNum
    {
        get
        {
            return maxEnemyEscapeNum;
        }
        set
        {
            maxEnemyEscapeNum = value;
            if (maxEnemyEscapeNum <= 0)
            {
                if (gameOver != null)
                {
                    gameOver();
                    //Debug.LogWarning("game over");
                }
                    
            }
                
        }
    }
    public void GameStart()
    {

        PathSearch.Instance.Search();
        EnemyManager.Instance.GetMap();
        EnemyManager.Instance.SpawnEnemy();


        endPos = MapCreate.Instance.mapEntity.EndPos;


        
        MapInSceneManager.Instance.GetMapUnitPre((int)endPos.x, (int)endPos.y).enemyEnter += EnemyEscape;

        isGameStarted = true;
    }
    private void Start()
    {
        gameOver += RemoveEnemyEscapeListener;
    }
    void EnemyEscape(List<BaseEnemy> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                MaxEnemyEscapeNum--;
                //Debug.Log(MaxEnemyEscapeNum);
                Destroy(enemies[i].gameObject);
            }
        }
    }
    void RemoveEnemyEscapeListener()
    {
        //Debug.LogWarning("remove listener");
        MapInSceneManager.Instance.GetMapUnitPre((int)endPos.x, (int)endPos.y).enemyEnter -= EnemyEscape;
    }

}
