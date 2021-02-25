using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using Battle;
namespace Map
{
    public class MapCreate : Singleton<MapCreate>
    {
        public GameObject mouseIndicator;
        string savePath;

        public MapEntity mapEntity;
        public GameObject[] MapUnitPres;

        public bool isGaming;

        //List<Sprite> MapUnitSprites = new List<Sprite>();

        //所有已经绘制的格子的副本
        List<GameObject> UnitCopies = new List<GameObject>();

        [Header("起点终点坐标")]
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
            mapEntity = new MapEntity(xMax, yMax, Vector2.zero, Vector2.zero);
            MapInSceneManager.Instance.Init(xMax, yMax);
            Gizmos.color = Color.red;
            if (!isGaming)
            {
                mouseIndicator = Instantiate(mouseIndicator);
                mouseIndicator.transform.position = Vector2.zero;
            }
            
        }

        private void Update()
        {
            if (isGaming)
                return;

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
            if (PlaceManager.Instance.IsPlacingOperator)
                return;
            if (Input.GetMouseButtonDown(0))
            {
                //如果不在地图范围内则退出
                if (!IsMousePosInMap())
                    return;
                mapEntity.Map[(int)MousePosition.x][(int)MousePosition.y].SwitchType(mapUnitType);
                GameObject t;
                t = Instantiate(MapUnitPres[(int)mapEntity.Map[(int)MousePosition.x][(int)MousePosition.y].GetUnitType()]);
                t.transform.position = mapEntity.Map[(int)MousePosition.x][(int)MousePosition.y].pos;
                t.GetComponent<MapUnitPre>().Init();

                MapInSceneManager.Instance.SetMapUnitPre(
                    (int)t.transform.position.x, (int)t.transform.position.y, t.GetComponent<MapUnitPre>());

                Debug.Log(MapInSceneManager.Instance.GetMapUnitPre((int)t.transform.position.x, (int)t.transform.position.y).type);

                UnitCopies.Add(t);               
            }
        }

        void SetStartPos()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!IsMousePosInMap())
                    return;
                mapEntity.StartPos = MousePosition;
                startPosText.text = "起点坐标:" + mapEntity.StartPos.x + "," + mapEntity.StartPos.y;
            }
        }
        void SetEndPos()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!IsMousePosInMap())
                    return;
                mapEntity.EndPos = MousePosition;
                endPosText.text = "终点坐标:" + mapEntity.EndPos.x + "," + mapEntity.EndPos.y;
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
                    t.GetComponent<MapUnitPre>().Init();
                    UnitCopies.Add(t);
                }
            }
        }
        public void Clear()
        {
            SceneManager.LoadScene(0);
        }
        public void ForbidAllBools()
        {
            isEditing = false;
            isSetingStartPos = false;
            isSetingEndPos = false;
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
            MapSaver.Save(savePath, mapEntity);
        }
        public void Load()
        {
            MapSaver.Load(savePath, mapEntity);

            //load以后的小处理
            MapInSceneManager.Instance.Init(mapEntity.xMax, mapEntity.yMax);

            if (!isGaming)
            {
                startPosText.text = "起点坐标:" + mapEntity.StartPos.x + "," + mapEntity.StartPos.y;
                endPosText.text = "终点坐标:" + mapEntity.EndPos.x + "," + mapEntity.EndPos.y;
            }
            
            DrawMap();
        }

        public void ChangePath(string path)
        {
            Debug.Log(path);
            if (path != "" && path.Length < 50)
                savePath = path;
        }
    }
}