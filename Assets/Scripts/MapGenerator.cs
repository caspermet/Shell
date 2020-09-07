using Packages.Rider.Editor.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Vector2 mapsize;

    [Range(0, 1)]
    public float outlinePercent;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;

    public int seed = 10;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        allTileCoords = new List<Coord>();
        for (int x = 0; x < mapsize.x; x++)
        {
            for (int y = 0; y < mapsize.y; y++)
            {
                allTileCoords.Add(new Coord(x, y));
            }
        }
        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), seed));


        string holderName = "Generated Map";
        if (transform.FindChild(holderName))
        {
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for (int x = 0; x < mapsize.x; x++)
        {
            for (int y = 0; y < mapsize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90));
                newTile.localScale = Vector3.one * (1 - outlinePercent);
                newTile.parent = mapHolder;
            }
        }

        int obstacleCount = 10;

        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoor = GetRandomCoord();
            Vector3 obstaclePosition = CoordToPosition(randomCoor.x, randomCoor.y);
            Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;
            newObstacle.parent = mapHolder;
        }
    }

    Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(-mapsize.x / 2 + 0.5f + x, 0, -mapsize.y / 2 + 0.5f + y); 
    }

    public Coord GetRandomCoord()
    {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }
}
