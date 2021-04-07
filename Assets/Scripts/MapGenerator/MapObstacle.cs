using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObstacle : MonoBehaviour
{
    private static long GenerteCount = 0;

    public Vector2 currentPosition;
    public Rect obstacleArea;

    private MapChunk currentChunk;
    private SpriteRenderer[] spriteRenderers;

    private long id = -1;
    public long ID => id;
    public void InitializeObstacle(Vector2 newPosition, MapChunk chunk, Transform parent = null)
    {
        if(id == -1)
        {
            id = GenerteCount;
            GenerteCount++;
            spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        }

        currentPosition = newPosition;
        this.gameObject.transform.position = newPosition;
        obstacleArea.center = newPosition;

        currentChunk = chunk;

        if (parent != null)
            this.gameObject.transform.parent = parent;

        if(spriteRenderers != null || spriteRenderers.Length >= 2)
        {
            foreach(var renderer in spriteRenderers)
            {
                renderer.sortingOrder = (int)Mathf.Abs(newPosition.y);
            }
        }
    }

    public bool IsCurrentChunk(MapChunk mapChunk)
    {
        return mapChunk == currentChunk;
    }
}
