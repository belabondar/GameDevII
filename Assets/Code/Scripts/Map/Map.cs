using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Map;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    public int tiles = 20;

    public float tileSize = 4f;

    public float scale = 20f;

    public Tile tile;
    private int[,] _map;

    private List<Tile> _tileStore;
    private List<Vector3> _wayPoints;

    private MapNode[,] _weightsMap;

    private float _xOffset;
    private float _yOffset;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _xOffset = Random.Range(0f, 1000f);
        _yOffset = Random.Range(0f, 1000f);


        _weightsMap = new MapNode[tiles, tiles];
        _map = new int[tiles, tiles];
        _tileStore = new List<Tile>();
        _wayPoints = new List<Vector3>();


        CreateMap();
    }

    private void CreateMap()
    {
        PrepareMap();
        PopulateMap();
    }

    private void PrepareMap()
    {
        SetWeights();
        FindOptimalPath();
    }

    public void ResetMap()
    {
        //Destroy each map tile
        foreach (var tileObject in _tileStore) tileObject.GetComponent<Tile>().Delete();

        Init();
        CreateMap();
    }

    private void SetWeights()
    {
        for (var x = 0; x < tiles; x++)
        for (var y = 0; y < tiles; y++)
            _weightsMap[x, y] =
                new MapNode(
                    Mathf.PerlinNoise((float)x / tiles * scale + _xOffset,
                        (float)y / tiles * scale + _yOffset),
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
            var totalSize = tiles * tileSize;
            var start = -1 * (totalSize / 2 - tileSize / 2);
            _map[destination.x, destination.y] = 3;
            _wayPoints.Insert(0, new Vector3(start + destination.x * tileSize, 0f, start + destination.y * tileSize));

            var successor = destination.bestSuccesor;

            while (successor != null)
            {
                _map[successor.x, successor.y] = 1;
                _wayPoints.Insert(0, new Vector3(start + successor.x * tileSize, 0f, start + successor.y * tileSize));
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
        var totalSize = tiles * tileSize;
        var position = new Vector3(-1 * (totalSize / 2 - tileSize / 2), 0f, -1 * (totalSize / 2 - tileSize / 2));
        for (var i = 0; i < tiles; i++)
        {
            for (var j = 0; j < tiles; j++)
            {
                var tileIndex = _map[i, j];
                var instance = Instantiate(tile, position, transform.rotation);
                var script = instance.GetComponent<Tile>();
                script.SetTile(tileIndex);
                instance.transform.parent = gameObject.transform;
                _tileStore.Add(instance);
                position += new Vector3(0f, 0f, tileSize);
            }

            position += new Vector3(tileSize, 0f, totalSize * -1);
        }
    }

    public List<Vector3> GetWaypoints()
    {
        return _wayPoints;
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