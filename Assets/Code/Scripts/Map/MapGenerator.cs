using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Map;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public int tiles = 20;

    public float tileSize = 4f;

    public float gap;

    public GameObject groundTile;
    public GameObject pathTile;
    public GameObject startTile;
    public GameObject endTile;
    public float scale = 20f;

    private readonly List<GameObject> _tileStore = new();
    private int[,] _map;
    private float _noiseOffset;
    private float _oldScale;

    private Vector3 _position;

    private MapNode[,] _weightsMap;

    private void Awake()
    {
        _oldScale = scale;
        PopulateMap();
        _noiseOffset = Random.Range(0f, 100f);
    }

    private void Update()
    {
        if (scale != _oldScale)
        {
            _oldScale = scale;
            ResetMap();
            PopulateMap();
        }
    }

    private void SetUpMap()
    {
        SetWeights();
        FindOptimalPath();
    }

    private void ResetMap()
    {
        foreach (var tile in _tileStore)
        {
            Debug.Log("Destroying");
            Destroy(tile);
        }
    }

    private void SetWeights()
    {
        _weightsMap = new MapNode[tiles, tiles];
        for (var x = 0; x < tiles; x++)
        for (var y = 0; y < tiles; y++)
            _weightsMap[x, y] =
                new MapNode(
                    Mathf.PerlinNoise((float)x / tiles * scale + Random.Range(0f, 1000f),
                        (float)y / tiles * scale + Random.Range(0f, 1000f)),
                    x, y);
    }

    private void FindOptimalPath()
    {
        var openList = new PriorityQueue<MapNode>();
        var closedList = new List<MapNode>();
        var destination = GetRandomEndPoint();
        var startPoint = GetRandomStartPoint();
        startPoint.totalWeight = 0f;
        openList.Enqueue(startPoint, startPoint.weight);

        //Find a path using A*
        while (openList.Count > 0)
        {
            //Pops
            var currentNode = openList.Dequeue();
            if (currentNode.Equals(destination)) break;
            closedList.Add(currentNode);
            ExpandNode(currentNode);
        }

        SetPath();


        void ExpandNode(MapNode currentNode)
        {
            foreach (var successor in GetNeighbours())
                //If successor not in closed List
                if (!closedList.Any(node => node.Equals(successor)))
                {
                    var tentative_g = currentNode.totalWeight + successor.weight;
                    if (!(openList.Contains(successor) && tentative_g > successor.totalWeight))
                    {
                        successor.bestSuccesor = currentNode;
                        successor.totalWeight = tentative_g;
                        if (!openList.Contains(successor)) openList.Enqueue(successor, successor.weight);
                    }
                }

            List<MapNode> GetNeighbours()
            {
                var neighbours = new List<MapNode>();

                if (currentNode.x > 0) neighbours.Add(_weightsMap[currentNode.x - 1, currentNode.y]);

                if (currentNode.x < tiles - 1) neighbours.Add(_weightsMap[currentNode.x + 1, currentNode.y]);
                if (currentNode.y > 0) neighbours.Add(_weightsMap[currentNode.x, currentNode.y - 1]);
                if (currentNode.y < tiles - 1) neighbours.Add(_weightsMap[currentNode.x, currentNode.y + 1]);

                return neighbours;
            }
        }

        void SetPath()
        {
            _map = new int[tiles, tiles];
            _map[destination.x, destination.y] = 3;
            var successor = destination.bestSuccesor;
            while (successor != null)
            {
                _map[successor.x, successor.y] = 1;
                successor = successor.bestSuccesor;
            }

            _map[startPoint.x, startPoint.y] = 2;
        }

        MapNode GetRandomStartPoint()
        {
            return _weightsMap[1, 1];
        }

        MapNode GetRandomEndPoint()
        {
            return _weightsMap[tiles - 2, tiles - 2];
        }
    }


    private void PopulateMap()
    {
        SetUpMap();
        var totalSize = tiles * tileSize + (tiles - 1) * gap;
        _position = new Vector3(-1 * (totalSize / 2 - tileSize), 0f, -1 * (totalSize / 2 - tileSize));
        for (var i = 0; i < tiles; i++)
        {
            for (var j = 0; j < tiles; j++)
            {
                var tileIndex = _map[i, j];
                switch (tileIndex)
                {
                    case 1:
                        InstantiateTile(pathTile, _position, gameObject.transform.rotation);
                        break;
                    case 2:
                        InstantiateTile(startTile, _position, gameObject.transform.rotation);
                        break;
                    case 3:
                        InstantiateTile(endTile, _position, gameObject.transform.rotation);
                        break;
                    default:
                        InstantiateTile(groundTile, _position, gameObject.transform.rotation);
                        break;
                }

                _position += new Vector3(0f, 0f, tileSize + gap);
            }

            _position += new Vector3(tileSize + gap, 0f, totalSize * -1 - gap);
        }
    }

    private void InstantiateTile(GameObject tile, Vector3 position, Quaternion rotation)
    {
        var instance = Instantiate(tile, _position, gameObject.transform.rotation);
        instance.transform.parent = gameObject.transform;
        _tileStore.Add(instance);
    }

    private class MapNode : IEquatable<MapNode>
    {
        public readonly float weight;
        public readonly int x;
        public readonly int y;
        public MapNode bestSuccesor;
        public float totalWeight;

        public MapNode(float weight, int x, int y)
        {
            this.weight = weight;
            this.x = x;
            this.y = y;
        }

        public bool Equals(MapNode other)
        {
            if (other == null) return false;
            return x == other.x && y == other.y;
        }
    }
}