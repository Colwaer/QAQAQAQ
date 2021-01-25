using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Battle
{
    public enum AttackKind { Physics = 0, Magic, Real }

    public class BaseCharacter : MonoBehaviour
    {
        private float m_maxHelath;
        private float m_attack;
        private float m_defend;
        private float m_magicDamage;
        private float m_magicDefend;
        public Action DieEvent;
        public float Health
        {
            get { return Health; }
            private set
            {
                if(value <=0) DieEvent();
                Health = value;
            }
        }
        public BaseCharacter(float attack, float defend, float magicDamage, float magicDefend, float maxHelath)
        {
            m_attack = attack;
            m_defend = defend;
            m_magicDamage = magicDamage;
            m_magicDefend = magicDefend;
            m_maxHelath = maxHelath;
        }
        virtual protected void Recover(float delta)
        {
            Health = Mathf.Min(m_maxHelath, delta + Health);
        }
        virtual protected void Hurt(float delta,AttackKind kind)
        {
            switch (kind)
            {
                case AttackKind.Physics:
                    delta = Mathf.Min(0,delta + m_defend);
                    break;
                case AttackKind.Magic:
                    delta = Mathf.Min(0,delta + m_magicDefend);
                    break;
                case AttackKind.Real:
                    delta = Mathf.Min(0,delta);
                    break;
            }
            Health = Mathf.Max(0,delta + Health);
        }
    
        virtual protected void Attack()
        {

        }
        
    }
}