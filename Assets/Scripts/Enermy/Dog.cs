using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class Dog : BaseEnemy
{
    protected override void Start()
    {
        base.Start();
        Init();
    }
    
    void Init()
    {
        health = 100f;
        m_maxHelath = 100f;
        m_attack = 20f;
        m_defend = 10f;
        m_magicDamage = 15f;
        m_magicDefend = 10f;
        m_attackDistance = 1.0f;
        m_attackInterval = 2.0f;

        attackDelayTime = 1.2f;
    }
    
}
