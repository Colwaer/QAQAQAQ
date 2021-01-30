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

        public Action<List<BaseEnermy>> enermyEnter;
        public Action<BaseEnermy> enermyExit;

        private void Awake()
        {
            enermies = new List<BaseEnermy>();
            
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
