using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;

public class MapCreate : MonoBehaviour
{
    public List<List<MapUnit>> Map = new List<List<MapUnit>>();

    public GameObject mouseIndicator;

    public GameObject[] MapUnitPres;
    List<Sprite> MapUnitSprites = new List<Sprite>();

    UnitType mapUnitType;

    string savePath;

    [Header("地图尺寸")]
    public int xMax = 17;
    public int yMax = 9;

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

        for (int i = 0; i < MapUnitPres.Length; i++)
        {
            MapUnitSprites.Add(MapUnitPres[i].GetComponent<SpriteRenderer>().sprite);
        }
    }

    private void Update()
    {
        if(!IsShelter())
        {
            HighlightUnit();
            EditMap();
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
    void EditMap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //如果不在地图范围内则退出
            if (MousePosition.x >= xMax || MousePosition.x < 0 || MousePosition.y >= yMax || MousePosition.y < 0)
                return;
            Map[(int)MousePosition.x][(int)MousePosition.y].type = UnitType.type1;
            Instantiate(MapUnitPres[(int)Map[(int)MousePosition.x][(int)MousePosition.y].type]).transform.position
                = Map[(int)MousePosition.x][(int)MousePosition.y].pos;
        }
    }
    public void DrawMap()
    {
        for (int i = 0; i < xMax; i++)
        {
            for (int j = 0; j < yMax; j++)
            {
                Instantiate(MapUnitPres[(int)Map[i][j].type]).transform.position = Map[i][j].pos;
            }
        }
    }
    public void Clear()
    {
        SceneManager.LoadScene(0);
    }

    public void Switch()
    {

    }
    public void Save()
    {
        using (
            var writer = new BinaryWriter(File.Open(savePath, FileMode.Create))
        ) {
            writer.Write(xMax);
            writer.Write(yMax);
            for (int i = 0; i < xMax; i++)
            {
                for (int j = 0; j < yMax; j++)
                {
                    writer.Write(Map[i][j].pos.x);
                    writer.Write(Map[i][j].pos.y);
                    writer.Write((int)Map[i][j].type);
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
            for (int i = 0; i < xCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    Map[i][j].pos.x = reader.ReadSingle();
                    Map[i][j].pos.y = reader.ReadSingle();
                    Map[i][j].type = IntToUnitType(reader.ReadInt32());
                }
            }
        }
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
        if(path != ""&&path.Length < 50)
            savePath = path;
    }
}
