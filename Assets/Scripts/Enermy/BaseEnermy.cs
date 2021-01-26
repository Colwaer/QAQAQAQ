using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

namespace Battle
{
    public class BaseEnermy : BaseCharacter
    {
        List<Vector2> path = new List<Vector2>();

        public bool isAttacking;
        public bool isMoving;
        public bool isIdling;

        LayerMask operatorLayer;

        private float m_attackDistance = 1.5f;
        private float m_attackInterval = 2.0f;
        private float attackTimer = 0;

        private Vector2 lookDir;

        public float moveSpeed = 3.0f;

        Rigidbody2D rb;
        Animator animator;

        private void Start()
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
            Move();
            Attack();
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
        virtual protected void Attack()
        {
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, lookDir, m_attackDistance, operatorLayer);
            
            if (hit.collider != null)
            {
                
                
                if (attackTimer >= m_attackInterval)
                {
                    isMoving = false;
                    isAttacking = true;
                    isIdling = false;
                    animator.SetBool("isMoving", false);
                    animator.SetBool("isAttacking", true);
                    animator.SetBool("isIdling", false);

                    attackTimer = 0;
                    
                    Debug.Log("Attack");
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

        public void GetPath()
        {
            List<Vector2> temp = new List<Vector2>(GameObject.FindGameObjectWithTag("SearchManager").GetComponent<PathSearch>().path.ToArray());
            path = temp;
        }

        public BaseEnermy(float attack, float defend, float magicDamage, float magicDefend, float maxHelath, float attackDistance) 
            : base(attack, defend, magicDamage, magicDefend, maxHelath)
        {
            m_attackDistance = attackDistance;
            path = GameObject.FindGameObjectWithTag("SearchManager").GetComponent<PathSearch>().path;
        }
    }
}

