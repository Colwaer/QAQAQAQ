using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Map
{
    public class MapSaver 
    {
        public static void Save(string savePath,MapEntity entity)
        {
            //检测是否能保存
            bool canBeSaved = false;
            if (entity.StartPos == -Vector2.one || entity.EndPos == -Vector2.one)
                canBeSaved = false;
            for (int i = 0; i < entity.xMax; i++)
            {
                for (int j = 0; j < entity.yMax; j++)
                {
                    if(entity.Map[i][j].canPlaceOperator)
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
                writer.Write(entity.xMax);
                writer.Write(entity.yMax);
                writer.Write(entity.StartPos.x);
                writer.Write(entity.StartPos.y);
                writer.Write(entity.EndPos.x);
                writer.Write(entity.EndPos.y);
                for (int i = 0; i < entity.xMax; i++)
                {
                    for (int j = 0; j < entity.yMax; j++)
                    {
                        writer.Write(entity.Map[i][j].pos.x);
                        writer.Write(entity.Map[i][j].pos.y);
                        writer.Write((int)entity.Map[i][j].GetUnitType());
                    }
                }
            }
        }
        public static void Load(string savePath,MapEntity entity)
        {
            using (
                var reader = new BinaryReader(File.Open(savePath,FileMode.Open))
            ) {
                int xCount = reader.ReadInt32();
                int yCount = reader.ReadInt32();
                entity.StartPos.x = reader.ReadSingle();
                entity.StartPos.y = reader.ReadSingle();
                entity.EndPos.x = reader.ReadSingle();
                entity.EndPos.y = reader.ReadSingle();
                for (int i = 0; i < xCount; i++)
                {
                    for (int j = 0; j < yCount; j++)
                    {
                        entity.Map[i][j].pos.x = reader.ReadSingle();
                        entity.Map[i][j].pos.y = reader.ReadSingle();
                        entity.Map[i][j].SwitchType(MapUnit.IntToUnitType(reader.ReadInt32()));
                    }
                }
            }
        }
    }
}