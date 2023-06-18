using UnityEngine;

public class TileFactory : MonoBehaviour
{
    //Singleton is not necessary but probably nicer to only have one factory running instead of recreating one every time
    //we want to instance a tile
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

    //Factory spawns tiles depending on input
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