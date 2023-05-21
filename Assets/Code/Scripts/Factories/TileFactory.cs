using UnityEngine;

public class TileFactory : MonoBehaviour
{
    public static TileFactory Instance;

    public Tile groundTile;
    public Tile pathTile;
    public Tile startTile;
    public Tile endTile;

    private void Awake()
    {
        if (Instance != null) // If there is already an instance and it's not `this` instance
        {
            Debug.LogError(
                "More than one TileFactory in scene!"); // Destroy the GameObject, this component is attached to
            return;
        }

        Instance = this;
    }

    public Tile SpawnGroundTile(Vector3 position, Quaternion rotation)
    {
        Debug.Log("Spawning Ground Tile");
        var tile = Instantiate(groundTile, position, rotation);
        tile.Init(TileType.Ground);
        return tile;
    }

    public Tile SpawnPathTile(Vector3 position, Quaternion rotation)
    {
        Debug.Log("Spawning Path Tile");
        var tile = Instantiate(pathTile, position, rotation);
        tile.Init(TileType.Path);
        return tile;
    }

    public Tile SpawnStartTile(Vector3 position, Quaternion rotation)
    {
        Debug.Log("Spawning Start Tile");
        var tile = Instantiate(startTile, position, rotation);
        tile.Init(TileType.Start);
        return tile;
    }

    public Tile SpawnEndTile(Vector3 position, Quaternion rotation)
    {
        Debug.Log("Spawning End Tile");
        var tile = Instantiate(endTile, position, rotation);
        tile.Init(TileType.End);
        return tile;
    }
}