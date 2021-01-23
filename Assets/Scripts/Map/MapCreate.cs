using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MapCreate : MonoBehaviour
{
    public GameObject mouseIndicator;
    string savePath;

    public List<List<MapUnit>> Map = new List<List<MapUnit>>();
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

        for (int i = 0; i < xMax; i++)
        {
            List<MapUnit> temp = new List<MapUnit>(yMax);
            Map.Add(temp);
            for (int j = 0; j < yMax; j++)
            {
                temp.Add(new MapUnit(i, j));
                //Instantiate(MapUnitPres[0]).transform.position = new Vector2(i, j);
                
            }
        }
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
            Map[(int)MousePosition.x][(int)MousePosition.y].SwitchType(mapUnitType);

            GameObject t;
            t = Instantiate(MapUnitPres[(int)Map[(int)MousePosition.x][(int)MousePosition.y].GetUnitType()]);
            t.transform.position = Map[(int)MousePosition.x][(int)MousePosition.y].pos;

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
                t = Instantiate(MapUnitPres[(int)Map[i][j].GetUnitType()]);
                t.transform.position = Map[i][j].pos;
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
        //检测是否能保存
        bool canBeSaved = false;
        if (startPos == -Vector2.one || endPos == -Vector2.one)
            canBeSaved = false;
        for (int i = 0; i < xMax; i++)
        {
            for (int j = 0; j < yMax; j++)
            {
                if(Map[i][j].canPlaceOperator)
                {
                    canBeSaved = true;
                    break;
                }
            }
        }
        if(!canBeSaved)
        {
            Debug.LogError("地图不能被保存");
            return;
        }

        using (
            var writer = new BinaryWriter(File.Open(savePath, FileMode.Create))
        ) {
            writer.Write(xMax);
            writer.Write(yMax);
            writer.Write(startPos.x);
            writer.Write(startPos.y);
            writer.Write(endPos.x);
            writer.Write(endPos.y);
            for (int i = 0; i < xMax; i++)
            {
                for (int j = 0; j < yMax; j++)
                {
                    writer.Write(Map[i][j].pos.x);
                    writer.Write(Map[i][j].pos.y);
                    writer.Write((int)Map[i][j].GetUnitType());
                }
            }
        }
    }
    public void Load()
    {
        using (
            var reader = new BinaryReader(File.Open(savePath,FileMode.Open))
        ) {
            int xCount = reader.ReadInt32();
            int yCount = reader.ReadInt32();
            startPos.x = reader.ReadSingle();
            startPos.y = reader.ReadSingle();
            endPos.x = reader.ReadSingle();
            endPos.y = reader.ReadSingle();
            for (int i = 0; i < xCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    Map[i][j].pos.x = reader.ReadSingle();
                    Map[i][j].pos.y = reader.ReadSingle();
                    Map[i][j].SwitchType(IntToUnitType(reader.ReadInt32()));
                }
            }
        }

        //load以后的小处理
        startPosText.text = "起点坐标:" + startPos.x + "," + startPos.y;
        endPosText.text = "终点坐标:" + endPos.x + "," + endPos.y;
        DrawMap();
    }
    /// <summary>
    /// 怎么把int转换成枚举类型，这里临时处理了下，后续学到了更好的方法再说
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public UnitType IntToUnitType(int value)
    {
        if (value == 0)
            return UnitType.defaultType;
        else if (value == 1)
            return UnitType.blank;
        else if (value == 2)
            return UnitType.type1;
        else if (value == 3)
            return UnitType.type2;
        return UnitType.blank;
    }

    public void ChangePath(string path)
    {
        if (path != "" && path.Length < 50)
            savePath = path;
    }
}
