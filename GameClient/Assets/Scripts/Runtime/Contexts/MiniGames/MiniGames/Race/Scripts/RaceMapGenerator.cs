using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.MiniGames.View.MiniGame;
using Runtime.Contexts.MiniGames.Vo;
using Runtime.Contexts.Network.Vo;
using UnityEngine;
using Random = UnityEngine.Random;

public class RaceMapGenerator : MapGenerator
{
    public RaceRoadItem[] roadList;
    public Dictionary<int, List<GameObject>> roads=new Dictionary<int, List<GameObject>>();
    public List<RaceRoadItem> roadItems = new();
    public GameObject startObject;
    public GameObject endObject;
    public override void SetMap(MiniGameMapGenerationVo miniGameMapGenerationVo)
    {
        SeperateRoads();
        GenerateMap(miniGameMapGenerationVo);
    }

    private void SeperateRoads()
    {
        for (int i = 0; i < roadList.Length; i++)
        {
            if (roads.ContainsKey(roadList[i].roadType))
            {
                roads[roadList[i].roadType].Add(roadList[i].gameObject);
            }
            else
            {
                roads[roadList[i].roadType] = new List<GameObject>();
                roads[roadList[i].roadType].Add(roadList[i].gameObject);
            }
        }
        roads[3] = new List<GameObject>(){startObject};
        roads[4] = new List<GameObject>(){endObject};
    }

    void GenerateMap(MiniGameMapGenerationVo vo)
    {
        for (int i = 0; i < vo.mapItems.Count; i++)
        {
            KeyValuePair<int, int> pair = vo.mapItems[i];
            GameObject selectedRoad = roads[pair.Key][pair.Value];
            GameObject newRoad = Instantiate(selectedRoad,vo.positions[i].ToVector3() ,vo.rotations[i].ToQuaternion(),transform);
            RaceRoadItem roadItem = newRoad.GetComponent<RaceRoadItem>();
            roadItems.Add(roadItem);
        }
    }
    
}
