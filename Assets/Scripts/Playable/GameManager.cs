using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MapChunkGenerator MapChunkGenerator;

    // Start is called before the first frame update
    void Start()
    {
        MapChunkGenerator.InitializeGenerator();
    }

    // Update is called once per frame
    void Update()
    {
        MapChunkGenerator.GeneratorChunkUpdate();
    }
}
