using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum UnitType
{
    defaultType,
    blank,
    type1,
    type2,
    
}
public class MapUnit
{
    public Vector2 pos;

    public UnitType type;

    public MapUnit()
    {
        
    }
    public MapUnit(int x, int y)
    {
        pos.x = x;
        pos.y = y;
        type = UnitType.defaultType;
    }
    public MapUnit(Vector2 Pos)
    {
        pos = Pos;
        type = UnitType.defaultType;
    }
    public MapUnit(int x, int y, UnitType Type)
    {
        pos.x = x;
        pos.y = y;
        type = Type;
    }
    public MapUnit(Vector2 Pos, UnitType Type)
    {
        pos = Pos;
        type = Type;
    }
}
