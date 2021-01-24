﻿using System.Collections;
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

    private UnitType type;

    public float f, g, h;

    public MapUnit father;

    public bool canPlaceOperator { get; private set; }
    public bool canEnermyPass { get; private set; }

    public void SwitchType(UnitType Type)
    {
        type = Type;
        ChangeCanPlaceOperator();
    }

    /// <summary>
    /// 设置某种格子类型能不能放置干员，能不能使敌人经过
    /// </summary>
    void ChangeCanPlaceOperator()
    {
        switch(type)
        {
            case UnitType.defaultType:
                canPlaceOperator = false;
                canEnermyPass = true;
                break;
            case UnitType.blank:
                canPlaceOperator = false;
                canEnermyPass = false;
                break;
            case UnitType.type1:
                canPlaceOperator = true;
                canEnermyPass = false;
                break;
            case UnitType.type2:
                canPlaceOperator = false;
                canEnermyPass = true;
                break;
            default:
                Debug.LogErrorFormat("格子{0}没有的类型为{1}，该类型不存在", pos, type);
                break;
        }
    }
    public UnitType GetUnitType()
    {
        return type;
    }

    public MapUnit()
    {
        
    }
    public MapUnit(int x, int y)
    {
        pos.x = x;
        pos.y = y;
        type = UnitType.defaultType;
        ChangeCanPlaceOperator();
    }
    public MapUnit(Vector2 Pos)
    {
        pos = Pos;
        type = UnitType.defaultType;
        ChangeCanPlaceOperator();
    }
    public MapUnit(int x, int y, UnitType Type)
    {
        pos.x = x;
        pos.y = y;
        type = Type;
        ChangeCanPlaceOperator();
    }
    public MapUnit(Vector2 Pos, UnitType Type)
    {
        pos = Pos;
        type = Type;
        ChangeCanPlaceOperator();
    }
}
