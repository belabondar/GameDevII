using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    private List<ResourceTile> _resourceTiles;

    private void Awake()
    {
        if (Instance != null) // If there is already an instance and it's not `this` instance
        {
            Debug.LogError(
                "More than one ResourceManager in scene!"); // Destroy the GameObject, this component is attached to
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _resourceTiles = new List<ResourceTile>();
    }

    public void AddResourceTile(ResourceTile tile)
    {
        _resourceTiles.Add(tile);
    }

    public void RemoveResourceTile(ResourceTile tile)
    {
        _resourceTiles.Remove(tile);
    }

    public List<ResourceTile> GetResourceTilesInRadius(Vector3 pos, float radius)
    {
        List<ResourceTile> finalList = new();

        foreach (var tile in _resourceTiles)
            if (Vector3.Distance(tile.gameObject.transform.position, pos) <= radius)
                finalList.Add(tile);

        finalList.Sort((tile1, tile2) => Vector3.Distance(tile1.transform.position, pos)
            .CompareTo(Vector3.Distance(tile2.transform.position, pos)));
        return finalList;
    }
}