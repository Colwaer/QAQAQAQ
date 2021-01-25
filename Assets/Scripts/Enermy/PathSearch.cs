using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
public class PathSearch : MonoBehaviour
{
        public List<List<MapUnit>> Map = new List<List<MapUnit>>();

        public Vector2 startPos;
        public Vector2 endPos;

        public int xMax;
        public int yMax;

        List<MapUnit> openList = new List<MapUnit>();
        List<MapUnit> closeList = new List<MapUnit>();
        public List<Vector2> path = new List<Vector2>();

        Vector2[] fourDir = { new Vector2(0, 1), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(1, 0) };

        private void Start()
        {
            //GetCurrentMap();
        }
        private void Update()
        {

            if(Input.GetKeyDown(KeyCode.S))
            {
                Search();
                //foreach(Vector2 p in path)
                //{
                //    Debug.Log(p);
                //}
                //Debug.Log(startPos);
                //Debug.Log(endPos);
            }
        }
        void Search()
        {
            GetCurrentMap();
            openList.Clear();
            closeList.Clear();
            path.Clear();
            do
            {
                if ((int)startPos.x < 0 || (int)startPos.x >= xMax)
                {
                    Debug.LogError("起点越界");
                    return;
                }
                if ((int)startPos.y < 0 || (int)startPos.y >= yMax)
                {
                    Debug.LogError("终点越界");
                    return;
                }
                MapUnit st = Map[(int)startPos.x][(int)startPos.y];

                st.g = 0;
                st.h = Mathf.Abs(st.pos.x - endPos.x) + Mathf.Abs(st.pos.y - endPos.y);
                st.f = st.g + st.h;
                closeList.Add(st);
                for (int i = 0; i < 4; i++)
                {
                    //这里还需要判断是否越界
                    if ((int)startPos.x + (int)fourDir[i].x < 0 || (int)startPos.x + (int)fourDir[i].x >= xMax)
                        continue;
                    if ((int)startPos.y + (int)fourDir[i].y < 0 || (int)startPos.y + (int)fourDir[i].y >= yMax)
                        continue;

                    MapUnit t = Map[(int)startPos.x + (int)fourDir[i].x][(int)startPos.y + (int)fourDir[i].y];
                    //这里还需要判断能不能行走
                    if (openList.Contains(t) || closeList.Contains(t))
                        continue;
                    if (!t.canEnermyPass)
                        continue;
                    t.father = st;
                    t.g = t.father.g + 1;
                    t.h = Mathf.Abs(t.pos.x - endPos.x) + Mathf.Abs(t.pos.y - endPos.y);
                    t.f = t.g + t.h;
                    openList.Add(t);
                }
                openList.Sort((MapUnit a, MapUnit b) => { return (int)(a.f - b.f); });
                startPos = openList[0].pos;
                closeList.Add(openList[0]);
                openList.RemoveAt(0);
            } while (startPos != endPos);

            MapUnit at = closeList[closeList.Count-1];
            path.Add(closeList[closeList.Count - 1].pos);
            while (at.father != null)
            {
                at = at.father;
                path.Add(at.pos);

            }
            path.Reverse();

            MapCreate mapCreator;
            mapCreator = GameObject.FindGameObjectWithTag("MapCreator").GetComponent<MapCreate>();
            mapCreator.mapEntity.path = path;

        }


        void GetCurrentMap()
        {
            MapCreate mapCreator;
            mapCreator = GameObject.FindGameObjectWithTag("MapCreator").GetComponent<MapCreate>();
            Map = mapCreator.mapEntity.Map;
            startPos = mapCreator.mapEntity.StartPos;
            endPos = mapCreator.mapEntity.EndPos;
            xMax = mapCreator.xMax;
            yMax = mapCreator.yMax;
        }
    }
}