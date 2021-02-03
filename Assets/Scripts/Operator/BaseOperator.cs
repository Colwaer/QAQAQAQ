using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

namespace Battle
{
    public class BaseOperator : BaseCharacter
    {
        List<MapUnitPre> attackAreas;

        List<BaseEnemy> enemiesInAttackAreas;
        BaseEnemy currentAttackTarget;

        public GameObject attackAreaIndicator;
        List<GameObject> attackAreaIndicatorList;

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
            enemiesInAttackAreas = new List<BaseEnemy>();
            attackAreaIndicatorList = new List<GameObject>();

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
            for (int i = 0; i < enemiesInAttackAreas.Count; i++)
            {
                if (enemiesInAttackAreas[i] == null)
                    enemiesInAttackAreas.RemoveAt(i);
            }
        }
        
        virtual public void ShowDirChoosePanel() { Debug.Log("enter father"); }
        virtual public void OffShowDirChoosePanel() { }

        void CalCurrentAttackTarget()
        {
            RemoveNull();
            if (enemiesInAttackAreas.Count == 0)
                currentAttackTarget = null;
            if (enemiesInAttackAreas.Count == 1)
                currentAttackTarget = enemiesInAttackAreas[0];
            float distance = float.MaxValue;
            BaseEnemy target;
            foreach(BaseEnemy item in enemiesInAttackAreas)
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

        public void ShowAttackArea()
        {
            foreach (Vector2 item in attackAreasPos)
            {
                GameObject t = Instantiate(attackAreaIndicator);
                t.transform.position = item + (Vector2)transform.position;
                attackAreaIndicatorList.Add(t);
            }
        }
        public void OffShowAttackArea()
        {
            if (attackAreaIndicatorList.Count == 0)
                return;
            foreach (GameObject t in attackAreaIndicatorList)
            {
                Destroy(t);
            }
            attackAreaIndicatorList.Clear();
        }
        //init此处只有设置监听的作用
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
            //attackAreas.Clear();
            foreach(MapUnitPre item in attackAreas)
            {
                //Debug.Log(item.transform.position);
                item.enemyEnter += AddEnermy;
                item.enemyExit += RemoveEnermy;
            }
        }
        void RemoveListener()
        {
            
            foreach (MapUnitPre item in attackAreas)
            {
                //Debug.Log(item.transform.position);
                item.enemyEnter -= AddEnermy;
                item.enemyExit -= RemoveEnermy;
            }
        }
        private void OnDestroy()
        {
            foreach (MapUnitPre item in attackAreas)
            {
                item.enemyEnter -= AddEnermy;
                item.enemyExit -= RemoveEnermy;
            }
        }
        void AddEnermy(List<BaseEnemy> enermyList)
        {
            foreach(BaseEnemy item in enermyList)
            {
                if (!enemiesInAttackAreas.Contains(item))
                {
                    enemiesInAttackAreas.Add(item);
                    //Debug.LogFormat("add{0}",item);
                }
                    
            }
        }
        void RemoveEnermy(BaseEnemy enermyToBeRemoved)
        {
            if (enemiesInAttackAreas.Contains(enermyToBeRemoved))
            {     
                enemiesInAttackAreas.Remove(enermyToBeRemoved);
                Debug.LogFormat("remove{0}", enermyToBeRemoved);
            }
        }
        virtual public void SetAttackAreas(PlaceDirection dir)
        {
            enemiesInAttackAreas.Clear();
            currentAttackTarget = null;
            RemoveListener();
            attackAreas.Clear();
            
            attackAreasPos = new Vector2[] { new Vector2(1, 0), new Vector2(-1, 0),
            new Vector2(1, 1), new Vector2(-1, 1), new Vector2(0, 1), new Vector2(0, 2) };

            switch (dir)
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
                        //Debug.Log(attackAreasPos[i]);
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