using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapChunkList
{
    [SerializeField] private List<MapChunk> usingChunks;
    public List<MapChunk> UsingChunks => usingChunks;

    [SerializeField] private List<MapChunk> unuseChunks;
    public List<MapChunk> UnuseChunks => unuseChunks;

    private int allChunkNumber;
    private Vector2 chunkSizeConfig;

    public MapChunkList(int chunkNumber)
    {
        usingChunks = new List<MapChunk>();
        unuseChunks = new List<MapChunk>();
        allChunkNumber = chunkNumber;
    }

    public void InitializeList(GameObject chunkPrefab, Transform parentObject, Vector2 boundSize)
    {
        chunkSizeConfig = boundSize;
        for (int genNum = allChunkNumber; genNum > 1; genNum--)
        {
            MapChunk mapChunk = CreateChunk(chunkPrefab, parentObject);
            mapChunk.enabled = false;
            unuseChunks.Add(mapChunk);
        }
        usingChunks.Add(CreateChunk(chunkPrefab, parentObject));
    }

    public MapChunk GetUnuseChunk()
    {
        if (unuseChunks.Count > 0)
        {
            MapChunk res = unuseChunks[0];
            res.enabled = true;
            unuseChunks.RemoveAt(0);
            return res;
        }
        return null;
    }

    public bool AddUseChunk(Vector2 center)
    {
        if (!IsCreatedChunk(center))
        {
            MapChunk newChunk = GetUnuseChunk();
            if (newChunk != null)
            {
                newChunk.InitializeChunk(chunkSizeConfig, center);
                UsingChunks.Add(newChunk);
                return true;
            }
        }
        return false;
    }

    public bool AddUnuseChunk(MapChunk mapChunk)
    {
        if (unuseChunks.Contains(mapChunk))
            return false;
        unuseChunks.Add(mapChunk);
        return true;
    }

    public void CleanUpUnuseChunk()
    {
        foreach(var chunk in unuseChunks)
        {
            Transform[] childTransform = chunk.gameObject.GetComponentsInChildren<Transform>();
            foreach(var child in childTransform)
            {
                if (child != chunk.gameObject.transform)
                    GameObject.Destroy(child.gameObject);
            }
            chunk.enabled = false;
        }
    }

    /// <summary>
    /// 判断此处是否生成了区块
    /// </summary>
    /// <param name="center"></param>
    /// <returns></returns>
    public bool IsCreatedChunk(Vector2 center)
    {
        foreach (MapChunk chunk in usingChunks)
        {
            if ((Vector2)chunk.bounds.center == center)
                return true;
        }
        return false;
    }

    MapChunk CreateChunk(GameObject prefab, Transform parent)
    {
        GameObject newChunk = GameObject.Instantiate(prefab);
        if (newChunk is null)
            throw new System.NullReferenceException();

        MapChunk newChunkMap = newChunk.GetComponent<MapChunk>();
        newChunkMap.InitializeChunk(chunkSizeConfig, Vector2.zero, parent);
        return newChunkMap;
    }
}
