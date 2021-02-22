﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace Battle
{

    public class MapUnitPre : MonoBehaviour
    {
        public UnitType type;

        //public Action onEnemyEnter;

        public bool canPlaceOperator
        {
            get
            {
                if (type != UnitType.type1 && type != UnitType.type2)
                {
                    return false;
                }
                    
                //Debug.Log(currentOperator == null);
                return currentOperator == null;
            }
        }

        List<BaseEnemy> enemies;

        public BaseOperator currentOperator;

        float broadCastTimer = 0;
        float broadCastTime = 0.7f;

        public Action<List<BaseEnemy>> enemyEnter;
        public Action<BaseEnemy> enemyExit;

        private void Awake()
        {
            enemies = new List<BaseEnemy>();
            broadCastTime = 0.7f;

        }
        private void Update()
        {
            broadCastTime += Time.deltaTime;
            if (broadCastTime >= broadCastTimer)
            {
                if (enemyEnter != null)
                    enemyEnter(enemies);
                broadCastTime = 0;
                
            }
        }
        public void Init()
        {
            MapInSceneManager.Instance.MapUnitPreLoadToList((int)transform.position.x, (int)transform.position.y, this);
        }

        void RemoveNull()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                    enemies.RemoveAt(i);
            }
        }

        //尚未测试，不知道每次添加前这样查重有效没有
        //***********************************************************************************************************
        virtual protected void OnTriggerEnter2D(Collider2D collision)
        {
            broadCastTime = 0;
            if (collision.tag == "Enermy" && !enemies.Contains(collision.gameObject.GetComponent<BaseEnemy>()))
            {

                RemoveNull();
                enemies.Add(collision.gameObject.GetComponent<BaseEnemy>());
                if (enemyEnter != null)
                    enemyEnter(enemies);
            }
        }
        /*
        virtual protected void OnTriggerStay2D(Collider2D collision)
        {
            Debug.Log(collision.name);
            if (collision.tag == "Enermy" && !enemies.Contains(collision.gameObject.GetComponent<BaseEnemy>()))
            {
                RemoveNull();
                Debug.Log("Staying Add");
                enemies.Add(collision.gameObject.GetComponent<BaseEnemy>());
            }
        }
        */
        virtual protected void OnTriggerExit2D(Collider2D collision)
        {
            
            if (collision.tag == "Enermy" && enemies.Contains(collision.gameObject.GetComponent<BaseEnemy>()))
            {
                RemoveNull();
                enemies.Remove(collision.gameObject.GetComponent<BaseEnemy>());
                if (enemyExit != null)
                    enemyExit(collision.gameObject.GetComponent<BaseEnemy>());
            }
        }


    }
}
