using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class MapInSceneManager : Singleton<MapInSceneManager>
{
    private List<List<MapUnitPre>> mapUnitsOnScene = new List<List<MapUnitPre>>();

    public int xMax;
    public int yMax;
    

    public void Init(int xMax, int yMax)
    {
        this.xMax = xMax;
        this.yMax = yMax;
        for (int i = 0; i < xMax; i++)
        {
            List<MapUnitPre> temp = new List<MapUnitPre>(yMax);
            mapUnitsOnScene.Add(temp);
            for (int j = 0; j < yMax; j++)
                temp.Add(null);
        }
    }
    public void MapUnitPreLoadToList(int x, int y, MapUnitPre prefab)
    {
        //Debug.LogFormat("{0} {1}", x, y);
        mapUnitsOnScene[x][y] = prefab;
    }
    public MapUnitPre GetMapUnitPre(int x, int y)
    {
        if (x >= xMax || x < 0)
            return null;
        if (y >= yMax || y < 0)
            return null;
        return mapUnitsOnScene[x][y];
        
    }
    public void SetMapUnitPre(int x, int y, MapUnitPre t)
    {
        mapUnitsOnScene[x][y] = t;
    }
}
