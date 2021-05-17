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

    // 目标测试物体的Transform组件
    private Transform testTransform;

    // 区块对角线的长度
    private float chunkSizeMagnitude;
    #endregion

    public void InitializeGenerator()
    {
        mapChunkList = new MapChunkList(InitializeChunkNum);
        mapChunkList.InitializeList(ChunkPrefab, ChunkInitializePosition, GeneratorGrid, ChunkSizeConfig, GroundFileTile);

        mapObstacleList = new MapObstacleList();
        mapObstacleList.InitializeList(BarriesPrefabs, ObstacleGenerateNumberForElem, ObstacleInitilizePosition, ObstacleParent);
        
        if(TestObject != null)
            testTransform = TestObject.transform;

        chunkSizeMagnitude = ChunkSizeConfig.magnitude;
    }

    public void GeneratorChunkUpdate()
    {
        if(TestObject != null)
        {
            FollowTestTargetCreateChunk();
            FollowTestTargetDeleteChunk();
        }
    }

    public void SetPlayer(Player target)
    {
        TestObject = target.gameObject;
        testTransform = target.transform;
    }

    bool IsLowerThanDistance(float target, float distance)
    {
        return target <= distance;
    }

    void FollowTestTargetCreateChunk()
    {
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
                GenerateChunk(new Vector2(nowChunkCenter.x - ChunkSizeConfig.x, nowChunkCenter.y));
            }
            // 判断检测物体离右侧是否小于等于指定距离
            else if (isXRightCloset)
            {
                GenerateChunk(new Vector2(nowChunkCenter.x + ChunkSizeConfig.x, nowChunkCenter.y));
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
                GenerateChunk(new Vector2(nowChunkCenter.x, nowChunkCenter.y - ChunkSizeConfig.y));
            }
            // 判断检测物体离上侧是否小于等于指定距离
            else if (isYUpCloset)
            {
                GenerateChunk(new Vector2(nowChunkCenter.x, nowChunkCenter.y + ChunkSizeConfig.y));
            }

            // 如果离左和下很近，需要在左下斜角再生成一个区块
            if (isXLeftCloset && isYDownCloset)
            {
                GenerateChunk(new Vector2(nowChunkCenter.x - ChunkSizeConfig.x, nowChunkCenter.y - ChunkSizeConfig.y));
            }
            // 如果离右和下很近，需要在右下斜角再生成一个区块
            else if (isXRightCloset && isYDownCloset)
            {
                GenerateChunk(new Vector2(nowChunkCenter.x + ChunkSizeConfig.x, nowChunkCenter.y - ChunkSizeConfig.y));
            }
            // 如果离左和上很近，需要在左上斜角再生成一个区块
            else if (isXLeftCloset && isYUpCloset)
            {
                GenerateChunk(new Vector2(nowChunkCenter.x - ChunkSizeConfig.x, nowChunkCenter.y + ChunkSizeConfig.y));
            }
            // 如果离右和上很近，需要在右上斜角再生成一个区块
            else if (isXRightCloset && isYUpCloset)
            {
                GenerateChunk(new Vector2(nowChunkCenter.x + ChunkSizeConfig.x, nowChunkCenter.y + ChunkSizeConfig.y));
            }
        }
    }

    /// <summary>
    /// 得到测试目标现在所在的区块
    /// </summary>
    /// <remarks>
    /// 遍历所有正在使用的区块，如果区块包含测试目标所在的位置，即此区块为所在区块
    /// </remarks>
    /// <returns></returns>
    public MapChunk GetPlayerCurrentChunk()
    {
        foreach (var chunk in mapChunkList.UsingChunks)
        {
            if(chunk.bounds.Contains(testTransform.position))
            {
                return chunk;
            }
        }
        return null;
    }

    /// <summary>
    /// 生成区块
    /// </summary>
    /// <param name="center">新区块的位置/中心</param>
    void GenerateChunk(Vector2 center)
    {
        MapChunk chunk = mapChunkList.AddUseChunk(center);
        if (chunk != null)
        {
            Bounds bounds = chunk.bounds;
            int genNum = Random.Range((int)(SingleChunkObstacleMax * Granularity), SingleChunkObstacleMax);

            // 为区块生成障碍物。genPos是为了记录已经生成了障碍物的位置
            List<Vector2> genPos = new List<Vector2>();
            while (genNum > 0)
            {
                Vector2 newPos = GenerateRandomPositionFromBounds(bounds);
                if (!genPos.Contains(newPos))
                {
                    mapObstacleList.AddUseObstacle(newPos, chunk);
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

    /// <summary>
    /// 删除区块
    /// </summary>
    void FollowTestTargetDeleteChunk()
    {
        foreach(MapChunk chunk in mapChunkList.UsingChunks)
        {
            // 如果离测试物体的距离大于设定距离就加入到未使用的区块中
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

        // 需要执行CleanUpList来清理两个列表
        mapChunkList.CleanUpList();
        mapObstacleList.CleanUpList();
    }
}
