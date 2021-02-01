﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Battle
{


    public class PlaceManager : Singleton<PlaceManager>
    {
        //取整后的坐标
        Vector2 MousePosition;

        public GameObject[] operatorPres;
        public GameObject dirChoosePanelPre;
        GameObject curDirChoosePanel;
        GameObject curOperator;


        int xMax;
        int yMax;

        PlaceDirection dir;

        public bool isPlacingOperator { get; private set; }
        bool showAttackAreaPreview = false;

        private void Start()
        {
            
        }

        private void Update()
        {
            CalMousePosition();

            if (showAttackAreaPreview)
            {
                ShowAttackAreaPreview();
                if (Input.GetMouseButtonDown(0))
                {
                    //Debug.Log("Place over!");
                    DetermineOperatorDir();
                }
            }

            if (isPlacingOperator)
            {
                PlaceOperator();
            }
            
            
        }
        void DetermineOperatorDir()
        {
            curOperator.GetComponent<BaseOperator>().Init(dir);
            Destroy(curDirChoosePanel);
            curOperator.GetComponent<BaseOperator>().OffShowAttackArea();
            showAttackAreaPreview = false;
            isPlacingOperator = false;
        }
        void PlaceOperator()
        {
            //Debug.Log("enterPlaceOperator");
            
            if (!IsMousePosInMap())
                return;
            //Debug.Log("MousePosInMap");
            if (!MapInSceneManager.Instance.GetMapUnitPre((int)MousePosition.x, (int)MousePosition.y).canPlaceOperator)
                return;
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Place Operator");

                curOperator = Instantiate(operatorPres[Random.Range(0, operatorPres.Length)]);
                curOperator.transform.position = MousePosition;

                ShowChooseDirPanel();

                isPlacingOperator = false;
            }
        }
        void ShowChooseDirPanel()
        {
            curDirChoosePanel = Instantiate(dirChoosePanelPre);
            curDirChoosePanel.transform.position = MousePosition;

            showAttackAreaPreview = true;
        }
        /*
        public void UpDir()
        {
            //这里的init作用为设置攻击方块的监听，故暂且不init并不出error
            curOperator.GetComponent<BaseOperator>().Init(PlaceDirection.up);
            Destroy(curDirChoosePanel);


            showAttackAreaPreview = false;
            curOperator.GetComponent<BaseOperator>().OffShowAttackArea();
            isPlacingOperator = false;
        }
        */
        void ShowAttackAreaPreview()
        {
            Vector2 lookDirection;

            lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - curOperator.transform.position;
            float angle = 90 - Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg;
            angle = Mathf.Deg2Rad * angle;
            //Debug.LogFormat("Cos:{0}, Sin:{1}", Mathf.Cos(angle), Mathf.Sin(angle));

            if (Mathf.Cos(angle) >= 0.7f && Mathf.Abs(Mathf.Sin(angle)) <= 0.7f)
            {
                if (dir != PlaceDirection.right)
                {
                    //Debug.Log("right");
                    curOperator.GetComponent<BaseOperator>().OffShowAttackArea();
                    dir = PlaceDirection.right;
                    curOperator.GetComponent<BaseOperator>().SetAttackAreas(PlaceDirection.right);
                    curOperator.GetComponent<BaseOperator>().ShowAttackArea();
                }
                
            }
            else if (Mathf.Abs(Mathf.Cos(angle)) <= 0.7f && Mathf.Sin(angle) >= 0.7f)
            {
                if (dir != PlaceDirection.up)
                {
                    //Debug.Log("up");
                    curOperator.GetComponent<BaseOperator>().OffShowAttackArea();
                    dir = PlaceDirection.up;
                    curOperator.GetComponent<BaseOperator>().SetAttackAreas(PlaceDirection.up);
                    curOperator.GetComponent<BaseOperator>().ShowAttackArea();
                }
                
            }
            else if (Mathf.Cos(angle) <= -0.7f && Mathf.Abs(Mathf.Sin(angle)) <= 0.7f)
            {
                if (dir != PlaceDirection.left)
                {
                    //Debug.Log("left");
                    curOperator.GetComponent<BaseOperator>().OffShowAttackArea();
                    dir = PlaceDirection.left;
                    curOperator.GetComponent<BaseOperator>().SetAttackAreas(PlaceDirection.left);
                    curOperator.GetComponent<BaseOperator>().ShowAttackArea();
                }
                
            }
            else
            {
                if (dir != PlaceDirection.down)
                {
                    //Debug.Log("down");
                    curOperator.GetComponent<BaseOperator>().OffShowAttackArea();
                    dir = PlaceDirection.down;
                    curOperator.GetComponent<BaseOperator>().SetAttackAreas(PlaceDirection.down);
                    curOperator.GetComponent<BaseOperator>().ShowAttackArea();
                }              
            }

        }


        void CalMousePosition()
        {
            MousePosition = Input.mousePosition;
            MousePosition = Camera.main.ScreenToWorldPoint(MousePosition);

            MousePosition.x = FloatToInt(MousePosition.x);
            MousePosition.y = FloatToInt(MousePosition.y);
        }
        float FloatToInt(float value)
        {
            value = Mathf.Round(value);
            return value;
        }
        bool IsMousePosInMap()
        {
            if (MousePosition.x >= xMax || MousePosition.x < 0 || MousePosition.y >= yMax || MousePosition.y < 0)
                return false;
            return true;
        }

        public void ButtonPlaceOperator()
        {
            xMax = MapInSceneManager.Instance.xMax;
            yMax = MapInSceneManager.Instance.yMax;

            isPlacingOperator = true;
        }

        


    }
}
