using UnityEngine;

public class TileFactory : MonoBehaviour
{
    public static TileFactory Instance;

    public Tile groundTile;
    public Tile pathTile;
    public Tile startTile;
    public Tile endTile;
    public Tile woodTile;
    public Tile stoneTile;

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

    public Tile SpawnTile(Vector3 position, Quaternion rotation, TileType type, int x, int y)
    {
        var tile = Instantiate(GetTileByType(type), position, rotation);
        tile.Init(x, y, type);
        return tile;
    }

    public Tile GetTileByType(TileType type)
    {
        return type switch
        {
            TileType.Ground => groundTile,
            TileType.Path => pathTile,
            TileType.Start => startTile,
            TileType.End => endTile,
            TileType.Wood => woodTile,
            TileType.Stone => stoneTile
        };
    }
}