using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapChunkGenerator : MonoBehaviour
{
    [Header("地图生成前置物体")]

    [Tooltip("需要检测的物体")]
    public GameObject TestObject;

    [Tooltip("指定生成区块的Grid")]
    public GameObject GeneratorGrid;

    [Tooltip("区块的预制体")]
    public GameObject ChunkPrefab;

    [Tooltip("指定填充区块的Tile")]
    public TileBase GroundFileTile;
    public List<GameObject> BarriesPrefabs;

    [Header("生成区块设置")]
    [Tooltip("区块大小")]
    public Vector2 ChunkSizeConfig;

    [Tooltip("与区块边缘的距离小与这个数值就会生成新的区块")]
    public float DistanceEdge = 10f;

    [Tooltip("按区块斜角计算距离的倍数，超过倍数数值删除区块")]
    public float DistanceDel = 2f;

    [Header("检查测试物体")]
    [Tooltip("指定碰撞的层")]
    public LayerMask HitLayer;

    [Tooltip("指定射线方向")]
    public Vector3 RayDirection;

    [Tooltip("指定射线检测距离")]
    public float RayDistance = 10f;


    [Header("区块显示相关")]
    public bool DrawChunk;

    [SerializeField] private List<MapChunk> Chunks;
    private IGetOtherCollider testTrigger;
    private Transform testTransform;
    private CircleCollider2D testDistanceTrigger;
    private float chunkSizeMagnitude;
    // Start is called before the first frame update
    void Start()
    {
        Chunks = new List<MapChunk>();
        testTrigger = TestObject.GetComponent<IGetOtherCollider>();
        testTransform = TestObject.transform;
        testDistanceTrigger = TestObject.GetComponent<CircleCollider2D>();
        Chunks.Add(CreateChunk(Vector2.zero));
        chunkSizeMagnitude = ChunkSizeConfig.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        FollowTestTargetCreateChunk();
        FollowTestTargetDeleteChunk();
        DrawChunks();
    }

    RaycastHit2D Raycast(Vector3 origin, Vector3 direction, float distance)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, direction, distance, HitLayer);
        Color drawRayColor;
        if (raycastHit2D)
            drawRayColor = Color.red;
        else
            drawRayColor = Color.green;

        Debug.DrawRay(origin, direction, drawRayColor, distance);

        return raycastHit2D;
    }

    void DrawChunks()
    {
        // 绘制区块
        if (DrawChunk)
        {
            if (Chunks.Count > 0)
            {
                foreach (MapChunk chunk in Chunks)
                {
                    chunk.SetShowChunkLine(true);
                }
            }
        }
        else
        {
            if (Chunks.Count > 0)
            {
                foreach (MapChunk chunk in Chunks)
                {
                    chunk.SetShowChunkLine(false);
                }
            }
        }
    }

    void FollowTestTargetCreateChunk()
    {
        if (testDistanceTrigger.radius != DistanceEdge)
            testDistanceTrigger.radius = DistanceEdge;

        //Collider2D chunkCollider = Raycast(testTransform.position, RayDirection, RayDistance).collider;
        Collider2D chunkCollider = testTrigger.GetOtherCollider;

        if (chunkCollider != null && chunkCollider.CompareTag("Ground"))
        {
            // 获取检测物体当前所在的区块
            MapChunk chunk = chunkCollider.gameObject.GetComponent<MapChunk>();

            // 获取当前的区块中心
            Vector3 nowChunkCenter = chunk.boundSize.center;

            // 三个布尔值，记录检测物体离当前区块的左、右、下的远近
            bool isXLeftCloset = false;
            bool isXRightCloset = false;
            bool isYDownCloset = false;
            bool isYUpCloset = false;

            // 判断检测物体离左侧是否小于等于指定距离
            if (testTransform.position.x - (nowChunkCenter.x - ChunkSizeConfig.x / 2.0f) <= DistanceEdge)
            {
                isXLeftCloset = true;
                Vector2 newCenter = new Vector2(nowChunkCenter.x - ChunkSizeConfig.x, nowChunkCenter.y);
                if (!IsCreatedChunk(newCenter))
                    Chunks.Add(CreateChunk(newCenter));
            }
            // 判断检测物体离右侧是否小于等于指定距离
            else if ((nowChunkCenter.x + ChunkSizeConfig.x / 2.0f) - testTransform.position.x <= DistanceEdge)
            {
                isXRightCloset = true;
                Vector2 newCenter = new Vector2(nowChunkCenter.x + ChunkSizeConfig.x, nowChunkCenter.y);
                if (!IsCreatedChunk(newCenter))
                    Chunks.Add(CreateChunk(newCenter));
            }

            // 判断检测物体离下侧是否小于等于指定距离
            if (testTransform.position.y - (nowChunkCenter.y - ChunkSizeConfig.y / 2.0f) <= DistanceEdge)
            {
                isYDownCloset = true;
                Vector2 newCenter = new Vector2(nowChunkCenter.x, nowChunkCenter.y - ChunkSizeConfig.y);
                if (!IsCreatedChunk(newCenter))
                    Chunks.Add(CreateChunk(newCenter));
            }
            // 判断检测物体离上侧是否小于等于指定距离
            else if ((nowChunkCenter.y + ChunkSizeConfig.y / 2.0f) - testTransform.position.y <= DistanceEdge)
            {
                isYUpCloset = true;
                Vector2 newCenter = new Vector2(nowChunkCenter.x, nowChunkCenter.y + ChunkSizeConfig.y);
                if (!IsCreatedChunk(newCenter))
                    Chunks.Add(CreateChunk(newCenter));
            }

            // 如果离左和下很近，需要在左下斜角再生成一个区块
            if (isXLeftCloset && isYDownCloset)
            {
                Vector2 newCenter = new Vector2(nowChunkCenter.x - ChunkSizeConfig.x, nowChunkCenter.y - ChunkSizeConfig.y);
                if (!IsCreatedChunk(newCenter))
                    Chunks.Add(CreateChunk(newCenter));
            }
            // 如果离右和下很近，需要在右下斜角再生成一个区块
            else if (isXRightCloset && isYDownCloset)
            {
                Vector2 newCenter = new Vector2(nowChunkCenter.x + ChunkSizeConfig.x, nowChunkCenter.y - ChunkSizeConfig.y);
                if (!IsCreatedChunk(newCenter))
                    Chunks.Add(CreateChunk(newCenter));
            }
            // 如果离左和上很近，需要在左上斜角再生成一个区块
            else if (isXLeftCloset && isYUpCloset)
            {
                Vector2 newCenter = new Vector2(nowChunkCenter.x - ChunkSizeConfig.x, nowChunkCenter.y + ChunkSizeConfig.y);
                if (!IsCreatedChunk(newCenter))
                    Chunks.Add(CreateChunk(newCenter));
            }
            // 如果离右和上很近，需要在右上斜角再生成一个区块
            else if (isXRightCloset && isYUpCloset)
            {
                Vector2 newCenter = new Vector2(nowChunkCenter.x + ChunkSizeConfig.x, nowChunkCenter.y + ChunkSizeConfig.y);
                if (!IsCreatedChunk(newCenter))
                    Chunks.Add(CreateChunk(newCenter));
            }
        }
    }

    void FollowTestTargetDeleteChunk()
    {
        List<MapChunk> mapChunks = new List<MapChunk>();
        foreach(var chunk in Chunks)
        {
            if(Vector2.Distance(testTransform.position, chunk.transform.position) > DistanceDel * chunkSizeMagnitude)
            {
                mapChunks.Add(chunk);
            }
        }

        foreach(var delChunk in mapChunks)
        {
            Chunks.Remove(delChunk);
            Destroy(delChunk.gameObject);
        }
    }

    /// <summary>
    /// 判断此处是否生成了区块
    /// </summary>
    /// <param name="center"></param>
    /// <returns></returns>
    bool IsCreatedChunk(Vector2 center)
    {
        foreach(MapChunk chunk in Chunks)
        {
            if ((Vector2)chunk.boundSize.center == center)
                return true;
        }
        return false;
    }

    Bounds GetBounds(Vector3 center, Vector3 size)
    {
        return new Bounds(center, size);
    }

    MapChunk CreateChunk(Vector2 center)
    {
        MapChunk newChunk = InitializeChunk(center);
        FillChunk(newChunk.gameObject.GetComponent<Tilemap>(), newChunk.boundSize);
        return newChunk;
    }

    /// <summary>
    /// 初始化区块
    /// </summary>
    /// <param name="center"></param>
    /// <returns></returns>
    MapChunk InitializeChunk(Vector2 center)
    {
        // 实例化一个新的区块
        GameObject newChunk = GameObject.Instantiate(ChunkPrefab);
        if (newChunk is null)
            throw new System.NullReferenceException();

        newChunk.transform.SetParent(GeneratorGrid.transform);
        newChunk.transform.position = center;

        // 获取区块属性，并将其设置为预先指定的数值
        MapChunk chunkConf = newChunk.GetComponent<MapChunk>();
        chunkConf.boundSize = GetBounds(center, ChunkSizeConfig);
        return chunkConf;
    }

    /// <summary>
    /// 填充区块（Tilemap）
    /// </summary>
    /// <param name="target"></param>
    /// <param name="bounds"></param>
    void FillChunk(Tilemap target, Bounds bounds)
    {
        // 获取左下角的位置
        Vector2Int fillPosStart = new Vector2Int(-(int)bounds.extents.x, -(int)bounds.extents.y);
        // 获取宽度
        int width = (int)bounds.size.x;

        //获取高度
        int height = (int)bounds.size.y;

        // 从左下角，填充高度X宽度大小的区域
        for (int xPos = 0;xPos < width;xPos ++)
        {
            for(int yPos = 0;yPos < height;yPos ++)
            {
                target.SetTile(new Vector3Int(fillPosStart.x + xPos, fillPosStart.y + yPos, 0), GroundFileTile);
            }
        }
    }
}
