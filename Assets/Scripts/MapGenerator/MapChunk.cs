using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapChunk : MonoBehaviour
{
    private static long GenerateCount = 0;

    public Bounds bounds;

    [Header("Debug用区块显示")]
    public LineRenderer LineRenderer;
    public bool ShowChunkLine;
    public float LineWidth = 0.00001f;
    
    private static readonly Vector2Int[] drawLineElem = { new Vector2Int(0, 1), new Vector2Int(1, 3), new Vector2Int(3, 2), new Vector2Int(2, 0) };

    private long id = -1;
    public long ID => id;

    private TileBase grondTile;
    private Tilemap tilemap;

    //private void Start()
    //{
    //    LineRenderer.positionCount = 5;
    //    LineRenderer.startWidth = LineWidth;
    //    LineRenderer.endWidth = LineWidth;
    //}

    //private void Update()
    //{
    //    if (ShowChunkLine)
    //    {
    //        DrawRect();
    //    }
    //    else
    //    {
    //        LineRenderer.enabled = false;
    //    }
    //}

    /// <summary>
    /// 使用区域大小、中心来初始化区块，可设置其父物体
    /// </summary>
    public void InitializeChunk(Vector2 boundSize, Vector2 center, TileBase grondTile = null, Transform transformParent = null)
    {
        // ======================================
        // 如果是第一次生成，赋予一个ID，之后不可变。
        // 暂时没有想到使用的地方，先留在这里
        // ======================================
        if (id == -1)
        {
            id = GenerateCount;
            GenerateCount++;
        }

        // ======================================
        // 生成的时候需要父物体，特别是使用了Tilemap，
        // 需要放在有Grid组件的父物体下。
        // 只有传入的父物体不为空时，该段代码才不会运行
        // ======================================
        if (transformParent != null)
            transform.SetParent(transformParent);

        // ======================================
        // 此处用于设置区块大小（bounds.size）、中心（bounds.center）
        // 以及位置（transform.position）
        // ======================================
        bounds.size = boundSize;
        bounds.center = center;
        transform.position = center;

        // ======================================
        // 在传入瓦片不为空时会重新为该区块的Tilemap
        // 填充新的瓦片。
        // ======================================
        if (grondTile != null)
        {
            this.grondTile = grondTile;
            if(tilemap == null)
                tilemap = gameObject.GetComponent<Tilemap>();
            FillChunk();
        }
    }

    public void FillChunk()
    {
        // 获取左下角的位置
        Vector2Int fillPosStart = new Vector2Int(-(int)bounds.extents.x, -(int)bounds.extents.y);
        // 获取宽度
        int width = (int)bounds.size.x;

        //获取高度
        int height = (int)bounds.size.y;
        // 从左下角，填充高度X宽度大小的区域
        for (int xPos = 0; xPos < width; xPos++)
        {
            for (int yPos = 0; yPos < height; yPos++)
            {
                tilemap.SetTile(new Vector3Int(fillPosStart.x + xPos, fillPosStart.y + yPos, 0), grondTile);
            }
        }
    }

    /// <summary>
    /// 调试时使用，可以显示目前活跃的区块。
    /// </summary>
    /// <remarks>
    /// 需要使用请将MapChunk的Start即Update取消注释
    /// </remarks>
    public void SetShowChunkLine(bool isShow)
    {
        ShowChunkLine = isShow;
    }

    /// <summary>
    /// 同上
    /// </summary>
    public void DrawRect()
    {
        LineRenderer.enabled = true;
        List<Vector3> vertex4Rect = new List<Vector3>();
        float x = bounds.extents.x;
        float y = bounds.extents.y;
        vertex4Rect.Add(new Vector3(x, y));
        vertex4Rect.Add(new Vector3(x, -y));
        vertex4Rect.Add(new Vector3(-x, -y));
        vertex4Rect.Add(new Vector3(-x, y));
        foreach (Vector2Int lineElem in drawLineElem)
        {
            Debug.DrawLine(vertex4Rect[lineElem.x] + bounds.center, vertex4Rect[lineElem.y] + bounds.center);
        }

        for(int index = 0;index < 4;index ++)
        {
            LineRenderer.SetPosition(index, vertex4Rect[index] + bounds.center);
        }
        LineRenderer.SetPosition(4, vertex4Rect[0] + bounds.center);
    }
}
