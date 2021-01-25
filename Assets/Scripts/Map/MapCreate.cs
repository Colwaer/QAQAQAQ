using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
namespace Map
{
    public class MapCreate : MonoBehaviour
    {
        public GameObject mouseIndicator;
        string savePath;

        public MapEntity mapEntity;
        public GameObject[] MapUnitPres;
        //List<Sprite> MapUnitSprites = new List<Sprite>();

        //所有已经绘制的格子的副本
        List<GameObject> UnitCopies = new List<GameObject>();

        [Header("起点终点坐标")]
        public Vector2 startPos = -Vector2.one;
        public Vector2 endPos = -Vector2.one;
        public Text startPosText;
        public Text endPosText;


        [Header("地图尺寸")]
        public int xMax = 17;
        public int yMax = 9;

        [Header("未命名")]
        public UnitType mapUnitType;

        public bool isEditing;
        public bool isSetingStartPos;
        public bool isSetingEndPos;

        //取整后的坐标
        Vector2 MousePosition;


        private void Start()
        {
            savePath = Path.Combine(Application.persistentDataPath, "saveFile");
            mapEntity = new MapEntity(xMax,yMax,startPos,endPos);
            Gizmos.color = Color.red;
            mouseIndicator = Instantiate(mouseIndicator);
            mouseIndicator.transform.position = Vector2.zero;

            //for (int i = 0; i < MapUnitPres.Length; i++)
            //{
            //    MapUnitSprites.Add(MapUnitPres[i].GetComponent<SpriteRenderer>().sprite);
            //}
        }

        private void Update()
        {
            if(!IsShelter())
            {
                HighlightUnit();
            }
            if (isEditing)
            {
                EditMap();
            }
            else if (isSetingStartPos)
            {
                SetStartPos();
            }
            else if (isSetingEndPos)
            {
                SetEndPos();
            }

        }
        bool IsShelter()
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                mouseIndicator.SetActive(false);
                return true;
            }
            else
            {
                mouseIndicator.SetActive(true);
                return false;
            }    
        }
        //高亮鼠标所在的格子
        void HighlightUnit()
        {
            MousePosition = Input.mousePosition;
            MousePosition = Camera.main.ScreenToWorldPoint(MousePosition);

            MousePosition.x = FloatToInt(MousePosition.x);
            MousePosition.y = FloatToInt(MousePosition.y);

            if (MousePosition.x >= xMax || MousePosition.x < 0 || MousePosition.y >= yMax || MousePosition.y < 0)
                return;

            mouseIndicator.transform.position = MousePosition;
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
        void EditMap()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //如果不在地图范围内则退出
                if (!IsMousePosInMap())
                    return;
                mapEntity.Map[(int)MousePosition.x][(int)MousePosition.y].SwitchType(mapUnitType);

                GameObject t;
                t = Instantiate(MapUnitPres[(int)mapEntity.Map[(int)MousePosition.x][(int)MousePosition.y].GetUnitType()]);
                t.transform.position = mapEntity.Map[(int)MousePosition.x][(int)MousePosition.y].pos;

                UnitCopies.Add(t);
            }
        }
        void SetStartPos()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!IsMousePosInMap())
                    return;
                startPos = MousePosition;
                startPosText.text = "起点坐标:" + startPos.x + "," + startPos.y;
            }
        }
        void SetEndPos()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!IsMousePosInMap())
                    return;
                endPos = MousePosition;
                endPosText.text = "终点坐标:" + endPos.x + "," + endPos.y;
            }
        }

        //下面的几个函数均为button点击事件
        public void DrawMap()
        {
            if (UnitCopies != null)
            {
                foreach(GameObject t in UnitCopies)
                {
                    Destroy(t);
                }
                UnitCopies.Clear();
            }
            for (int i = 0; i < xMax; i++)
            {
                for (int j = 0; j < yMax; j++)
                {
                    GameObject t;
                    t = Instantiate(MapUnitPres[(int)mapEntity.Map[i][j].GetUnitType()]);
                    t.transform.position = mapEntity.Map[i][j].pos;
                    UnitCopies.Add(t);
                }
            }
        }
        public void Clear()
        {
            SceneManager.LoadScene(0);
        }

        public void Edit()
        {
            isEditing = true;
            isSetingStartPos = false;
            isSetingEndPos = false;
        }
        public void ButtonSetStart()
        {
            isSetingStartPos = true;
            isEditing = false;
            isSetingEndPos = false;
        }
        public void ButtonSetEnd()
        {
            isSetingEndPos = true;
            isSetingStartPos = false;
            isEditing = false;     
        }
        public void Save()
        {
            MapSaver.Save(savePath,mapEntity);
        }
        public void Load()
        {
            MapSaver.Load(savePath,mapEntity);

            //load以后的小处理
            startPosText.text = "起点坐标:" + startPos.x + "," + startPos.y;
            endPosText.text = "终点坐标:" + endPos.x + "," + endPos.y;
            DrawMap();
        }

        public void ChangePath(string path)
        {
            if (path != "" && path.Length < 50)
                savePath = path;
        }
    }
}