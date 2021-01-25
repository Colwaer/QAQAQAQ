using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

namespace Battle
{
    public class BaseEnermy : BaseCharacter
    {
        List<Vector2> path = new List<Vector2>();

        public bool isMoving;
        public bool isAttacking;

        public float moveSpeed = 3.0f;

        Rigidbody2D rb;

        private void Start()
        {
            isMoving = false;
            rb = GetComponent<Rigidbody2D>();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                isMoving = !isMoving;
                GetPath();
            }
                
        }
        private void FixedUpdate()
        {
            if (isMoving)
                Move();
        }

        virtual protected void Move()
        {
            if (isMoving)
            {

                if (path.Count == 0)
                    return;
                if (((Vector2)transform.position - path[0]).magnitude > 0.05f)
                {
                    rb.transform.Translate((path[0] - (Vector2)transform.position).normalized * Time.deltaTime * moveSpeed);
                }
                else
                {
                    //Debug.Log("delete node");
                    
                    path.RemoveAt(0);
                }
            }
        }

        public void GetPath()
        {
            path = GameObject.FindGameObjectWithTag("SearchManager").GetComponent<PathSearch>().path;
        }

        public BaseEnermy(float attack, float defend, float magicDamage, float magicDefend, float maxHelath) 
            : base(attack, defend, magicDamage, magicDefend, maxHelath)
        {
            path = GameObject.FindGameObjectWithTag("SearchManager").GetComponent<PathSearch>().path;
        }
    }
}

