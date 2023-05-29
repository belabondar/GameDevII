using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Map;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    public int tiles = 20;

    public float tileSize = 4f;

    public float pathScale = 10f;

    public float envScale = 5f;
    public float woodThreshold = 0.5f;
    public float stoneOccurence = 0.2f;

    //Randomize settings
    public bool randomizeStartAndEnd;
    public PointRange startRange;
    public PointRange endRange;

    public TileType[] passableTiles = { TileType.Ground, TileType.Stone, TileType.Wood };

    public float groundEdgeCostMultiplier = 1f;
    public float woodEdgeCostMultiplier = 1.5f;
    public float stoneEdgeCostMultiplier = 2f;

    private MapNode[,] _map;

    private TileFactory _tileFactory;

    private List<Tile> _tileStore;
    private List<Vector3> _wayPoints;

    private float _xOffset;
    private float _yOffset;


    private void Start()
    {
        _tileFactory = TileFactory.Instance;
        Init();
    }

    private void Init()
    {
        _xOffset = Random.Range(0f, 1000f);
        _yOffset = Random.Range(0f, 1000f);

        _map = new MapNode[tiles, tiles];
        _tileStore = new List<Tile>();
        _wayPoints = new List<Vector3>();


        CreateMap();
    }

    /* Layer the map  in the following steps
     * 1. Fill whole map with ground tiles
     * 2. Add Woods and stones
     * 3. Add start and end point
     * 4. Connect start and end point using random noise
     */
    private void CreateMap()
    {
        //Generate Map Layer by Layer
        FillMapWithGround();
        AddResourceTiles();
        SetStartAndEnd();
        SetPath();

        //Instance map tiles
        BuildMap();
    }

    public void ResetMap()
    {
        //Destroy each map tile
        foreach (var tileObject in _tileStore) tileObject.Destroy();

        Init();
    }

    private void FillMapWithGround()
    {
        for (var x = 0; x < tiles; x++)
        for (var y = 0; y < tiles; y++)
            _map[x, y] = new MapNode(TileType.Ground, x, y, GetPosFromCoords(x, y));
    }

    private void AddResourceTiles()
    {
        for (var x = 0; x < tiles; x++)
        for (var y = 0; y < tiles; y++)
        {
            var noise = GetEnvNoise(x, y);
            if (noise > woodThreshold)
                _map[x, y].type = TileType.Wood;
            else if (Random.Range(0f, 1f) < stoneOccurence) _map[x, y].type = TileType.Stone;
        }
    }

    private void SetStartAndEnd()
    {
        if (randomizeStartAndEnd)
        {
            _map[startRange.xRange.Random, startRange.yRange.Random].type = TileType.Start;
            _map[endRange.xRange.Random, endRange.yRange.Random].type = TileType.End;
        }
        else
        {
            _map[1, 1].type = TileType.Start;
            _map[tiles - 2, tiles - 2].type = TileType.End;
        }
    }

    private void SetPath()
    {
        MapNode startTile = null, endTile = null;
        foreach (var node in _map)
            if (node.type == TileType.Start) startTile = node;
            else if (node.type == TileType.End) endTile = node;

        FindPath(new IntPoint(startTile), new IntPoint(endTile), true);
        SetPathTiles(endTile);
        _wayPoints = BacktraceFrom(endTile);
    }

    [CanBeNull]
    private MapNode FindPath(IntPoint start, IntPoint end, bool useRandomWeight)
    {
        ResetMapWeights();

        var openList = new PriorityQueue<MapNode>();
        var closedList = new List<MapNode>();
        var startPoint = _map[start.x, start.y];
        var destination = _map[end.x, end.y];
        startPoint.totalWeight = 0f;
        openList.Enqueue(startPoint, 0);

        //Find a path using A*
        while (openList.Count > 0)
        {
            //Pops
            var currentNode = openList.Dequeue();
            if (currentNode.Equals(destination)) return currentNode;
            closedList.Add(currentNode);
            ExpandNode(currentNode);
        }

        Debug.LogError("No Path found");
        return null;

        void ExpandNode(MapNode currentNode)
        {
            foreach (var successor in GetNeighbours(currentNode, false))
            {
                //If successor not in closed List
                if (closedList.Any(node => node.Equals(successor))) continue;

                var tentative_g = currentNode.totalWeight + GetEdgeWeight(successor, useRandomWeight);
                if (openList.Contains(successor) && tentative_g >= successor.totalWeight) continue;

                successor.bestSuccessor = currentNode;
                successor.totalWeight = tentative_g;
                openList.Enqueue(successor,
                    successor.totalWeight + successor.EstimatedDistanceTo(new Vector2(destination.x, destination.y)));
            }
        }
    }

    private float GetEdgeWeight(MapNode node, bool randomWeight)
    {
        var edgeWeight = tileSize * GetWeightMultiplierForTile(node.type);
        return edgeWeight * (randomWeight ? GetPathNoise(node.x, node.y) + 1 : 1f);

        float GetWeightMultiplierForTile(TileType type)
        {
            return type switch
            {
                TileType.Ground => groundEdgeCostMultiplier,
                TileType.Wood => woodEdgeCostMultiplier,
                TileType.Stone => stoneEdgeCostMultiplier,
                _ => 1f
            };
        }
    }

    private List<MapNode> GetNeighbours(MapNode node, bool limitMovement)
    {
        var neighbours = new List<MapNode>();
        var x = node.x;
        var y = node.y;
        var directions = new[]
        {
            new[] { x - 1, y },
            new[] { x + 1, y },
            new[] { x, y - 1 },
            new[] { x, y + 1 }
        };
        var tileRange = new Range(0, tiles - 1);

        foreach (var direction in directions)
        {
            x = direction[0];
            y = direction[1];
            if (!(tileRange.InRange(x) && tileRange.InRange(y))) continue;

            var neighbour = _map[x, y];
            if (passableTiles.Contains(neighbour.type) || !limitMovement) neighbours.Add(neighbour);
        }

        return neighbours;
    }

    private List<Vector3> BacktraceFrom(MapNode node)
    {
        List<Vector3> output = new();

        output.Insert(0, GetPosFromCoords(node.x, node.y));

        var successor = node.bestSuccessor;

        while (successor != null)
        {
            if (successor.type == TileType.Start) break;
            output.Insert(0, GetPosFromCoords(successor.x, successor.y));
            successor = successor.bestSuccessor;
        }

        return output;
    }

    private void SetPathTiles(MapNode end)
    {
        var successor = end.bestSuccessor;

        while (successor != null)
        {
            if (successor.type == TileType.Start) return;
            successor.type = TileType.Path;
            successor = successor.bestSuccessor;
        }
    }

    private void ResetMapWeights()
    {
        foreach (var node in _map) node.ResetWeights();
    }


    private float GetPathNoise(int x, int y)
    {
        //If tile is on edge, set weight to 1000f so A* is unlikely to take this path
        return x == 0 || y == 0 || x == tiles - 1 || y == tiles - 1
            ? 1000f
            : Mathf.PerlinNoise((float)x / tiles * pathScale + _xOffset, (float)y / tiles * pathScale + _yOffset);
    }

    private float GetEnvNoise(int x, int y)
    {
        return Mathf.PerlinNoise((float)x / tiles * envScale + _xOffset, (float)y / tiles * envScale + _yOffset);
    }

    private void BuildMap()
    {
        foreach (var node in _map)
        {
            var instance = _tileFactory.SpawnTile(node.position, transform.rotation, node.type, node.x, node.y);
            instance.transform.parent = gameObject.transform;
            instance.hideFlags = HideFlags.HideInHierarchy;
            _tileStore.Add(instance);
        }
    }

    private Vector3 GetPosFromCoords(int x, int y)
    {
        var totalSize = tiles * tileSize;
        var start = -1 * (totalSize / 2 - tileSize / 2);

        return new Vector3(start + x * tileSize, 0f, start + y * tileSize);
    }

    public List<Vector3> GetWaypoints()
    {
        return _wayPoints;
    }

    private class MapNode : IEquatable<MapNode>
    {
        public readonly Vector3 position;
        public readonly int x;
        public readonly int y;
        [CanBeNull] public MapNode bestSuccessor;
        public float totalWeight;
        public TileType type;

        public MapNode(TileType type, int x, int y, Vector3 position)
        {
            this.type = type;
            this.x = x;
            this.y = y;
            this.position = position;
        }

        public bool Equals(MapNode other)
        {
            if (other == null) return false;
            return x == other.x && y == other.y;
        }

        public void ResetWeights()
        {
            bestSuccessor = null;
            totalWeight = 0f;
        }

        public float EstimatedDistanceTo(Vector2 target)
        {
            return Vector2.Distance(new Vector2(x, y), target);
        }

        public override string ToString()
        {
            return $"({x}, {y}), {type}";
        }
    }

    [Serializable]
    public class PointRange
    {
        public Range xRange;

        public Range yRange;

        public PointRange(int xMin, int xMax, int yMin, int yMax)
        {
            xRange = new Range(xMin, xMax);
            yRange = new Range(yMin, yMax);
        }
    }

    [Serializable]
    public class Range
    {
        public int min;
        public int max;

        public Range(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public int Random => UnityEngine.Random.Range(min, max);

        public bool InRange(int num)
        {
            return min <= num && num <= max;
        }
    }

    private class IntPoint
    {
        public readonly int x;
        public readonly int y;

        public IntPoint(MapNode node)
        {
            x = node.x;
            y = node.y;
        }

        public IntPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals([CanBeNull] IntPoint other)
        {
            if (other == null) return false;
            return x == other.x && y == other.y;
        }
    }
}