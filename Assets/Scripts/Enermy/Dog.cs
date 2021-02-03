using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class Dog : BaseEnemy
{
    

    public Dog(float attack, float defend, float magicDamage, float magicDefend, float maxHelath, float attackDistance)
            : base(attack, defend, magicDamage, magicDefend, maxHelath, attackDistance)
    {
        
    }
}
