using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

namespace Battle
{
    public class BaseEnemy : BaseCharacter
    {
        List<Vector2> path = new List<Vector2>();

        public bool isAttacking;
        public bool isMoving;
        public bool isIdling;

        protected float attackDelayTime = 0.5f;

        LayerMask operatorLayer;

        protected float m_attackDistance = 1.5f;
        protected float m_attackInterval = 2.0f;
        private float attackTimer = 0;

        private Vector2 lookDir;

        public float moveSpeed = 3.0f;

        Rigidbody2D rb;
        Animator animator;

        public GameObject dizzyEffect;

        BaseOperator attackTarget;

        bool isDizzying = false;


        protected virtual void Start()
        {
            m_attackInterval = 2.0f;
            Physics2D.queriesStartInColliders = false;

            operatorLayer = LayerMask.GetMask("Operator");
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            m_attackDistance = 1.5f;

            GetPath();
        }
        private void Update()
        {
            
               
        }
        private void FixedUpdate()
        {         
            
            if (!isDizzying)
            {
                Move();
                Attack();
            }
            else
            {

            }
            

        }
        private void OnDestroy()
        {
            
        }

        virtual protected void Move()
        {
            if (isMoving && !isAttacking)
            {
                if (path.Count == 0)
                    return;
                isMoving = true;
                animator.SetBool("isMoving", true);
                animator.SetBool("isAttacking", false);
                
                if (((Vector2)transform.position - path[0]).magnitude > 0.05f)
                {
                    lookDir = (path[0] - (Vector2)transform.position).normalized;
                    rb.transform.Translate(lookDir * Time.deltaTime * moveSpeed);
                }
                else
                {
                    //Debug.Log("delete node");
                    
                    path.RemoveAt(0);
                }
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + lookDir * m_attackDistance);
        }
        void Attack()
        {
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, lookDir, m_attackDistance, operatorLayer);
            
            if (hit.collider != null)
            {            
                
                if (attackTimer >= m_attackInterval)
                {
                    attackTarget = hit.collider.GetComponent<BaseOperator>();
                    isMoving = false;
                    isAttacking = true;
                    isIdling = false;
                    animator.SetBool("isMoving", false);
                    animator.SetBool("isAttacking", true);
                    animator.SetBool("isIdling", false);

                    attackTimer = 0;
                    if (attackTarget != null)
                    {
                        StartCoroutine(IEAttack());              
                    }
                                         
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
            else
            {                
                isAttacking = false;
                isMoving = true;
                isIdling = false;
                animator.SetBool("isAttacking", false);
                animator.SetBool("isMoving", true);
                animator.SetBool("isIdling", false);
                attackTimer = m_attackInterval;
            }
        }
        IEnumerator IEAttack()
        {
            yield return new WaitForSeconds(attackDelayTime);
            if (attackTarget != null)
                AttackDetail();
        }
        protected virtual void AttackDetail()
        {
            attackTarget.Hurt(m_attack, AttackKind.Physics);
            attackTarget.Hurt(m_magicDamage, AttackKind.Magic);
        }


        public void GetPath()
        {
            List<Vector2> temp = new List<Vector2>(PathSearch.Instance.path.ToArray());
            path = temp;
        }

        public BaseEnemy()
        {
            
        }

        public void Dizzy(float time)
        {
            isDizzying = true;
            isMoving = false;
            isIdling = true;
            animator.SetBool("isMoving", false);
            animator.SetBool("isIdling", true);
            animator.SetBool("isAttacking", false);

            dizzyEffect.SetActive(true);

            StartCoroutine(IEDizzy(time));
        }
        IEnumerator IEDizzy(float time)
        {
            yield return new WaitForSeconds(time);
            
            isDizzying = false;

            dizzyEffect.SetActive(false);
        }
    }
}

