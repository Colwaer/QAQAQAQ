using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class Type1 : MapUnitPre
    {
        private void Start()
        {
            type = UnitType.type1;
            canPlaceOperator = true;
        }
    }
}
