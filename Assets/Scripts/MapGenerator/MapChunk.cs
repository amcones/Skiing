using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChunk : MonoBehaviour
{
    public Bounds boundSize;

    [Header("Debug用区块显示")]
    public LineRenderer LineRenderer;
    public bool ShowChunkLine;
    public float LineWidth = 0.00001f;
    
    private static readonly Vector2Int[] drawLineElem = { new Vector2Int(0, 1), new Vector2Int(1, 3), new Vector2Int(3, 2), new Vector2Int(2, 0) };

    private void Start()
    {
        LineRenderer.positionCount = 5;
        LineRenderer.startWidth = LineWidth;
        LineRenderer.endWidth = LineWidth;
    }

    private void Update()
    {
        boundSize.center = transform.position;
        if (ShowChunkLine)
        {
            DrawRect();
        }
        else
        {
            LineRenderer.enabled = false;
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
        float x = boundSize.extents.x;
        float y = boundSize.extents.y;
        vertex4Rect.Add(new Vector3(x, y));
        vertex4Rect.Add(new Vector3(x, -y));
        vertex4Rect.Add(new Vector3(-x, -y));
        vertex4Rect.Add(new Vector3(-x, y));
        foreach (Vector2Int lineElem in drawLineElem)
        {
            Debug.DrawLine(vertex4Rect[lineElem.x] + boundSize.center, vertex4Rect[lineElem.y] + boundSize.center);
        }

        for(int index = 0;index < 4;index ++)
        {
            LineRenderer.SetPosition(index, vertex4Rect[index] + boundSize.center);
        }
        LineRenderer.SetPosition(4, vertex4Rect[0] + boundSize.center);

    }
}
