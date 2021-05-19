using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapObstacleList
{
    [SerializeField] private List<MapObstacle> usingObstacle;
    public List<MapObstacle> UsingObstacle => usingObstacle;

    [SerializeField] private List<MapObstacle> unuseObstacle;
    public List<MapObstacle> UnuseObstacle => unuseObstacle;

    public MapObstacleList()
    {
        usingObstacle = new List<MapObstacle>();
        unuseObstacle = new List<MapObstacle>();
    }

    public void InitializeList(List<GameObject> obstaclePrefabConfigs, int obstacleGenNum, Vector2 genPosition, Transform generateParentObject = null)
    {
        foreach(GameObject obstacleConfiguration in obstaclePrefabConfigs)
        {
            for(int genNum = obstacleGenNum; genNum > 0; genNum --)
            {
                MapObstacle obstacle = CreateObstacle(obstacleConfiguration, genPosition, generateParentObject);
                unuseObstacle.Add(obstacle);
            }
        }
    }

    public MapObstacle CreateObstacle(GameObject prefab, Vector2 position, Transform parent)
    {
        MapObstacle obstacle = GameObject.Instantiate(prefab).GetComponent<MapObstacle>();
        if (obstacle == null)
            throw new System.NullReferenceException();

        obstacle.InitializeObstacle(position, null, parent);
        return obstacle;
    }

    private MapObstacle GetUnuseObstacle()
    {
        if(unuseObstacle.Count > 0)
        {
            int randomObstacleIndex = Random.Range(0, unuseObstacle.Count);
            MapObstacle res = unuseObstacle[randomObstacleIndex];
            unuseObstacle.RemoveAt(randomObstacleIndex);
            return res;
        }
        return null;
    }

    public MapObstacle AddUseObstacle(Vector2 position, MapChunk chunk)
    {
        if (!IsCreateObstacle(position))
        {
            MapObstacle obstacle = GetUnuseObstacle();
            if(obstacle != null)
            {
                obstacle.InitializeObstacle(position, chunk);
                usingObstacle.Add(obstacle);
                return obstacle;
            }
        }
        return null;
    }

    public bool IsCreateObstacle(Vector2 position)
    {
        foreach(MapObstacle obstacle in usingObstacle)
        {
            if(obstacle.obstacleArea.Contains(position))
                return true;
        }
        return false;
    }

    public bool AddUnuseObstacle(MapObstacle obstacle)
    {
        if (unuseObstacle.Contains(obstacle))
            return false;
        unuseObstacle.Add(obstacle);
        return true;
    }

    public void CleanUpList()
    {
        foreach(var obstacle in unuseObstacle)
        {
            usingObstacle.Remove(obstacle);
        }
    }
}
