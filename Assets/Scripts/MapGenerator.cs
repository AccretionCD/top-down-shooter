using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] Transform tile;
    [SerializeField] Transform obstacle;
    [SerializeField] Vector2 size;
    [Range(0, 1)] [SerializeField] float outline;
    [SerializeField] int obstacleCount;
    [SerializeField] int shuffleSeed;

    Transform generatedMap;
    string generatedMapName;

    List<Coordinates> tileCoordinates;
    Queue<Coordinates> shuffledTileCoordinates;

    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        StoreMap();
        SortTiles();
        ShuffleCoordinates();
        SpawnObstacles();
    }

    void StoreMap()
    {
        generatedMapName = "Generated Map";

        if (transform.Find(generatedMapName))
            DestroyImmediate(transform.Find(generatedMapName).gameObject);

        generatedMap = new GameObject(generatedMapName).transform;
        generatedMap.parent = transform;
    }

    void SortTiles()
    {
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.x; y++)
            {
                Vector3 tilePosition = new Vector3(-size.x / 2 + 0.5f + x, 0, -size.y / 2 + 0.5f + y);
                Transform newTile = Instantiate(tile, tilePosition, Quaternion.Euler(Vector3.right * 90));
                newTile.localScale = Vector3.one * (1 - outline);
                newTile.parent = generatedMap;
            }
    }

    void ShuffleCoordinates()
    {
        tileCoordinates = new List<Coordinates>();

        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.x; y++)
                tileCoordinates.Add(new Coordinates(x, y));

        shuffledTileCoordinates = new Queue<Coordinates>(Helper.Shuffle(tileCoordinates.ToArray(), shuffleSeed));
    }

    void SpawnObstacles()
    {
        for (int i = 0; i < obstacleCount; i++)
        {
            Coordinates randomCoordinates = GetRandomCoordinates();
            Vector3 obstaclePosition = new Vector3(-size.x / 2 + 0.5f + randomCoordinates.x, 0, -size.y / 2 + 0.5f + randomCoordinates.y) + Vector3.up * 0.5f;
            Transform newObstacle = Instantiate(obstacle, obstaclePosition, Quaternion.identity);
            newObstacle.parent = generatedMap;
        }
    }

    Coordinates GetRandomCoordinates()
    {
        Coordinates randomCoordinates = shuffledTileCoordinates.Dequeue();
        shuffledTileCoordinates.Enqueue(randomCoordinates);

        return randomCoordinates;
    }

    public class Coordinates
    {
        public int x;
        public int y; 

        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
