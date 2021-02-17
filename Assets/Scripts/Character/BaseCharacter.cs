using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Battle
{
    public enum AttackKind { Physics = 0, Magic, Real }

    public class BaseCharacter : MonoBehaviour
    {
        protected float m_maxHelath;
        protected float m_attack;
        protected float m_defend;
        protected float m_magicDamage;
        protected float m_magicDefend;


        public Slider healthSlider;

        public Action DieEvent;
        


        protected Action OnHealthChanged;
        protected float health;

        public float Health
        {
            get { return health; }
            private set
            {
                if (OnHealthChanged != null)
                    OnHealthChanged();
                if (value <= 0) 
                    if (DieEvent != null)
                        DieEvent();
                health = value;
            }
        }
        
        public BaseCharacter()
        {
            DieEvent += Die;
            OnHealthChanged += RefreshHealthSlider;
        }
        private void OnDestroy()
        {
            DieEvent -= Die;
            OnHealthChanged -= RefreshHealthSlider;
        }
        void RefreshHealthSlider()
        {
            if (healthSlider != null)
                healthSlider.value = Health / m_maxHelath;
        }
        virtual protected void Recover(float delta)
        {
            Health = Mathf.Min(m_maxHelath, delta + Health);
        }
        virtual public void Hurt(float delta, AttackKind kind)
        {
            switch (kind)
            {
                case AttackKind.Physics:
                    delta = Mathf.Min(0, -delta + m_defend);
                    break;
                case AttackKind.Magic:
                    delta = Mathf.Min(0, -delta + m_magicDefend);
                    break;
                case AttackKind.Real:
                    delta = Mathf.Min(0, -delta);
                    break;
            }
            Health = Mathf.Max(0,delta + Health);
        }
    
        void Die()
        {
            Destroy(gameObject);
        }
        
    }
}