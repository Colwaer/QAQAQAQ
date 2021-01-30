using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Battle
{


    public class PlaceManager : Singleton<PlaceManager>
    {
        //取整后的坐标
        Vector2 MousePosition;

        public GameObject[] operatorPres;

        int xMax;
        int yMax;

        bool isPlacingOperator;

        private void Start()
        {
            
        }

        private void Update()
        {
            CalMousePosition();

            if (isPlacingOperator)
            {
                PlaceOperator();
            }
            
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

                GameObject t = Instantiate(operatorPres[Random.Range(0, operatorPres.Length)]);

                t.GetComponent<BaseOperator>().Init(PlaceDirection.left);

                isPlacingOperator = false;
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
