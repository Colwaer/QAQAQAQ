using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class DefaultType : MapUnitPre
    {
        private void Start()
        {
            type = UnitType.defaultType;
            canPlaceOperator = false;
        }
    }
}
