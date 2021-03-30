using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapChunkGenerator : MonoBehaviour
{
    #region Atrribute Field
    [Header("目标测试物体")]
    [Tooltip("需要检测的物体")]
    public GameObject TestObject;

    [Header("区块生成参数设置")]
    [Tooltip("指定生成区块的Grid")]
    public Transform GeneratorGrid;

    [Tooltip("区块的预制体")]
    public GameObject ChunkPrefab;

    [Tooltip("指定填充区块的Tile")]
    public TileBase GroundFileTile;

    [Tooltip("游戏开始时生成区块的数量")]
    public int InitializeChunkNum = 8;

    [Tooltip("区块大小")]
    public Vector2 ChunkSizeConfig;

    [Tooltip("与区块边缘的距离小与这个数值就会生成新的区块")]
    public float DistanceEdge = 10f;

    [Tooltip("按区块斜角计算距离的倍数，超过倍数数值删除区块")]
    public float DistanceDel = 2f;

    [Tooltip("区块初始化生成时的位置")]
    public Vector2 ChunkInitializePosition = new Vector2(100, 100);

    [Header("区块显示相关")]
    public bool DrawChunk;

    [Header("障碍物生成设置")]
    [Tooltip("指定生成障碍物的预置体")]
    public List<GameObject> BarriesPrefabs;

    [Tooltip("生成障碍物时的父物体")]
    public Transform ObstacleParent;

    [Tooltip("每个障碍物初始化生成的数量")]
    public int ObstacleGenerateNumberForElem = 6;

    [Tooltip("障碍物初始化时的位置")]
    public Vector2 ObstacleInitilizePosition = new Vector2(100, 100);

    [Tooltip("每个区块生成障碍物的数量最大值")]
    [Range(0, 20)] public int SingleChunkObstacleMax;

    [Tooltip("障碍物粒度")]
    [Range(0.0f, 1.0f)]public float Granularity = 0.5f;
    [SerializeField] MapChunkList mapChunkList;
    [SerializeField] MapObstacleList mapObstacleList;

    // 目标测试物体的碰撞体或触发器在碰撞或触发之后可返回碰撞或触发的其他物体，用于检测物体是否到达了可以生成新区块的距离
    private IGetOtherCollider testTrigger;

    // 目标测试物体的Transform组件
    private Transform testTransform;

    // 目标测试物体的碰撞体或触发器
    private CircleCollider2D testDistanceTrigger;

    // 区块对角线的长度
    private float chunkSizeMagnitude;
    #endregion

    void Start()
    {
        mapChunkList = new MapChunkList(InitializeChunkNum);
        mapChunkList.InitializeList(ChunkPrefab, ChunkInitializePosition, GeneratorGrid, ChunkSizeConfig, GroundFileTile);

        //FillAllChunk(mapChunkList.UsingChunks);
        //FillAllChunk(mapChunkList.UnuseChunks);

        mapObstacleList = new MapObstacleList();
        mapObstacleList.InitializeList(BarriesPrefabs, ObstacleGenerateNumberForElem, ObstacleInitilizePosition, ObstacleParent);
        
        testTrigger = TestObject.GetComponent<IGetOtherCollider>();
        testTransform = TestObject.transform;
        testDistanceTrigger = TestObject.GetComponent<CircleCollider2D>();

        chunkSizeMagnitude = ChunkSizeConfig.magnitude;
    }

    void Update()
    {
        FollowTestTargetCreateChunk();
        FollowTestTargetDeleteChunk();
        DrawChunks();
    }

    void DrawChunks()
    {
        if (DrawChunk)
        {
            if (mapChunkList.UsingChunks.Count > 0)
            {
                foreach (MapChunk chunk in mapChunkList.UsingChunks)
                {
                    chunk.SetShowChunkLine(true);
                }
            }
        }
        else
        {
            if (mapChunkList.UsingChunks.Count > 0)
            {
                foreach (MapChunk chunk in mapChunkList.UsingChunks)
                {
                    chunk.SetShowChunkLine(false);
                }
            }
        }
    }

    bool IsLowerThanDistance(float target, float distance)
    {
        return target <= distance;
    }

    void FollowTestTargetCreateChunk()
    {
        if (testDistanceTrigger.radius != DistanceEdge)
            testDistanceTrigger.radius = DistanceEdge;

        MapChunk chunk = GetPlayerCurrentChunk();
        if (chunk != null)
        {
            // 获取当前的区块中心
            Vector3 nowChunkCenter = chunk.bounds.center;

            // 两个布尔值，记录检测物体离当前区块的左、右的远近
            bool isXLeftCloset = IsLowerThanDistance(
                testTransform.position.x - (nowChunkCenter.x - ChunkSizeConfig.x / 2.0f), 
                DistanceEdge);
            bool isXRightCloset = IsLowerThanDistance(
                nowChunkCenter.x + ChunkSizeConfig.x / 2.0f - testTransform.position.x, 
                DistanceEdge);
            
            // 判断检测物体离左侧是否小于等于指定距离
            if (isXLeftCloset)
            {
                GenerateChunk(new Vector2(nowChunkCenter.x - ChunkSizeConfig.x, nowChunkCenter.y),
                    chunk);
            }
            // 判断检测物体离右侧是否小于等于指定距离
            else if (isXRightCloset)
            {
                GenerateChunk(new Vector2(nowChunkCenter.x + ChunkSizeConfig.x, nowChunkCenter.y),
                    chunk);
            }

            // 两个布尔值，记录检测物体离当前区块的上、下的远近
            bool isYDownCloset = IsLowerThanDistance(
                testTransform.position.y - (nowChunkCenter.y - ChunkSizeConfig.y / 2.0f),
                DistanceEdge);
            bool isYUpCloset = IsLowerThanDistance(
                (nowChunkCenter.y + ChunkSizeConfig.y / 2.0f) - testTransform.position.y,
                DistanceEdge);

            // 判断检测物体离下侧是否小于等于指定距离
            if (isYDownCloset)
            {
                GenerateChunk(new Vector2(nowChunkCenter.x, nowChunkCenter.y - ChunkSizeConfig.y),
                    chunk);
            }
            // 判断检测物体离上侧是否小于等于指定距离
            else if (isYUpCloset)
            {
                GenerateChunk(new Vector2(nowChunkCenter.x, nowChunkCenter.y + ChunkSizeConfig.y),
                    chunk);
            }

            // 如果离左和下很近，需要在左下斜角再生成一个区块
            if (isXLeftCloset && isYDownCloset)
            {
                GenerateChunk(new Vector2(nowChunkCenter.x - ChunkSizeConfig.x, nowChunkCenter.y - ChunkSizeConfig.y),
                    chunk);
            }
            // 如果离右和下很近，需要在右下斜角再生成一个区块
            else if (isXRightCloset && isYDownCloset)
            {
                GenerateChunk(new Vector2(nowChunkCenter.x + ChunkSizeConfig.x, nowChunkCenter.y - ChunkSizeConfig.y),
                    chunk);
            }
            // 如果离左和上很近，需要在左上斜角再生成一个区块
            else if (isXLeftCloset && isYUpCloset)
            {
                GenerateChunk(new Vector2(nowChunkCenter.x - ChunkSizeConfig.x, nowChunkCenter.y + ChunkSizeConfig.y),
                    chunk);
            }
            // 如果离右和上很近，需要在右上斜角再生成一个区块
            else if (isXRightCloset && isYUpCloset)
            {
                GenerateChunk(new Vector2(nowChunkCenter.x + ChunkSizeConfig.x, nowChunkCenter.y + ChunkSizeConfig.y),
                    chunk);
            }
        }
    }

    MapChunk GetPlayerCurrentChunk()
    {
        foreach (var chunk in mapChunkList.UsingChunks)
        {
            if(IsInArea(testTransform.position, chunk.bounds))
            {
                return chunk;
            }
        }
        Collider2D chunkCollider = testTrigger.GetOtherCollider;
        if (chunkCollider.CompareTag("Ground"))
        {
            return chunkCollider.gameObject.GetComponent<MapChunk>();
        }
        return null;
    }

    bool IsInArea(Vector2 pos, Bounds field)
    {
        if(pos.x <= field.max.x && pos.x >= field.min.x
            && pos.y <= field.max.y && pos.y >= field.min.y)
        {
            return true;
        }

        return false;
    }

    void GenerateChunk(Vector2 center, MapChunk currentChunk)
    {
        MapChunk chunk = mapChunkList.AddUseChunk(center);
        if (chunk != null)
        {
            Bounds bounds = chunk.bounds;
            int genNum = Random.Range((int)(SingleChunkObstacleMax * Granularity), SingleChunkObstacleMax);
            List<Vector2> genPos = new List<Vector2>();
            while (genNum > 0)
            {
                Vector2 newPos = GenerateRandomPositionFromBounds(bounds);
                if (!genPos.Contains(newPos))
                {
                    mapObstacleList.AddUseObstacle(newPos, currentChunk);
                    genPos.Add(newPos);
                    genNum--;
                }
            }
        }
    }

    Vector2 GenerateRandomPositionFromBounds(Bounds bounds)
    {
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
            );
    }

    void FollowTestTargetDeleteChunk()
    {
        foreach(MapChunk chunk in mapChunkList.UsingChunks)
        {
            if(Vector2.Distance(testTransform.position, chunk.transform.position) > DistanceDel * chunkSizeMagnitude)
            {
                mapChunkList.AddUnuseChunk(chunk);
                foreach(MapObstacle obstacle in mapObstacleList.UsingObstacle)
                {
                    if (obstacle.IsCurrentChunk(chunk))
                        mapObstacleList.AddUnuseObstacle(obstacle);
                }
            }
        }

        mapChunkList.CleanUpList();
        mapObstacleList.CleanUpList();
    }

    void FillAllChunk(List<MapChunk> mapChunks)
    {
        foreach(var chunk in mapChunks)
        {
            chunk.FillChunk();
        }
    }
}
