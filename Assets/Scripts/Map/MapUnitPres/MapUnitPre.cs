using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace Battle
{

    public class MapUnitPre : MonoBehaviour, IEnumerable
    {
        public UnitType type;

        public bool canPlaceOperator { get; protected set; }

        List<BaseEnermy> enermies;

        float broadCastTimer = 0;
        float broadCastTime = 0.7f;

        public Action<List<BaseEnermy>> enermyEnter;
        public Action<BaseEnermy> enermyExit;

        private void Awake()
        {
            enermies = new List<BaseEnermy>();
            broadCastTime = 0.7f;

        }
        private void Update()
        {
            broadCastTime += Time.deltaTime;
            if (broadCastTime >= broadCastTimer)
            {
                if (enermyEnter != null)
                    enermyEnter(enermies);
                broadCastTime = 0;
            }
        }
        public void Init()
        {
            MapInSceneManager.Instance.MapUnitPreLoadToList((int)transform.position.x, (int)transform.position.y, this);
        }
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < enermies.Count; i++)
            {
                yield return enermies[i];
            }
        }
        void RemoveNull()
        {
            for (int i = 0; i < enermies.Count; i++)
            {
                if (enermies[i] == null)
                    enermies.RemoveAt(i);
            }
        }

        //尚未测试，不知道每次添加前这样查重有效没有
        //***********************************************************************************************************
        virtual protected void OnTriggerEnter2D(Collider2D collision)
        {
            broadCastTime = 0;
            if (collision.tag == "Enermy" && !enermies.Contains(collision.gameObject.GetComponent<BaseEnermy>()))
            {
                RemoveNull();
                enermies.Add(collision.gameObject.GetComponent<BaseEnermy>());
                if (enermyEnter != null)
                    enermyEnter(enermies);
            }
        }
        
        virtual protected void OnTriggerExit2D(Collider2D collision)
        {
            
            if (collision.tag == "Enermy" && enermies.Contains(collision.gameObject.GetComponent<BaseEnermy>()))
            {
                RemoveNull();
                enermies.Remove(collision.gameObject.GetComponent<BaseEnermy>());
                if (enermyExit != null)
                    enermyExit(collision.gameObject.GetComponent<BaseEnermy>());
            }
        }


    }
}
