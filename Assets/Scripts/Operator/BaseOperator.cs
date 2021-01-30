using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

namespace Battle
{
    public enum PlaceDirection
    {
        up,
        down,
        left,
        right
    }

    public class BaseOperator : BaseCharacter
    {
        List<MapUnitPre> attackAreas;

        List<BaseEnermy> enermiesInAttackAreas;
        BaseEnermy currentAttackTarget;

        Vector2[] attackAreasPos;

        private float m_attackInterval;
        private float attackTimer = 0;

        public bool isIdling;
        public bool isAttacking;

        bool isInited;

        public PlaceDirection placeDirection;

        Animator animator;

        public BaseOperator(float attack, float defend, float magicDamage, float magicDefend, float maxHelath, float attackDistance)
            : base(attack, defend, magicDamage, magicDefend, maxHelath)
        {

        }
        
        private void Awake()
        {
            isIdling = true;
            isAttacking = false;
            m_attackInterval = 1.0f;

            animator = GetComponent<Animator>();

            attackAreas = new List<MapUnitPre>();
            enermiesInAttackAreas = new List<BaseEnermy>();
            attackAreasPos = new Vector2[] { new Vector2(1, 0), new Vector2(-1, 0),
            new Vector2(1, 1), new Vector2(-1, 1), new Vector2(0, 1), new Vector2(0, 2) };
        }


        private void Update()
        { 
            if (isInited)
                CalCurrentAttackTarget();
        }
        private void FixedUpdate()
        {
            if (isInited)
            {
                Attack();
            }
        }
        void RemoveNull()
        {
            for (int i = 0; i < enermiesInAttackAreas.Count; i++)
            {
                if (enermiesInAttackAreas[i] == null)
                    enermiesInAttackAreas.RemoveAt(i);
            }
        }

        void CalCurrentAttackTarget()
        {
            RemoveNull();
            if (enermiesInAttackAreas.Count == 0)
                currentAttackTarget = null;
            if (enermiesInAttackAreas.Count == 1)
                currentAttackTarget = enermiesInAttackAreas[0];
            float distance = float.MaxValue;
            BaseEnermy target;
            foreach(BaseEnermy item in enermiesInAttackAreas)
            {
                float tmpDistance = (item.transform.position - transform.position).magnitude;
                if (tmpDistance < distance)
                {
                    distance = tmpDistance;
                    target = item;
                }
            }
        }
        void Attack()
        {
            if (currentAttackTarget == null)
            {
                isIdling = true;
                isAttacking = false;
                animator.SetBool("isIdling", true);
                animator.SetBool("isAttacking", false);
                attackTimer = m_attackInterval;
                return;
            }
            if (attackTimer >= m_attackInterval)
            {
                isAttacking = true;
                isIdling = false;
                animator.SetBool("isIdling", false);
                animator.SetBool("isAttacking", true);
                attackTimer = 0;

                Debug.Log("Operator attack");
            }
            else
            {
                isAttacking = false;
                isIdling = true;
                animator.SetBool("isIdling", true);
                animator.SetBool("isAttacking", false);
                attackTimer += Time.deltaTime;
            }

        }

        public void Init(PlaceDirection dir)
        {
            placeDirection = dir;
            SetAttackAreas(placeDirection);
            isInited = true;

            if (placeDirection == PlaceDirection.left)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }


            SetListener();
        }
        void SetListener()
        {
            foreach(MapUnitPre item in attackAreas)
            {
                Debug.Log(item.transform.position);
                item.enermyEnter += AddEnermy;
                item.enermyExit += RemoveEnermy;
            }
        }
        private void OnDestroy()
        {
            foreach (MapUnitPre item in attackAreas)
            {
                item.enermyEnter -= AddEnermy;
                item.enermyExit -= RemoveEnermy;
            }
        }
        void AddEnermy(List<BaseEnermy> enermyList)
        {
            foreach(BaseEnermy item in enermyList)
            {
                if (!enermiesInAttackAreas.Contains(item))
                {
                    enermiesInAttackAreas.Add(item);
                    Debug.LogFormat("add{0}",item);
                }
                    
            }
        }
        void RemoveEnermy(BaseEnermy enermyToBeRemoved)
        {
            if (enermiesInAttackAreas.Contains(enermyToBeRemoved))
            {     
                enermiesInAttackAreas.Remove(enermyToBeRemoved);
                Debug.LogFormat("remove{0}", enermyToBeRemoved);
            }
        }
        virtual public void SetAttackAreas(PlaceDirection dir)
        {

            switch(dir)
            {
                case PlaceDirection.up:
                    break;
                case PlaceDirection.down:
                    for (int i = 0; i < attackAreasPos.Length; i++)
                    {
                        attackAreasPos[i] = -attackAreasPos[i];
                    }
                    break;
                case PlaceDirection.left:

                    for (int i = 0; i < attackAreasPos.Length; i++)
                    {
                        float xTmp = attackAreasPos[i].x;
                        float yTmp = -attackAreasPos[i].y;
                        attackAreasPos[i].x = yTmp;
                        attackAreasPos[i].y = xTmp;
                    }
                    break;
                case PlaceDirection.right:
                    for (int i = 0; i < attackAreasPos.Length; i++)
                    {
                        float xTmp = attackAreasPos[i].x;
                        float yTmp = attackAreasPos[i].y;
                        attackAreasPos[i].x = yTmp;
                        attackAreasPos[i].y = xTmp;
                    }
                    break;
                default:
                    break;
            }

            foreach(Vector2 item in attackAreasPos)
            {
                MapUnitPre t = MapInSceneManager.Instance.GetMapUnitPre((int)item.x + (int)transform.position.x, (int)item.y + (int)transform.position.y);
                if (t != null)
                {
                    attackAreas.Add(t);
                }
            }
        }
    }
}