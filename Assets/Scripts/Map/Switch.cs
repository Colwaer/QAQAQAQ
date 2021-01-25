using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Map
{
    public class Switch : MonoBehaviour
    {
        MapCreate mapCreator;

        private void Start()
        {
            mapCreator = gameObject.GetComponent<MapCreate>();
        }

        public void DeafaultType()
        {
            mapCreator.mapUnitType = UnitType.defaultType;

        }
        public void Blank()
        {
            mapCreator.mapUnitType = UnitType.blank;
        }
        public void Type1()
        {
            mapCreator.mapUnitType = UnitType.type1;
        }
        public void Type2()
        {
            mapCreator.mapUnitType = UnitType.type2;
        }
    }
}