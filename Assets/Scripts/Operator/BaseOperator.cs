using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Map;
using UnityEngine.UI;

namespace Battle
{
    public class BaseOperator : BaseCharacter
    {
        List<MapUnitPre> attackAreas;

        protected List<BaseEnemy> enemiesInAttackAreas;
        BaseEnemy currentAttackTarget;

        public GameObject attackAreaIndicator;
        List<GameObject> attackAreaIndicatorList;

        Vector2[] attackAreasPos;

        private float m_attackInterval;
        private float attackTimer = 0;

        public bool isIdling;
        public bool isAttacking;
        public bool isSpecialAttacking;


        public int cost;

        bool isInited;

        public PlaceDirection placeDirection;

        Animator animator;

        public Slider energySlider;

        public Action OnEnergyChanged;
        public Action OnAttack;
        public Action OnHurt;

        protected float attackDelay = 0.5f;
        protected float specialAttackDelay = 0.5f;

        protected float currentEnergy;
        protected float maxEnergy;
       

        public float CurrentEnergy
        {
            get
            {
                return currentEnergy;
            }
            protected set
            {
                OnEnergyChanged();
                currentEnergy = value;
                if (currentEnergy >= maxEnergy)
                {
                    SpecialAttack();
                }
            }
        }

   
        public BaseOperator()
        {
            OnEnergyChanged += RefreshEnergySlider;
            OnAttack += OnAttack_EnergyAdd;
        }
        private void OnDestroy()
        {
            OnEnergyChanged -= RefreshEnergySlider;
            OnAttack -= OnAttack_EnergyAdd;

            foreach (MapUnitPre item in attackAreas)
            {
                item.enemyEnter -= AddEnermy;
                item.enemyExit -= RemoveEnermy;
            }
        }
        protected virtual void Awake()
        {
            cost = 10;
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

            TimePass_EnergyAdd();
        }
        private void FixedUpdate()
        {
            if (isInited)
            {
                if (currentEnergy >= maxEnergy)
                {
                    SpecialAttack();
                }
                else
                {
                    Attack();
                }
                
            }

        }
        protected virtual void TimePass_EnergyAdd()
        {
            Debug.LogWarning("TimePass_EnergyAdd未override");
        }
        protected virtual void OnAttack_EnergyAdd()
        {
            Debug.LogWarning("OnAttack_EnergyAdd未override");
        }
        protected virtual void OnHurt_EnergyAdd()
        {
            Debug.LogWarning("OnHurt_EnergyAdd未override");
        }

        void RefreshEnergySlider()
        {
            if (energySlider != null)
                energySlider.value = CurrentEnergy / maxEnergy;
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
            {
                currentAttackTarget = null;
                return;
            }
                
            if (enemiesInAttackAreas.Count == 1)
            {
                currentAttackTarget = enemiesInAttackAreas[0];
                return;
            }
                
            float distance = float.MaxValue;
            BaseEnemy target = null;
            foreach(BaseEnemy item in enemiesInAttackAreas)
            {
                float tmpDistance = (item.transform.position - transform.position).magnitude;
                if (tmpDistance < distance)
                {
                    distance = tmpDistance;
                    target = item;
                }
            }
            currentAttackTarget = target;
        }
        void Attack()
        {
            if (currentAttackTarget == null)
            {
                //Debug.Log("Idling");
                //Debug.Log(enemiesInAttackAreas.Count);
                isIdling = true;
                isAttacking = false;
                isSpecialAttacking = false;
                animator.SetBool("isIdling", true);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isSpecialAttacking", false);
                attackTimer = m_attackInterval;
                return;
            }
            if (attackTimer >= m_attackInterval)
            {
                isAttacking = true;
                isIdling = false;
                isSpecialAttacking = false;
                animator.SetBool("isIdling", false);
                animator.SetBool("isAttacking", true);
                animator.SetBool("isSpecialAttacking", false);
                attackTimer = 0;

                if (currentAttackTarget != null)
                {
                    StartCoroutine(IEAttack());
                }
            }
            else
            {
                isAttacking = false;
                isIdling = true;
                isSpecialAttacking = false;
                animator.SetBool("isIdling", true);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isSpecialAttacking", false);
                attackTimer += Time.deltaTime;
            }

        }
        IEnumerator IEAttack()
        {
            yield return new WaitForSeconds(attackDelay);
            if (OnAttack != null)
            {
                OnAttack();
            }
            if (currentAttackTarget != null)
                AttackDetail();
        }
        protected virtual void AttackDetail()
        {
            currentAttackTarget.Hurt(m_attack, AttackKind.Physics);
            currentAttackTarget.Hurt(m_attack, AttackKind.Magic);
        }

        void SpecialAttack()
        {
            if (currentAttackTarget == null)
            {
                isAttacking = false;
                isIdling = true;
                isSpecialAttacking = false;
                animator.SetBool("isIdling", true);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isSpecialAttacking", false);
                return;
            }
            else
            {
                isAttacking = false;
                isIdling = false;
                isSpecialAttacking = true;
                animator.SetBool("isIdling", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isSpecialAttacking", true);

                CurrentEnergy = 0;

                StartCoroutine(IESpecialAttack());
            }      
        }
        IEnumerator IESpecialAttack()
        {
            yield return new WaitForSeconds(specialAttackDelay);
            SpecialAttackDetail();
        }
        protected virtual void SpecialAttackDetail()
        {
            Debug.LogWarning("未定义特殊攻击");
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