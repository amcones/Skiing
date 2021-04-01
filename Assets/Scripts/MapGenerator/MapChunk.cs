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
    /// <param name="boundSize"></param>
    /// <param name="center"></param>
    /// <param name="transformParent"></param>
    public void InitializeChunk(Vector2 boundSize, Vector2 center, TileBase grondTile = null, Transform transformParent = null)
    {
        if(id == -1)
        {
            id = GenerateCount;
            GenerateCount++;
        }

        if (transformParent != null)
            transform.SetParent(transformParent);

        bounds.size = boundSize;
        bounds.center = center;
        transform.position = center;

        if(grondTile != null)
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

    public void SetShowChunkLine(bool isShow)
    {
        ShowChunkLine = isShow;
    }

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
